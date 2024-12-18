using LS.Plugins.AccountCapitalize;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Plugins.ContactAssociate
{
    public class AssociateContactsOnCreate : PluginBoilerplate
    {
        public override void Action(IPluginExecutionContext context, IOrganizationService service, ITracingService trace)
        {
            // Get the target - Contact
            var target = GetTarget(context);
            if (target == null)
            {
                return;
            }

            // Extract the email address
            var EMAIL_ATTR = "emailaddress1";
            var emailAttribute = AttributeOrDefault<string>(target, EMAIL_ATTR, null);
            if (emailAttribute == null) {
                return;
            }

            // Extract the email domain
            // eourfhero@domain.com
            var emailDomain = emailAttribute.Substring(emailAttribute.IndexOf("@") + 1);

            // Find other Contacts with the same email domain
            var query = $@"<fetch version=""1.0""
       output-format=""xml-platform""
       mapping=""logical""
       savedqueryid=""00000000-0000-0000-00aa-000010001003""
       no-lock=""false""
       distinct=""true"">
    <entity name=""contact"">
        <attribute name=""entityimage_url"" />
        <attribute name=""statecode"" />
        <attribute name=""fullname"" />
        <order attribute=""fullname""
               descending=""false"" />
        <attribute name=""parentcustomerid"" />
        <attribute name=""telephone1"" />
        <attribute name=""emailaddress1"" />
        <attribute name=""contactid"" />
        <filter type=""and"">
            <condition attribute=""statecode""
                       operator=""eq""
                       value=""0"" />
            <condition attribute=""parentcustomerid""
                       operator=""not-null"" />
            <condition attribute=""emailaddress1""
                       operator=""like""
                       value=""%{emailDomain}"" />
        </filter>
    </entity>
</fetch>";
            var results = service.RetrieveMultiple(new FetchExpression(query));
            if (results.Entities.Count == 0)
            {
                return; // TODO Create a new account.
            }


            // Get one of them - let's call it MatchedContact
            var matchedContact = results[0];

            // Extract the account from that MatchedContact
            var ACCOUNT_ATTR = "parentcustomerid";
            if (!matchedContact.Attributes.ContainsKey(ACCOUNT_ATTR))
            {
                return; // TODO - maybe throw an exception?
            }

            var account = matchedContact[ACCOUNT_ATTR] as EntityReference;

            // Copy the Account from the MatchedContact into our Target 
            target[ACCOUNT_ATTR] = account;
        }
    }
}

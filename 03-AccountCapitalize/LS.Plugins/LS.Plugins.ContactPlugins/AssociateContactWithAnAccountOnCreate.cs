using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Plugins.ContactPlugins
{
    public class AssociateContactWithAnAccountOnCreate : PluginBoilerplate
    {
        public override void Action(IPluginExecutionContext context, IOrganizationService service, ITracingService trace)
        {
            var target = GetTarget(context);
            if (target == null)
            {
                return;
            }

            // Target = Contact
            // Email Address = emailaddress1
            var EMAIL_ATTR = "emailaddress1";
            var emailVal = AttributeOrDefault<string>(target, EMAIL_ATTR, null);
            if (emailVal == null)
            {
                return;
            }

            // erdfoniorwej@example.com
            var domain = emailVal.Substring(emailVal.IndexOf("@") + 1);

            var query = $@"<fetch>
    <entity name=""contact"">
        <order attribute=""fullname""
               descending=""false"" />
        <attribute name=""parentcustomerid"" />
        <attribute name=""contactid"" />
        <filter type=""and"">
            <condition attribute=""statecode""
                       operator=""eq""
                       value=""0"" />
            <condition attribute=""emailaddress1""
                       operator=""like""
                       value=""%{domain}"" />
            <condition attribute=""parentcustomerid""
                       operator=""not-null"" />
        </filter>
    </entity>
</fetch>";

            var matchedContacts = service.RetrieveMultiple(new FetchExpression(query))
                .Entities.FirstOrDefault();
            if (matchedContacts == null)
            {
                return;
            }

            var ACCOUNT_ATTR = "parentcustomerid";
            var accountRef = AttributeOrDefault<EntityReference>(matchedContacts, ACCOUNT_ATTR, null);
            if (accountRef == null)
            {
                return;
            }

            target[ACCOUNT_ATTR] = accountRef;

        }
    }

}

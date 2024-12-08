using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;

namespace LS.Plugins.ContactPlugins
{
    public class AssociateContactWithAccountPreCreate : PluginBoilerplate
    {
        public override void Action(IPluginExecutionContext context, IOrganizationService service, ITracingService trace)
        {

            var target = GetTarget(context);
            if (target == null)
            {
                return;
            }

            var EMAIL_ATTR = "emailaddress1";
            var email = AttributeOrDefault<string>(target, EMAIL_ATTR, null);
            if (email == null || !email.Contains("@"))
            {
                return;
            }

            var atPosition = email.IndexOf("@");
            var domain = email.Substring(atPosition + 1);


            var COMPANY_ATTR = "parentcustomerid";
            var matchedContact = service.RetrieveMultiple(new FetchExpression($@"<fetch version=""1.0""
       output-format=""xml-platform""
       mapping=""logical""
       no-lock=""false""
       distinct=""true"">
    <entity name=""contact"">
        <attribute name=""entityimage_url"" />
        <attribute name=""statecode"" />
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
        </filter>
    </entity>
</fetch>")).Entities.FirstOrDefault();

            if (matchedContact == null)
            {
                return;
            }

            var accountRef = matchedContact[COMPANY_ATTR] as EntityReference;
            if (accountRef == null)
            {
                return;
            }

            target[COMPANY_ATTR] = accountRef;


        }
    }
}

using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Plugins.AccountCapitalize
{
    public class AccountCapitalize : PluginBoilerplate
    {
        public override void Action(IPluginExecutionContext context, IOrganizationService service, ITracingService trace)
        {
            var target = GetTarget(context);
            if (target == null)
            {
                return;
            }


            var NAME_ATTR = "name";
            var nameVal = AttributeOrDefault<string>(target, NAME_ATTR, null);
            if (nameVal == null)
            {
                return;
            }

            target[NAME_ATTR] = nameVal.ToUpperInvariant();
            
        }
    }
}

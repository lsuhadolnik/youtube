using LS.Utils;
using Microsoft.Xrm.Sdk;
using System;

namespace LS.YT01.NewsletterSubscribePlugin
{
    public class UpdateNewsletterFieldOnNewsletterSubscriptionCreate : PluginBoilerplate
    {
        public override void Action(IPluginExecutionContext context, IOrganizationService service, ITracingService trace)
        {
            Entity target = context.InputParameters["Target"] as Entity;
            target.Attributes[""]
        }
    }
}

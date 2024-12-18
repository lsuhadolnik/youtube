using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace LS.Plugins.AccountCapitalize
{

    /**
     * A boilerplate class to simplify plugin bootstrapping and enhance testability 
     */
    public abstract class PluginBoilerplate : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            if (context.Depth > 10)
            {

                throw new InvalidPluginExecutionException("Prevented infinite loop. This may cause some other problems. Please inform the CRM team.");
            };

            Action(context, service, trace);
        }

        public Entity GetTarget(IPluginExecutionContext context)
        {
            if (!context.InputParameters.ContainsKey("Target")
                || context.InputParameters["Target"] == null
                || !(context.InputParameters["Target"] is Entity))
            {
                return null;
            }

            // throw new InvalidPluginExecutionException("I'm here 2 :) :)");
            return context.InputParameters["Target"] as Entity;
        }

        public EntityReference GetTargetRef(IPluginExecutionContext context, string pluginname)
        {
            if (context.InputParameters.ContainsKey("Target")
                && context.InputParameters["Target"] != null
                && context.InputParameters["Target"] is EntityReference)
            {
                return context.InputParameters["Target"] as EntityReference;
            }

            throw new InvalidPluginExecutionException($"[err. {pluginname}] Target is not present, null or not an EntityReference.");
        }

        public Entity GetRawPostEntityImage(IPluginExecutionContext context, string pluginname)
        {
            if (context.PostEntityImages.Keys.Count == 0)
            {
                throw new InvalidPluginExecutionException($"[err. {pluginname}] Could not get PostEntityImage. There are none present.");
            }

            string key = context.PostEntityImages.Keys.First();
            return context.PostEntityImages[key];
        }

        public Entity GetRawPreEntityImage(IPluginExecutionContext context, string pluginname)
        {
            if (context.PreEntityImages.Keys.Count == 0)
            {
                throw new InvalidPluginExecutionException($"[err. {pluginname}] Could not get PreEntityImages. There are none present.");
            }

            string key = context.PreEntityImages.Keys.First();
            return context.PreEntityImages[key];
        }

        public T AttributeOrDefault<T>(Entity entity, string attributeName, T defaultValue)
        {
            if (entity.Attributes.ContainsKey(attributeName)
                && entity.Attributes[attributeName] != null)
            {
                return (T)entity.Attributes[attributeName];
            }
            else
            {
                return defaultValue;
            }
        }

        public T GetInputParameter<T>(IPluginExecutionContext context, string parameterName)
        {
            if (context.InputParameters.ContainsKey(parameterName))
            {
                return (T)context.InputParameters[parameterName];
            }

            throw new Exception($"Input parameter {parameterName} was not provided.");
        }


        public abstract void Action(IPluginExecutionContext context, IOrganizationService service, ITracingService trace);
    }


}

using LS.Plugins.Tests.Mocks;
using LS.Plugins.AccountCapitalize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;

namespace LS.Plugins.Tests
{

    [TestClass]
    public class AccountCapitalizeTests
    {

        private string NAME_ATTR = "name";

        // Not break if there's no target
        [TestMethod]
        public void AccountCapitalize_DosentBreakIfNoTarget()
        {
            var context = new MockPluginExecutionContext();
            var plugin = new AccountCapitalize.AccountCapitalize();
            plugin.Action(context, null, null);
        }

        // Not break if there's no AccountName
        [TestMethod]
        public void AccountCapitalize_DosentBreakIfNoAccountName()
        {
            var context = new MockPluginExecutionContext()
            {
                InputParameters = new ParameterCollection()
                {
                    ["Target"] = new Entity("account", Guid.NewGuid())
                    {
                        [NAME_ATTR] = null
                    }
                }
            };
            var plugin = new AccountCapitalize.AccountCapitalize();
            plugin.Action(context, null, null);

        }

        // Capitalize te account name
        [TestMethod]
        public void AccountCapitalize_CapitalizesAccountName()
        {

            var accName = "eroikvnoi";

            var context = new MockPluginExecutionContext()
            {
                InputParameters = new ParameterCollection()
                {
                    ["Target"] = new Entity("account", Guid.NewGuid())
                    {
                        [NAME_ATTR] = accName
                    }
                }
            };
            var plugin = new AccountCapitalize.AccountCapitalize();
            plugin.Action(context, null, null);

            var target = context.InputParameters["Target"] as Entity;
            Assert.AreEqual(accName.ToUpperInvariant(), target[NAME_ATTR]);
        }
    }
}

using LS.Plugins.ContactAssociate;
using LS.Plugins.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Plugins.Tests
{
    [TestClass]
    public class ContactAssociateTests
    {

        [TestMethod]
        public void ContactsAssociateTest()
        {

            var EMAIL_ATTR = "emailaddress1";
            var ACCOUNT_ATTR = "parentcustomerid";

            var target = new Entity("contact")
            {
                [EMAIL_ATTR] = "test@domain.com"
            };

            var ACCOUNT_ID = Guid.NewGuid();

            var ctx = new MockPluginExecutionContext();
            ctx.InputParameters.Add("Target", target);

            var service = new MockOgranizationService();
            service.RetrieveMultipleResult = new EntityCollection(new List<Entity>()
            {
                new Entity("contact")
                {
                    [ACCOUNT_ATTR] = new EntityReference("account", ACCOUNT_ID)
                }
            });

            var plugin = new AssociateContactsOnCreate();
            plugin.Action(ctx, service, null);

            var targetAccountRef = target[ACCOUNT_ATTR] as EntityReference;
            Assert.AreEqual(ACCOUNT_ID, targetAccountRef.Id);
        }

        [TestMethod]
        public void ContactsAssociateTest_NoTarget()
        {

            var EMAIL_ATTR = "emailaddress1";
            var ACCOUNT_ATTR = "parentcustomerid";

            var target = new Contact()
            {
                EMailAddress1 = "test@domain.com"
            };

            var ACCOUNT_ID = Guid.NewGuid();

            var ctx = new MockPluginExecutionContext();
            // ctx.InputParameters.Add("Target", target);

            var service = new MockOgranizationService();
            service.RetrieveMultipleResult = new EntityCollection(new List<Entity>()
            {
                new Contact
                {
                    ParentCustomerId = new EntityReference("account", ACCOUNT_ID)
                }
            });

            var plugin = new AssociateContactsOnCreate();
            plugin.Action(ctx, service, null);

            Assert.IsFalse(target.Attributes.ContainsKey(ACCOUNT_ATTR));
        }

        [TestMethod]
        public void ContactsAssociateTest_NoEmail()
        {

            var EMAIL_ATTR = "emailaddress1";
            var ACCOUNT_ATTR = "parentcustomerid";

            var target = new Entity("contact")
            {
                // [EMAIL_ATTR] = "test@domain.com"
            };

            var ACCOUNT_ID = Guid.NewGuid();

            var ctx = new MockPluginExecutionContext();
            ctx.InputParameters.Add("Target", target);

            var service = new MockOgranizationService();
            service.RetrieveMultipleResult = new EntityCollection(new List<Entity>()
            {
                new Entity("contact")
                {
                    [ACCOUNT_ATTR] = new EntityReference("account", ACCOUNT_ID)
                }
            });

            var plugin = new AssociateContactsOnCreate();
            plugin.Action(ctx, service, null);

            Assert.IsFalse(target.Attributes.ContainsKey(ACCOUNT_ATTR));
        }

        [TestMethod]
        public void ContactsAssociateTest_InvalidEmail()
        {

            var EMAIL_ATTR = "emailaddress1";
            var ACCOUNT_ATTR = "parentcustomerid";

            var target = new Entity("contact")
            {
                [EMAIL_ATTR] = "test"
            };

            var ACCOUNT_ID = Guid.NewGuid();

            var ctx = new MockPluginExecutionContext();
            ctx.InputParameters.Add("Target", target);

            var service = new MockOgranizationService();
            service.RetrieveMultipleResult = new EntityCollection(new List<Entity>()
            {
                new Entity("contact")
                {
                    [ACCOUNT_ATTR] = new EntityReference("account", ACCOUNT_ID)
                }
            });

            var plugin = new AssociateContactsOnCreate();
            plugin.Action(ctx, service, null);

            Assert.IsFalse(target.Attributes.ContainsKey(ACCOUNT_ATTR));
        }

        [TestMethod]
        public void ContactsAssociateTest_NoMatches()
        {

            var EMAIL_ATTR = "emailaddress1";
            var ACCOUNT_ATTR = "parentcustomerid";

            var target = new Entity("contact")
            {
                [EMAIL_ATTR] = "test"
            };

            var ACCOUNT_ID = Guid.NewGuid();

            var ctx = new MockPluginExecutionContext();
            ctx.InputParameters.Add("Target", target);

            var service = new MockOgranizationService();

            var plugin = new AssociateContactsOnCreate();
            plugin.Action(ctx, service, null);

            Assert.IsFalse(target.Attributes.ContainsKey(ACCOUNT_ATTR));
        }

    }
}

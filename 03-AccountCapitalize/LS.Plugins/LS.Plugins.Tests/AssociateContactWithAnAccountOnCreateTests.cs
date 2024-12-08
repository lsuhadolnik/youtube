using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Plugins.Tests
{
    [TestClass]
    public class AssociateContactWithAnAccountOnCreateTests
    {

        [TestMethod]
        public void GetTheDomainFromAnEmail()
        {
            var exampleEmail = "drsfijrpoj@example.com";

            var domain = exampleEmail.Substring(exampleEmail.IndexOf("@") + 1);

            Assert.AreEqual("example.com", domain);
        }

    }
}

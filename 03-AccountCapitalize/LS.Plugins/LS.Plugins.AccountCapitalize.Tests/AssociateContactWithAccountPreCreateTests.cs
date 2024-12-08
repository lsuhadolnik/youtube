using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Plugins.AccountCapitalize.Tests
{

    [TestClass]
    public class AssociateContactWithAccountPreCreateTests
    {

        [TestMethod]
        public void TestAtSign()
        {
            var email = "drfksjmoreij@domain.com";

            var atPos = email.IndexOf("@");
            var domain = email.Substring(atPos + 1);

            Assert.AreEqual("domain.com", domain);
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Plugins.Tests
{
    [TestClass]
    public class DomainExtraction
    {
        [TestMethod]
        public void ExtractTheDomain()
        {
            var emailAttribute = "kjerheri@eriohior.com";
            var emailDomain = emailAttribute.Substring(emailAttribute.IndexOf("@") + 1);

            Assert.AreEqual("eriohior.com", emailDomain);
        }
    }
}

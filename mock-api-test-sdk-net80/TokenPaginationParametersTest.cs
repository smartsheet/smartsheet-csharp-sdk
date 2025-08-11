using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api.Models;
using System.Collections.Generic;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TokenPaginationParametersTest
    {
        [TestMethod]
        public void TestTokenPaginationParameters_DefaultPaginationType()
        {
            TokenPaginationParameters parameters = new TokenPaginationParameters(null, 10);
            
            Assert.AreEqual("token", parameters.PaginationType);
            
            IDictionary<string, string> dict = parameters.toDictionary();
            Assert.IsTrue(dict.ContainsKey("paginationType"));
            Assert.AreEqual("token", dict["paginationType"]);
            Assert.IsTrue(dict.ContainsKey("maxItems"));
            Assert.AreEqual("10", dict["maxItems"]);
        }

        [TestMethod]
        public void TestTokenPaginationParameters_ExplicitPaginationType()
        {
            TokenPaginationParameters parameters = new TokenPaginationParameters("abc123", 20, "token");
            
            Assert.AreEqual("token", parameters.PaginationType);
            Assert.AreEqual("abc123", parameters.LastKey);
            Assert.AreEqual(20, parameters.MaxItems);
            
            IDictionary<string, string> dict = parameters.toDictionary();
            Assert.IsTrue(dict.ContainsKey("paginationType"));
            Assert.AreEqual("token", dict["paginationType"]);
            Assert.IsTrue(dict.ContainsKey("lastKey"));
            Assert.AreEqual("abc123", dict["lastKey"]);
            Assert.IsTrue(dict.ContainsKey("maxItems"));
            Assert.AreEqual("20", dict["maxItems"]);
        }

        [TestMethod]
        public void TestTokenPaginationParameters_ToQueryString()
        {
            TokenPaginationParameters parameters = new TokenPaginationParameters("abc123", 15);
            
            string queryString = parameters.ToQueryString();
            Assert.IsNotNull(queryString);
            Assert.IsTrue(queryString.Contains("paginationType=token"));
            Assert.IsTrue(queryString.Contains("lastKey=abc123"));
            Assert.IsTrue(queryString.Contains("maxItems=15"));
        }

        [TestMethod]
        public void TestTokenPaginationParameters_NullValues()
        {
            TokenPaginationParameters parameters = new TokenPaginationParameters(null, null);
            
            Assert.AreEqual("token", parameters.PaginationType);
            Assert.IsNull(parameters.LastKey);
            Assert.IsNull(parameters.MaxItems);
            
            IDictionary<string, string> dict = parameters.toDictionary();
            Assert.IsTrue(dict.ContainsKey("paginationType"));
            Assert.AreEqual("token", dict["paginationType"]);
            Assert.IsFalse(dict.ContainsKey("lastKey"));
            Assert.IsFalse(dict.ContainsKey("maxItems"));
        }
    }
}
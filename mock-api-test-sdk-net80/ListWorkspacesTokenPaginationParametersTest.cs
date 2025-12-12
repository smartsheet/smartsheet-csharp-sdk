using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api.Models;
using System.Collections.Generic;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class ListWorkspacesTokenPaginationParametersTest
    {
        [TestMethod]
        public void TestListWorkspacesTokenPaginationParameters_DefaultPaginationType()
        {
            ListWorkspacesTokenPaginationParameters parameters = new ListWorkspacesTokenPaginationParameters(null, 10);

            Assert.AreEqual("token", parameters.PaginationType);
            Assert.IsNull(parameters.LastKey);
            Assert.AreEqual(10, parameters.MaxItems);

            IDictionary<string, string> dict = parameters.toDictionary();
            Assert.IsTrue(dict.ContainsKey("paginationType"));
            Assert.AreEqual("token", dict["paginationType"]);
            Assert.IsTrue(dict.ContainsKey("maxItems"));
            Assert.AreEqual("10", dict["maxItems"]);
            Assert.IsFalse(dict.ContainsKey("lastKey"));
        }

        [TestMethod]
        public void TestListWorkspacesTokenPaginationParameters_ExplicitPaginationType()
        {
            ListWorkspacesTokenPaginationParameters parameters = new ListWorkspacesTokenPaginationParameters("abc123", 20, "token");

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
        public void TestListWorkspacesTokenPaginationParameters_ToQueryString()
        {
            ListWorkspacesTokenPaginationParameters parameters = new ListWorkspacesTokenPaginationParameters("abc123", 15);

            string queryString = parameters.ToQueryString();
            Assert.IsNotNull(queryString);
            Assert.IsTrue(queryString.Contains("paginationType=token"));
            Assert.IsTrue(queryString.Contains("lastKey=abc123"));
            Assert.IsTrue(queryString.Contains("maxItems=15"));
        }

        [TestMethod]
        public void TestListWorkspacesTokenPaginationParameters_NullValues()
        {
            ListWorkspacesTokenPaginationParameters parameters = new ListWorkspacesTokenPaginationParameters(null, null);

            Assert.AreEqual("token", parameters.PaginationType);
            Assert.IsNull(parameters.LastKey);
            Assert.IsNull(parameters.MaxItems);

            IDictionary<string, string> dict = parameters.toDictionary();
            Assert.IsTrue(dict.ContainsKey("paginationType"));
            Assert.AreEqual("token", dict["paginationType"]);
            Assert.IsFalse(dict.ContainsKey("lastKey"));
            Assert.IsFalse(dict.ContainsKey("maxItems"));
        }

        [TestMethod]
        public void TestListWorkspacesTokenPaginationParameters_InheritsFromBase()
        {
            ListWorkspacesTokenPaginationParameters parameters = new ListWorkspacesTokenPaginationParameters("test123", 25, "token");

            // Test that it inherits from TokenPaginationParameters
            Assert.IsInstanceOfType(parameters, typeof(TokenPaginationParameters));

            // Test base class properties
            Assert.AreEqual("test123", parameters.LastKey);
            Assert.AreEqual(25, parameters.MaxItems);

            // Test derived class property
            Assert.AreEqual("token", parameters.PaginationType);
        }

        [TestMethod]
        public void TestListWorkspacesTokenPaginationParameters_OverridesToDictionary()
        {
            ListWorkspacesTokenPaginationParameters parameters = new ListWorkspacesTokenPaginationParameters("key456", 30, "token");

            IDictionary<string, string> dict = parameters.toDictionary();

            // Should contain all three parameters
            Assert.AreEqual(3, dict.Count);
            Assert.IsTrue(dict.ContainsKey("lastKey"));
            Assert.AreEqual("key456", dict["lastKey"]);
            Assert.IsTrue(dict.ContainsKey("maxItems"));
            Assert.AreEqual("30", dict["maxItems"]);
            Assert.IsTrue(dict.ContainsKey("paginationType"));
            Assert.AreEqual("token", dict["paginationType"]);
        }
    }
}

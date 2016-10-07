using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserMessages.Infrastructure;
using System.Collections.Generic;

namespace UserMessages.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            HtmAgilityParser parser = new HtmAgilityParser();
            List<NodeOfParse> result =  parser.ParseHtmlOnliner();
        }      
    }
}
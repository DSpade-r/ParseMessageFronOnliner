using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserMessages.Infrastructure;
using System.Collections.Generic;
using UserMessages.Models;
using HtmlAgilityPack;

namespace UserMessages.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            HtmAgilityParser parser = new HtmAgilityParser();
            ParseInfo parseInfoTest = new ParseInfo { MaxMessage = 4, Name = "1", PageCount = 1, UserID = 1, url = @"http://forum.onliner.by/viewtopic.php?t=1863541&start=16380" };
            List<HtmlNode> listNode =  parser.GetPostNode(parseInfoTest);
            var message = parser.GetMessage(listNode[18]);
        }      
    }
}
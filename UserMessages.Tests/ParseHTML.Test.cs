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
        //тест на корректность трансформации адреса смайла
        [TestMethod]
        public void Test_ParseMessage_ConvertUrlSmile()
        {
            HtmAgilityParser parser = new HtmAgilityParser();
            ParseInfo parseInfoTest = new ParseInfo { MaxMessage = 4, Name = "1", PageCount = 1, UserID = 1, url = @"http://forum.onliner.by/viewtopic.php?t=1863541&start=8340" };
            List<HtmlNode> listNode =  parser.GetPostNode(parseInfoTest);
            string testString = @"<p>а что слышна про дом№8???? <img src=""http://forum.onliner.by/images/smilies/confused.gif""></p>";
            string message = parser.GetMessage(listNode[14]);
            Assert.AreEqual(message, testString);

        }
        [TestMethod]
        public void Test_ParseMessage_WithLink()
        {
            HtmAgilityParser parser = new HtmAgilityParser();
            ParseInfo parseInfoTest = new ParseInfo { MaxMessage = 4, Name = "1", PageCount = 1, UserID = 1, url = @"http://forum.onliner.by/viewtopic.php?t=1863541&start=8340" };
            List<HtmlNode> listNode = parser.GetPostNode(parseInfoTest);
            string testString = @"<p><strong>Garik_p</strong>, <a href=""http://ru.homestyler.com/designer""><!-- u -->И ничего качать не надо.<!-- u --></a></p>";
            string message = parser.GetMessage(listNode[12]);
            Assert.AreEqual(message, testString);
        }
    }
}
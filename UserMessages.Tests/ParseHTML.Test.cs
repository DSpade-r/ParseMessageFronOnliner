using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserMessages.Infrastructure;
using System.Collections.Generic;
using UserMessages.Models;
using HtmlAgilityPack;
using System;
using System.Globalization;

namespace UserMessages.Tests
{
    [TestClass]
    public class Test_ParsingPost
    {    
        //тест на корректность трансформации адреса смайла
        [TestMethod]
        public void Test_ParseMessage_ConvertUrlSmile()
        {
            //Arrange
            HtmAgilityParser parser = new HtmAgilityParser();
            ParseInfo parseInfoTest = new ParseInfo { MaxMessage = 4, Name = "1", PageCount = 1, UserID = 1, url = @"http://forum.onliner.by/viewtopic.php?t=1863541&start=8340" };
            List<HtmlNode> listNode =  parser.GetPostNode(parseInfoTest);
            //Act
            string testString = @"<p>а что слышна про дом№8???? <img src=""http://forum.onliner.by/images/smilies/confused.gif""></p>";
            string message = parser.GetMessage(listNode[14]);
            //Assert
            Assert.AreEqual(message, testString);
        }
        //Тест на корректность удаления атрибутов(ссылки остаются)
        [TestMethod]
        public void Test_ParseMessage_WithLink()
        {
            //Arrange
            HtmAgilityParser parser = new HtmAgilityParser();
            ParseInfo parseInfoTest = new ParseInfo { MaxMessage = 4, Name = "1", PageCount = 1, UserID = 1, url = @"http://forum.onliner.by/viewtopic.php?t=1863541&start=8340" };
            List<HtmlNode> listNode = parser.GetPostNode(parseInfoTest);
            //Act
            string testString = @"<p><strong>Garik_p</strong>, <a href=""http://ru.homestyler.com/designer""><!-- u -->И ничего качать не надо.<!-- u --></a></p>";
            string message = parser.GetMessage(listNode[12]);
            //Assert
            Assert.AreEqual(message, testString);
        }       
        [TestMethod]
        public void Test_ParseIDMessage()
        {
            //Arrange
            HtmAgilityParser parser = new HtmAgilityParser();
            ParseInfo parseInfoTest = new ParseInfo { MaxMessage = 4, Name = "1", PageCount = 1, UserID = 1, url = @"http://forum.onliner.by/viewtopic.php?t=1863541&start=8340" };
            List<HtmlNode> listNode = parser.GetPostNode(parseInfoTest);
            int testId = 42122394;
            //Act
            int messageID = parser.GetIdMessage(listNode[13]);
            //Assert
            Assert.AreEqual(messageID, testId);
        } 
        [TestMethod]
        public void Test_ParseDateMessage()
        {
            //Arrange
            HtmAgilityParser parser = new HtmAgilityParser();
            ParseInfo parseInfoTest = new ParseInfo { MaxMessage = 4, Name = "1", PageCount = 1, UserID = 1, url = @"http://forum.onliner.by/viewtopic.php?t=1863541&start=8340" };
            List<HtmlNode> listNode = parser.GetPostNode(parseInfoTest);
            DateTime testDate = new DateTime(2012, 10, 28, 18, 44, 00);
            //CultureInfo ci = CultureInfo.CreateSpecificCulture("ru-RU");
            //ci.DateTimeFormat.YearMonthPattern = "MMMM YYYY";
            //string innerDate = testDate.ToString("f", ci);
            //DateTime innerDateDT = DateTime.ParseExact(innerDate, "f", CultureInfo.CreateSpecificCulture("ru-RU"));
            //Act
            DateTime messageDate = parser.GetDateMessage(listNode[13]);
            //Assert
            Assert.AreEqual(messageDate, testDate);
        }
        [TestMethod]
        public void Test_ParseUserIdMessage()
        {
            //Arrange
            HtmAgilityParser parser = new HtmAgilityParser();
            ParseInfo parseInfoTest = new ParseInfo { MaxMessage = 4, Name = "1", PageCount = 1, UserID = 1, url = @"http://forum.onliner.by/viewtopic.php?t=1863541&start=8340" };
            List<HtmlNode> listNode = parser.GetPostNode(parseInfoTest);
            int testUserId = 496400;
            //Act
            int userId = parser.GetUserID(listNode[13]);
            //Assert
            Assert.AreEqual(userId, testUserId);
        }
    }
}
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
        [TestMethod]
        public void Test_GetPostNode()
        {
            //GetPostNode по логике свое работы должен возвращать 20 нодов(постов) + 1 пост это шапка(фиксированный пост)
            //с каждой заполненной на форуме страницы - это и проверю
            //каждая запись в listNode должна иметь имя ноды - li
            //Arrange
            HtmAgilityParser parser = new HtmAgilityParser();
            ParseInfo parseInfoTest = new ParseInfo { MaxMessage = 4, Name = "1", PageCount = 1, UserID = 1, url = @"http://forum.onliner.by/viewtopic.php?t=1863541&start=8340" };
            //Act
            List<HtmlNode> listNode = parser.GetPostNode(parseInfoTest);
            int countNode = 21;
            string nameItemOfNode = "li";
            //Assert
            Assert.AreEqual(countNode, listNode.Count);
            Assert.AreEqual(nameItemOfNode, listNode[2].Name);
        }
        [TestMethod]
        public void Test_ParseHtmlOnliner()
        {
            //Arrange
            HtmAgilityParser parser = new HtmAgilityParser();
            ParseInfo parseInfoTest = new ParseInfo { MaxMessage = 20, Name = "DSpade", PageCount = 10, UserID = 1, url = @"http://forum.onliner.by/viewtopic.php?t=1863541&start=16360" };
            NodeOfParse testNote = new NodeOfParse();
            testNote.Date = new DateTime(2016, 09, 22, 21, 24, 00);
            testNote.IdMessage = 91125917;
            testNote.IdUser = 371661;
            testNote.NameUser = "DSpade";
            testNote.Message = "<p><strong>ирэн-12</strong>, ерунда все это. Не будьте такими пугаными<img src=\"http://forum.onliner.by/images/smilies/icon_smile.gif\"></p>";
            //Act
            List<NodeOfParse> Notes = parser.ParseHtmlOnliner(parseInfoTest);
            //Assert
            Assert.AreEqual(Notes[0].Date, testNote.Date);
            Assert.AreEqual(Notes[0].IdMessage, testNote.IdMessage);
            Assert.AreEqual(Notes[0].IdUser, testNote.IdUser);
            Assert.AreEqual(Notes[0].NameUser, testNote.NameUser);
            Assert.AreEqual(Notes[0].Message, testNote.Message);
        }
    }
}
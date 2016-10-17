using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using UserMessages.Models;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using Ninject;
using UserMessages.Infrastructure.Interfaces;

namespace UserMessages.Infrastructure
{
    public class HtmlAgilityParser : IHtmlParser 
    {
        private IDownloader downloader;
        private IParser parser;
        public HtmlAgilityParser(IParser parser, IDownloader downloader)
        {
            this.parser = parser;
            this.downloader = downloader;
        } 
        public List<NodeOfParse> ParseHtmlOnliner(ParseInfo parseInfo)
        {
            List<NodeOfParse> Notes = new List<NodeOfParse>();
            //цикл для перебора страниц парсинга
            for (int parsePage = 1; parsePage <= parseInfo.PageCount; parsePage++)
            {                
                //формирую url в зависимости от количества необходимых страниц
                string pattern = @"=\d{1,10}$"; //паттерн для определения количества постов
                Regex url = new Regex(pattern);
                string strNumberPage = url.Match(parseInfo.url).Value;  //нахожу страницу форума из url(нужня для модификации)
                strNumberPage = strNumberPage.Remove(0, 1);             //удаляю "=" для корректного преобразования в int
                //int forumList = Int32.Parse(strNumberPage)
                strNumberPage = (Int32.Parse(strNumberPage) + (parsePage == 1? 0 : 20)).ToString(); //преобразовываю в int, модифицирую значение на след страницу и возвращаю обратно в string
                strNumberPage = "=" + strNumberPage;
                parseInfo.url = url.Replace(parseInfo.url, strNumberPage); //получаю конечную строку с новым адресом страницы
                //получаю коллекцию постов на странице из parseInfo
                var liList = parser.GetPostNode(downloader.DounloadHtml(parseInfo.url));
                //просматриваю все посты и записываю те, которые удовлетворяют условию
                foreach (var node in liList)
                {
                    //получаю никнейм пользователя
                    var author = node.Descendants().
                        Where(div => div.GetAttributeValue("class", "").Equals("b-mtauthor")).Single().
                        Descendants().
                        Where(div => div.GetAttributeValue("class", "").Equals("b-mtauthor-i")).Single().
                        SelectSingleNode("big").
                        SelectSingleNode("span").
                        SelectSingleNode("a").InnerText;                    
                    //проверяю "наш ли клиент":)
                    if (author == parseInfo.Name)
                    {
                        //Усли уже найдено необходимо количество сообщений - то заканчиваем дальше работать 
                        //и возвращаем то что есть
                        if (Notes.Count == parseInfo.MaxMessage) return Notes;
                        //вбиваю в коллекцию данные по записи:
                        //1. id сообщения
                        //2. дата и время сообщения
                        //3. само солбщение    
                        NodeOfParse temp = new NodeOfParse();
                        temp.IdMessage = parser.GetIdMessage(node);
                        if (Notes.Find(it => it.IdMessage == temp.IdMessage) != null) continue;
                        temp.Message = parser.GetMessage(node);                        
                        temp.NameUser = author;
                        temp.Date = parser.GetDateMessage(node);
                        temp.UserId = parser.GetUserID(node);
                        Notes.Add(temp);                        
                    }
                }
            }
            return Notes;
        }  
    }
}
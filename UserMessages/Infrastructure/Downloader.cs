using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UserMessages.Models;

namespace UserMessages.Infrastructure
{
    public class Downloader
    {
        //метод определения коллекции постов по адресу страницы из parseInfo
        public List<HtmlNode> GetPostNode(ParseInfo parseInfo)
        {
            WebClient web = new WebClient();
            //адрес html
            string url = web.DownloadString(parseInfo.url);
            //создаю экземпляр класса
            HtmlDocument document = new HtmlDocument();
            //Загружаю в класс(парсер) наш html
            document.LoadHtml(url);
            var ul = document.DocumentNode.SelectSingleNode("//ul[@class='b-messages-thread']");
            var liList = ul.SelectNodes("descendant::li")               //выбираю все элементы li
                .Where(li => (li.Attributes.Count > 1))                 //у которых атрибутов больше 2
                .Where(li => li.Attributes[1].Value.Contains("msgpost"))//и второй атрибут содержит слово msgpost
                .Select(li => li).ToList();
            //возвращаю коллекцию постов на странице parseInfo.url        
            return liList;
        }
    }
}
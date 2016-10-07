using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using UserMessages.Models;
using System.Text;

namespace UserMessages.Infrastructure
{
    public class HtmAgilityParser
    {
        //парсинг страницы онлайнера view-source:http://forum.onliner.by/viewtopic.php?t=5920148&p=91344704#p91344704
        //необходимо со страницы получить данные:
        //1. id сообщения
        //2. дату(и аремя) сообщения
        //3. текст сообщения
        //Эти данные берутся для указанного пользователя.
        //Анализ:
        //<ul class="b-messages-thread"></ul>
        //  <li id ="p000000" class ="msgpost....."></li>
        //сначала необходимо выдернуть блок ul
        //далее перебирать все li, выдергивая в запись данные по сообщению,
        //если все данные записаны - добавляем в коллекцию для последующего внесения в БД
        //если не все(номер поста записан, текст записан, но не те данные по пользователю - то обнуляем запись и переходим к следующей
        public List<NodeOfParse> ParseHtmlOnliner(ParseInfo parseInfo)
        {
            List<NodeOfParse> Nodes = new List<NodeOfParse>();
            WebClient web = new WebClient();
            //адрес html
            string url = web.DownloadString("http://forum.onliner.by/viewtopic.php?t=16513761&start=0");
            //создаю экземпляр класса
            HtmlDocument document = new HtmlDocument();            
            //Загружаю в класс(парсер) наш html
            document.LoadHtml(url);            
            var ul = document.DocumentNode.SelectSingleNode("//ul[@class='b-messages-thread']");
            var liList = ul.SelectNodes("descendant::li")               //выбираю все элементы li
                .Where(li => (li.Attributes.Count > 1))                 //у которых атрибутов больше 2
                .Where(li => li.Attributes[1].Value.Contains("msgpost"))//и второй атрибу содержит слово msgpost
                .Select(li => li).ToList();
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
                //попровки по кодировке onlinet.by utf-8 использует
                byte[] bytes = Encoding.Default.GetBytes(author);
                author = Encoding.UTF8.GetString(bytes);
                //проверяю "наш ли клиент":)
                if (author == parseInfo.Name)
                {
                    //вбиваю в коллекцию данные по записи:
                    //1. id сообщения
                    //2. дата и время сообщения
                    //3. само солбщение

                }
            }
            return Nodes;
        }
        //метод изъятия текста сообщения
        public string GetMessage(HtmlNode node)
        {
            var message = node.Descendants().
                    Where(div => div.GetAttributeValue("class", "").Equals("b-msgpost-txt")).Single().
                    Descendants().
                    Where(div => div.GetAttributeValue("class", "").Equals("b-msgpost-txt-i")).Single().
                    Descendants().
                    Where(div => div.GetAttributeValue("class", "").Equals("content")).Single().
                    SelectSingleNode("big").
                    SelectSingleNode("span").
                    SelectSingleNode("a").InnerText;
        }
    }
}
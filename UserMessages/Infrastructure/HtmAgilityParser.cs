using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using UserMessages.Models;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

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
            List<NodeOfParse> Notes = new List<NodeOfParse>();
            //получаю коллекцию постов на странице из parseInfo
            var liList = GetPostNode(parseInfo);
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
                int countNote = 0; //количество записей по автору
                //проверяю "наш ли клиент":)
                if (author == parseInfo.Name)
                {
                    //вбиваю в коллекцию данные по записи:
                    //1. id сообщения
                    //2. дата и время сообщения
                    //3. само солбщение
                    if (Notes.Count < parseInfo.MaxMessage)
                    {
                        Notes[countNote].Message = GetMessage(node);
                        Notes[countNote].IdMessage = GetIdMessage(node);
                        Notes[countNote].NameUser = author;
                        Notes[countNote].Date = GetDateMessage(node);
                        Notes[countNote].IdUser = GetUserID(node);
                    }
                }
            }
            return Notes;
        }  
        //метод       
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
        #region Изъятие текста сообщения
        //метод изъятия текста сообщения
        public string GetMessage(HtmlNode node)
        {
            //блок <div class= "content"> является нашим сообщением
            var message = node.SelectNodes("descendant::div").
                Where(div => div.Attributes.Count > 1 &&
                div.Attributes[0].Value.Contains("content")).
                Select(div => div).ToList();
            //теперь необходимо удалить ненужные классы и модифицировать
            //ссылку на смайлики:)
            ModNode(message[0]);
            string htmlMessage = message[0].InnerHtml;
            byte[] bytes = Encoding.Default.GetBytes(htmlMessage);
            htmlMessage = Encoding.UTF8.GetString(bytes);
            htmlMessage = htmlMessage.Trim();
            //var smileNode = message.SelectNodes("img").Where(src => src.Attributes[])
            return htmlMessage;
        }
        //метод рекурсивного перебора всех элементов сообщения и удаления ненужных атрибутов
        //нужные атрибуты:
        // src и href для смайликов и ссылок
        public void ModNode(HtmlNode nodeForMod)
        {         
            //проверяю на наличие "нужных атрибутов" и удаляю ненужные  
            for (int currAttr = 0; currAttr < nodeForMod.Attributes.Count; currAttr++)
            {
                //с ссылками ничего делать не приходится - они оригинальные и рабочие
                //к рисункам смайликов придется модифицировать путь, так как у нас он внутренний(относительно)
                //то есть например вместо ./images/smilies/molotok.gif стоит подставить http://forum.onliner.by/images/smilies/molotok.gif
                if (nodeForMod.Attributes[currAttr].Name == "src")
                {
                    nodeForMod.Attributes[currAttr].Value = nodeForMod.Attributes[currAttr].Value.Replace(@"./images/smilies", @"http://forum.onliner.by/images/smilies");
                    continue;
                }
                if (nodeForMod.Attributes[currAttr].Name == "href") continue;
                nodeForMod.Attributes.RemoveAt(currAttr);   //удаление атрибута(с концами:))
                currAttr--; //счетчик назад так как атрибут удален полностью(чтобы не пропустить след по логике)                
            }
            if (!nodeForMod.HasChildNodes) return;
            for (int currChild = 0; currChild < nodeForMod.ChildNodes.Count; currChild++)
            {
                ModNode(nodeForMod.ChildNodes[currChild]);
            }   
        }
        #endregion
        #region Изъятие номера сообщения
        public int GetIdMessage(HtmlNode node)
        {
            //блок <small class= "msgpost-date"> дает информацию о номере сообщения(Id)
            var TagWithIdMessage = node.SelectNodes("descendant::small").
                Where(small => small.Attributes.Count > 1 &&
                small.Attributes[0].Value.Contains("msgpost-date")).
                Select(small => small).ToList();
            return Int32.Parse(TagWithIdMessage[0].Id);
        }
        #endregion
        #region Изъятие даты сообщения
        public DateTime GetDateMessage(HtmlNode node)
        {
            //блок <small class= "msgpost-date"> дает информацию о номере сообщения(Id)
            var TagWithIdMessage = node.SelectNodes("descendant::small").
                Where(small => small.Attributes.Count > 1 &&
                small.Attributes[0].Value.Contains("msgpost-date")).
                Select(small => small).ToList();
            string innerDate = TagWithIdMessage[0].SelectSingleNode("span").InnerText;
            //получаю русские символы
            byte[] bytes = Encoding.Default.GetBytes(innerDate);
            innerDate = Encoding.UTF8.GetString(bytes);   
            //получаю год из строки и добавляю к нему "г."
            //это необходимо для приведения к региональному формату         
            string year = Regex.Match(innerDate, @"\b\d{4}\b").Value;
            innerDate = innerDate.Replace(year, year + " г.");
            //конвертирую дату в формат DateTime(для удобства использования)
            DateTime innerDateDT = DateTime.ParseExact(innerDate, "f", CultureInfo.CreateSpecificCulture("ru-RU"));
            return innerDateDT; 
        }
        #endregion
        #region Изъятие идентификатора пользователя
        public int GetUserID(HtmlNode node)
        {
            //блок <small class= "msgpost-date"> дает информацию о номере сообщения(Id)
            var TagWithIdMessage = node.SelectNodes("descendant::div").
                Where(div => div.Attributes.Count > 1 &&
                div.Attributes[0].Value.Contains("b-mtauthor")).
                Select(div => div).ToList();
            string userID = TagWithIdMessage[0].Attributes[1].Value;
            return Int32.Parse(userID);
        }
        #endregion
    }
}
using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace UserMessages.Infrastructure
{
    public class Parser
    {
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
        private void ModNode(HtmlNode nodeForMod)
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
            string[] formats = { "dd MMMM yyyy HH:mm", "d MMMM yyyy HH:mm" };
            DateTime innerDateDT = DateTime.ParseExact(innerDate, formats, CultureInfo.CreateSpecificCulture("ru-RU"), DateTimeStyles.None);
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
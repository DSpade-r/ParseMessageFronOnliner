using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UserMessages.Infrastructure.Interfaces;
using UserMessages.Models;

namespace UserMessages.Infrastructure
{
    public class Downloader : IDownloader
    {
        public HtmlDocument DounloadHtml(string urlString)
        {
            WebClient web = new WebClient();
            web.Encoding = System.Text.Encoding.UTF8;   //кодировка
            //адрес html
            string url = web.DownloadString(urlString);
            //создаю экземпляр класса
            HtmlDocument document = new HtmlDocument();
            //Загружаю в класс(парсер) наш html
            document.LoadHtml(url);
            return document;
        }        
    }
}
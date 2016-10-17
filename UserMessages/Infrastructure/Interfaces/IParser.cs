using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace UserMessages.Infrastructure.Interfaces
{
    public interface IParser
    {
        List<HtmlNode> GetPostNode(HtmlDocument document);
        string GetMessage(HtmlNode node);
        int GetIdMessage(HtmlNode node);
        DateTime GetDateMessage(HtmlNode node);
        int GetUserID(HtmlNode node);
    }
}

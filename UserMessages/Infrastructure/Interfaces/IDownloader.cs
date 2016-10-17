using HtmlAgilityPack;

namespace UserMessages.Infrastructure.Interfaces
{
    public interface IDownloader
    {
        HtmlDocument DounloadHtml(string urlString);
    }
}

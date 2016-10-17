using System.Collections.Generic;
using UserMessages.Models;

namespace UserMessages.Infrastructure.Interfaces
{
    public interface IHtmlParser
    {
        List<NodeOfParse> ParseHtmlOnliner(ParseInfo parseInfo);
    }
}

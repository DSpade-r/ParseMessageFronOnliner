using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserMessages.Models
{
    public class ParseInfo
    {
        public string Name { set; get; }
        public int UserID { set; get; }
        public int MaxMessage { set; get; }
        public int PageCount { set; get; }
    }
}
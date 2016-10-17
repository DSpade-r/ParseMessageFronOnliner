using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserMessages.Infrastructure
{
    public class NodeOfParse
    {
        public int IdMessage { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string NameUser { get; set; }
    }
}
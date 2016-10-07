using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserMessages.Infrastructure;
using UserMessages.Models;

namespace UserMessages.Controllers
{
    public class HomeController : Controller
    {
        UserMessageContext db = new UserMessageContext();
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        // GET: Home
        public ActionResult Index()
        {
            //HtmAgilityParser parser = new HtmAgilityParser();
            //ParseInfo parseInfo = new ParseInfo { UserID = 0, MaxMessage=20, Name = "DSpade", PageCount=1};
            //List<NodeOfParse> result = parser.ParseHtmlOnliner(parseInfo);
            return View(db.Users);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserMessages.Models;

namespace UserMessages.Controllers
{
    public class MessagesController : Controller
    {
        private OnlinerForum db = new OnlinerForum();

        // GET: Messages
        public ActionResult ViewBase()
        {
            var messages = db.Messages.Include(m => m.User);
            return View(messages.ToList());
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

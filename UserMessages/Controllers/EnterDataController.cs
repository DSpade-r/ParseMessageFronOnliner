using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserMessages.Infrastructure;
using UserMessages.Models;

namespace UserMessages.Controllers
{
    public class EnterDataController : Controller
    {        
        //UserMessageContext db = new UserMessageContext();
        // GET: EnterData
        [HttpGet]
        public ActionResult UIEnter()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UIEnter(ParseInfo parseInfo)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<OnlinerForum>());
            using (OnlinerForum db = new OnlinerForum())
            {
                HtmAgilityParser parser = new HtmAgilityParser();
                List<NodeOfParse> Notes = parser.ParseHtmlOnliner(parseInfo);
                List<Message> messages = new List<Message>();                       
                for (int currMsg = 0; currMsg < Notes.Count; currMsg++)
                {
                    Message message = new Message()
                    {
                        Id = Notes[currMsg].IdMessage,
                        DateTime = Notes[currMsg].Date,
                        UserId = Notes[currMsg].IdUser,
                        Text = Notes[currMsg].Message
                    };
                    //проверка на наличие в базе такого сообщения
                    if (db.Messages.Find(message.Id) == null)
                    {
                        db.Messages.Add(message);
                        messages.Add(message);
                    }                                   
                }              
                User user = new User()
                {
                    Id = Notes[0].IdUser,
                    Name = parseInfo.Name,
                    Messages = messages
                };
                //если такой пользователь есть - не добавляем
                if (db.Users.Find(user.Id) == null)
                    db.Users.Add(user);
                db.SaveChanges();
            }
            return View();
        }
    }
}
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UserMessages.DBContext;
using UserMessages.Infrastructure;
using UserMessages.Infrastructure.Interfaces;
using UserMessages.Models;

namespace UserMessages.Controllers
{
    public class EnterDataController : Controller
    {
        private IHtmlParser parser;
        public EnterDataController(IHtmlParser htmlParser)
        {
            parser = htmlParser;
        }
        [HttpGet]
        public ActionResult UIEnter()
        {  
            return View();
        }
        [HttpPost]
        public ActionResult UIEnter(ParseInfo parseInfo)
        {
            using (OnlinerForum db = new OnlinerForum())
            {
                List<NodeOfParse> Notes = parser.ParseHtmlOnliner(parseInfo);
                List<Message> messages = new List<Message>();
                User user = new User();
                //db.Users.Load();                        
                user = db.Users.FirstOrDefaultAsync(u => u.Id == parseInfo.UserID || u.Name == parseInfo.Name).Result; //нахожу есть ли пользователь в базе                
                if (Notes.Count != 0)
                {
                    for (int currMsg = 0; currMsg < Notes.Count; currMsg++)
                    {
                        Message message = new Message()
                        {
                            Id = Notes[currMsg].IdMessage,
                            DateTime = Notes[currMsg].Date,
                            UserId = Notes[currMsg].UserId,
                            Text = Notes[currMsg].Message
                        };
                        messages.Add(message);
                    }
                    if (user == null) //нет БД Users
                    {
                        user = db.Users.Create();
                        user.Messages = new List<Message>(messages);
                        user.Id = Notes[0].UserId;
                        user.Name = parseInfo.Name;
                        db.Users.Add(user);
                    }
                    else
                    {
                        if (user.Id == 0) //нет пользователя
                        {
                            user.Messages = new List<Message>(messages);
                            user.Id = Notes[0].UserId;
                            user.Name = parseInfo.Name;
                            db.Users.Add(user);
                        }
                        else
                        {
                            foreach (var message in messages)
                            {
                                if (user.Messages.All(m => m.Id != message.Id))
                                {
                                    user.Messages.Add(message);
                                }
                            }
                            db.Users.Attach(user);
                        }
                    }                    
                    db.SaveChanges();
                }
            }        
            return View();
        }
    }
}
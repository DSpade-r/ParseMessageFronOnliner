using System;

namespace UserMessages.Models
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserMessages.Models
{
    public class Message
    {
        //одному сообщении соответствует только 1 пользователь
        //не может быть ситуации, когда для 1-го сообщения есть два пользователя - следовательно для сообщения есть связь 1 к 1
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
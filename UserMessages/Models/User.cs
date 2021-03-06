﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserMessages.Models
{
    public class User
    {
        //Для таблицы пользователей организую связь одни ко многим
        //по логике 1 пользователь может иметь много сообщений
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public User()
        {
            Messages = new List<Message>();
        }
    }
}
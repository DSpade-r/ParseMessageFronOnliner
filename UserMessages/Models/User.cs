using System.Collections.Generic;

namespace UserMessages.Models
{
    public class User
    {
        //Для таблицы пользователей организую связь одни ко многим
        //по логике 1 пользователь может иметь много сообщений
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Message> Messages { get; set; }
        public User()
        {
            Messages = new List<Message>();
        }
    }
}
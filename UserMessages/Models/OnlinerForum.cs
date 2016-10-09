namespace UserMessages.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class OnlinerForum : DbContext
    {       
        public OnlinerForum()
            : base("name=OnlinerForum")
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
    }
}
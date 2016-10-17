namespace UserMessages.DBContext
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class OnlinerForum : DbContext
    {
        public OnlinerForum()
        //    : base("OnlinerForum")
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
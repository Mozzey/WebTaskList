using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTaskList.Domain.Models;
using System.Data.Entity;
using WebTaskList.Data.Maps;

namespace WebTaskList.Data
{
    public class WebTaskListContext : DbContext
    {
        public WebTaskListContext() : base("WebTaskList")
        {
            Database.SetInitializer(new WebTaskListInitializer());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserTaskMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}

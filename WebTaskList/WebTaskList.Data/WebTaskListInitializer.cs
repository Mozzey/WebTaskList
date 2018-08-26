using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WebTaskList.Domain.Models;

namespace WebTaskList.Data
{
    public class WebTaskListInitializer : DropCreateDatabaseAlways<WebTaskListContext>
    {
        protected override void Seed(WebTaskListContext context)
        {
            context.Users.Add(new User()
            {
                Id = 1,
                Email = "michael.cacciano@gmail.com",
                Password = "password1"
            });
            context.Users.Add(new User()
            {
                Id = 2,
                Email = "michaelb@gmail.com",
                Password = "password2"
            });
            context.SaveChanges();
            context.UserTasks.Add(new UserTask()
            {
                Id = 1,
                Description = "Task One",
                DueDate = DateTime.Now.AddYears(1),
                Complete = true,
                UserId = 1
            });
            context.UserTasks.Add(new UserTask()
            {
                Id = 2,
                Description = "Task Two",
                DueDate = DateTime.Now.AddYears(1),
                UserId = 1
            });
            context.UserTasks.Add(new UserTask()
            {
                Id = 3,
                Description = "Task Three",
                DueDate = DateTime.Now.AddYears(1),
                UserId = 2
            });
            context.SaveChanges();

            base.Seed(context);
        }
    }
}

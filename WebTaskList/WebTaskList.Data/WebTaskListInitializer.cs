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
            var user = new User()
            {
                Id = 1,
                Email = "michael.cacciano@gmail.com",
                Password = "password1"
            };
            context.Users.Add(user);
            context.SaveChanges();
            var task = new UserTask()
            {
                Id = 1,
                Description = "Task One",
                DueDate = DateTime.Now,
                UserId = 1
            };
            context.UserTasks.Add(task);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}

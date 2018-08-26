using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTaskList.Models
{
    public class LogInModel
    {
        // ctor
        public LogInModel() { }
        // props
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
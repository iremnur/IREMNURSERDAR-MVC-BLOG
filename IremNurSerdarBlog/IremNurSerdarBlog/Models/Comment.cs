using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IremNurSerdarBlog.Models
{
    public class Comment
    {
        public Comment()
        {
            Date = DateTime.Now;
        }
        public int ID { set; get; }

        public int PostID { set; get; }

        public System.DateTime Date { set; get; }

        public string Name { set; get; }
        public string Email { set; get; }
        public string Body { set; get; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
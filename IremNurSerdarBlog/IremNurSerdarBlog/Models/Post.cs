using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IremNurSerdarBlog.Models
{
    public class Post
    {
        public Post()
        {
            DateTime = DateTime.Now;
        }
        public int ID { get; set; }
        public string postName { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public System.DateTime DateTime { get; set; }
        public string Body { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<File> Files { set; get; }
    }
}
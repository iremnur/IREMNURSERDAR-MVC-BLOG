using IremNurSerdarBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace IremNurSerdarBlog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult About()
        {     
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Blog()
        {
            return View();
        }
        public ActionResult Posts(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var posts = from s in db.Posts select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    posts = posts.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    posts = posts.OrderBy(s => s.DateTime);
                    break;
                case "date_desc":
                    posts = posts.OrderByDescending(s => s.Body);
                    break;
                default:
                    posts = posts.OrderBy(s => s.Title);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(posts.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Comments()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Comments([Bind(Include = "ID,PostID,Date,Name,Email,Body")] Comment comments)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(comments);
                    db.SaveChanges();
                    //burası Indexti
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(comments);
        }

        public ActionResult CommentsShow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comments = db.Comments.Find(id);

            if (comments == null)
            {
                return HttpNotFound();
            }
            db.SaveChanges();
            return View(comments);
        }
        // GET: /Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post posts = db.Posts.Include(s => s.Files).SingleOrDefault(s => s.ID == id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IremNurSerdarBlog.Models;
using PagedList;
using System.Data.Entity.Infrastructure;

namespace IremNurSerdarBlog.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Posts/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
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

        // GET: /Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post posts = db.Posts.Include(s => s.Files).Include(c => c.Comments).SingleOrDefault(s => s.ID == id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // GET: /Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,DateTime,Body")] Post posts, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        var uploadedImage = new File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.uploadedImage,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            uploadedImage.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        posts.Files = new List<File> { uploadedImage };
                    }
                    db.Posts.Add(posts);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(posts);
        }

        // GET: /Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Posts posts = db.Posts.Find(id);
            Post posts = db.Posts.Include(s => s.Files).SingleOrDefault(s => s.ID == id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, HttpPostedFileBase upload)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var postToUpdate = db.Posts.Find(id);
            if (TryUpdateModel(postToUpdate, "",
                new string[] { "ID", "postName", "Title", "DateTime", "Body" }))
            {
                try
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        if (postToUpdate.Files.Any(f => f.FileType == FileType.uploadedImage))
                        {
                            db.Files.Remove(postToUpdate.Files.First(f => f.FileType == FileType.uploadedImage));
                        }
                        var avatar = new File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.uploadedImage,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            avatar.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        postToUpdate.Files = new List<File> { avatar };
                    }
                    db.Entry(postToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(postToUpdate);
        }
        // GET: Posts/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Post posts = db.Posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // POST: Posts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Post posts = db.Posts.Find(id);
                db.Posts.Remove(posts);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

            Post postToDelete = new Post() { ID = id };
            db.Entry(postToDelete).State = EntityState.Deleted;

            return RedirectToAction("Index");
        }

       
      

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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


    }
}

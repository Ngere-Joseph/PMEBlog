using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMEBlog.Data;
using PMEBlog.Models;
using System.Linq;

namespace PMEBlog.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var allPost = _context.Posts.ToList();
            return View(allPost);
        }


        //GET: AddOrEdit
        public IActionResult AddOrEdit(int? id)
        {
            ViewBag.PageName = id == null ? "Create Post":"Edit Post";
            ViewBag.IsEdit = id == null? false : true;
            
            if(id == null)
            {
                return View();
            }
            else
            {
                var post = _context.Posts.Find(id);
                if(post == null)
                {
                    return NotFound();
                }
                return View(post);
            }
        }
        //POST: AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult AddOrEdit(int id, [Bind("PostId", "Title", "Content")] Post Payload)
        {
            bool IsPostExist = false;

            Post post = _context.Posts.Find(id);

            if(post == null)
            {
                IsPostExist = true;
            }
            else
            {
                post = new Post();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    post.Title = Payload.Title;
                    post.Content = Payload.Content;

                    if (IsPostExist)
                    {
                        _context.Update(post);
                    }
                    else
                    {
                        _context.Add(post);
                    }
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index)); 

            }
            return View(post);
        }
    }
}

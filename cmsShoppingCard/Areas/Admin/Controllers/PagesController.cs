using cmsShoppingCard.Models.Data;
using cmsShoppingCard.Models.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace cmsShoppingCard.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            IEnumerable<PageViewModel> pagesList;
            using (DB context = new DB())
            {
                pagesList = context.Pages
                                   .ToArray()
                                   .OrderBy(p => p.Sorting)
                                   .Select(x => new PageViewModel(x)).ToList();

            }
            return View(pagesList);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (DB db = new DB())
            {
                PageDTO dto = new PageDTO();
                string slug;

                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                // Make sure title and slug are unique
                if (db.Pages.Any(p => p.Title == model.Title) || db.Pages.Any(p => p.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                }

                // DTO the rest
                dto.Title = model.Title;
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                db.Pages.Add(dto);
                db.SaveChanges();

                //Set tempData message
                TempData["SuccessMessage"] = "You have added a new page";
                ViewBag.FormAction = "Create";

                // Redirect
                return RedirectToAction("AddPage");
            }
        }

        // GET: Admin/Pages/EditPage
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            PageViewModel model;
            using (DB db = new DB())
            {
                // GET the Page
                var page = db.Pages.Find(id);

                // Confirm page exist
                if (page == null)
                    return Content("The Page doesn't exist.");

                // Initialize PageVM
                model = new PageViewModel(page);
                ViewBag.FormAction = "Edit";
            }
            return View(model);
        }

        // POST: Admin/Pages/EditPage
        [HttpPost]
        public ActionResult EditPage(PageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (DB db = new DB())
            {
                // Init DTOPage
                PageDTO dto = db.Pages.Find(model.Id);

                string slug;

                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                // Make sure title and slug are unique
                if (db.Pages.Where(p => p.Id != model.Id).Any(p => p.Title == model.Title) ||
                    db.Pages.Where(p => p.Id != model.Id).Any(p => p.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                }

                // DTO the rest
                dto.Title = model.Title;
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                db.SaveChanges();

                //Set tempData message
                TempData["SuccessMessage"] = "You have edited the page";
                // Redirect
                return RedirectToAction("EditPage");
            }
        }

        // POST: Admin/Pages/DeletePage
        public ActionResult DeletePage(int id)
        {
            using (DB db = new DB())
            {
                // Init DTOPage
                var page = db.Pages.Find(id);

                // Confirm page exist
                if (page == null)
                    return Content("The Page doesn't exist.");

                db.Pages.Remove(page);
                db.Entry(page).State = EntityState.Deleted;
                db.SaveChanges();
            }

            ViewBag.FormAction = "Delete";

            // Redirect
            return RedirectToAction("Index");
        }

        // GET: Admin/Pages/PageDetails/id
        [HttpGet]
        public ActionResult PageDetails(int id)
        {
            PageViewModel model;
            using (DB db = new DB())
            {
                // GET the Page
                var page = db.Pages.Find(id);

                // Confirm page exist
                if (page == null)
                    return Content("The Page doesn't exist.");

                // Initialize PageVM
                model = new PageViewModel(page);
            }
            return View(model);
        }

        // GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            SidebarViewModel model;
            using (DB db = new DB())
            {
                // GET the Page
                var sidebar = db.Sidebars.Find(1);

                // Confirm page exist
                if (sidebar == null)
                    return Content("The Sidebar doesn't exist.");

                // Initialize PageVM
                model = new SidebarViewModel(sidebar);
            }
            return View(model);
        }


        // POST: Admin/Pages/EditPage
        [HttpPost]
        public ActionResult EditSidebar(SidebarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (DB db = new DB())
            {
                // Init DTOPage
                SidebarDTO dto = db.Sidebars.Find(model.Id);

                dto.Body = model.Body;
                db.SaveChanges();

                //Set tempData message
                TempData["SuccessMessage"] = "You have edited the sidebar";
                // Redirect
                return RedirectToAction("EditSidebar");
            }
        }

        // POST: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (DB db = new DB())
            {
                // Set init count
                int count = 1; // Home is only one which isn't sortable

                // Declare PageDO
                PageDTO dto;

                // Set sorting fore each
                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }


        }

    }
}
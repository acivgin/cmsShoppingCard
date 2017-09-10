using cmsShoppingCard.Models.Data;
using cmsShoppingCard.Models.Shop.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace cmsShoppingCard.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop
        public ActionResult Categories()
        {
            List<CategoryViewModel> categoriesList;

            using (DB db = new DB())
                categoriesList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryViewModel(x)).ToList();

            return View(categoriesList);
        }

        // POST: Admin/Shop
        [HttpPost]
        public string AddNewCategory(string catName)
        {

            string id;

            using (DB db = new DB())
            {
                if (db.Categories.Any(c => c.Name == catName))
                    return "titletaken";


                var dto = new CategoryDTO();

                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-");
                dto.Sorting = 100;

                db.Categories.Add(dto);
                db.SaveChanges();

                id = dto.Id.ToString();
            }

            return id;

        }

        public ActionResult DeleteCategory(int id)
        {
            using (DB db = new DB())
            {
                // Init CategoryDTO
                var category = db.Categories.Find(id);

                // Confirm category exist
                if (category == null)
                    return Content("The Category doesn't exist.");

                db.Categories.Remove(category);
                db.Entry(category).State = EntityState.Deleted;
                db.SaveChanges();
            }

            ViewBag.FormAction = "Delete";

            // Redirect
            return RedirectToAction("Categories");
        }

        // POST: Admin/Shop/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (DB db = new DB())
            {
                // Set init count
                int count = 1; // Home is only one which isn't sortable

                // Declare PageDO
                CategoryDTO dto;

                // Set sorting fore each
                foreach (var catId in id)
                {
                    dto = db.Categories.Find(catId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }
        }

        // POST: Admin/Shop/RenameCategory
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            string Id;
            using (DB db = new DB())
            {
                var category = db.Categories.Find(id);

                if (category.Name == newCatName)
                    return "titletaken";


                category.Name = newCatName;
                category.Slug = newCatName.Replace(" ", "-").ToLower();

                db.SaveChanges();

                Id = category.Id.ToString();
            }

            return Id;
        }
    }
}
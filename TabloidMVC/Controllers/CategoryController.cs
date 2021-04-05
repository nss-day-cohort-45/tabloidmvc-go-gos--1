using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _catRepo;


        public CategoryController(ICategoryRepository categoryRepository)
        {
            _catRepo = categoryRepository;
        }

        //GET: CategoryController
        public ActionResult Index()
        {
            List<Category> categories = _catRepo.GetAll();
            return View(categories);
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            int userId = GetCurrentUserProfileId();
            Category cat = _catRepo.GetCategoryById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category cat)
        {
            try
            {
                _catRepo.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(cat);
            }
        }
    }
}

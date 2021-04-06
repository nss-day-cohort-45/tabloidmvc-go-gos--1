using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _catRepo;
        private readonly IPostRepository _postRepo;


        public CategoryController(ICategoryRepository categoryRepository, IPostRepository postRepository)
        {
            _catRepo = categoryRepository;
            _postRepo = postRepository;
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
            List<Post> posts= _postRepo.GetAllPublishedPosts();
            CategoryDeleteViewModel vm = new CategoryDeleteViewModel();
            vm.Category = _catRepo.GetCategoryById(id);
            vm.Message = null;
            foreach(Post post in posts)
            {
                if(post.CategoryId == vm.Category.Id)
                {
                    vm.Message = "you suck";
                    return View(vm);
                }
                else if(post.CategoryId != vm.Category.Id)
                {
                    vm.Post = post;
                }
            }
            if (vm.Category == null)
            {
                return NotFound();
            }
            return View(vm);
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
                
                return View(cat);
            }
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            Category cat = _catRepo.GetCategoryById(id);

            int userId = GetCurrentUserProfileId();
            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category cat)
        {
            var databaseCategory = _catRepo.GetCategoryById(id);
            try
            {
                cat.Id = id;
                _catRepo.UpdateCategory(cat);

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

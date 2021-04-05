using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
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

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
    }
}

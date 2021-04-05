﻿using Microsoft.AspNetCore.Authorization;
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
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepo;
        public TagController(ITagRepository tagRepository)
        {
            _tagRepo = tagRepository;
        }

        public ActionResult Index()
        {
            List<Tag> tags = _tagRepo.GetAllTags();

            return View(tags);
        }

        //Get: Create Tag
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //Post: Create Tag
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(Tag tag)
        {
            try
            {
                _tagRepo.AddTag(tag);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }

    }
}

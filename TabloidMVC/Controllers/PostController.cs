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
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepo;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepo = tagRepository;
        }
        
        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult MyPosts(int userProfileId)
        {
            int userId = GetCurrentUserProfileId();
            List<Post> posts = _postRepository.GetPostsByUserId(userId);
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            var vm = new PostCreateViewModel();
            vm.PostTags = _tagRepo.GetTagzByPostId(id);

            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }

            }
            vm.Post = post;
            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
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
            Post post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            else if (userId != post.UserProfileId)
            {
                return Unauthorized();
            }
            return View(post);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
           
                Post post = _postRepository.GetPublishedPostById(id);
                List<Category> CategoryOptions = _categoryRepository.GetAll();
                List<Tag> TagOptions = _tagRepo.GetAllTags();

                PostCreateViewModel vm = new PostCreateViewModel()
                {
                    Post = post,
                    CategoryOptions = CategoryOptions,
                    TagOptions = TagOptions,
                };
                try { 
                int userId = GetCurrentUserProfileId();
                    if (post == null)
                    {
                        return NotFound();
                    }
                    else if (userId != post.UserProfileId)
                    {
                        return Unauthorized();
                    }
                    return View(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            var databasePost = _postRepository.GetPublishedPostById(id);
            List<Category> CategoryOptions = _categoryRepository.GetAll();
            List<Tag> SelectedTags = new List<Tag>();
            var ChosenTags = _tagRepo.GetTagById(id);
            SelectedTags.Add(ChosenTags);
            PostCreateViewModel vm = new PostCreateViewModel()
            {
                Post = databasePost,
                CategoryOptions = CategoryOptions,
                SelectedTags = SelectedTags
            };
            try
            {
                post.Id = id;
                _postRepository.UpdatePost(post);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(vm);
            }
        }

    }
}

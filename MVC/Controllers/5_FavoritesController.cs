﻿using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize] // only authenticated users can perform favorites operations
    public class FavoritesController : Controller
    {
        // HttpService injection:
        private readonly HttpServiceBase _httpService;

        // ResourceService injection: for getting the resource title and score by resource id from the database
        private readonly IService<Resource, ResourceModel> _resourceService;

        const string SESSIONKEY = "favorites"; // fields or variables declared with const (constant) can never be assigned again

        public FavoritesController(HttpServiceBase httpService, IService<Resource, ResourceModel> resourceService)
        {
            _httpService = httpService;
            _resourceService = resourceService;
        }



        public IActionResult Get() // returns the favorites list from session to the List view
        {
            List<FavoritesModel> favorites = _httpService.GetSession<List<FavoritesModel>>(SESSIONKEY);
            if (favorites is null || favorites.Count == 0)
                TempData["Message"] = "No favorites found.";
            return View("List", favorites?.OrderBy(f => f.Title).ToList());
        }

        public IActionResult Remove(int resourceId) // removes the favorite by resourceId and userId from session
        {
            List<FavoritesModel> favorites = _httpService.GetSession<List<FavoritesModel>>(SESSIONKEY);
            if (favorites is not null && favorites.Count > 0)
            {
                int userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == "Id").Value);
                favorites.RemoveAll(f => f.ResourceId == resourceId && f.UserId == userId);
                _httpService.SetSession(SESSIONKEY, favorites);
            }
            return RedirectToAction(nameof(Get));
        }

        public IActionResult Clear() // clears all of the favorites from session by userId
        {
            List<FavoritesModel> favorites = _httpService.GetSession<List<FavoritesModel>>(SESSIONKEY);
            if (favorites is not null && favorites.Count > 0)
            {
                int userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == "Id").Value);
                favorites.RemoveAll(f => f.UserId == userId);
                _httpService.SetSession(SESSIONKEY, favorites);
            }
            return RedirectToAction(nameof(Get));
        }

        public IActionResult Add(int resourceId) // adds the favorite to session by resourceId
        {
            // If there is no favorites data in session which means that if favorites data is null,
            // initialize an empty list and assign it to the favorites variable.
            // Otherwise assign session data to the favorites variable.
            List<FavoritesModel> favorites = _httpService.GetSession<List<FavoritesModel>>(SESSIONKEY) ?? new List<FavoritesModel>();
            ResourceModel resource = _resourceService.Query().SingleOrDefault(r => r.Record.Id == resourceId);
            if (resource is null)
            {
                TempData["Message"] = "Resource not found!";
            }
            else if (!favorites.Any(f => f.ResourceId == resourceId)) // if no favorite with the same resource id exists
            {
                FavoritesModel favorite = new FavoritesModel() // initializing the favorite to add to favorites list
                {
                    ResourceId = resource.Record.Id,
                    UserId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == "Id").Value), // from authentication cookie
                    Title = resource.Record.Title,
                    Score = resource.Record.Score
                };
                favorites.Add(favorite);
                _httpService.SetSession(SESSIONKEY, favorites);
                TempData["Message"] = $"\"{favorite.Title}\" successfully added to favorites.";
            }
            return RedirectToAction("Index", "Resources"); // redirecting application user to the Index action
                                                           // of the Resources controller therefore application user
                                                           // can continue adding favorites
        }
    }
}
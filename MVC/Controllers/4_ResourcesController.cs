#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BLL.Controllers.Bases;
using BLL.Services;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// Generated from Custom Template.

namespace MVC.Controllers
{
    [Authorize] // only logged in application users with authentication cookie can call the controller's actions (application users with Admin and User roles)
    public class ResourcesController : MvcController
    {
        // Service injections:
        private readonly IResourceService _resourceService;

        /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
        private readonly IUserService _userService; // for the injection of user service for retrieving user data to be used in create and edit actions

        public ResourcesController(
			IResourceService resourceService

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            , IUserService userService
        )
        {
            _resourceService = resourceService;

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            _userService = userService;
        }

        // GET: Resources
        [AllowAnonymous] // if Authorize is used for the controller but an action is wanted to be executed by all application users
                         // even without authentication cookie AllowAnonymous is used,
                         // AllowAnonymous overrides the controller's Authorize attribute 
        public IActionResult Index()
        {
            // Get collection service logic:
            var list = _resourceService.Query().ToList();
            return View(list);
        }

        // GET: Resources/Details/5
        public IActionResult Details(int id) // both Admin and User roles can execute this action
        {
            // Get item service logic:
            var item = _resourceService.Query().SingleOrDefault(q => q.Record.Id == id);

            // Optionally HTTP 404 Not Found Status Code or a shared view for displaying the error can be returned if item is null:
            if (item is null)
                // Way 1:
                //return NotFound();
                // Way 2:
                return View("_Error", "Resource not found!"); // _Error view can be found under Views/Shared folder.
                                                              // Since we want to return the _Error view in any controller's any action, we created it under the Shared folder.
                                                              // The second message parameter is optional and if not sent (return View("_Error");), 
                                                              // "An error occurred while processing your request!" message within the view will be displayed.

            return View(item);
        }

        protected void SetViewData()
        {
            // Related items service logic to set ViewData (Record.Id and Name parameters may need to be changed in the SelectList constructor according to the model):

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            ViewBag.UserIds = new MultiSelectList(_userService.Query().ToList(), "Record.Id", "UserName"); // will be used in the list boxes in the Create and Edit views
        }

        // GET: Resources/Create
        public IActionResult Create() // both Admin and User roles can execute this action
        {
            SetViewData();
            return View();
        }

        // POST: Resources/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ResourceModel resource) // both Admin and User roles can execute this action
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("User")) // for User role, we initialize the UserIds with the authenticated user's Id for assigning resource to the user who creates the resource
                {
                    int userId; // used for allowing application users with role "User" to only create their own resources, not other users' resources

                    // retrieving the user Id from user claims for type Sid and converting its string value to integer, then assigning its value to the userId variable
                    userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);

                    resource.UserIds = new List<int>() { userId }; // initializing the UserIds of the resource parameter with the retrieved userId

                    resource.Record.Date = DateTime.Today; // for User role, the application user can't select the date so we assign it to the value when this operation is performed,
                                                           // DateTime.Today returns only the date part, however DateTime.Now returns the date and time parts
                }

                // Insert item service logic:
                var result = _resourceService.Create(resource.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = resource.Record.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            SetViewData();
            return View(resource);
        }

        // GET: Resources/Edit/5
        public IActionResult Edit(int id) // both Admin and User roles can execute this action
        {
            // Get item to edit service logic:
            var item = _resourceService.Query().SingleOrDefault(q => q.Record.Id == id);

            // returning the _Error view if item is null:
            if (item is null)
                return View("_Error", "Resource not found!");

            // for User role checking whether the resource to be edited belongs to the user who created the resource, or not:

            // retrieving the user Id from user claims for type Sid and converting its string value to integer, then assigning its value to the userId variable
            var userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            if (User.IsInRole("User") && !item.UserIds.Contains(userId)) // Contains method of collections returns true if parameter exists in the collection, otherwise false
                return View("_Error", "Edit operation is not allowed since you don't own the resource!");

            SetViewData();
            return View(item);
        }

        // POST: Resources/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(ResourceModel resource) // both Admin and User roles can execute this action
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("User")) // for User role, we initialize the UserIds with the authenticated user's Id for assigning resource to the user who edits the resource
                {
                    // retrieving the user Id from user claims for type Sid and converting its string value to integer, then assigning its value to the userId variable
                    var userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);

                    resource.UserIds = new List<int>() { userId }; // initializing the UserIds of the resource parameter with the retrieved userId

                    resource.Record.Date = DateTime.Today; // for User role, the application user can't select the date so we assign it to the value when this operation is performed,
                                                           // DateTime.Today returns only the date part, however DateTime.Now returns the date and time parts
                }

                // Update item service logic:
                var result = _resourceService.Update(resource.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = resource.Record.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            SetViewData();
            return View(resource);
        }

        // GET: Resources/Delete/5
        public IActionResult Delete(int id) // both Admin and User roles can execute this action
        {
            // Get item to delete service logic:
            var item = _resourceService.Query().SingleOrDefault(q => q.Record.Id == id);

            // returning the _Error view if item is null:
            if (item is null)
                return View("_Error", "Resource not found!");

            // for User role checking whether the resource to be deleted belongs to the user who created the resource, or not:

            // retrieving the user Id from user claims for type Sid and converting its string value to integer, then assigning its value to the userId variable
            var userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            if (User.IsInRole("User") && !item.UserIds.Contains(userId)) // Contains method of collections returns true if parameter exists in the collection, otherwise false
                return View("_Error", "Delete operation is not allowed since you don't own the resource!");

            return View(item);
        }

        // POST: Resources/Delete
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id) // both Admin and User roles can execute this action
        {
            // Delete item service logic:
            var result = _resourceService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
	}
}

#nullable disable
using BLL.Controllers.Bases;
using BLL.DAL;
using BLL.Models;
using BLL.Services;
using BLL.Services.Bases;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

// Generated from Custom Template.

namespace MVC.Controllers
{
    // Way 1: application users with Admin or User roles can execute all of the actions in the controller
    //[Authorize(Roles = "Admin,User")]
    // Way 2: because we have only 2 roles, usage of the Authorize attribute without the Roles parameter is better
    [Authorize]
    public class UsersController : MvcController
    {
        // Service injections:
        private readonly IService<User, UserModel> _userService;
        private readonly IService<Role, RoleModel> _roleService; // for the injection of role service for retrieving role data to be used in create and edit actions

        /* Can be uncommented and used for many to many relationships. {Entity} may be replaced with the related entiy name in the controller and views. */
        //private readonly IService<{Entity}, {Entity}Model> _{Entity}Service;

        public UsersController(
            IService<User, UserModel> userService
            , IService<Role, RoleModel> roleService

            /* Can be uncommented and used for many to many relationships. {Entity} may be replaced with the related entiy name in the controller and views. */
            //, IService<{Entity}, {Entity}Model> {Entity}Service
        )
        {
            _userService = userService;
            _roleService = roleService;

            /* Can be uncommented and used for many to many relationships. {Entity} may be replaced with the related entiy name in the controller and views. */
            //_{Entity}Service = {Entity}Service;
        }

        // GET: Users
        public IActionResult Index() // only authenticated users with roles "Admin" or "User" can execute this action
        {
            // Get collection service logic:
            var list = _userService.Query().ToList();
            return View(list);
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin")] // action's Authorize attribute overrides the controller's Authorize attribute
        public IActionResult Details(int id) // only authenticated users with role "Admin" can execute this action
        {
            // Get item service logic:
            var item = _userService.Query().SingleOrDefault(q => q.Record.Id == id);
            return View(item);
        }

        protected void SetViewData()
        {
            // Related items service logic to set ViewData (Record.Id and Name parameters may need to be changed in the SelectList constructor according to the model):

            // We fill the role ids as the value and role names as the text part in a SelectList therefore the user can
            // select the role from a drop down list (select HTML tag) in the view.
            // SelectList is used for drop down list which user can perform a single selection,
            // MultiSelectList is used for list box (select HTML tag with multiple attribute) which user can perform multiple selections.
            // Drop down list value that the user selects is assigned to an int property of the model (Record.RoleId), 
            // List box values that the user selects are assigned to a List<int> property of the model.
            // Way 1:
            //ViewData["RoleId"] = new SelectList(_roleService.Query().ToList(), "Record.Id", "Name"); 
            // Way 2: ViewData and ViewBag refers to the same collection
            ViewBag.RoleId = new SelectList(_roleService.Query().ToList(), "Record.Id", "Name"); // creation of a SelectList object with parameters in order as role list,
                                                                                                 // value member of each element to be used in the background through
                                                                                                 // related model property name (Record.Id) and display member of each element
                                                                                                 // to be shown to the user through related model property name (Name) and
                                                                                                 // assignment to the ViewData collection through the RoleId key

            /* Can be uncommented and used for many to many relationships. {Entity} may be replaced with the related entiy name in the controller and views. */
            //ViewBag.{Entity}Ids = new MultiSelectList(_{Entity}Service.Query().ToList(), "Record.Id", "Name");
        }

        // GET: Users/Create
        [Authorize(Roles = "Admin")] // action's Authorize attribute overrides the controller's Authorize attribute
        public IActionResult Create() // only authenticated users with role "Admin" can execute this action
        {
            SetViewData();

            // Optionally a model with initial property data can be instantiated and sent to the view:
            var user = new UserModel()
            {
                Record = new User()
                {
                    IsActive = true,
                    Status = (int)Statuses.Junior, // casting to int to assign the value (3) of the element in the enum
                    RoleId = (int)Roles.User // will assign the value 2
                }
            };
            return View(user);
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // action's Authorize attribute overrides the controller's Authorize attribute
        public IActionResult Create(UserModel user) // only authenticated users with role "Admin" can execute this action
        {
            if (ModelState.IsValid)
            {
                // Insert item service logic:
                var result = _userService.Create(user.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = user.Record.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            SetViewData();
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")] // action's Authorize attribute overrides the controller's Authorize attribute
        public IActionResult Edit(int id) // only authenticated users with role "Admin" can execute this action
        {
            // Get item to edit service logic:

            var item = _userService.Query().SingleOrDefault(q => q.Record.Id == id);

            SetViewData();
            return View(item);
        }

        // POST: Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // action's Authorize attribute overrides the controller's Authorize attribute
        public IActionResult Edit(UserModel user) // only authenticated users with role "Admin" can execute this action
        {
            if (ModelState.IsValid)
            {
                // Update item service logic:
                var result = _userService.Update(user.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = user.Record.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            SetViewData();
            return View(user);
        }

        // GET: Users/Delete/5
        public IActionResult Delete(int id) // only authenticated users with roles "Admin" or "User" can execute this action
        {
            // Checking if the application user's role is User and id parameter value is the application user's Id value for allowing users to delete their own user data:
            // retrieving the user Id from user claims for type Id and converting its string value to integer, then assigning its value to the userId variable
            var userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == "Id").Value);

            if (User.IsInRole("User") && userId != id)
                return View("_Error", "You don't have permission to delete other users!"); // _Error view can be found under Views/Shared folder.
                                                                                           // Since we want to return the _Error view in any controller's any action,
                                                                                           // we created it under the Shared folder.
                                                                                           // The second message parameter is optional and if not sent (return View("_Error");), 
                                                                                           // "An error occurred while processing your request!" message within the view
                                                                                           // will be displayed.

            // Get item to delete service logic:
            var item = _userService.Query().SingleOrDefault(q => q.Record.Id == id);

            return View(item);
        }

        // POST: Users/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id) // only authenticated users with roles "Admin" or "User" can execute this action
        {
            // Delete item service logic:
            var result = _userService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }



        // Region can be used to group relevant code.
        #region Account Actions
        [AllowAnonymous]
        public IActionResult Login() // anyone can execute this action
        {
            return View(); // returning the Login view for the application user to enter login data in the form
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserModel user) // anyone can execute this action
        {
            ModelState.Remove("Record.RoleId"); // a validation check can be removed from the ModelState, we only get the user name and password with
                                                // the user parameter's Record instance, however the validation also occurs for the Record instance's
                                                // RoleId property value which we don't need to validate

            if (ModelState.IsValid) // if model data including user name and password has no validation errors
            {
                // get the user with model data from the database
                var existingUser = _userService.Query().SingleOrDefault(u => u.Record.UserName == user.Record.UserName && 
                    u.Record.Password == user.Record.Password && u.Record.IsActive);

                if (existingUser is not null) // if user exists in the database
                {
                    // Creating the claim list that will be hashed in the authentication cookie which will be sent with each request to the web application.
                    // Only non-critical user data, which will be generally used in the web application such as user name to show in the views or user role
                    // to check if the user is authorized to perform specific actions, should be put in the claim list.
                    // Critical data such as password must never be put in the claim list!
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim("Id", existingUser.Record.Id.ToString()), // existingUser contains the data within the database of the user
                        new Claim(ClaimTypes.Name, existingUser.UserName),
                        new Claim(ClaimTypes.Role, existingUser.Role)
                        // Id claim type key is used for storing the user Id value, Name claim type key is used for storing the user name value
                        // and Role claim type key is used for storing the role name value within the items of the Claims collection
                    };

                    // creating an identity by the claim list and default cookie authentication
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // creating a principal by the identity
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    // signing the user in to the MVC web application and returning the hashed authentication cookie to the client
                    await HttpContext.SignInAsync(principal);
                    // Methods ending with "Async" should be used with the "await" (asynchronous wait) operator therefore
                    // the execution of the task run by the asynchronous method can be waited to complete and the
                    // result of the method can be used. If the "await" operator is used in a method, the method definition
                    // must be changed by adding "async" keyword before the return type and the return type must be written 
                    // in "Task". If the method is void, only "Task" should be written.

                    // redirecting user to the home page which has the controller action route /Home/Index
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid user name or password!"); // if user doesn't exist in the database, send the result's invalid message to the view's validation summary
            }
            return View(); // return the Login view with a null model for validation errors or unsuccessful login operation
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout() // anyone can execute this action
        {
            // signing out the user by removing the authentication cookie from the client
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // redirecting user to the home page which has the controller action route /Home/Index
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Register() // anyone can execute this action
        {
            return View(); // returning the Register view for the application user to enter registration data in the form
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Register(UserModel user) // anyone can execute this action
        {
            ModelState.Remove("Record.RoleId"); // a validation check can be removed from the ModelState, we only get the user name and password with
                                                // the user parameter's Record instance, however the validation also occurs for the Record instance's
                                                // RoleId property value which we don't need to validate

            if (ModelState.IsValid) // if model data including user name and password has no validation errors
            {
                // since Register method is not defined in IService and exists only in UserService, we can only invoke it
                // by casting _userService field to the type UserService and assign it to userService variable
                var userService = _userService as UserService;
                var result = userService.Register(user.Record); // insert the registered user data to the database
                if (result.IsSuccessful) // if insert operation is successful
                    return RedirectToAction(nameof(Login)); // redirect to the login action
                ModelState.AddModelError("", result.Message); // add register operation result's message to the ModelState to show in the validation summary of the view
            }
            return View();// return the Register view with a null model for validation errors or unsuccessful register operation
        }
        #endregion
    }
}

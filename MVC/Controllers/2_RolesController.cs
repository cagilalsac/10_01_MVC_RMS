#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BLL.Controllers.Bases;
using BLL.Services;
using BLL.Models;

// Generated from Custom Template.

namespace MVC.Controllers
{
    public class RolesController : MvcController // instead of inheriting from the Controller class, we inherit from the MvcController class to set the culture
    {
        // Service injections:
        private readonly IRoleService _roleService; // for role service Constructor Injection

        /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
        //private readonly IManyToManyRecordService _ManyToManyRecordService;

        // Object of type RoleService which is implemented from the IRoleService interface is injected to this class through the constructor,
        // therefore role CRUD and other operations can be performed with this service object in the actions.
        public RolesController(
			IRoleService roleService

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            //, IManyToManyRecordService ManyToManyRecordService
        )
        {
            _roleService = roleService;

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            //_ManyToManyRecordService = ManyToManyRecordService;
        }

        // GET: Roles
        public IActionResult Index()
        {
            // Get collection service logic:
            var list = _roleService.Query().ToList(); // The role query is executed in the database and the result is stored
                                                      // in the collection of type List<RoleModel> when ToList method is called.
                                                      // ToList method can be used with any collection type to get a list such as
                                                      // IQueryable, DbSet, IEnumerable, ICollection, IList, List and arrays.

            return View(list); // model of type List<RoleModel> will be passed to the Index view under Views/Roles folder
        }

        // GET: Roles/Details/5
        public IActionResult Details(int id)
        {
            // Get item service logic:
            var item = _roleService.Query().SingleOrDefault(q => q.Record.Id == id); // gets the record of type RoleModel by id from the Roles table

            return View(item); // sends item (model) of type RoleModel to the Views/Roles/Details.cshtml view
        }

        // The method for setting ViewData (ViewBag) to be sent to the views of the actions that return ViewResult (no redirection).
        // ViewData collection is used to send extra data other than model data to the views.
        // ViewData and ViewBag refers to the same collection and can be used interchangeably.
        // We don't have any extra data other than RoleModel data to send the views in the actions, therefore we don't need to implement any code.
        protected void SetViewData()
        {
            // Related items service logic to set ViewData (Record.Id and Name parameters may need to be changed in the SelectList constructor according to the model):
            
            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            //ViewBag.ManyToManyRecordIds = new MultiSelectList(_ManyToManyRecordService.Query().ToList(), "Record.Id", "Name");
        }

        // GET: Roles/Create
        //[HttpGet] // action method which is get by default when not written, so we don't need to write this
        public IActionResult Create()
        {
            SetViewData();

            return View(); // returning Create view under Views/Roles folder which contains a form, so that user can enter role data and submit it
        }

        // POST: Roles/Create
        [HttpPost] // action method which is used for processing request data sent by a form or AJAX
        [ValidateAntiForgeryToken] // attribute for preventing Cross-Site Request Forgery (CSRF)
        public IActionResult Create(RoleModel role) // the role data of type RoleModel that user enters in the Create view's form is submitted to this action
        {
            if (ModelState.IsValid) // validates the role action parameter (role model) through data annotations of its properties
            {
                // Insert item service logic: If role model data (Record) is valid, insert role service logic should be written here.
                var result = _roleService.Create(role.Record); // We assign the result of the insert (create) operation to the result variable of type Service.

                if (result.IsSuccessful) // If insert operation is successful, assign the result message to the TempData collection with key "Message"
                                         // and redirect user to the Details action by sending the inserted role's Record.Id as parameter as the route value.
                                         // Route values are always used as anonymous types.
                                         // Anonymous types are the types for objects without any class implementations.
                                         // Since there is a redirection here, we can't use ViewData (ViewBag) for carrying extra data to the view.
                {
                    TempData["Message"] = result.Message; // Will be shown at the redirected view.

                    // Way 1: We can also redirect to the Index action to get to role list with the inserted role.
                    //return RedirectToAction("Index");
                    // Way 2: Showing user the details of the inserted role is a better approach.
                    //return RedirectToAction("Details", "Roles", new { id = role.Record.Id });
                    // Way 3: We don't need to send the controller name as second parameter, since we are redirecting within the RolesController.
                    return RedirectToAction(nameof(Details), new { id = role.Record.Id }); // nameof returns the name of the method, property, class, etc. as a string.
                }
                ModelState.AddModelError("", result.Message); // Error message to be shown in the validation summary of the view,
                                                              // this is a better approach instead of sending the error message with ViewBag (ViewData)
                                                              // to the view because we will need to implement the display of ViewBag in the view.
            }
            SetViewData();

            return View(role); // Returning the role model containing the data entered by the user to the Create.cshtml view under Views/Roles folder
                               // therefore the user can correct validation errors without losing data of the form input elements.
        }

        // GET: Roles/Edit/5
        public IActionResult Edit(int id)
        {
            // Get item to edit service logic:
            var item = _roleService.Query().SingleOrDefault(q => q.Record.Id == id); // getting the role model for editing from the service

            SetViewData();

            return View(item); // returning the role model to the view so that user can see the model data for editing
        }

        // POST: Roles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoleModel role) // the role data that user enters in the Edit view's form is submitted to this action
        {
            if (ModelState.IsValid) // if there are no validation errors through data annotations of the role model
            {
                // Update item service logic:
                var result = _roleService.Update(role.Record); // update the role (Record) in the service
                if (result.IsSuccessful) // if operation is successful, set operation result's message to TempData and
                                         // redirect the user to the Details action with id assigned to Record.Id as route value
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = role.Record.Id });
                }
                ModelState.AddModelError("", result.Message); // if operation is unsuccessful, carry error result message to the view's validation summary
            }
            SetViewData();
            return View(role); // returning the role model sent by the user to the view so the user can correct the validation errors and try again
        }

        // GET: Roles/Delete/5
        public IActionResult Delete(int id)
        {
            // Get item to delete service logic:
            var item = _roleService.Query().SingleOrDefault(q => q.Record.Id == id); // getting the role model to delete from the service
            return View(item); // returning the role model to the view so that the user can see the details of the role
        }

        // POST: Roles/Delete
        [HttpPost, ActionName("Delete")] // ActionName attribute (Delete) renames and overrides the action method name (DeleteConfirmed) for the route
                                         // so that it can be requested as not Roles/DeleteConfirmed but as Roles/Delete.
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete item service logic:
            var result = _roleService.Delete(id); // deleting the role in the service by id
            TempData["Message"] = result.Message; // setting TempData as the operation result
            return RedirectToAction(nameof(Index)); // redirection to the Index action
        }
	}
}

using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;

namespace MVC.Controllers
{
    // The name of the file is given "1_HomeController.cs" for following the implementation order of the controllers,
    // which are the starting point for executing the requests in actions, within the project.
    // Normally we give the file name "HomeController".
    // Related services and models used in the controller can be easily browsed from here by hitting the F12 key after clicking on them.
    public class HomeController : Controller
    {
        // _logger instance Constructor Injection for logging operations if needed in actions:
        private readonly ILogger<HomeController> _logger;

        // Object that implements the ILogger interface is injected to this controller class
        // through the constructor therefore logging operations can be performed in actions.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        /*
        IActionResult
        |
        (implements IActionResult) ActionResult 
        |
        (inherits from ActionResult) ViewResult - ContentResult - RedirectResults - HttpStatusCodeResults - JsonResult
        */
        // Way 1:
        //public ViewResult Index()
        // Way 2:
        //public ActionResult Index()
        // Way 3:
        public IActionResult Index() // calling this controller's action: https://localhost/Home/Index or https://localhost/Home or https://localhost
                                     // as configured in Program.cs file's MapControllerRoute method
        {
            // _logger instance can be used to log to the Kestrel Console or Output Window of Visual Studio when IIS (Internet Information Services) Express is used,
            // we will use the Kestrel Console
            _logger.LogDebug("Home controller's Index action invoked."); // LogTrace, LogDebug, LogInformation, LogWarning, LogError and LogCritical methods can also be used

            // Way 1:
            //return new ViewResult();
            // Way 2:
            return View(); // returns an object of type ViewResult as ~/Views/Home/Index.cshtml,
                           // under the Views folder of the project Home folder's name is the controller name and Index file's name is the action name,
                           // this view can be returned from only this controller since it is placed under the Home folder of the Views folder
        }

        public IActionResult Privacy() // calling this controller's action: https://localhost/Home/Privacy
        {
            // Way 1:
            //return View("Privacy"); // if needed and view name to be returned is different than the action name, it can be provided as parameter
            // Way 2:
            return View(); // no need to write the view name since it will automatically return the "Privacy" view which is ~/Views/Home/Privacy.cshtml,
                           // this view can be returned from only this controller
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // response caching management
        public IActionResult Error() // calling this controller's action: https://localhost/Home/Error
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            // returns model of type ErrorViewModel that has the RequestId data to the view ~/Views/Shared/Error.cshtml,
            // shared views can be returned from any controller's any action,
            // Current?: property value can be null and if it is null, assign null to RequestId otherwise assign Current's Id value,
            // ??: if Activity.Current?.Id is null, assign HttpContext.TraceIdentifier value to RequestId otherwise assign Activity.Current?.Id value
        }
    }
}

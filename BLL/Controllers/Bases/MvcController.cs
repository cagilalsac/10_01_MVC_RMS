using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BLL.Controllers.Bases
{
    public abstract class MvcController : Controller // all controllers for CRUD will inherit from this abstract base class,
                                                     // this controller inherits from Controller class of the MVC library which is
                                                     // in the Microsoft.AspNetCore.Mvc.ViewFeatures NuGet Package,
                                                     // this controller's first purpose is managing the culture of the application
    {
        protected MvcController()
        {
            CultureInfo culture = new CultureInfo("en-US"); // for Turkish, "tr-TR" constructor parameter must be used,
                                                            // this will be our default culture for our MVC Web Application
                                                            // and can be changed by using session or a cookie if needed
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}

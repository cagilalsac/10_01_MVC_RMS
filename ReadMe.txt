Visual Studio Code Extensions, .NET 8 SDK and MySQL:
Here's the list of extensions required for Visual Studio Code:
1) C#
2) C# Dev Kit
3) NuGet Package Manager GUI

You also need to install .NET 8 SDK which can be downloaded from the link below:
https://dotnet.microsoft.com/en-us/download/dotnet/8.0

If you don't have MySQL installed, you can download and install the WampServer from the link below:
Please select MySQL version starting with 5 during the installation.
https://www.wampserver.com/en/#wampserver-64-bits-php-5-6-25-php-7



Preparation:
I) MVC (ASP.NET Core Web App Model-View-Controller) by giving the solution name RMS and project name MVC, if Visual Studio is used "Place solution and project in the same directory"
option should not be checked, and project creation should be completed by selecting ".NET 8", "None" Authentication type, checking "Configure for HTTPS", 
not checking "Enable container support", not checking "Do not use top-level statements" and not checking "Enlist in .NET Aspire orchestration".
II) Create the project BLL (Business Logic Layer) as a Class Library by right clicking Add -> New Project on the Solution. 
III) Build the solution, then add the BLL Project's reference to the MVC Project by right clicking on the MVC Project in the Solution Explorer 
and selecting Add -> Project Reference. Check the BLL Project in the opened window.
IV) For Database-First Development, click Extensions -> Manage Extensions on the Visual Studio menu then search for "ef core power tools", install it 
and close Visual Studio, click Modify while closing. Then you can launch Visual Studio again. 



Roadmap:
1) For Database-First Development, right click the BLL Project in Solution Explorer, then click EF Core Power Tools -> Reverse Engineer. From the appearing window 
click Add -> Add Database Connection on the top right side, enter (localdb)\mssqllocaldb as the "Server name" and select the database of the project from 
"Select or enter a database name" drop down list. Select EF Core 8 for "Choose Your EF Core version" drop down list, do not check "Filter schemas" and click 
the OK button. Select all the tables of your project other than "__EFMigrationsHistory" if it exists and click the OK button. 
Enter the "Context name" Db and "Namespace" BLL and enter DAL for "EntityTypes path (f.ex. Models) - optional". Check "Use DataAnnotation attributes to configure the model" 
and uncheck "Install the EF Core provider package in the project" and click the OK button. If you are using Visual Studio Code, you may not be able to use this tool so
just copy the DAL folder under your BLL Project folder. You can skip the operations numbered as 2, 3, 4, 5 and 6 for Database-First Development.

2) We will use Code-First Development in this project so under the DAL folder in the BLL Project, entities are created.

3) In the BLL, Microsoft.EntityFrameworkCore.SqlServer package is downloaded through NuGet. The .NET version you are using determines which packages with that version number 
should be downloaded from NuGet. For example, if you are using .NET 8, you should look for the packages compatible with .NET 8 and install their latest versions. 
The version number typically corresponds to the major version of .NET, so in this case, you should search for packages starting with version 8.x.x.
If you are using Visual Studio instead of Visual Studio Code, right click on the BLL Project in the Solution Explorer and click Manage NuGet Packages. 
Packages can be installed after being searched in the Browse tab.

4) In the DAL folder of the BLL Project, a context class derived from the DbContext class is created including the DbSets of type entites of our project. 
Then a parameterized constructor is created which will accept an object of type DbContextOptions containing the connection string information. 
Afterward in the MVC Project, dependency injection for the context class derived from the DbContext is configured in the IoC (Inversion of Control) Container of the Program.cs file.

5) In the MVC Project, Microsoft.EntityFrameworkCore.Tools package is downloaded through NuGet. If you are using Visual Studio instead of Visual Studio Code, 
you should set the MVC project as the start up project by right clicking on the project and clicking "Set as Startup Project".

6.1) Entity Framework Code-First Development migration Visual Studio Code terminal commands:
6.1.1) First "dotnet tool install --global dotnet-ef" command should be run.
6.1.2) Then you need to change directory to the DAL folder of the BLL Project by entering "cd bll" then "cd dal".
A new database migration (version) can be added with "dotnet-ef --startup-project ../MVC/ migrations add v1" command, v1 can be any unique name which hasn't been used before.
6.1.3) Then the database migration is applied to the database by running "dotnet-ef --startup-project ../MVC/ database update" command.
6.2) Entity Framework Code-First Development migration Visual Studio Package Manager Console commands:
6.2.1) Click Tools menu item of the Visual Studio menu then NuGet Package Manager -> Package Manager Console.
6.2.2) Select the Default project where the DbContext class is created, which is BLL in our solution.
6.2.3) A new database migration (version) can be added with "add-migration v1" command, v1 can be any unique name which hasn't been used before.
6.2.4) Then the database migration is applied to the database by running "update-database" command.

7) For scaffolding:
7.1) In the MVC Project, Microsoft.VisualStudio.Web.CodeGeneration.Design package is downloaded.
7.2) The Templates folder under the MVC Project folder of the resource project should be copied under your MVC Project folder.
7.3.1) Visual Studio Code scaffolding terminal commands:
7.3.1.1) "dotnet tool install -g dotnet-aspnet-codegenerator" command should be run in the terminal.
7.3.1.2) Change directory to MVC by "cd mvc" terminal command, then for creating the Roles controller, its actions and views: 
"dotnet aspnet-codegenerator controller -name RolesController --relativeFolderPath Controllers --useDefaultLayout --dataContext Db --model Role"
command should be run in the terminal. 
7.3.2) Visual Studio scaffolding:
7.3.2.1) Right-click on the /Controllers folder within the MVC project.
7.3.2.2) Select Add and then Controller.
7.3.2.3) In the dialog that appears, select "MVC Controller with views, using Entity Framework".
7.3.2.4) Choose the Model class (always an entity class, in this case Role).
7.3.2.5) Select the DbContext class which should be a class inheriting from the DbContext (in this case Db).
7.3.2.6) If you want to generate views using the selected entity model, check the "Generate views" option.
7.3.2.7) If you want to use jQuery Validation for client-side validation in the views, check "Reference script libraries". 
If not checked, validation will be done server-side and client-side validation can be added manually for create and edit operations if desired. Do not check this option.
7.3.2.8) If you want the generated views to use a layout view defined in _ViewStart.cshtml (typically /Views/Shared/_Layout.cshtml), check "Use a layout page". 
You can leave the text box below empty because it is defined in _ViewStart.cshtml in our project.
7.3.2.9) Optionally, you can change the Controller name to specify a different name for the generated controller.
Created controller names must always end with "Controller".
7.4) Now you can see the RolesController under the Controllers folder and Roles view folder under the Views folder of the MVC project.

8.1) In the BLL Project, Services folder is created and under this folder service classes with their interfaces for CRUD and other operations are created. 
Since this is a small project, the interfaces and classes of the services will be in the same file but generally a Bases folder is created under the Services folder 
and abstract classes or interfaces are created in the Bases folder, concrete classes which inherit the abstract classes or implement the interfaces are created 
in the Services folder. Services are used for the assignment of database table raw data from entities to the models by assigning the queried entity to the 
model's entity property which we will call Record. Record and other properties in the model will be used for user interaction in the MVC project. 
Creating an abstract service base class (Service) and inheriting all of the service classes from this class with the implementation of related interfaces is a good way 
for managing the DbContext object injection and managing operation results. Also creating a generic interface as IService in the Bases folder and implementing this
interface to all of the concrete service classes prevents the code repetition, so we won't need to create an interface for every concrete service class.

The data flow from the view to the database or from database to the view can be shown as below:
View <-> Controller (Action) <-> Service (Model) <-> DbContext (Entity) <-> Database

8.2) In the MVC Project, in the Program.cs file, the created services inversion of control must be added into the IoC Container.

8.3) In the BLL Project, Models folder is created and under this folder model classes are created by firstly creating a related entity property called Record. 
Other model class properties are created from Record entity properties by using getters in order to format the entity property value or 
get entity's relational entity data when needed. Even not needed, the model class properties are created from the Record entity properties with the same name using getters.

8.4) If the view that the model will be used requires formatted or extra relational data, new properties can be added to the model class and 
their getters and setters can be written by using the Record's related properties.

8.5) Within the model classes, DisplayName attributes can be defined above the properties to be shown in the related views. DisplayName attribute
is used for setting the display name of the model property to be shown in the views or to be used in other data annotation error messages.

Here is a list of some data annotations for entities:

Key: Indicates that the property is a primary key and creates it as an automatically incremented column in the table.
Required: Indicates that the property value is required.
Column: Specifies settings related to the property's column in the database table, such as the column name, data type, and order (used for multiple keys).
DataType: Used to specify the data type of the property. For example Text, Date, Time, DateTime, Currency, EmailAddress, PhoneNumber, Password, etc.
ReadOnly: Used to make the property read-only therefore its value can't be set.
DisplayFormat: Specifies the format to be used in text data representation, often used for formatting operations for date, decimal numbers, etc.
Table: Used to change the name of the table that will be created in the database.
StringLength (Entity and Model): Used to specify the maximum character length for text properties.
MinLength: Used to specify the minimum character length for text properties.
MaxLength: Used to specify the maximum character length for text properties.
Compare: Used to compare data of the property with another property specified.
RegularExpression: A validation pattern that can be used for more detailed data validation.
Range: Used to specify a range for numerical values.
EmailAddress: Used to ensure that the property data is in e-mail format.
Phone: Used to ensure that the property data is in phone number format.
NotMapped: Used to prevent the property from being created as a column in the corresponding table in the database.
JsonIgnore: Ensures that the property is not included in the generated JSON data.

Note: Data annotations are used for simple validations of entity properties based on their values only. Other validations may need to be performed in the service methods. 
For instance, if a validation based on a table in the database is required, it should be done in the service class methods.
Data annotations of entities are also used for database table strcutures. In some cases data annotations can also be used above model class properties.

8.6) In the MVC Project, the views should be edited according to the view's model such that the model properties for user interaction should be used.
The default MVC route is: controller/action/id? (? means optional), for example a request to the Roles controller's Index action can be sent by writing 
https://localhost/Roles/Index in the address of a browser or creating a link in a view using the HTML anchor tag such that <a href="/Roles/Index">Roles</a>.
Instead of HTML, HTML Helpers or Tag Helpers should be used. For example, another request to the Roles controller's Details action can be sent by writing
https://localhost/Roles/Details/6 in the address of a browser or creating a link in a view using the HTML anchor tag such that <a href="/Roles/Details/6">Details</a>.
Some HTML Helpers Commonly Used in ASP.NET Core MVC:
Html.TextBox
Html.TextBoxFor
Html.Password
Html.PasswordFor
Html.TextArea
Html.TextAreaFor
Html.CheckBox
Html.CheckBoxFor
Html.RadioButton
Html.RadioButtonFor
Html.DropDownList
Html.DropDownListFor
Html.ListBox
Html.ListBoxFor
Html.Hidden
Html.HiddenFor
Html.Editor
Html.EditorFor
Html.Display
Html.DisplayFor
Html.DisplayNameFor
Html.Label
Html.LabelFor
Html.ActionLink
Html.BeginForm (can be generally invoked with using, or without using and Html.EndForm)

Note: HTML Helpers ending with For that use the model class property delegate as an expression parameter are mainly used in the views.
In general only DisplayNameFor and DisplayFor HTML Helpers are used in Index and Details views. Tag Helpers are used for links, forms and other model class properties for input.

8.7) By scaffolding defaults, the Create, Edit and Delete actions have two overloaded methods. For create and edit operations the first action is the get action 
which returns the view containing the HTML form to the user for editing or entering new model's Record data. If required, extra data not related to the model can be carried 
to the view by using ViewData (ViewBag). The second action for create and edit operations is the post action marked with the HttpPost action method which is used for 
sending form data to the action using the view's model as parameter. Inside the post actions, after the model data is validated by the ModelState's IsValid bool property, 
database create and update operations are performed by the create or update service methods. If there are validation errors catched by the ModelState through model's Record 
data annotations, the view is returned with the model containing user entered data and validation messages. If Client-Side Validation is needed instead of Server-Side Validation, 
in the Create or Edit views <partial name="_ValidationScriptsPartial" /> must be added in the Scripts section.
For delete operation, the get action returns the model data to the view to show the details. When the submit button within the form of the view is clicked, 
id value used in the hidden input is sent to the delete post action (DeleteConfirmed) and catched by the id action parameter, then delete operation is performed 
through the delete service method by using the id value.

8.8) The connection string of the database should be read from the appsettings.json file in the Program.cs file. Other application configuration data can also be read from
the appsettings.json file in the Program.cs file by binding the same property names to the class with the same section name, which is AppSettings, in BLL Project's Models folder.
The static properties of this class can directly be reached using the class name (AppSettings) without initialization in the views or controller actions.

8.9) Javascript and CSS libraries such as jquery-datetimepicker can be added to the project by right clicking on wwwroot -> lib, selecting Add -> Client-Side Library. 
After clicking, a search for the library can be performed and the relevant result can be selected. The correctness of the library, including its name and version, 
can be verified on the library web site before adding it to the project.

8.10) Whether using a Client-Side Library or not, Javascript codes can be written in a view's Scripts section, which is also named as Scripts and rendered at the 
bottom of _Layout.cshtml view. Therefore, the Javascript codes can use the necessary Client-Side Libraries such as jQuery since their file references are above the section
that will be rendered by the RenderSectionAsync method in the _Layout view. If the required parameter of this method is sent as true, all views must have a Scripts section. 
However, we generally don't need a Scripts section in all views therefore this parameter should be sent as false.

8.11) MVC Web Application's default culture (for example English or Turkish) configuration can be made in the base controller class under BLL Project's 
Controllers\Bases folder which is called MvcController. If implemented, we won't need to use an instance of CultureInfo anywhere else in our project for 
formatting decimal or date time values to string anymore. Moreover, application can easily gain multi-language support in the future by using 
session or a cookie carrying culture information in the MvcController's constructor.

8.12) In general, cookie authentication is used in MVC Web Applications by creating a list of user claims, which include non-critical user data such as user name 
for displaying and user role for authorization, then the data in the claim list is hashed and sent to the client as an authentication cookie.
For each request, the authentication cookie is sent to the server and the hashed user data in the cookie is used in necessarry controllers, actions and views. 
In the Program.cs file of the MVC Project, authentication configuration should be made as written in the Authentication sections.

8.13) In controllers, authentication cookie can be checked by the "Authorize" attribute. It can either be written above the controller to execute for all actions 
or above specific actions. "Authorize" attribute checks whether the authentication cookie exists or not. It can also be used for role checking such as 
"[Authorize(Roles = "admin")]" or "[Authorize(Roles = "admin,user")]" for multiple roles.

8.14) In controller actions and views, "User" object can be used to get the authenticated user information through the authentication cookie. 
"User.Identity.IsAuthenticated" checks whether the authentication cookie exists or not, "User.Identity.Name" returns the authenticated user's user name and 
"User.IsInRole(roleName)" method checks whether the authenticated user has the specified role name sent as parameter.

8.15) If you want to use "Sneat Dashboard Bootstrap 5" template, copy the "sneat" folder under the "wwwroot" folder of this MVC Project to your MVC Project's "wwwroot" folder 
and copy the "_SneatLayout.cshtml" file under "Views/Shared" folder of this MVC Project to your MVC Project's "Views/Shared" folder, 
then modify the title, links, icons, etc. in this layout view.
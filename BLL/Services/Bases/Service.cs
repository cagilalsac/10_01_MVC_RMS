using BLL.DAL;

namespace BLL.Services.Bases
{
    public abstract class Service // all CRUD (Create, Read, Update and Delete) service classes of our project will inherit from this abstract class,
                                  // therefore Db object injection can be easily done
    {
        public bool IsSuccessful { get; set; } // will be used to check if the operation is successful in the controllers
        public string Message { get; set; } = string.Empty; // will be used to set the operation result message to be displayed in the service
                                                            // and will be retrieved in the controllers to be sent to the views,
                                                            // string.Empty is same as ""

        protected virtual string OperationFailed => "Operation failed!"; // Read only property assignment for error messages,
                                                                         // for example "Operation failed! Role not found!".
                                                                         // Marked as virtual to be overriden in inherited service classes if needed.


        protected readonly Db _db; // we set protected as the accessibility modifier, so we can use this field in the inherited service classes

        protected Service(Db db) // DbContext object Constructor Injection: An object of type Db which inherits from DbContext class is
        {                        // injected to this class through the constructor therefore CRUD and other operations can be performed
                                 // with this object in the service classes.
            {
                _db = db;
            }
        }

        // method for successful operations, marked as virtual to be overriden in inherited service classes if needed
        public virtual Service Success(string message = "")
        {
            IsSuccessful = true;
            Message = message;
            return this; // returns the instance of this class so that we can use it as a service result object after invocation in controllers
        }

        // method for unsuccessful operations, marked as virtual to be overriden in inherited service classes if needed
        public virtual Service Error(string message = "")
        {
            IsSuccessful = false;
            // Way 1:
            //Message = OperationFailed + " " + message; // string concatenation
            // Way 2:
            Message = $"{OperationFailed} {message}"; // in C#, strings can be used with "$" for using properties, variables, parameters and methods inside curly braces
            return this;
        }
    }
}

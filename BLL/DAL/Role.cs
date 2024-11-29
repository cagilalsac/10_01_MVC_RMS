// Way 1: For all entities and models, #nullable disable can be added for preventing reference type property validations
// which will cause validation errors if reference types are not declared as nullable using ? such as
// public string? Name { get; set; } or public List<User>? Users { get; set; }
//#nullable disable
// Way 2: Nullable disable can also be configured for the whole projects through the project properties by right clicking
// the project then setting Nullable configuration to Disable, this must be done for both BLL and MVC Projects

using System.ComponentModel.DataAnnotations; // This using directive should also be added for using classes by only their names
                                             // such as Required and StringLength.

namespace BLL.DAL
{
    public class Role
    {
        public int Id { get; set; }

        [Required] // can't be null
        [StringLength(5)] // must have maximum 5 characters
        // Required and StringLength are called C# Attributes and may be used on top of properties,
        // fields (class variables), behaviors (methods) or classes.
        // For entities C# Attributes are called Data Annotations and they gain new features to the
        // properties, fields, behaviors or classes by the implementation of Aspect Oriented Programming.
        public string Name { get; set; } // instead of "string", "String" class type can also be used, general usage "string" data type,
                                         // value assignment is not required (can be assigned as null)
                                         // because the type of the property is "string" which is a reference type in C#,
                                         // we define that this property value is mandatory by using the Required Data Annotation

        // class has a relationship for one to many tables relationship (Users reference therefore Users table is the many side)
        // Way 1:
        //public IEnumerable<User> Users { get; set; }
        // Way 2:
        //public ICollection<User> Users { get; set; }
        // Way 3: since List class implements IEnumerable and ICollection interfaces, we can use List class as the property type
        public List<User> Users { get; set; } = new List<User>(); // called Navigation Property in Entity Framework, a role may have none, one or many users,
                                                                  // initializing to prevent Null Reference Exception (occurs when Users property assigned as null)
    }
}

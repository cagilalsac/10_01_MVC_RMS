#nullable disable

using BLL.DAL.Bases; // Since Record class is under different folder therefore different namespace (DAL.Bases),
                     // we can include the namespace with "using" (similar to import in Java),
                     // therefore we can use the classes in the namespace directly.
using System.ComponentModel.DataAnnotations; // This using directive should also be added for using classes by only their names
                                             // such as Required and StringLength.

namespace BLL.DAL
{
    // Way 1:
    //public class Role : BLL.DAL.Bases.Record
    // Way 2:
    public class Role : Record // Role is a Record relationship,
                               // no need to write the namespace of the class if using directive is added at the top.
    {
        [Required] // can't be null
        [StringLength(5)] // must have maximum 5 characters
        // Required and StringLength are called C# Attributes and may be used on top of properties,
        // fields (class variables), behaviors (methods) or classes.
        // For entities C# Attributes are called Data Annotations and they gain new features to the
        // properties, fields, behaviors or classes by the implementation of Aspect Oriented Programming.
        // Data Annotations can also be used in models when necessary.
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

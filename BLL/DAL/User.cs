#nullable disable

using BLL.DAL.Bases;
using System.ComponentModel.DataAnnotations;

namespace BLL.DAL
{
    public class User : Record
    {
        [Required]
        [StringLength(10, MinimumLength = 3)] // value must have minimum 3 maximum 10 characters,
                                              // Length, MinLength and MaxLength Data Annotations can also be used
        public string UserName { get; set; } // value assignment is not required (can be assigned as null),
                                             // value assignment is mandatory by the Required Data Annotation

        [Required]
        [StringLength(8, MinimumLength = 3)]
        public string Password { get; set; }

        public bool IsActive { get; set; } // boolean data type which can have the value of true or false,
                                           // value assignment is required
                                           // because the type of the property is "bool" which is a value type in C#,
                                           // if not assigned during object initialization, false will be assigned as default value

        public int Status { get; set; }

        // class has-a relationship for one to many tables relationship (Role reference is the one side)
        public Role Role { get; set; } // called Navigation Property in Entity Framework

        // tables one to many relationship (Roles table is the one side)
        public int RoleId { get; set; } // a user must have a role, if the type of the property was int?, it could have been assigned as null
                                        // therefore a user may have a role or not

        // class has a relationship for many to many tables relationship (UserResources reference therefore UserResources relational table is the many side)
        public List<UserResource> UserResources { get; set; } = new List<UserResource>(); // value assignment is not required, can be null
                                                                                          // because List is a class which is a reference type in C#,
                                                                                          // a user may have none, one or many user resources,
                                                                                          // we should initialize UserResources to an empty list object
                                                                                          // to prevent Null Reference Exception, which means
                                                                                          // UserResources can not be null
    }
}

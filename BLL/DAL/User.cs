#nullable disable

using BLL.DAL.Bases;
using System.ComponentModel.DataAnnotations;

namespace BLL.DAL
{
    public class User : Record
    {
        // Way 1: all data annotations can be used with or without ErrorMessage
        //[Required]
        // Way 2: overriding default validation error message
        [Required(ErrorMessage = "This field is required!")]
        [StringLength(10, ErrorMessage = "This field must be maximum {1} characters!")] // Output: "This field must be maximum 10 characters!",
                                                                                        // Length, MinLength and MaxLength Data Annotations can also be used

        public string UserName { get; set; } // value assignment is not required (can be assigned as null),
                                             // value assignment is mandatory by the Required Data Annotation

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(8, MinimumLength = 3, ErrorMessage = "This field must be minimum {2} maximum {1} characters!")] // Output: "This field must be minimum 3 maximum 8 characters!"
        public string Password { get; set; }

        public bool IsActive { get; set; } // boolean data type which can have the value of true or false,
                                           // value assignment is required
                                           // because the type of the property is "bool" which is a value type in C#,
                                           // if not assigned during object initialization, false will be assigned as default value

        public int Status { get; set; }

        // class has-a relationship for one to many tables relationship (Role reference is the one side)
        public Role Role { get; set; } // called Navigation Property in Entity Framework

        // tables one to many relationship (Roles table is the one side)
        // a user must have a role, if the type of the property was int?, it could have been assigned as null therefore a user may have a role or not
        // Way 1:
        //public int RoleId { get; set; } 
        // Way 2: if we want to show the validation error message "This field is required!" instead of "The value '' is invalid."
        [Required(ErrorMessage = "This field is required!")]
        public int? RoleId { get; set; } // since Required is used, the value is mandatory

        // class has a relationship for many to many tables relationship (UserResources reference therefore UserResources relational table is the many side)
        public List<UserResource> UserResources { get; set; } = new List<UserResource>(); // value assignment is not required, can be null
                                                                                          // because List is a class which is a reference type in C#,
                                                                                          // a user may have none, one or many user resources,
                                                                                          // we should initialize UserResources to an empty list object
                                                                                          // to prevent Null Reference Exception, which means
                                                                                          // UserResources can not be null
    }
}

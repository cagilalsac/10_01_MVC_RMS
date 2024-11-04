#nullable disable

using BLL.DAL.Bases;
using System.ComponentModel.DataAnnotations;

namespace BLL.DAL
{
    public class Resource : Record
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; } // can't be null

        // since StringLength, Length or MaxLength Data Annotations is not used,
        // will have table column of type nvarchar(MAX), MAX: 4000 unicode characters
        public string Content { get; set; } // can be null

        // Way 1:
        //[Range(1, 5, ErrorMessage = "{0} must be between {1} and {2}!")] // Output: "Score must be between 1 and 5!",
                                                                           // {0} placeholder uses the property's display name if exists, otherwise property name
        // Way 2:
        //[Range(1, 5, ErrorMessage = "This field must be between {1} and {2}!")] // Output: "This field must be between 1 and 5!"
        // Way 3: can also be validated in ResourceService class Create and Update methods
        //[Range(1, 5)]
        public decimal Score { get; set; } // can't be null,
                                           // decimal number data types: float, double and decimal,
                                           // example value assignment for decimal: = 1.2m or 1.2M, 
                                           // example value assignment for float: = 1.2f or 1.2F, 
                                           // example value assignment for double: = 1.2

        // Way 1:
        //public Nullable<DateTime> Date { get; set; }
        // Way 2:
        public DateTime? Date { get; set; } // value types ending with "?" are called nullable types in C# and can be null

        // class has a relationship for many to many tables relationship (UserResources reference therefore UserResources relational table is the many side)
        public List<UserResource> UserResources { get; set; } = new List<UserResource>(); // initializing to prevent Null Reference Exception
    }
}

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

        // decimal number data types:
        // Way 1:
        // public float Score { get; set; } // example value assignment: = 1.2f or 1.2F
        // Way 2:
        // public double Score { get; set; } // example value assignment: = 1.2;
        // Way 3:
        public decimal Score { get; set; } // example value assignment: = 1.2m or 1.2M, can't be null

        // Way 1:
        //public Nullable<DateTime> Date { get; set; }
        // Way 2:
        public DateTime? Date { get; set; } // value types ending with "?" are called nullable types in C# and can be null

        // class has a relationship for many to many tables relationship (UserResources reference therefore UserResources relational table is the many side)
        public List<UserResource> UserResources { get; set; } = new List<UserResource>(); // initializing to prevent Null Reference Exception
    }
}

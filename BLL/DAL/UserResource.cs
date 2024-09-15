#nullable disable

using BLL.DAL.Bases;

namespace BLL.DAL
{
    public class UserResource : Record // many to many relational entity
    {
        // tables many to many relationship (Users table is the one side)
        public int UserId { get; set; }

        // class has-a relationship for many to many tables relationship (User reference is the one side)
        public User User { get; set; }

        // tables many to many relationship (Resources table is the one side)
        public int ResourceId { get; set; }

        // class has-a relationship for many to many tables relationship (Resource reference is the one side)
        public Resource Resource { get; set; }
    }
}

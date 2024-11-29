using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class UserService : Service, IService<User, UserModel>
    {
        public UserService(Db db) : base(db)
        {
        }

        public IQueryable<UserModel> Query()
        {
            // Retrieving the query joined with role data, then ordering descending by User entity delegate's (u) IsActive property,
            // then ordering ascending by User entity delegate's (u) UserName property, finally projecting entity query to UserModel query
            // by setting each item's Record to the User entity delegate (u). 
            // OrderBy or OrderByDescending must be used once. For other orderings, ThenBy or ThenByDescending can be used once or more.
            return _db.Users.Include(u => u.Role).OrderByDescending(u => u.IsActive).ThenBy(u => u.UserName).Select(u => new UserModel() { Record = u });
        }

        public Service Create(User user)
        {
            // Checking if an active user with the entered user name exists:
            // Case sensitivity can be simply eliminated by using ToUpper string method, which returns all characters in upper case, for both operands.
            // Way 1:
            //if (_db.Users.Any(u => u.UserName.ToUpper() == user.UserName.ToUpper().Trim() && u.IsActive == true))
            // Way 2:
            if (_db.Users.Any(u => u.UserName.ToUpper() == user.UserName.ToUpper().Trim() && u.IsActive)) // no need to use u.IsActive == true since IsActive is bool
                // Way 1:
                //return Error("Active user with the same user name (\"" + user.UserName + "\") exists!"); // string concatenation, \": escape sequence
                // Way 2:
                return Error($"Active user with the same user name (\"{user.UserName}\") exists!"); // $ usage for strings

            // Modifying the entered user parameter (entity) property values:
            user.UserName = user.UserName.Trim();
            user.Password = user.Password.Trim();

            // Adding the user entity to the Users DbSet (no commits to the database):
            _db.Users.Add(user);

            // Commiting Users DbSet changes to the Users table in the database by Unit of Work:
            _db.SaveChanges();

            // Returning success as operation result:
            return Success($"User with \"{user.UserName}\" created successfully.");
        }

        public Service Update(User user)
        {
            // Checking if an active record with the user name other than the user parameter's user name exists in the Users table:
            if (_db.Users.Any(u => u.Id != user.Id && u.UserName.ToUpper() == user.UserName.ToUpper().Trim() && u.IsActive))
                return Error($"Active user with the same user name (\"{user.UserName}\") exists!");

            // Retrieving user entity data by id from the Users table:
            var entity = _db.Users.SingleOrDefault(u => u.Id == user.Id);

            // Optionally if user entity is not found by id, error operation result can be returned:
            // Way 1:
            //if (user == null)
            // Way 2:
            if (user is null)
                return Error("User not found!");

            // Modifying the retrieved user entity property values by the entered property values of the user parameter:
            entity.UserName = user.UserName.Trim();
            entity.Password = user.Password.Trim();
            entity.IsActive = user.IsActive;
            entity.Status = user.Status;
            entity.RoleId = user.RoleId;

            // Updating the retrieved and modified user entity in the Users DbSet (no commits to the database):
            _db.Users.Update(entity);

            // Commiting Users DbSet changes to the Users table in the database by Unit of Work:
            _db.SaveChanges();

            // Returning success as operation result:
            return Success($"User with \"{user.UserName}\" updated successfully.");
        }

        // Way 1: Deleting user from the Users table
        //public Service Delete(int id)
        //{
        //    // Retrieving user entity with relational UserResources collection data by id from the Users table:
        //    var entity = _db.Users.Include(u => u.UserResources).SingleOrDefault(u => u.Id == id);

        //    // Optionally if user entity is not found by id, error operation result can be returned:
        //    if (entity is null)
        //        return Error("User not found!");

        //    // Deleting the user entity's relational UserResources collection data from the UserResources DbSet (no commits to the database):
        //    _db.UserResources.RemoveRange(entity.UserResources); // RemoveRange method can be used to delete a collection from another collection

        //    // Deleting the user entity from the Users DbSet (no commits to the database):
        //    _db.Users.Remove(entity); // Remove method only deletes one item from a collection

        //    // Commiting Users and UserResources DbSet changes to the related tables in the database by Unit of Work:
        //    _db.SaveChanges();

        //    // Returning success as operation result:
        //    return Success($"User with \"{entity.UserName}\" deleted successfully.");
        //}
        // Way 2: Updating the user's IsActive property to false therefore the user data won't be deleted from the Users table and the user won't be able to login
        public Service Delete(int id)
        {
            // Retrieving user entity by id from the Users table:
            var entity = _db.Users.SingleOrDefault(u => u.Id == id);

            // Updating the user entity's IsActive property to false:
            entity.IsActive = false;

            // Invoking the Update method of the UserService to complete the update operation of the User entity:
            var result = Update(entity);

            if (!result.IsSuccessful) 
                return result; // returning error result of the Update service method since the Update operation is not successful

            // returning success result with the delete message since Update operation is successful
            return Success($"User with \"{entity.UserName}\" deleted successfully.");
        }



        // Region can be used to group relevant code.
        #region Account Method Implementations:
        public Service Register(User user)
        {
            // checking whether or not an active user with the same user name exists in the database
            if (_db.Users.Any(u => u.UserName.ToUpper() == user.UserName.ToUpper().Trim() && u.IsActive))
                return Error("Active user with the same user name exists!"); // returning error result

            // modifying user parameter's properties
            user.UserName = user.UserName.Trim();
            user.Password = user.Password.Trim();
            user.IsActive = true; // all registered users will be active by default
            user.Status = (int)Statuses.Junior; // all registered users will have the "Junior" status by default
            user.RoleId = (int)Roles.User; // all registered users will have the role "User" by default

            // adding the modified user to the Users DbSet
            _db.Users.Add(user);

            // commiting Users DbSet changes to the database
            _db.SaveChanges();

            // returning success result
            return Success("User registered successfully.");
        }
        #endregion
    }
}

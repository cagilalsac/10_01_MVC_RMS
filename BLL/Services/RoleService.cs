using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    /// <summary>
    /// Performs role CRUD operations.
    /// </summary>
    public interface IRoleService // summary can be used for displaying the explanation when mouse is hovered over this type (IRoleService) anywhere in the project,
                                  // summaries can be written above interfaces, classes, methods and properties by hitting "/" character on the keyboard 3 times
    {
        // method definitions: method definitions must be created here for what the class implements this interface can do and to be used in the controllers

        /// <summary>
        /// Queries and returns role data query from the Roles table.
        /// </summary>
        /// <returns>IQueryable</returns>
        public IQueryable<RoleModel> Query();

        /// <summary>
        /// Inserts the role data to the Roles table. IsSuccessful and Message properties can be used for the operation result after method invocation.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>Service</returns>
        public Service Create(Role role);

        /// <summary>
        /// Updates the role data in the Roles table by role parameter's Id value. IsSuccessful and Message properties can be used for the operation result after method invocation.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>Service</returns>
        public Service Update(Role role);

        /// <summary>
        /// Deletes the role data in the Roles table by role id parameter. IsSuccessful and Message properties can be used for the operation result after method invocation.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Service</returns>
        public Service Delete(int id);
    }

    public class RoleService : Service, IRoleService // RoleService is a Service and implements IRoleService
    {
        public RoleService(Db db) : base(db) // DbContext object base service class Constructor Injection
        {
        }



        // Method implementations of the method definitions in the interface:
        public IQueryable<RoleModel> Query() // Query method will be used for generating SQL queries without executing them.
        {
            // LINQ (Language Integrated Query) Select method maps the properties of RoleModel from the properties of r (Role) delegate,
            // this is called projection.
            // Way 1:
            //return _db.Roles.Select(r => new RoleModel() { Record = r }); // "r =>" is called Lambda Expression
            // Way 2: LINQ OrderBy method orders the records by the r (Role) delegate property (r.Name: Role Name),
            // OrderBy orders records in ascending, OrderByDescending orders records in descending order.
            // From the Roles DbSet collection, first order Role entities by Name in ascending order,
            // then for each element of Role entity collection map Role entity to the Record property (projection) of the RoleModel,
            // finally return the query.
            // Retrieving the query containing only the ordered role data:
            //return _db.Roles.OrderBy(r => r.Name).Select(r => new RoleModel() { Record = r });
            // Way 3: Retrieving the query containing the role data joined with the user data (Include method must be used):
            // An entity's Navigational Properties can be used as Lambda Expressions in the Include methods for retrieving their data with the entity data.
            // Including the relational data on demand is called Eager Loading in Entity Framework and it is used as default.
            // On the contrary, Lazy Loading in Entity Framework always includes the relational data (not recommended) and must be configured before for the usage.
            return _db.Roles.Include(r => r.Users).OrderBy(r => r.Name).Select(r => new RoleModel() { Record = r });

            // Generally used LINQ methods:
            // OrderBy, OrderByDescending, ThenBy, ThenByDescending, Where, Any, SingleOrDefault and Select.
            // LINQ methods can be used for any collection type such as DbSet, IQueryable, IEnumerable, List and arrays, even for strings.
        }

        public Service Create(Role role) // role parameter is the model that the user enters data in the view
        {
            // Role names must be unique in the Roles table, therefore we should check if the role with the same name exists:
            // LINQ SingleOrDefault method returns the single record (object) that matches the condition (r => r.Name == role.Name.Trim()),
            // if no record matching the condition is found, returns null, if more than one records matching the condition are found, throws exception.
            // Instead of SingleOrDefault, LINQ FirstOrDefault method can be used for retrieving the first record and
            // LINQ LastOrDefault method can be used for retrieving the last record,
            // FirstOrDefault and LastOrDefault don't throw an exception if more than one records matching the condition are found and
            // if no record matching the condition is found, they return null.
            // Alternatively LINQ Single, LINQ First and LINQ Last methods can be used which throw exception if no record matching the condition is found.
            // Generally, methods ending with OrDefault that return a null result when no elements are found
            // are used when dealing with a situation where no match is expected.
            // Trim string method trims the white space characters from the beginning and end of a string value.
            // LINQ Any method returns true if at least one record matches the condition (r => r.Name == role.Name.Trim()), otherwise returns false.
            // Way 1:
            //Role entity = _db.Roles.SingleOrDefault(r => r.Name == role.Name.Trim());
            //if (entity is not null)
            //    return Error("Role with the same name exists!");
            // Way 2:
            if (_db.Roles.Any(r => r.Name == role.Name.Trim()))
                return Error("Role with the same name exists!"); // will return a Service instance with the Message as the parameter and IsSuccessful false

            role.Name = role.Name.Trim(); // trimming of role name data that the user enters to be saved in the table without white space characters

            // Role entity insert operation:
            _db.Roles.Add(role); // we add the Role entity to the Roles DbSet saying Entity Framework that this Role entity will be inserted to the Roles table,
                                 // no changes to the Roles table are committed with this operation

            _db.SaveChanges(); // we invoke this method for committing all DbSet changes to the database with one operation, this is called Unit of Work

            return Success("Role created successfully."); // will return a Service instance with the Message as the parameter and IsSuccessful true
        }

        public Service Update(Role role)
        {
            // Checking if a record name other than the role paramater's name exists in the Roles table: we perform this check by adding the Id condition
            if (_db.Roles.Any(r => r.Id != role.Id && r.Name == role.Name.Trim()))
                return Error("Role with the same name exists!");

            // retrieving the Role entity from the Roles table:
            // Way 1:
            //Role entity = _db.Roles.Find(role.Id); // DbSet collections' Find method can also be used for retrieving a record by primary key values (here Id),
                                                     // if no record is found, returns null
            // Way 2: we will prefer using SingleOrDefault
            Role entity = _db.Roles.SingleOrDefault(r => r.Id == role.Id);

            // updating the Role entity property values with the role parameter property values that the user enters:
            entity.Name = role.Name.Trim();

            // Role entity update operation:
            _db.Roles.Update(entity); // we say Entity Framework that this Role entity will be updated (modified) in the Roles table,
                                      // no changes to the Roles table are committed with this operation

            _db.SaveChanges(); // we invoke this method for committing all DbSet changes to the database with one operation, this is called Unit of Work

            return Success("Role updated successfully.");
        }

        public Service Delete(int id)
        {
            // Checking if the role record to be deleted has relational users. If any, we shouldn't delete the role record:
            // Include method retrieves the relational data for entity's Navigation Property (r.Users) with the query.
            // In other words, we get the role records joined with user records.
            Role entity = _db.Roles.Include(r => r.Users).SingleOrDefault(r => r.Id == id);

            // Way 1:
            //if (entity.Users.Count > 0) // a List object's Count property returns the element count
            //    return Error("Role has relational users!");
            // Way 2:
            if (entity.Users.Any()) // if role has any relational users
                return Error("Role has relational users!");

            // Role entity delete operation:
            _db.Roles.Remove(entity); // we say Entity Framework that this Role entity will be deleted in the Roles table,
                                      // no changes to the Roles table are committed with this operation

            _db.SaveChanges(); // we invoke this method for committing all DbSet changes to the database with one operation, this is called Unit of Work

            return Success("Role deleted successfully.");
        }
    }
}

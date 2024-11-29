using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class ResourceService : Service, IService<Resource, ResourceModel>
    {
        public ResourceService(Db db) : base(db)
        {
        }

        public IQueryable<ResourceModel> Query()
        {
            // Retrieving the query joined with user resource and user data, then ordering descending by the Resource entity delegate's (r) Date property,
            // then ordering descending by the Resource entity delegate's (r) Score property, then ordering ascending by the Resource entity delegate's (r) Title property,
            // finally projecting entity query to ResourceModel query by setting each item's Record to the Resource entity delegate (r).
            // For the Resources - UserResources - Users many to many relationship after the queried Resources DbSet, Include method must be used
            // to retrieve relational UserResources data with the Resource entity delegate (r),
            // for the relational Users data ThenInclude method must be used with the UserResource entity delegate (ur).
            return _db.Resources.Include(r => r.UserResources).ThenInclude(ur => ur.User)
                .OrderByDescending(r => r.Date).ThenByDescending(r => r.Score).ThenBy(r => r.Title)
                .Select(r => new ResourceModel() { Record = r });
        }

        public Service Create(Resource resource)
        {
            if (resource.Score < 1 || resource.Score > 5)
                return Error("Score must be between 1 and 5!");

            if (!resource.UserResources.Any())
                return Error("At least one user must be selected!");

            // Modifying the entered resource parameter (entity) property values:
            resource.Title = resource.Title.Trim();
            resource.Content = resource.Content?.Trim(); // "?" must be used after the Content property since its value may be null

            // Adding the resource entity to the Resources DbSet (no commits to the database):
            _db.Resources.Add(resource);

            // Commiting Resources DbSet changes to the Resources table in the database by Unit of Work:
            _db.SaveChanges();

            // Returning success as operation result:
            return Success("Resource created successfully.");
        }

        public Service Update(Resource resource)
        {
            if (resource.Score < 1 || resource.Score > 5)
                return Error("Score must be between 1 and 5!");

            if (!resource.UserResources.Any())
                return Error("At least one user must be selected!");

            // Retrieving resource entity with relational UserResources collection data by id from the Resources table:
            var entity = _db.Resources.Include(r => r.UserResources).SingleOrDefault(r => r.Id == resource.Id);

            // Deleting the resource entity's relational UserResources collection data from the UserResources DbSet (no commits to the database): 
            _db.UserResources.RemoveRange(entity.UserResources);

            // Modifying the retrieved resource entity property values by the entered property values of the resource parameter:
            entity.Title = resource.Title.Trim();
            entity.Content = resource.Content?.Trim();
            entity.Score = resource.Score;
            entity.Date = resource.Date;
            entity.UserResources = resource.UserResources;

            // Updating the retrieved and modified resource entity in the Resources DbSet (no commits to the database):
            _db.Resources.Update(entity);

            // Commiting Resources and UserResources DbSet changes to the related tables in the database by Unit of Work:
            _db.SaveChanges();

            // Returning success as operation result:
            return Success("Resource updated successfully.");
        }

        public Service Delete(int id)
        {
            // Retrieving resource entity with relational UserResources collection data by id from the Resources table:
            var entity = _db.Resources.Include(r => r.UserResources).SingleOrDefault(r => r.Id == id);

            // Deleting the resource entity's relational UserResources collection data from the UserResources DbSet (no commits to the database):
            _db.UserResources.RemoveRange(entity.UserResources);

            // Deleting the resource entity from the Resources DbSet (no commits to the database):
            _db.Resources.Remove(entity);

            // Commiting Resources and UserResources DbSet changes to the related tables in the database by Unit of Work:
            _db.SaveChanges();

            // Returning success as operation result:
            return Success("Resource deleted successfully.");
        }
    }
}

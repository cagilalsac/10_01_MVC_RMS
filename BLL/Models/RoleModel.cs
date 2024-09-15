#nullable disable

using BLL.DAL;
using System.ComponentModel;

namespace BLL.Models
{
    public class RoleModel // Model classes may also be called DTO (Data Transfer Object) classes such as RoleDTO
    {
        public Role Record { get; set; } // RoleModel has a Record of type Role entity.
                                         // We name the property as "Record" since it is the entity for Role records in the database.
                                         // We will use this entity to set some entity properties to the model properties
                                         // that we will create below, and some model properties to the entity properties.
                                         // This setting properties operation is called mapping and can also be done under service classes.

        // Way 1: readonly properties: can be created by only implementing the getter, therefore their values can't be changed anywhere
        //public string Name
        //{
        //    get
        //    {
        //        return Record.Name; // we get the name of the role for displaying its value
        //    }
        //}
        // Way 2:
        [DisplayName("Role Name")] // DisplayName attribute for "DisplayNameFor" HTML Helper or "label asp-for" Tag Helper in views.
                                   // If no DisplayName is defined, "Name" will be written, else "Role Name" will be written in the view.
        public string Name => Record.Name; // Since we should never modify the entities with extra attributes such as DisplayName
                                           // and we should never change the property implementations,
                                           // we create models and set entity property values with same property names.
                                           // Display name of a property is not related to the database, thus it shouldn't be in entity classes.
                                           // In models we can use any extra attributes we need or do any extra property implementations.

        // Extra optional properties for displaying Record data in the views:
        [DisplayName("User Count")]
        public string UserCount => Record.Users?.Count.ToString(); // "?" used after a reference variable, field or property (Users) indicates that
                                                                   // it may be null. If Users collection is null, assign null to the UserCount property,
                                                                   // otherwise assign the Users collection's element count as string.
                                                                   // "?" should be used for all Navigational Properties which is only Users here.

        public string Users => string.Join("<br>", Record.Users?.Select(u => u.UserName));  // we reach the Record's Users Navigational Property and project
                                                                                            // each User entity's UserName value to a string collection (IEnumerable),
                                                                                            // then we concatenate each string value in the collection
                                                                                            // by using the "<br>" seperator in the Join method of the string type
                                                                                            // and assign the result to the model's Users property
    }
}

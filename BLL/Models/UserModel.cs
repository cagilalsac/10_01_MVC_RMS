using BLL.DAL;
using System.ComponentModel;

namespace BLL.Models
{
    public class UserModel
    {
        public User Record { get; set; }

        [DisplayName("User Name")]
        public string UserName => Record.UserName;

        public string Password => Record.Password;

        [DisplayName("Active")]
        public string IsActive => Record.IsActive ? "Yes" : "No"; // converting the entity bool property value to string model property value using Ternary Operator,
                                                                  // since we don't want to show "true" or "false" for the field in the view

        public Statuses Status => (Statuses)Record.Status; // instead of showing 1, 2 or 3 in the view for the field, we want to show the text part of the enum
                                                           // by casting entity's int Status property value to Statuses type



        // Extra optional properties for displaying Record data in the views:
        // 1 to many relationship relational data handling for output to display:
        public string Role => Record.Role?.Name; // model property for relational role name data
    }
}

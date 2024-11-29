using BLL.DAL;
using System.ComponentModel;
using System.Globalization;

namespace BLL.Models
{
    public class ResourceModel
    {
        public Resource Record { get; set; }
        public string Title => Record.Title;
        public string Content => Record.Content;

        // Way 1:
        //public string Score => Record.Score.ToString("N1", new CultureInfo("en-US")); // CultureInfo should be used when formatting decimal and date time values to string,
                                                                                        // "tr-TR" can be used for Turkish Culture
        // Way 2:
        public string Score => Record.Score.ToString("N1"); // no need to use CultureInfo anywhere in our project anymore, since we manage
                                                            // the culture info configuration in the MvcController base class,
                                                            // N: number format, 1: one decimal after decimal point,
                                                            // for currency "C" can be used the same way

        // Way 1:
        //public string Date => Record.Date.HasValue ? Record.Date.Value.ToString("MM/dd/yyyy HH:mm:ss") : ""; // "Record.Date is not null" can also be written for the condition,
                                                                                                               // MM: 2 digits month, dd: 2 digits day, yyyy: 4 digits year,
                                                                                                               // HH: 2 digits 24 hour, mm: 2 digits minute, ss: 2 digits second
        // Way 2:
        public string Date => Record.Date.HasValue ? Record.Date.Value.ToShortDateString() : ""; // Example output: "9/17/2024"



        // Extra optional properties for displaying Record data as output and getting values for Record data as input in the views:
        // Number of users who share the resource:
        // Many to many relationship relational data handling for output to display:
        [DisplayName("User Count")]
        // Way 1: Ternary Operator
        //public int UserCount => Record.UserResources is null ? 0 : Record.UserResources.Count; // if UserResources is null assign 0, otherwise assign UserResources collection's count
        // Way 2: Null-Coalescing Operator
        public int UserCount => Record.UserResources?.Count ?? 0; // ??: if left side operand is null assign right side operand's value, otherwise assign left side operand's value

        // User names of the active users in ascending order seperated by ", " who share the resource:
        // Many to many relationship relational data handling for output to display:
        public string Users => string.Join(", ", Record.UserResources?.OrderBy(ur => ur.User?.UserName).Where(ur => ur.User?.IsActive == true).Select(ur => ur.User?.UserName));

        // Many to many relationship relational data handling for input to get from the user:
        [DisplayName("Users")]
        public List<int> UserIds 
        {
            get => Record.UserResources?.Select(ur => ur.UserId).ToList(); // for retrieving the model data in edit operation,
                                                                           // return the projection of UserIds integer list from entity's UserResources list if not null,
                                                                           // if entity's UserResources list is null return null
            set => Record.UserResources = value.Select(v => new UserResource() { UserId = v }).ToList(); // set entity's UserResources list from the value,
                                                                                                         // which is an integer list containing the ids of the users,
                                                                                                         // selected in the view for inserting or updating the model data
                                                                                                         // in create or update operations
        }
    }
}

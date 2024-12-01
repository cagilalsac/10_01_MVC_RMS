using System.ComponentModel;

namespace BLL.Models
{
    /// <summary>
    /// For managing the favorites collection which will be kept in the session.
    /// </summary>
    public class FavoritesModel
    {
        public int ResourceId { get; set; }
        public int UserId { get; set; }

        [DisplayName("Resource Title")]
        public string ResourceTitle { get; set; }

        [DisplayName("Resource Score")]
        public string ResourceScore { get; set; } 
    }
}

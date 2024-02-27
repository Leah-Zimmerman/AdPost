using AdPost.Data;

namespace AdPost.Web.Models
{
    public class AdViewModel
    {
        public List<Ad> Ads { get; set; }
        public int UserId { get; set; }
    }
}

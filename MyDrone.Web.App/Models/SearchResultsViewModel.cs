using MyDrone.Kernel.Models;

namespace MyDrone.Web.App.Models
{
    public class SearchResultsViewModel
    {
        public string Query { get; set; }
        public List<Device> Devices { get; set; }
        public List<User> Users { get; set; }

        public SearchResultsViewModel()
        {
            Devices = new List<Device>();
            Users = new List<User>();
        }
    }

}

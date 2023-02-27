using System.Reflection;
using ProductsClient.Classes;
using ProductsClient.Model;

namespace ProductsClient.ViewModel
{
    public class UserTitleVM : BaseVM
    {
        public UserTitleVM()
        {

        }

        public User User => DataInfo.User;
        public string Version => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}

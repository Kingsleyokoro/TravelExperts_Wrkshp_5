using System.Web;
using System.Web.Mvc;

namespace TravelExperts_Wrkshp_5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

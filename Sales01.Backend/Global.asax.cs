namespace Sales01.Backend
{
    using System.Data.Entity;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Sales01.Backend.Helpers;
    using Sales01.Backend.Migrations;
    using Sales01.Backend.Models;
    using Sales01.Common.Models;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LocalDataContext, Configuration>());

            this.CheckRolesAndSuperUser();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CheckRolesAndSuperUser()
        {
            UsersHelper.CheckRole(RolesHelper.ADMIN);
            UsersHelper.CheckRole(RolesHelper.PowerUser);

            UsersHelper.CheckSuperUser();
        }
    }
}

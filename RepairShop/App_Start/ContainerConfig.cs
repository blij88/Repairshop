using Autofac;
using Autofac.Integration.Mvc;
using RepairShop.Data.Services;
using System.Web.Mvc;

namespace RepairShop.Web
{
    public class ContainerConfig
    {
        internal static void RegisterContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<SqlRepairJobsData>().As<IRepairJobsData>().InstancePerRequest();
            builder.RegisterType<SqlPartsData>().As<IPartsData>().InstancePerRequest();
            builder.RegisterType<SqlCustomersData>().As<ICustomersData>().InstancePerRequest();
            builder.RegisterType<SqlEmployeesData>().As<IEmployeesData>().InstancePerRequest();
            builder.RegisterType<RepairShopDbContext>().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
using MediaStorage.Common;
using MediaStorage.Common.Interfaces;
using MediaStorage.Config;
using MediaStorage.Data;
using MediaStorage.Data.Interfaces.IRepository;
using MediaStorage.Data.Read;
using MediaStorage.Data.Repository;
using MediaStorage.Data.Write;
using MediaStorage.Service;
using System;
using Unity;
using Unity.Lifetime;

namespace MediaStorage.Host
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<ILogger, Logger>();
            //container.RegisterAllRepositories(typeof(IRepository), Assembly.GetAssembly(typeof(DepartmentWriteRepository)), typeof(ContainerControlledLifetimeManager));
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IMediaContext,MediaContext>();
            container.RegisterType<IMailSender, MailSender>();
            container.RegisterType<IConfigurationProvider, ConfigurationProvider>();

            /*Services Registration*/
            container.RegisterType<IDepartmentService, DepartmentService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILibraryService, LibraryService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMenuService, MenuService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITagService, TagService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMaterialTypeService, MaterialTypeService>(new ContainerControlledLifetimeManager());


            /*Repository Registrations*/
            container.RegisterType<IDepartmentRepository, DepartmentRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDepartmentReadRepository, DepartmentReadRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDepartmentWriteRepository, DepartmentWriteRepository>(new ContainerControlledLifetimeManager());

            container.RegisterType<ILibraryRepository, LibraryRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILibraryReadRepository, LibraryReadRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILibraryWriteRepository, LibraryWriteRepository>(new ContainerControlledLifetimeManager());

            container.RegisterType<IUserRepository, UserRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserReadRepository, UserReadRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserWriteRepository, UserWriteRepository>(new ContainerControlledLifetimeManager());

            container.RegisterType<ICategoryWriteRepository, CategoryWriteRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICategoryReadRepository, CategoryReadRepository>(new ContainerControlledLifetimeManager());

            container.RegisterType<IMaterialTypeReadRepository, MaterialTypeReadRepositoryExtended>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMaterialTypeWriteRepository, MaterialTypeWriteRepositoryExtended>(new ContainerControlledLifetimeManager());
            
            


        }
    }
}
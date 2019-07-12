namespace MediaStorage.Host
{
    using MediaStorage.Common;
    using MediaStorage.Service;
    using Unity;

    class Program
    {
        static void Main(string[] args)
        {
            var departmentService = UnityConfig.Container.Resolve<IDepartmentService>();

            var libraryService = UnityConfig.Container.Resolve<ILibraryService>();
            var materialTypeService = UnityConfig.Container.Resolve<IMaterialTypeService>();
            var menuService = UnityConfig.Container.Resolve<IMenuService>();
            var tagService = UnityConfig.Container.Resolve<ITagService>();
            var userService = UnityConfig.Container.Resolve<IUserService>();

            var lib = libraryService.AddLibrary(new Common.ViewModels.Library.LibraryViewModel { Name = "TestLiblary" }).Result;
            //var lib = new ServiceResult()
            //{
            //    Id = 1
            //};
            var dep = departmentService.AddDepartment(new Common.ViewModels.Department.DepartmentViewModel { Name = "Test", LibraryId = lib.Id }).Result;
            var mtype = materialTypeService.AddMaterialType(new Common.ViewModels.MaterialType.MaterialTypeViewModel { Name = "TestMaterialType" }).Result;
            var menu = menuService.AddMenu(new Common.ViewModels.Menu.MenuViewModel { Name = "Test Menu", Description = "Demo" });
            var tegData = tagService.AddTag(new Common.ViewModels.Tag.TagViewModel { Name = "TestTag" });
            var userData = userService.AddUser(new Common.ViewModels.User.UserPostViewModel { Username = "TestUSer", IsActive = true, Mail = "a@b.com" });
        }

    }
}

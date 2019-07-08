namespace MediaStorage.Host
{
    using MediaStorage.Common;
    using MediaStorage.Data.Read;
    using MediaStorage.Data.Repository;
    using MediaStorage.Data.Write;
    using MediaStorage.Service;
    class Program
    {
        static void Main(string[] args)
        {
            var departmentService = new DepartmentService(new Logger(), new DepartmentRepository(new DepartmentReadRepository(), new DepartmentWriteRepository()));
            var libraryService = new LibraryService(new LibraryRepository(new LibraryReadRepository(), new LibraryWriteRepository()), new DepartmentRepository(new DepartmentReadRepository(), new DepartmentWriteRepository()), new Logger());
            var materialTypeService = new MaterialTypeService();
            var menuService = new MenuService();
            var tagService = new TagService();
            var userService = new UserService();


            var lib = libraryService.AddLibrary(new Common.ViewModels.Library.LibraryViewModel { Name = "TestLiblary" }).Result;
            var dep = departmentService.AddDepartment(new Common.ViewModels.Department.DepartmentViewModel { Name = "Test", LibraryId = lib.Id }).Result;
            var mtype = materialTypeService.AddMaterialType(new Common.ViewModels.MaterialType.MaterialTypeViewModel { Name = "TestMaterialType" }).Result;
            var menu = menuService.AddMenu(new Common.ViewModels.Menu.MenuViewModel { Name = "Test Menu", Description = "Demo" });
            var tegData = tagService.AddTag(new Common.ViewModels.Tag.TagViewModel { Name = "TestTag" });
            var userData = userService.AddUser(new Common.ViewModels.User.UserPostViewModel { Username = "TestUSer", IsActive = true, Mail = "a@b.com" });

        }

    }
}

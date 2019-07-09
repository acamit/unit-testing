using MediaStorage.Common.ViewModels.Library;
using System.Collections.Generic;

namespace MediaStorage.Service.Tests.TestData
{
    public static class LibraryServiceTestData
    {
        public static List<LibraryViewModel> AllLibraries => new List<LibraryViewModel>() {
               new LibraryViewModel(){Id = 5}
        };
        public static List<LibraryViewModel> AllLibrariesEmptyList => new List<LibraryViewModel>() { };
        public static LibraryViewModel DepartmentNull => null;
    }
}

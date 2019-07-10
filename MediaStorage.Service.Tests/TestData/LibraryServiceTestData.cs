using MediaStorage.Common;
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
        public static List<CustomSelectListItem> CustomSelectListItems => new List<CustomSelectListItem>() {
               new CustomSelectListItem(){Selected= false,Text="random",Value="Value"}
        };
        public static List<CustomSelectListItem> CustomSelectListItemsEmptyList => new List<CustomSelectListItem>() { };

        public static LibraryViewModel Library => new LibraryViewModel() { };

    }
}

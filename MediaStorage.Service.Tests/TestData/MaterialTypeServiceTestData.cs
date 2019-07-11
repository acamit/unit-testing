using MediaStorage.Common;
using MediaStorage.Common.ViewModels.MaterialType;
using System.Collections.Generic;

namespace MediaStorage.Service.Tests.TestData
{
    public static class MaterialTypeServiceTestData
    {
        public static MaterialTypeViewModel MaterialType = new MaterialTypeViewModel() { Id=1, Name="Random"};

        public static List<CustomSelectListItem> CustomSelectListItems => new List<CustomSelectListItem>() { new CustomSelectListItem() { Selected = true, Text = "random", Value = "random value" } };

        public static List<CustomSelectListItem> CustomSelectListItemsEmpty => new List<CustomSelectListItem>() {};

        public static List<CustomSelectListItem> CustomSelectListItemsWithSelectedFalse => new List<CustomSelectListItem>() { new CustomSelectListItem() { Selected = false, Text = "random", Value = "random value" } };

        public static List<MaterialTypeViewModel> AllMaterials => new List<MaterialTypeViewModel>()
        {
            new MaterialTypeViewModel()
            {
                Id = 1
            }
        };
        public static List<MaterialTypeViewModel> AllMaterialsEmptyList => new List<MaterialTypeViewModel>() { };
    }
}

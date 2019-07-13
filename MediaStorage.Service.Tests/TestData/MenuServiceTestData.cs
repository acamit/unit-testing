using MediaStorage.Common;
using MediaStorage.Common.ViewModels.Menu;
using MediaStorage.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MediaStorage.Service.Tests.TestData
{
    public static class MenuServiceTestData
    {
        public static List<Menu> Menus => new List<Menu>() { new Menu() {
                MenuItems = new List<MenuItem>()
                {
                    new MenuItem()
                    {
                        Id = 1
                    }
                }
            }
        };
        public static Menu Menu => new Menu() { };

        public static ServiceResult SuccessServiceResult => new ServiceResult() { IsSuccessful = true, Message = "Action Successful" };

        public static MenuViewModel NewMenu = new MenuViewModel() { Id = 2, Name = "Name", Description = "Description" };

        public static List<MenuItem> MenuItems = new List<MenuItem>() {
            new MenuItem(){ Id= 1 , UserRoles = new List<UserRole>(){ } },
            new MenuItem(){ Id= 2 , UserRoles = new List<UserRole>(){ } }
        };
        public static List<CustomSelectListItem> CustomSelectListItems = new List<CustomSelectListItem>()
        {
            new CustomSelectListItem(){ },
            new CustomSelectListItem(){ },
            new CustomSelectListItem(){ }
        };
    }
}

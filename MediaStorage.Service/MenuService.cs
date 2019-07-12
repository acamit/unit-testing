using MediaStorage.Common;
using MediaStorage.Common.ViewModels.Menu;
using MediaStorage.Config;
using MediaStorage.Data;
using MediaStorage.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MediaStorage.Service
{

    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _uow;
        IConfigurationProvider _configurationProvider;
        public MenuService(IUnitOfWork unitOfWork, IConfigurationProvider configurationProvider)
        {
            _uow = unitOfWork;
            _configurationProvider = configurationProvider;
        }


        public List<MenuViewModel> GetAllMenus()
        {
            List<MenuViewModel> data = null;
            var canGetAllMenuValue = _configurationProvider.CanGetAllMenus();
            bool canGetAllMenu;
            bool.TryParse(canGetAllMenuValue, out canGetAllMenu);
            if (canGetAllMenu)
            {

                data = _uow.MenuRepository
                   .GetAll()
                   .Select(s => new MenuViewModel
                   {
                       Id = s.Id,
                       Name = s.Name,
                       Description = s.Description
                   }).ToList();
            }
            return data;
        }

        //todo : write test cases
        public List<CustomSelectListItem> GetAllMenusBySelectListItem(int? id)
        {
            return _uow.MenuRepository
                .GetAll()
                .Select(s => new CustomSelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name,
                    Selected = id.HasValue ? s.MenuItems.Any(a => a.Id == id.Value) : false
                }).ToList();
        }

        public MenuViewModel GetMenuById(int id)
        {
            var menu = _uow.MenuRepository.Find(id);
            return menu == null ? null : new MenuViewModel
            {
                Id = menu.Id,
                Name = menu.Name,
                Description = menu.Description
            };
        }

        public ServiceResult AddMenu(MenuViewModel entity)
        {
            _uow.MenuRepository.Add(new Menu
            {
                Name = entity.Name,
                Description = entity.Description
            });

            return GetAddResult(_uow.Commit() == 1);
        }


        public ServiceResult UpdateMenu(MenuViewModel entity)
        {
            _uow.MenuRepository.Update(new Menu
            {
                Id = entity.Id.Value,
                Name = entity.Name,
                Description = entity.Description
            });

            return GetUpdateResult(_uow.Commit() == 1);
        }

        public ServiceResult RemoveMenu(int id, bool cascadeRemove = false)
        {
            if (cascadeRemove)
            {
                var menuItems = _uow.MenuItemRepository.GetAll(w => w.MenuId == id, i => i.UserRoles).ToList();
                if (menuItems.Count > 0)
                    _uow.MenuItemRepository.DeleteRange(menuItems);
            }
            _uow.MenuRepository.Delete(id);
            return GetRemoveResult(_uow.Commit() > 0);
        }

        protected virtual ServiceResult GetRemoveResult(bool v)
        {
            return ServiceResult.GetRemoveResult(_uow.Commit() > 0);
        }

        protected virtual ServiceResult GetAddResult(bool isCommited)
        {
            return ServiceResult.GetAddResult(isCommited);
        }

        protected virtual ServiceResult GetUpdateResult(bool v)
        {
            return ServiceResult.GetUpdateResult(_uow.Commit() == 1);
        }

    }
}

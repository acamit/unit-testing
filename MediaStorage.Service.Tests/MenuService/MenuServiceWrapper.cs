using MediaStorage.Common;
using MediaStorage.Config;
using MediaStorage.Data;
using MediaStorage.Service.Tests.MenuService;

namespace MediaStorage.Service.Tests.MenuServiceTests
{
    public class MenuServiceWrapper : Service.MenuService
    {
        private readonly IMenuServiceMockHelper _menuServiceMockHelper;

        public MenuServiceWrapper(IUnitOfWork unitOfWork, IConfigurationProvider configurationProvider, IMenuServiceMockHelper menuServiceMockHelper) : base(unitOfWork, configurationProvider)
        {
            _menuServiceMockHelper = menuServiceMockHelper;
        }

        protected override ServiceResult GetAddResult(bool isCommited)
        {
            return _menuServiceMockHelper.GetAddResult(isCommited);
        }

        protected override ServiceResult GetRemoveResult(bool isRemoved)
        {
            return _menuServiceMockHelper.GetRemoveResult(isRemoved);
        }

        protected override ServiceResult GetUpdateResult(bool isUpdated)
        {
            return _menuServiceMockHelper.GetUpdateResult(isUpdated);
        }
    }
}

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
            return base.GetAddResult(isCommited);
        }

        protected override ServiceResult GetRemoveResult(bool v)
        {
            return base.GetRemoveResult(v);
        }

        protected override ServiceResult GetUpdateResult(bool v)
        {
            return base.GetUpdateResult(v);
        }
    }
}

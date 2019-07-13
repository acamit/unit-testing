using MediaStorage.Common;

namespace MediaStorage.Service.Tests.TagService
{
    public interface ITagServiceMockHelper
    {
        ServiceResult GetAddResult(bool isCommited);
        ServiceResult GetRemoveResult(bool isRemoved);
        ServiceResult GetUpdateResult(bool isUpdated);
    }
}

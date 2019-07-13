
using MediaStorage.Common;
using MediaStorage.Data;
using MediaStorage.Data.Entities;

namespace MediaStorage.Service.Tests.TagService
{
    public class TagServiceWrapper : Service.TagService
    {
        public ITagServiceMockHelper _tagServiceMockHelper;

        public TagServiceWrapper(IUnitOfWork unitOfWork, IRepository<Tag> repository, ITagServiceMockHelper tagServiceMockHelper) : base(unitOfWork, repository)
        {
            _tagServiceMockHelper = tagServiceMockHelper;
        }

        protected override ServiceResult GetAddResult(bool isCommited)
        {
            return _tagServiceMockHelper.GetAddResult(isCommited);
        }

        protected override ServiceResult GetRemoveResult(bool isRemoved)
        {
            return _tagServiceMockHelper.GetRemoveResult(isRemoved);
        }

        protected override ServiceResult GetUpdateResult(bool isUpdated)
        {
            return _tagServiceMockHelper.GetUpdateResult(isUpdated);
        }
    }
}

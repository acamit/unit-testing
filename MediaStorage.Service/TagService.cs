using MediaStorage.Common;
using MediaStorage.Common.ViewModels.Tag;
using MediaStorage.Data;
using MediaStorage.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MediaStorage.Service
{
    public class TagService : ITagService
    {
        private IUnitOfWork _uow;
        private IRepository<Tag> _tagRepository;


        public TagService(IUnitOfWork unitOfWork, IRepository<Tag> repository)
        {
            this._uow = unitOfWork;
            this._tagRepository = repository;
        }


        public List<TagViewModel> GetAllTags()
        {
            return _tagRepository
                .GetAll()
                .Select(s => new TagViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
        }
        public TagViewModel GetTagById(int id)
        {
            var tag = _tagRepository.Find(id);

            return tag == null ? null : new TagViewModel
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        public ServiceResult AddTag(TagViewModel entity)
        {
            _tagRepository.Add(new Tag
            {
                Name = entity.Name
            });

            return GetAddResult(_uow.Commit() == 1);
        }

        public ServiceResult UpdateTag(TagViewModel entity)
        {
            _tagRepository.Update(new Tag
            {
                Id = entity.Id.Value,
                Name = entity.Name
            });

            return GetUpdateResult(_uow.Commit() == 1);
        }

        public ServiceResult RemoveTag(int id)
        {
            _tagRepository.Delete(id);

            return GetRemoveResult(_uow.Commit() > 0);
        }

        protected virtual ServiceResult GetRemoveResult(bool isRemoved)
        {
            return ServiceResult.GetRemoveResult(isRemoved);
        }

        protected virtual ServiceResult GetAddResult(bool isCommited)
        {
            return ServiceResult.GetAddResult(isCommited);
        }
        protected virtual ServiceResult GetUpdateResult(bool isUpdated)
        {
            return ServiceResult.GetUpdateResult(isUpdated);
        }
    }
}

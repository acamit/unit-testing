using MediaStorage.Common;
using MediaStorage.Common.ViewModels.Tag;
using MediaStorage.Data;
using MediaStorage.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MediaStorage.Service
{
    public class TagService
    {
        private UnitOfWork uow;
        private Repository<Tag> tagRepository;


        public TagService()
        {
            MediaContext context = new MediaContext();
            this.uow = new UnitOfWork(context);
            this.tagRepository = new Repository<Tag>(context); 
        }


        public List<TagViewModel> GetAllTags()
        {
            return tagRepository
                .GetAll()
                .Select(s => new TagViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
        }
        public TagViewModel GetTagById(int id)
        {
            var tag = tagRepository.Find(id);

            return tag == null ? null : new TagViewModel
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        public ServiceResult AddTag(TagViewModel entity)
        {
            tagRepository.Add(new Tag
            {
                Name = entity.Name
            });

            return ServiceResult.GetAddResult(uow.Commit() == 1);
        }

        public ServiceResult UpdateTag(TagViewModel entity)
        {
            tagRepository.Update(new Tag
            {
                Id = entity.Id.Value,
                Name = entity.Name
            });

            return ServiceResult.GetUpdateResult(uow.Commit() == 1);
        }

        public ServiceResult RemoveTag(int id)
        {
            tagRepository.Delete(id);

            return ServiceResult.GetRemoveResult(uow.Commit() > 0);
        }
    }
}

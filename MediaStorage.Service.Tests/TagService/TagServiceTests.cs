using MediaStorage.Data;
using MediaStorage.Data.Entities;
using MediaStorage.Service.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MediaStorage.Service.Tests.TagService
{
    [TestClass]
    public class TagServiceTests
    {
        private TagServiceWrapper _tagService;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IRepository<Tag>> _tagRepository;
        private readonly Mock<ITagServiceMockHelper> _tagServiceMockHelper;

        public TagServiceTests()
        {
            _uow = new Mock<IUnitOfWork>(); 
            _tagRepository = new Mock<IRepository<Tag>>();
            _tagServiceMockHelper = new Mock<ITagServiceMockHelper>();
        }
        #region GetAllTags
        [TestMethod]
        public void GetAllTags_ShouldGetTagsFromRepository()
        {
            //_tagRepository.Setup(x=>x.GetAll()).Returns()
            var result = _tagService.GetAllTags();
            _tagRepository.Verify(x => x.GetAll(), Times.Once, "Should Fetch Tags from repository");
            Assert.IsNotNull(result);
        }
        #endregion

        #region GetTagById
        [TestMethod] 
        public void GetTagById_RecordFound_ShouldNotReturnNull()
        {
            _tagRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(TagServiceTestData.Tag);
            var result = _tagService.GetTagById(1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetTagById_RecordNotFound_ShouldReturnNull()
        {
            _tagRepository.Setup(x => x.Find(It.IsAny<int>())).Returns((Tag)null);
            var result = _tagService.GetTagById(1);
            Assert.IsNull(result);
        }

        #endregion

        #region AddTag 
        [TestMethod]
        public void AddTag_ShouldAddNewMenuUsingRepository()
        {
            _tagServiceMockHelper.Setup(x => x.GetAddResult(It.IsAny<bool>())).Returns(MenuServiceTestData.SuccessServiceResult);
            var result = _tagService.AddTag(TagServiceTestData.NewTag);
            _tagRepository.Verify(x => x.Add(It.IsAny<Tag>()), Times.Once, "Should Add new item using repository");
        }

        [TestMethod]
        public void AddTag_ShouldCommitChanges()
        {
            _tagServiceMockHelper.Setup(x => x.GetAddResult(It.IsAny<bool>())).Returns(MenuServiceTestData.SuccessServiceResult);
            var result = _tagService.AddTag(TagServiceTestData.NewTag);
            _uow.Verify(x => x.Commit(), Times.Once, "Should commit changes after adding the menu.");
        }

        [TestMethod]
        public void AddTag_TagAdded_ShouldGetServiceResult()
        {
            var result = _tagService.AddTag(TagServiceTestData.NewTag);

            _tagServiceMockHelper.Verify(x => x.GetAddResult(It.IsAny<bool>()), Times.Once, "Should get result from service result");
        }

        #endregion

        #region UpdateTag
        [TestMethod]
        public void UpdateTag_ShouldUpdateTagUsingRepository()
        {
            _tagServiceMockHelper.Setup(x => x.GetUpdateResult(It.IsAny<bool>())).Returns(MenuServiceTestData.SuccessServiceResult);
            var result = _tagService.UpdateTag(TagServiceTestData.NewTag);
            _tagRepository.Verify(x => x.Update(It.IsAny<Tag>()), Times.Once, "Should update menu using repository");
        }

        [TestMethod]
        public void UpdateTag_ShouldCommitChanges()
        {
            _tagServiceMockHelper.Setup(x => x.GetUpdateResult(It.IsAny<bool>())).Returns(MenuServiceTestData.SuccessServiceResult);
            var result = _tagService.UpdateTag(TagServiceTestData.NewTag);
            _uow.Verify(x => x.Commit(), Times.Once, "Should commit changes after adding the menu.");
        }

        [TestMethod]
        public void UpdateTag_ItemUpdated_ShouldGetServiceResult()
        {
            var result = _tagService.UpdateTag(TagServiceTestData.NewTag);

            _tagServiceMockHelper.Verify(x => x.GetUpdateResult(It.IsAny<bool>()), Times.Once, "Should get result from service result");
        }

        #endregion

        #region RemoveTag
        [TestMethod]
        public void RemoveTag_WithOrWithoutCascade_ShouldAddNewTagUsingRepository()
        {
            var result = _tagService.RemoveTag(3);
            _tagRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once, "Should delete using repository");
        }

        [TestMethod]
        public void RemoveTag_WithOrWithoutCascade_ShouldCommitChanges()
        {
            var result = _tagService.RemoveTag(2);
            _uow.Verify(x => x.Commit(), Times.Once, "Should commit changes after adding the tag.");
        }

        [TestMethod]
        public void RemoveTag_ItemDeleted_ShouldGetServiceResult()
        {
            var result = _tagService.RemoveTag(2);
            _tagServiceMockHelper.Verify(x => x.GetRemoveResult(It.IsAny<bool>()), Times.Once, "Should get result from service result");
        }
        [TestMethod]
        public void RemoveTag_CascadeDelete_ShouldRemoveAllTagItems()
        {
            var result = _tagService.RemoveTag(1);
            _tagRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once, "Should delete all tag items");
        }


        #endregion

        [TestInitialize]
        public void GetServiceInstance()
        {
            _tagService = new TagServiceWrapper(_uow.Object, _tagRepository.Object, _tagServiceMockHelper.Object);
        }
    }
}

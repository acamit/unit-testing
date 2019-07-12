using MediaStorage.Common;
using MediaStorage.Common.Interfaces;
using MediaStorage.Common.ViewModels.MaterialType;
using MediaStorage.Data.Read;
using MediaStorage.Data.Repository;
using MediaStorage.Data.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MediaStorage.Common.Constants;

namespace MediaStorage.Service
{
    public class MaterialTypeService : IMaterialTypeService
    {
        private readonly IMaterialTypeReadRepository _materialTypeReadRepository;
        private readonly IMaterialTypeWriteRepository  _materialTypeWriteRepository;
        private readonly ILogger _logger;

        public MaterialTypeService(IMaterialTypeReadRepository materialTypeReadRepository , IMaterialTypeWriteRepository materialTypeWriteRepository, ILogger logger)
        {
            _materialTypeReadRepository = materialTypeReadRepository;
            _materialTypeWriteRepository = materialTypeWriteRepository;
            _logger = logger;
        }

        public async Task<List<MaterialTypeViewModel>> GetAllMaterialTypes()
        {
            var materialTypes = await _materialTypeReadRepository.GetAllMaterials();
            if (materialTypes == null || !materialTypes.Any())
            {
                _logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return materialTypes;
        }

        public async Task<List<CustomSelectListItem>> GetMaterialTypesAsSelectListItem(int? categoryId)
        {
            var materialTypes = await _materialTypeReadRepository.GetMaterialTypesAsSelectListItem(categoryId);

            if (materialTypes == null || !materialTypes.Any())
            {
                _logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }

            if (!materialTypes.Any(x => x.Selected))
            {
                _logger.Error(SelectedSubCategoriesDoesNotExistErrorMessage);
                throw new Exception(SelectedSubCategoriesDoesNotExistErrorMessage);
            }

            return materialTypes;
        }

        public async Task<MaterialTypeViewModel> GetMaterialTypeById(int id)
        {
            var materialType = await _materialTypeReadRepository.GetMaterialTypeById(id);

            if (materialType == null)
            {
                _logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return materialType;
        }

        public async Task<ServiceResult> AddMaterialType(MaterialTypeViewModel entity)
        {

            var id = await _materialTypeWriteRepository.AddMaterialType(entity);
            ServiceResult result = new ServiceResult() { Id = id };
            if (id < 0)
            {
                result.SetFailure(MaterialAddFailedMessage);
            }
            else
            {
                result.SetSuccess(MaterialAddSuccessMessage);
            }
            return result;
        }

        public async Task<ServiceResult> UpdateMaterialType(MaterialTypeViewModel entity)
        {
            var isUpdated = await _materialTypeWriteRepository.UpdateMaterialType(entity);
            ServiceResult result = new ServiceResult();
            if (!isUpdated)
            {
                result.SetFailure(UpdateMaterialFailedMessage);
            }
            else
            {
                result.SetSuccess(UpdateMaterialSuccessMessage);
            }
            return result;
        }

        public async Task<ServiceResult> RemoveMaterialType(int id)
        {
            var isUpdated = await _materialTypeWriteRepository.RemoveMaterialType(id);
            ServiceResult result = new ServiceResult();
            if (!isUpdated)
            {
                result.SetFailure(DeleteMaterialFailedMessage);
            }
            else
            {
                result.SetSuccess(DeleteMaterialSuccessMessage);
            }
            return result;
        }
    }
}

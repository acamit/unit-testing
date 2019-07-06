using MediaStorage.Common;
using MediaStorage.Common.ViewModels.MaterialType;
using MediaStorage.Data.Read;
using MediaStorage.Data.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MediaStorage.Common.Constants;

namespace MediaStorage.Service
{
    public class MaterialTypeService
    {
        MaterialTypeReadRepository materialTypeReadRepository = new MaterialTypeReadRepository();
        MaterialTypeWriteRepository materialTypeWriteRepository = new MaterialTypeWriteRepository();
        private Logger logger;

        public MaterialTypeService()
        {
            logger = new Logger();
        }

        public async Task<List<MaterialTypeViewModel>> GetAllMaterialTypes()
        {
            var materialTypes = await materialTypeReadRepository.GetAllMaterials();
            if (materialTypes == null || !materialTypes.Any())
            {
                logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return materialTypes;
        }

        public async Task<List<CustomSelectListItem>> GetMaterialTypesAsSelectListItem(int? categoryId)
        {
            var materialTypes = await materialTypeReadRepository.GetMaterialTypesAsSelectListItem(categoryId);

            if (materialTypes == null || !materialTypes.Any())
            {
                logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }

            if (!materialTypes.Any(x => x.Selected))
            {
                logger.Error(SelectedSubCategoriesDoesNotExistErrorMessage);
                throw new Exception(SelectedSubCategoriesDoesNotExistErrorMessage);
            }

            return materialTypes;
        }

        public async Task<MaterialTypeViewModel> GetMaterialTypeById(int id)
        {
            var materialType = await materialTypeReadRepository.GetMaterialTypeById(id);

            if (materialType == null)
            {
                logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return materialType;
        }

        public async Task<ServiceResult> AddMaterialType(MaterialTypeViewModel entity)
        {

            var id = await materialTypeWriteRepository.AddMaterialType(entity);
            ServiceResult result = new ServiceResult() { Id = id };
            if (id < 0)
            {
                result.SetFailure("Error while inserting material.");
            }
            else
            {
                result.SetSuccess("Material added successfully.");
            }
            return result;
        }

        public async Task<ServiceResult> UpdateLibrary(MaterialTypeViewModel entity)
        {
            var isUpdated = await materialTypeWriteRepository.UpdateMaterialType(entity);
            ServiceResult result = new ServiceResult();
            if (!isUpdated)
            {
                result.SetFailure("Error while material library.");
            }
            else
            {
                result.SetSuccess("Material updated successfully.");
            }
            return result;
        }

        public async Task<ServiceResult> RemoveLibrary(int id)
        {
            var isUpdated = await materialTypeWriteRepository.RemoveMaterialType(id);
            ServiceResult result = new ServiceResult();
            if (!isUpdated)
            {
                result.SetFailure("Error while deleting material.");
            }
            else
            {
                result.SetSuccess("Library deleted successfully.");
            }
            return result;
        }
    }
}

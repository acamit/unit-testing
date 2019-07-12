using MediaStorage.Common;
using MediaStorage.Common.ViewModels.MaterialType;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaStorage.Data.Repository
{
    public interface IMaterialTypeReadRepository: IRepository
    {
        Task<List<MaterialTypeViewModel>> GetAllMaterials();
        Task<MaterialTypeViewModel> GetMaterialTypeById(int id);
        Task<List<CustomSelectListItem>> GetMaterialTypesAsSelectListItem(int? categoryId);
    }
}
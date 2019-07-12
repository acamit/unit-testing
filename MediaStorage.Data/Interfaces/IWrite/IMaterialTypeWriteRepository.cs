using MediaStorage.Common.ViewModels.MaterialType;
using System.Threading.Tasks;

namespace MediaStorage.Data.Write
{
    public interface IMaterialTypeWriteRepository:IRepository
    {
        Task<int> AddMaterialType(MaterialTypeViewModel entity);

        Task<bool> RemoveMaterialType(int id);

        Task<bool> UpdateMaterialType(MaterialTypeViewModel entity);
    }
}
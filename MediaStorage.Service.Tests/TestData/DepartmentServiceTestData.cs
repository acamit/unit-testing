using MediaStorage.Common.ViewModels.Department;
using System.Collections.Generic;

namespace MediaStorage.Service.Tests.TestData
{
    public static class DepartmentServiceTestData
    {
        public static List<DepartmentListViewModel> AllDepartments => new List<DepartmentListViewModel> { new DepartmentListViewModel() { } };
        public static List<DepartmentListViewModel> AllDepartmentsEmptyList => new List<DepartmentListViewModel> { };
    }
}

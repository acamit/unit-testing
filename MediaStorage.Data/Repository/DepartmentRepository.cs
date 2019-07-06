using MediaStorage.Data.Read;
using MediaStorage.Data.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaStorage.Data.Repository
{
    public class DepartmentRepository
    {
        public DepartmentReadRepository DepartmentReadRepository { get; set; }
        public DepartmentWriteRepository DepartmentWriteRepository { get; set; }
    }
}

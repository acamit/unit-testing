﻿using MediaStorage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaStorage.Service.Tests.MenuService
{
    public interface IMenuServiceMockHelper
    {
        ServiceResult GetAddResult(bool isCommited);
        ServiceResult GetRemoveResult(bool isRemoved);
        ServiceResult GetUpdateResult(bool isUpdated);
    }
}

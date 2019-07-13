using MediaStorage.Common.ViewModels.Tag;
using MediaStorage.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaStorage.Service.Tests.TestData
{
    public static class TagServiceTestData
    {
        public static TagViewModel NewTag => new TagViewModel() { Id= 1, Name="TagName"};

        public static Tag Tag => new Tag()
        {
            Id = 2,
            Name = "Tag",
            Materials = new List<Material>()
            {
            },
        };

    }
}

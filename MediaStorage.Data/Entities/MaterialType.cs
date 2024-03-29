﻿using System.Collections.Generic;

namespace MediaStorage.Data.Entities
{
    public class MaterialType : BaseEntity<int>
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Material> Materials { get; set; }

        public virtual ICollection<MaterialTypeProperty> MaterialTypeProperties { get; set; }
    }

    class MaterialTypeConfiguration : BaseConfiguration<MaterialType>
    {
        internal MaterialTypeConfiguration()
        {
            HasKey(m => m.Id);
            Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(255);
            HasMany(m => m.Categories)
                .WithRequired()
                .HasForeignKey(m => m.MaterialTypeId);
            HasMany(m => m.Materials)
                .WithRequired()
                .HasForeignKey(m => m.MaterialTypeId);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Passports;
using Models.StoragePoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DBRealization.EntityConfiguration.StoragePoints
{
    public class StoragePointConfiguration : IEntityTypeConfiguration<StoragePoint>
    {
        public void Configure(EntityTypeBuilder<StoragePoint> builder)
        {
            builder.ToTable("storage_point")
            .HasKey(x => x.Id)
            .HasName("PK_storage_point");

            builder.HasMany(x => x.LicenseInfoList)
            .WithOne(x => x.Storage)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Id)
           .ValueGeneratedOnAdd();

        }
    }
}

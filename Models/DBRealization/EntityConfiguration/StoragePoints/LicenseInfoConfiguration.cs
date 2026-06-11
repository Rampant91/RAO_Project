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
    public class LicenseInfoConfiguration : IEntityTypeConfiguration<LicenseInfo>
    {
        public void Configure(EntityTypeBuilder<LicenseInfo> builder)
        {
            builder.ToTable("license_info")
            .HasKey(x => x.Id)
            .HasName("PK_license_info");

            builder.HasOne(x => x.Storage)
            .WithMany(x => x.LicenseInfoList)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Id)
           .ValueGeneratedOnAdd();

        }
    }
}

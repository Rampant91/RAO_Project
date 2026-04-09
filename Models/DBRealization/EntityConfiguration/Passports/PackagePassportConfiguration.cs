using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Passports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DBRealization.EntityConfiguration.Passports
{
    public class PackagePassportConfiguration : IEntityTypeConfiguration<PackagePassport>
    {
        public void Configure(EntityTypeBuilder<PackagePassport> builder)
        {
            builder.ToTable("package_passport")
            .HasKey(x => x.Id)
            .HasName("PK_package_passport");

            builder.HasMany(x => x.ContentCharacteristics)
            .WithOne(x => x.Passport)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Id)
           .ValueGeneratedOnAdd();

        }
    }
}

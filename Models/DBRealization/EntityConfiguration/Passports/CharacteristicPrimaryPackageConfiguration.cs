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
    public class CharacteristicPrimaryPackageConfiguration : IEntityTypeConfiguration<CharacteristicPrimaryPackage>
    {
        public void Configure(EntityTypeBuilder<CharacteristicPrimaryPackage> builder)
        {
            builder.ToTable("characteristic_package")
            .HasKey(x => x.Id)
            .HasName("PK_characteristic_package");

            builder.HasMany(x => x.RadionuclidsList)
            .WithOne(x => x.Characteristic)
            .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("characteristic_package")
                .HasOne(x => x.Passport)
                .WithMany(x => x.ContentCharacteristics)
                .HasForeignKey(x => x.PassportId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Id)
           .ValueGeneratedOnAdd();

        }
    }
}

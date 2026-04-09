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
    public class RadionuclidConfiguration : IEntityTypeConfiguration<Radionuclid>
    {
        public void Configure(EntityTypeBuilder<Radionuclid> builder)
        {
            builder.ToTable("radionuclid")
            .HasKey(x => x.Id)
            .HasName("PK_radionuclid");

            builder.ToTable("radionuclid")
                .HasOne(x => x.Characteristic)
                .WithMany(x => x.RadionuclidsList)
                .HasForeignKey(x => x.CharacteristicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Id)
           .ValueGeneratedOnAdd();

        }
    }
}

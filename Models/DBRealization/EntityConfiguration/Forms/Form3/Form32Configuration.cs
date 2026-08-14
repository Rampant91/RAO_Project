using Microsoft.EntityFrameworkCore;
using Models.Forms.Form1;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Forms.Form3;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form32Configuration : IEntityTypeConfiguration<Form32>
{
    public void Configure(EntityTypeBuilder<Form32> builder)
    {
        builder.ToTable("form_32")
            .HasOne(x => x.Report)
            .WithOne(x => x.Rows32One)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("form_32")
            .HasMany(x => x.ExportedZriInfoCollection)
            .WithOne(x => x.Form32)
            .HasForeignKey(x => x.Form32Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("form_32")
            .HasMany(x => x.ContainersInfoCollection)
            .WithOne(x => x.Form32)
            .HasForeignKey(x => x.Form32Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("form_32")
            .HasMany(x => x.IdentificatorsCollection)
            .WithOne(x => x.Form32)
            .HasForeignKey(x => x.Form32Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
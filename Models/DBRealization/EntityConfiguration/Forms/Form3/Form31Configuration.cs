using Microsoft.EntityFrameworkCore;
using Models.Forms.Form1;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Forms.Form3;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form31Configuration : IEntityTypeConfiguration<Form31>
{
    public void Configure(EntityTypeBuilder<Form31> builder)
    {
        builder.ToTable("form_31")
            .HasOne(x => x.Report)
            .WithOne(x => x.Rows31One)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("form_31")
            .HasMany(x => x.ExportedZriOziiiInfoCollection)
            .WithOne(x => x.Form31)
            .HasForeignKey(x => x.Form31Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
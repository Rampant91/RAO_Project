using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form1;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form16Configuration : IEntityTypeConfiguration<Form16>
{
    public void Configure(EntityTypeBuilder<Form16> builder)
    {
        builder.ToTable("form_16")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows16)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
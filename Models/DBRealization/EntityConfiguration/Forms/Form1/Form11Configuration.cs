using Microsoft.EntityFrameworkCore;
using Models.Forms.Form1;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form11Configuration : IEntityTypeConfiguration<Form11>
{
    public void Configure(EntityTypeBuilder<Form11> builder)
    {
        builder.ToTable("form_11")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows11)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
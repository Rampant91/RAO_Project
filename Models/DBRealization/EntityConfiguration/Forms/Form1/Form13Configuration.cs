using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form1;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form13Configuration : IEntityTypeConfiguration<Form13>
{
    public void Configure(EntityTypeBuilder<Form13> builder)
    {
        builder.ToTable("form_13")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows13)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
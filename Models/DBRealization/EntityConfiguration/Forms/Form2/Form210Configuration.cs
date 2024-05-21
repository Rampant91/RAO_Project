using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form2;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form210Configuration : IEntityTypeConfiguration<Form210>
{
    public void Configure(EntityTypeBuilder<Form210> builder)
    {
        builder.ToTable("form_210")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows210)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
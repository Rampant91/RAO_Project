using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form1;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form19Configuration : IEntityTypeConfiguration<Form19>
{
    public void Configure(EntityTypeBuilder<Form19> builder)
    {
        builder.ToTable("form_19")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows19)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
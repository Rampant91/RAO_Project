using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form1;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form18Configuration : IEntityTypeConfiguration<Form18>
{
    public void Configure(EntityTypeBuilder<Form18> builder)
    {
        builder.ToTable("form_18")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows18)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
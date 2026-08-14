using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Forms.Form1;
using Models.Forms.Form3;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form30Configuration : IEntityTypeConfiguration<Form30>
{
    public void Configure(EntityTypeBuilder<Form30> builder)
    {
        builder.ToTable("form_30")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows30)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
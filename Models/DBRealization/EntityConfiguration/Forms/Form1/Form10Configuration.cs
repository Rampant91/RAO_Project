using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Forms.Form1;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form10Configuration : IEntityTypeConfiguration<Form10>
{
    public void Configure(EntityTypeBuilder<Form10> builder)
    {
        builder.ToTable("form_10")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows10)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
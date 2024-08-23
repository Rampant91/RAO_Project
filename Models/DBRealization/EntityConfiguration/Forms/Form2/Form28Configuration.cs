using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form2;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form28Configuration : IEntityTypeConfiguration<Form28>
{
    public void Configure(EntityTypeBuilder<Form28> builder)
    {
        builder.ToTable("form_28")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows28)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form2;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form26Configuration : IEntityTypeConfiguration<Form26>
{
    public void Configure(EntityTypeBuilder<Form26> builder)
    {
        builder.ToTable("form_26")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows26)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form2;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form24Configuration : IEntityTypeConfiguration<Form24>
{
    public void Configure(EntityTypeBuilder<Form24> builder)
    {
        builder.ToTable("form_24")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows24)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
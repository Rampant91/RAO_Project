using Microsoft.EntityFrameworkCore;
using Models.Forms.Form2;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form21Configuration : IEntityTypeConfiguration<Form21>
{
    public void Configure(EntityTypeBuilder<Form21> builder)
    {
        builder.ToTable("form_21")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows21)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
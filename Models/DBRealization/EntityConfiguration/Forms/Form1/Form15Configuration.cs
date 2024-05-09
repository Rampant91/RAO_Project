using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form1;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form15Configuration : IEntityTypeConfiguration<Form15>
{
    public void Configure(EntityTypeBuilder<Form15> builder)
    {
        builder.ToTable("form_15")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows15)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
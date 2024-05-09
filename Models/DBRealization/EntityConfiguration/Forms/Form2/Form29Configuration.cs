using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form2;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form29Configuration : IEntityTypeConfiguration<Form29>
{
    public void Configure(EntityTypeBuilder<Form29> builder)
    {
        builder.ToTable("form_29")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows29)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
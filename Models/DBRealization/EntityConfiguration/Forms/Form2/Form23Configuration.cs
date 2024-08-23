using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form2;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form23Configuration : IEntityTypeConfiguration<Form23>
{
    public void Configure(EntityTypeBuilder<Form23> builder)
    {
        builder.ToTable("form_23")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows23)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
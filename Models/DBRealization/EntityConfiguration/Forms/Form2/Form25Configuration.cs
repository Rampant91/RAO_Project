using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form2;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form25Configuration : IEntityTypeConfiguration<Form25>
{
    public void Configure(EntityTypeBuilder<Form25> builder)
    {
        builder.ToTable("form_25")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows25)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form2;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form212Configuration : IEntityTypeConfiguration<Form212>
{
    public void Configure(EntityTypeBuilder<Form212> builder)
    {
        builder.ToTable("form_212")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows212)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
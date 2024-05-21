using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Forms.Form2;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form20Configuration : IEntityTypeConfiguration<Form20>
{
    public void Configure(EntityTypeBuilder<Form20> builder)
    {
        builder.ToTable("form_20")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows20)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
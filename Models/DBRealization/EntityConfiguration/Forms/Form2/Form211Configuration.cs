using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form2;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form211Configuration : IEntityTypeConfiguration<Form211>
{
    public void Configure(EntityTypeBuilder<Form211> builder)
    {
        builder.ToTable("form_211")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows211)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
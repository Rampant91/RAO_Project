using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form1;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form17Configuration : IEntityTypeConfiguration<Form17>
{
    public void Configure(EntityTypeBuilder<Form17> builder)
    {
        builder.ToTable("form_17")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows17)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
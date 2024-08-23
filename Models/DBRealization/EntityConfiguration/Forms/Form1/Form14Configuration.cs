using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form1;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form14Configuration : IEntityTypeConfiguration<Form14>
{
    public void Configure(EntityTypeBuilder<Form14> builder)
    {
        builder.ToTable("form_14")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows14)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
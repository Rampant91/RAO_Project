using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form1;

namespace Models.DBRealization.EntityConfiguration.Forms.Form1;

public class Form12Configuration : IEntityTypeConfiguration<Form12>
{
    public void Configure(EntityTypeBuilder<Form12> builder)
    {
        builder.ToTable("form_12")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows12)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
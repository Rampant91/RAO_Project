using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form2;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form22Configuration : IEntityTypeConfiguration<Form22>
{
    public void Configure(EntityTypeBuilder<Form22> builder)
    {
        builder.ToTable("form_22")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows22)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
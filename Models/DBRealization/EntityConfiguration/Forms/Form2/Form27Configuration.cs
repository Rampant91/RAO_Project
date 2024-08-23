using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Forms.Form2;

namespace Models.DBRealization.EntityConfiguration.Forms.Form2;

public class Form27Configuration : IEntityTypeConfiguration<Form27>
{
    public void Configure(EntityTypeBuilder<Form27> builder)
    {
        builder.ToTable("form_27")
            .HasOne(x => x.Report)
            .WithMany(x => x.Rows27)
            .HasForeignKey(x => x.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Collections;

namespace Models.DBRealization.EntityConfiguration;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("ReportCollection_DbSet")
            .HasOne(x => x.Reports)
            .WithMany(x => x.Report_Collection)
            //.HasForeignKey(x => x.ReportsId)
            //.HasConstraintName("FK_ReportCollection_DbSet_Repo~")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows10)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows11)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows12)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows13)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows14)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows15)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows16)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows17)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows18)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows19)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows20)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows21)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows22)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows23)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows24)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows25)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows26)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows27)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.Rows28)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows29)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows210)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows211)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows212)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows40)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows41)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows50)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows51)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows52)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows53)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows54)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows55)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows56)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rows57)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Notes)
            .WithOne(x => x.Report)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
    }
}
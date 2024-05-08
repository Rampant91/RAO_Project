using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Collections;

namespace Models.DBRealization.EntityConfiguration;

public class ReportsConfiguration : IEntityTypeConfiguration<Reports>
{
    public void Configure(EntityTypeBuilder<Reports> builder)
    {
        builder.HasOne(x => x.DBObservable)
            .WithMany(x => x.Reports_Collection)
            .HasForeignKey(x => x.DBObservableId)
            .HasConstraintName("FK_ReportsCollection_DbSet_DBO~")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Master_DB)
            .WithMany()
            .HasForeignKey(x => x.Master_DBId)
            .HasConstraintName("FK_ReportsCollection_DbSet_Rep~");

        builder.HasKey(x => x.Id)
            .HasName("PK_ReportsCollection_DbSet");

        builder.HasIndex(x => x.DBObservableId);
        builder.HasIndex(x => x.Master_DBId);

        builder.ToTable("ReportsCollection_DbSet");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Collections;

namespace Models.DBRealization.EntityConfiguration;

public class DbObservableConfiguration : IEntityTypeConfiguration<DBObservable>
{
    public void Configure(EntityTypeBuilder<DBObservable> builder)
    {
        builder.ToTable("DBObservable_DbSet")
            .HasKey(x => x.Id)
            .HasName("PK_DBObservable_DbSet");

        builder.HasMany(x => x.Reports_Collection)
            .WithOne(x => x.DBObservable)
            .HasForeignKey(x => x.DBObservableId)
            .HasConstraintName("FK_ReportsCollection_DbSet_DBO~")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
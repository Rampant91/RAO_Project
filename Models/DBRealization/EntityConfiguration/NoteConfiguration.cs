using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Models.Forms;

namespace Models.DBRealization.EntityConfiguration;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("notes")
            .HasKey(x => x.Id)
            .HasName("PK_notes");

        builder.HasOne(x => x.Report)
            .WithMany(x => x.Notes)
            .HasForeignKey(x => x.ReportId)
            .HasConstraintName("FK_notes_ReportCollection_DbSe~")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
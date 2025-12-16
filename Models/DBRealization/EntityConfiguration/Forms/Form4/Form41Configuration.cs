using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Forms.Form2;
using Models.Forms.Form4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DBRealization.EntityConfiguration.Forms.NewFolder
{
    public class Form41Configuration : IEntityTypeConfiguration<Form41>
    {
        public void Configure(EntityTypeBuilder<Form41> builder)
        {
            builder.ToTable("form_41")
                .HasOne(x => x.Report)
                .WithMany(x => x.Rows41)
                .HasForeignKey(x => x.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

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
    public class Form40Configuration : IEntityTypeConfiguration<Form40>
    {
        public void Configure(EntityTypeBuilder<Form40> builder)
        {
            builder.ToTable("form_40")
                .HasOne(x => x.Report)
                .WithMany(x => x.Rows40)
                .HasForeignKey(x => x.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

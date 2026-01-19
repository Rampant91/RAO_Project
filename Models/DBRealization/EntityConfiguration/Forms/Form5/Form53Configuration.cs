using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Forms.Form2;
using Models.Forms.Form4;
using Models.Forms.Form5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DBRealization.EntityConfiguration.Forms.NewFolder
{
    public class Form53Configuration : IEntityTypeConfiguration<Form53>
    {
        public void Configure(EntityTypeBuilder<Form53> builder)
        {
            builder.ToTable("form_53")
                .HasOne(x => x.Report)
                .WithMany(x => x.Rows53)
                .HasForeignKey(x => x.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

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
    public class Form54Configuration : IEntityTypeConfiguration<Form54>
    {
        public void Configure(EntityTypeBuilder<Form54> builder)
        {
            builder.ToTable("form_54")
                .HasOne(x => x.Report)
                .WithMany(x => x.Rows54)
                .HasForeignKey(x => x.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

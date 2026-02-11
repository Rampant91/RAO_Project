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
    public class Form55Configuration : IEntityTypeConfiguration<Form55>
    {
        public void Configure(EntityTypeBuilder<Form55> builder)
        {
            builder.ToTable("form_55")
                .HasOne(x => x.Report)
                .WithMany(x => x.Rows55)
                .HasForeignKey(x => x.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

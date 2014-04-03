using Kooboo.CMS.ModuleArea.Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kooboo.CMS.ModuleArea.Monitor.Repositories.Default
{
    public class LogMap : EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            this.HasKey(it => it.Id).Property(it => it.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(it => it.Method).IsRequired().HasMaxLength(10);
            this.Property(it => it.Port);
            this.Property(it => it.Referrer);
            this.Property(it => it.ClientIP).IsRequired();
            this.Property(it => it.SiteName).IsRequired();
            this.Property(it => it.Status);
            this.Property(it => it.SubStatus);
            this.Property(it => it.UriQuery);
            this.Property(it => it.UriStem).IsRequired();
            this.Property(it => it.UserAgent);
            this.Property(it => it.UserName);
            this.Property(it => it.DateTime).IsRequired();
            this.Property(it => it.Exception);
            this.Property(it => it.StackTrace).IsMaxLength();
            this.Property(it => it.UniqueVisitorKey);
            this.Property(it => it.VisitCount);
            this.Property(it => it.LastVisitDate);

            this.ToTable("KBM_Monitor_Logs");
        }
    }
}

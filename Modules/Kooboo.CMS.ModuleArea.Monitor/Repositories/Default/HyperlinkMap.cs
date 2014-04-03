using Kooboo.CMS.ModuleArea.Monitor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Repositories.Default
{
    public class HyperlinkMap:EntityTypeConfiguration<Hyperlink>
    {
        public HyperlinkMap()
        {
            this.HasKey(it => it.Id).Property(it => it.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(it => it.UtcVisitDate);
            this.Property(it => it.ParentUrl);
            this.Property(it => it.Status);
            this.Property(it => it.Url).IsRequired();
            this.Property(it => it.VisitTime);
            this.Property(it => it.ContentLength);
            this.Property(it => it.ContentType);
            this.Property(it => it.ResourceType);
            this.Property(it => it.UtcCreationDate);
            this.Property(it => it.Depth);
            this.Property(it => it.ExtractQty);
            this.Property(it => it.ExtractTime);

            this.Ignore(it => it.IsCss);
            this.Ignore(it => it.IsPage);
            this.Ignore(it => it.IsImage);
            this.Ignore(it => it.IsJavascript);

            this.ToTable("KBM_Monitor_Hyperlinks");
        }
    }
}

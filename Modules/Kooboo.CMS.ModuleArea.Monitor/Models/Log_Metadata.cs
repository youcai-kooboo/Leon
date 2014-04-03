using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.ModuleArea.Monitor.Grid;
using Kooboo.CMS.ModuleArea.Monitor.Grid.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    [MetadataFor(typeof(Log))]
    [Grid(IdProperty = "Id",GridItemType=typeof(HighlightGridItem))]
    public class Log_Metadata
    {
        [Display(Name = "Uri stem")]
        [GridColumn(GridItemColumnType = typeof(DetailGridActionItemColumn), GridColumnType = typeof(SortableGridColumn),Order=-10, HeaderText = "Uri stem")]
        public string UriStem { get; set; }

        [Display(Name = "Uri query")]
        [GridColumn(GridColumnType = typeof(SortableGridColumn),Order=-9, HeaderText = "Uri query")]
        public string UriQuery { get; set; }

        [Display(Name = "Visit date")]
        [DisplayFormat(DataFormatString="yyyy-MM-dd HH:mm:ss")]
        [GridColumn(GridItemColumnType = typeof(VisitDateGridItemColumn), GridColumnType = typeof(SortableGridColumn),Order=-8, HeaderText="Visit date")]
        public DateTime DateTime { get; set; }

        [Display(Name = "User name")]
        [GridColumn(GridItemColumnType=typeof(UserNameGridActionItemColumn),GridColumnType = typeof(SortableGridColumn),HeaderText="User name")]
        public string UserName { get; set; }

        [Display(Name = "Client IP")]
        [GridColumn(GridItemColumnType = typeof(ClientIPGridActionItemColumn), GridColumnType = typeof(SortableGridColumn),HeaderText="Client ip")]
        public string ClientIP { get; set; }

        [GridColumn(GridColumnType = typeof(SortableGridColumn))]
        public int Status { get; set; }

        [Display(Name = "Sub status")]
        public int SubStatus { get; set; }

        [Display(Name = "Time taken")]
        [GridColumn(GridColumnType = typeof(SortableGridColumn),GridItemColumnType = typeof(DoubleGridItemColumn),HeaderText="Time taken(s)")]
        public double TimeTaken { get; set; }

        [Display(Name="Visit count")]
        [GridColumn(GridItemColumnType=typeof(VisitCountGridItemColumn),GridColumnType = typeof(SortableGridColumn), Order = -11, HeaderText = "Visit count")]
        public int VisitCount { get; set; }

        [Display(Name = "Last visit date")]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd HH:mm:ss")]
        [GridColumn(GridItemColumnType = typeof(DateTimeGridItemColumn), GridColumnType = typeof(SortableGridColumn),HeaderText="Last visit date")]
        public DateTime? LastVisitDate { get; set; }

        [Display(Name = "Stack trace")]
        public string StackTrace { get; set; }

        [Display(Name = "Search key")]
        public string SearchKey { get; set; }

        [Display(Name = "Referrer domain")]
        public string ReferrerDomain { get; set; }

        [Display(Name = "User agent")]
        public string UserAgent { get; set; }

        [Display(Name = "Site name")]
        public string SiteName { get; set; }
    }
}
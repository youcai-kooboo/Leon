using Kooboo.ComponentModel;
using Kooboo.CMS.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.CMS.ModuleArea.Monitor.Grid;
using Kooboo.CMS.ModuleArea.Monitor.Grid.Hyperlink;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    [MetadataFor(typeof(Hyperlink))]
    [Grid(Checkable=true,IdProperty="Id",GridItemType=typeof(HighlightGridItem))]
    public class Hyperlink_Metadata
    {
        [GridColumn(GridColumnType = typeof(SortableGridColumn),Order = -1)]
        public string Url
        {
            get;
            set;
        }

        [GridColumn(GridColumnType = typeof(SortableGridColumn),HeaderText="Parent url")]
        public string ParentUrl
        {
            get;
            set;
        }

        [GridColumn(GridColumnType = typeof(SortableGridColumn), HeaderText = "Inner text")]
        public string InnerText
        {
            get;
            set;
        }

        [GridColumn(GridColumnType = typeof(SortableGridColumn))]
        public int Depth
        {
            get;
            set;
        }

        [GridColumn(GridColumnType = typeof(SortableGridColumn))]
        public int Status 
        { 
            get;
            set; 
        }

        [GridColumn(GridColumnType = typeof(SortableGridColumn), HeaderText = "Resource type")]
        public string ResourceType
        {
            get;
            set;
        }

        [GridColumn(GridColumnType = typeof(SortableGridColumn),HeaderText="Extract qty")]
        public int ExtractQty
        {
            get;
            set;
        }


        [GridColumn(GridItemColumnType = typeof(DoubleGridItemColumn),HeaderText="Extract time(s)", GridColumnType = typeof(SortableGridColumn))]
        public double ExtractTime
        {
            get;
            set;
        }

        [GridColumn(GridItemColumnType = typeof(DoubleGridItemColumn),HeaderText="Visit time(s)", GridColumnType = typeof(SortableGridColumn))]
        public double VisitTime
        {
            get;
            set;
        }


        [GridColumn(GridItemColumnType = typeof(DateTimeGridItemColumn), GridColumnType = typeof(SortableGridColumn),HeaderText="Visit date")]
        public DateTime? UtcVisitDate
        {
            get;
            set;
        }
    }
}

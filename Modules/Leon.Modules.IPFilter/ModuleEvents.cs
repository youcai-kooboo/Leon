#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.CMS.Form;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Common;


namespace Leon.Modules.IPFilter
{
    public static class ColumnExtensions
    {
        public static Column AddColumn(this Schema schema, string columnName, string controlType, DataType dataType,
          string label = null, bool showInGrid = false, bool summarize = false, string tooltip = null, SelectionSource selectionSource = SelectionSource.ManuallyItems, Kooboo.CMS.Form.SelectListItem[] selectionItems = null)
        {
            var column = new Column()
            {
                Name = columnName,
                Label = label,
                ControlType = controlType,
                DataType = dataType,
                ShowInGrid = showInGrid,
                Summarize = summarize,
                SelectionSource = selectionSource,
                SelectionItems = selectionItems
            };

            schema.AddColumn(column);
            return column;
        }
    }


    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IModuleEvents), Key = ModuleAreaRegistration.ModuleName)]
    public class ModuleEvents : IModuleEvents
    {
        private SchemaManager _schemaManager;
        private TextFolderManager _textFolderManager;
        public ModuleEvents(SchemaManager schemaManager, TextFolderManager textFolderManager)
        {
            this._schemaManager = schemaManager;
            this._textFolderManager = textFolderManager;
        }
        public void OnExcluded(ModuleContext moduleContext)
        {
            var repository = moduleContext.Site.AsActual().GetRepository();
            if (repository != null)
            {
                #region IPSetting
                var ipSettingFolder = new TextFolder(repository, "IPSetting");
                if (ipSettingFolder.AsActual() != null)
                {
                    _textFolderManager.Remove(repository, ipSettingFolder);
                }
                var ipSettingSchema = new Schema(repository, "IPSetting");
                if (ipSettingSchema.AsActual() != null)
                {
                    _schemaManager.Remove(repository, ipSettingSchema);
                }
                
                #endregion

                #region IPList
                var ipListFolder = new TextFolder(repository, "IPList");
                if (ipListFolder.AsActual() != null)
                {
                    _textFolderManager.Remove(repository, ipListFolder);
                }
                var ipListSchema = new Schema(repository, "IPList");
                if (ipListSchema.AsActual() != null)
                {
                    _schemaManager.Remove(repository, ipListSchema);
                } 
                #endregion
            }
        }

        public void OnIncluded(ModuleContext moduleContext)
        {
            var repository = moduleContext.Site.AsActual().GetRepository();
            if (repository != null)
            {
                #region ipSetting
                var ipSettingSchema = new Schema(repository, "IPSetting");
                ipSettingSchema.AddColumn("FilterScope", "DropDownList", DataType.Int, "", true, true, "",
                    SelectionSource.ManuallyItems, new Kooboo.CMS.Form.SelectListItem[]
                    {
                        new Kooboo.CMS.Form.SelectListItem() {Text = "All", Selected = false, Value = "0"},
                        new Kooboo.CMS.Form.SelectListItem() {Text = "Backend", Selected = false, Value = "1"},
                        new Kooboo.CMS.Form.SelectListItem() {Text = "Frontend", Selected = true, Value = "2"}

                    });

                ipSettingSchema.AddColumn("FilterType", "DropDownList", DataType.Int, "", true, true, "",
                    SelectionSource.ManuallyItems, new Kooboo.CMS.Form.SelectListItem[]
                    {
                        new Kooboo.CMS.Form.SelectListItem() {Text = "Whitelist", Selected = true, Value = "0"},
                        new Kooboo.CMS.Form.SelectListItem() {Text = "Blacklist", Selected = false, Value = "1"}
                    });

                ipSettingSchema.AddColumn("ForbiddenHtml", "Tinymce", DataType.String);

                if (ipSettingSchema.AsActual() == null)
                {
                    _schemaManager.Add(repository, ipSettingSchema);
                }

                var ipSettingfolder = new TextFolder(repository, "IPSetting")
                {
                    SchemaName = "IPSetting",
                    Hidden = true
                };
                if (ipSettingfolder.AsActual() == null)
                {
                    _textFolderManager.Add(repository, ipSettingfolder);
                }
                #endregion

                #region IPList
                var ipListSchema = new Schema(repository, "IPList");
                ipListSchema.AddColumn("IP", "TextBox", DataType.String, "", true, true);
                ipListSchema.AddColumn("Description", "TextArea", DataType.String, "", false, true);
                if (ipListSchema.AsActual() == null)
                {
                    _schemaManager.Add(repository, ipListSchema);
                }

                var ipListfolder = new TextFolder(repository, "IPList")
                {
                    SchemaName = "IPList",
                    Hidden = true
                };
                if (ipListfolder.AsActual() == null)
                {
                    _textFolderManager.Add(repository, ipListfolder);
                } 
                #endregion
            }
        }


        public void OnInstalling(ModuleContext moduleContext, ControllerContext controllerContext)
        {
            // Add code here that will be executed when the module installing.
            // Installing UI template is defined in the module.config
        }

        public void OnUninstalling(ModuleContext moduleContext, ControllerContext controllerContext)
        {
            // Add code here that will be executed when the module uninstalling.
            // To use custom UI during uninstalling, define the view location in the module.config
        }
        public void OnReinstalling(ModuleContext moduleContext, ControllerContext controllerContext, Kooboo.CMS.Sites.Extension.ModuleArea.Management.InstallationContext installationContext)
        {
            // Add code here that will be executed when the module reinstalling.
        }
    }
}

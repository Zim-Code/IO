﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

using ZimCode.IO;

namespace ZimCode.IO.Platforms.Windows
{
    /// <summary>
    /// Open file dialog extensions.
    /// </summary>
    public static class OpenFileDialogExtensions
    {
        /// <summary>
        /// Adds the importer file types to the dialog.
        /// </summary>
        /// <returns>This <see cref="System.Windows.Forms.FileDialog"/> for method chaining.</returns>
        /// <param name="dialog">Dialog to add the import filters to.</param>
        /// <param name="importers">Importers to add to the filter.</param>
        public static T AddImporterFileTypes<T>(this T dialog, params BaseImporter[] importers) where T : FileDialog
        {
            dialog.Filter = GetFilterString(dialog.Filter, importers);
            return dialog;
        }

        /// <summary>
        /// Get a filter string for a FileDialog.
        /// </summary>
        /// <param name="initialFilter">Initial import filter.</param>
        /// <param name="importers">The importers to add to the initialFilter string.</param>
        /// <returns></returns>
        public static string GetFilterString(string initialFilter, IEnumerable<BaseImporter> importers)
        {
            string filterResult = string.IsNullOrWhiteSpace(initialFilter) ? "" : initialFilter + "|";

            string importerSeparator = "";
            foreach (var importer in importers)
            {
                filterResult += importerSeparator;

                string filterSeparator = "";
                filterResult += importer.Description + " (";
                foreach (var extension in importer.FileExtensions)
                {
                    filterResult += filterSeparator + "*." + extension;
                    filterSeparator = ",";
                }
                filterResult += ")";

                filterSeparator = "";
                filterResult += "|";
                foreach (var extension in importer.FileExtensions)
                {
                    filterResult += filterSeparator + "*." + extension;
                    filterSeparator = ";";
                }
                importerSeparator = "|";
            }

            return filterResult;
        }
    }
}


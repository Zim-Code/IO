using System;
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
            string filterResult = string.IsNullOrWhiteSpace(dialog.Filter) ? "" : dialog.Filter + "|";

            string importerSeparator = "";
            foreach (var importer in importers)
            {
                dialog.Filter += importerSeparator;

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

            dialog.Filter = filterResult;
            return dialog;
        }
    }
}


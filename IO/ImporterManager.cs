using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace ZimCode.IO
{
    /// <summary>
    /// Importer manager used to track importers across an application and
    /// get an importer for a file extension.
    /// </summary>
    public class ImporterManager
    {
        readonly ReadOnlyObservableCollection<BaseImporter> importersReadOnly;
        readonly ObservableCollection<BaseImporter> importers = new ObservableCollection<BaseImporter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ZimCode.IO.ImporterManager"/> class.
        /// </summary>
        public ImporterManager()
        {
            importersReadOnly = new ReadOnlyObservableCollection<BaseImporter>(importers);
        }

        /// <summary>
        /// Adds the importer to the manager.
        /// </summary>
        /// <param name="importer">Importer.</param>
        public void AddImporter(BaseImporter importer)
        {
            importers.Add(importer);
        }

        /// <summary>
        /// Gets the importers.
        /// </summary>
        /// <value>The importers.</value>
        public ReadOnlyCollection<BaseImporter> Importers
        {
            get { return importersReadOnly; }
        }

        /// <summary>
        /// Gets the importer for file extension.
        /// </summary>
        /// <returns>The importer for file extension.</returns>
        /// <param name="fileExtension">The file extension to get an importer for.</param>
        public BaseImporter GetImporterForFileExtension(string fileExtension)
        {
            fileExtension = fileExtension.ToUpper();
            return importers.FirstOrDefault(i => i.FileExtensions.Contains(fileExtension));
        }
    }
}


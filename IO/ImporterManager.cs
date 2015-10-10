using System;
using System.Collections.Generic;
using System.Linq;

namespace ZimCode.IO
{
    public class ImporterManager
    {
        readonly List<BaseImporter> importers = new List<BaseImporter>();

        public ImporterManager()
        {
        }

        public void AddImporter(BaseImporter importer)
        {
            importers.Add(importer);
        }

        public BaseImporter GetImporterForFileExtension(string fileExtension)
        {
            fileExtension = fileExtension.ToUpper();
            return importers.FirstOrDefault(i => i.FileExtensions.Contains(fileExtension));
        }
    }
}


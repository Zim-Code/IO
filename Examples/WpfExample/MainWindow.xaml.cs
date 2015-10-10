using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using ZimCode.IO;
using ZimCode.IO.Platforms.Windows;

namespace WpfExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImporterManager importManager = new ImporterManager();

        public ObservableCollection<ViewModel> ViewModels { get; } = new ObservableCollection<ViewModel>();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            importManager.AddImporter(new ExampleImporter());
        }

        private void OpenClicked(object sender, RoutedEventArgs e)
        {
            // Create the file dialog with the filter string from the importers.
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = OpenFileDialogExtensions.GetFilterString(dialog.Filter, importManager.Importers);
            dialog.InitialDirectory = Assembly.GetEntryAssembly().Location;

            // Show the open file dialog.
            if (dialog.ShowDialog() == true)
            {
                // If we opened a file, try to get an importer for it.
                var importer = importManager.GetImporterForFileExtension(Path.GetExtension(dialog.FileName).Substring(1));

                if (importer == null)
                {
                    MessageBox.Show("No importer was found.", "Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Create a stream to the file.
                var fileStream = File.OpenRead(dialog.FileName);

                ViewModels.Add(new ViewModel(importer, fileStream));
            }
        }
    }
}

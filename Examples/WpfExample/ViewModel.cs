using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimCode.IO;

namespace WpfExample
{
    public class ViewModel : INotifyPropertyChanged
    {
        string label;
        string value;
        ProgressReporter reporter = new ProgressReporter(100);

        public ViewModel(BaseImporter importer, Stream fileStream)
        {
            reporter.LabelChanged += OnChange;
            reporter.ProgressChanged += OnChange;

            Load(importer, fileStream);
        }

        private void OnChange(object sender, EventArgs e)
        {
            Label = $"{reporter.Progress}% | {reporter.Label}";
        }

        async void Load(BaseImporter importer, Stream fileStream)
        {
            Value = (await importer.ImportAsync(fileStream, reporter)).ToString();
        }

        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                OnPropertyChanged(nameof(Label));
            }
        }

        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

using System;

namespace ZimCode.IO
{
    public class ProgressReporter
    {
        double maxProgress;
        double progress = 0.0;

        public ProgressReporter(double maxProgress = 1.0)
        {
            this.maxProgress = maxProgress;
        }

        public double Progress
        {
            get { return progress; }
            private set
            {
                if (progress == value)
                    return;
                progress = value;
                OnProgressChanged(new EventArgs());
            }
        }

        internal void SetProgress(double progress)
        {
            Progress = progress * maxProgress;
        }

        protected virtual void OnProgressChanged(EventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }

        public event EventHandler ProgressChanged;
    }
}


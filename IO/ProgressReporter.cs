using System;

namespace ZimCode.IO
{
    /// <summary>
    /// Progress reporter is used to provide feedback on an async method.
    /// </summary>
    public class ProgressReporter
    {
        string label;
        double maxProgress;
        double progress = 0.0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZimCode.IO.ProgressReporter"/> class.
        /// </summary>
        /// <param name="maxProgress">Max progress.</param>
        public ProgressReporter(double maxProgress = 1.0)
        {
            this.maxProgress = maxProgress;
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label
        {
            get { return label; }
            internal set
            {
                if (label == value) return;
                label = value;
                OnLabelChanged(new EventArgs());
            }
        }

        /// <summary>
        /// Gets the progress.
        /// </summary>
        /// <value>The progress.</value>
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

        /// <summary>
        /// Gets a value indicating whether the consuming method completed without error.
        /// </summary>
        /// <value><c>true</c> if completed without error; otherwise, <c>false</c>.</value>
        public bool CompletedWithoutError { get; private set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; private set; }

        internal void SetLabel(string label)
        {
            Label = label;
        }

        internal void SetCompletedWithoutError(bool value)
        {
            CompletedWithoutError = value;
        }

        internal void SetErrorMessage(string message)
        {
            ErrorMessage = message;
        }

        internal void SetProgress(double progress)
        {
            Progress = maxProgress * progress;
        }

        /// <summary>
        /// Raises the label changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected virtual void OnLabelChanged(EventArgs e)
        {
            LabelChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the progress changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected virtual void OnProgressChanged(EventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when label changed.
        /// </summary>
        public event EventHandler LabelChanged;

        /// <summary>
        /// Occurs when progress changed.
        /// </summary>
        public event EventHandler ProgressChanged;
    }
}


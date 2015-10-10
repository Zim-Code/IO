using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ZimCode.IO
{
    /// <summary>
    /// Base importer, you should not extend this yourself. This is for internal use.
    /// 
    /// Extend <see cref="ZimCode.IO.BaseImporter{TResult}"/>
    /// </summary>
    public abstract class BaseImporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZimCode.IO.BaseImporter"/> class.
        /// </summary>
        /// <param name="description">Description.</param>
        /// <param name="fileExtensions">File extensions.</param>
        protected BaseImporter(string description, params string[] fileExtensions)
        {
            Description = description;
            FileExtensions = fileExtensions.Select(e => e.ToUpper());
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the file extensions.
        /// </summary>
        /// <value>The file extensions.</value>
        public IEnumerable<string> FileExtensions { get; private set; }

        /// <summary>
        /// Run the import operations.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="stream">Stream.</param>
        /// <param name="reporter">Reporter.</param>
        public abstract Task<object> ImportAsync(Stream stream, ProgressReporter reporter = null);

        /// <summary>
        /// Sets if ImportAsync completed without error.
        /// </summary>
        /// <param name="reporter">Reporter.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        protected void SetCompletedWithoutError(ProgressReporter reporter, bool value)
        {
            reporter?.SetCompletedWithoutError(value);
        }

        /// <summary>
        /// Sets the error message.
        /// </summary>
        /// <param name="reporter">Reporter.</param>
        /// <param name="message">Message.</param>
        protected void SetErrorMessage(ProgressReporter reporter, string message)
        {
            reporter?.SetErrorMessage(message);
        }

        /// <summary>
        /// Sets the progress.
        /// </summary>
        /// <param name="reporter">Reporter.</param>
        /// <param name="progress">Progress.</param>
        protected void SetProgress(ProgressReporter reporter, double progress)
        {
            reporter?.SetProgress(progress);
        }
    }

    /// <summary>
    /// Base importer to extend for creating Importers.
    /// </summary>
    public abstract class BaseImporter<TResult> : BaseImporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZimCode.IO.BaseImporter{TResult}"/> class.
        /// </summary>
        /// <param name="description">Description.</param>
        /// <param name="fileExtensions">File extensions.</param>
        public BaseImporter(string description, params string[] fileExtensions)
            : base(description, fileExtensions)
        {
        }

        /// <summary>
        /// Gets a new loader to execute the operations from.
        /// </summary>
        /// <returns>The loader.</returns>
        /// <param name="stream">Stream.</param>
        protected abstract IGetOperations GetLoader(Stream stream);

        /// <summary>
        /// Run the import operations.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="stream">Stream.</param>
        /// <param name="reporter">Reporter.</param>
        public override async Task<object> ImportAsync(Stream stream, ProgressReporter reporter = null)
        {
            try
            {
                IEnumerable<Operation> operations = GetLoader(stream).GetOperations();

                if (operations == null)
                    throw new NullReferenceException("No loading tasks were provided by GetLoader()");

                Operation o = operations.LastOrDefault();
                if (!o.IsValidReturnValue(typeof(TResult)))
                    throw new Exception($"The last operation in the loader must be a Generate operation with the result type of {typeof(TResult).FullName}.");

                int operationCount = operations.Count();
                int operationsCompleted = 0;
                object result = null;
                foreach (Operation operation in operations)
                {
                    result = await operation.ExecuteAsync(result);
                    SetProgress(reporter, operationCount / ++operationsCompleted);
                }
                TResult resultCast = (TResult)result;
                SetCompletedWithoutError(reporter, true);
                return resultCast;
            }
            catch (ImportException importException)
            {
                SetErrorMessage(reporter, importException.Message);
                return default(TResult);
            }
            #if DEBUG
            catch (Exception e)
            #else
            catch (Exception)
            #endif
            {
                #if DEBUG
                SetErrorMessage(reporter, e.Message + "\n" + e.StackTrace);
                #endif
                return default(TResult);
            }
        }
    }

    /// <summary>
    /// Used to abort the import process from an operationwith a message that 
    /// is set to the ProgressReporter used to run the Importer if it exists.
    /// </summary>
    public class ImportException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZimCode.IO.ImportException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public ImportException(string message)
            : base(message)
        {
        }
    }
}


using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ZimCode.IO
{
    public abstract class BaseImporter
    {
        protected BaseImporter(string description, params string[] fileExtensions)
        {
            Description = description;
            FileExtensions = fileExtensions.Select(e => e.ToUpper());
        }

        public string Description { get; private set; }

        public IEnumerable<string> FileExtensions { get; private set; }

        public abstract Task<object> ImportAsync(Stream stream, ProgressReporter reporter = null);

        protected void SetProgress(ProgressReporter reporter, double progress)
        {
            reporter?.SetProgress(progress);
        }
    }

    public abstract class BaseImporter<TResult> : BaseImporter
    {
        public BaseImporter(string description, params string[] fileExtensions)
            : base(description, fileExtensions)
        {
        }

        protected abstract IGetOperations GetLoader(Stream stream);

        public bool CompletedWithoutError { get; private set; }

        public string ErrorMessage { get; private set; }

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
                CompletedWithoutError = true;
                return resultCast;
            }
            catch (ImportException importException)
            {
                ErrorMessage = importException.Message;
                return default(TResult);
            }
            catch (Exception e)
            {
                #if DEBUG
                ErrorMessage = e.Message + "\n" + e.StackTrace; 
                #endif
                return default(TResult);
            }
        }
    }

    public class ImportException : Exception
    {
        public ImportException(string message)
            : base(message)
        {
        }
    }
}


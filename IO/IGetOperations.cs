using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZimCode.IO
{
    /// <summary>
    /// Used to provide a list of operations to execute in order.
    /// </summary>
    public interface IGetOperations
    {
        /// <summary>
        /// Gets the operations.
        /// </summary>
        /// <returns>The operations to execute in order.</returns>
        IEnumerable<Operation> GetOperations();
    }
}


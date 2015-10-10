using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZimCode.IO
{
    public interface IGetOperations
    {
        IEnumerable<Operation> GetOperations();
    }
}


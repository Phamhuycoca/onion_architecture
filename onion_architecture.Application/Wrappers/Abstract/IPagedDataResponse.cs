using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Application.Wrappers.Abstract
{
    interface IPagedDataResponse<T> : IResponse
    {
        int TotalItems { get; }
        List<T> Data { get; }
    }
}

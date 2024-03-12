using onion_architecture.Application.Wrappers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Application.Wrappers.Concrete
{
    public class PagedDataResponse<T> : IPagedDataResponse<T>
    {
        public bool Success { get; } = true;
        public int TotalItems { get; }

        public List<T> Data { get; }

        public int StatusCode { get; }

        public PagedDataResponse(List<T> data, int statuscode, int totalitems)
        {
            Data = data;
            StatusCode = statuscode;
            TotalItems = totalitems;
        }
    }
}

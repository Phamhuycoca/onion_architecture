using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Application.Wrappers.Abstract
{
    public interface IErrorResponse : IResponse
    {
        List<string> Errors { get; }
    }
}

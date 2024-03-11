using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Domain.Base
{
    public class BaseEntity
    {
        public long createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public long updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public long deletedBy { get; set; }
        public DateTime deletedAt { get; set; }
        public bool IsDelete { get; set; }

    }
}

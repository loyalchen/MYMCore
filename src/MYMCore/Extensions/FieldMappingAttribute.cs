using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYMCore.Extensions {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class FieldMappingAttribute : Attribute {
        public string FieldName { get; set; }

        public int OrderId { get; set; }
    }
}

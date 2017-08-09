using System;

namespace MYMCore.Extensions {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FieldMappingAttribute : Attribute {
        public FieldMappingAttribute() {
                
        }
        public FieldMappingAttribute(string fieldName, int orderId) {
            FieldName = fieldName;
            OrderId = orderId;
        }
        public string FieldName { get; set; }

        public int OrderId { get; set; }
    }
}

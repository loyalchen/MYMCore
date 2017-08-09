using System;

namespace MYMCore.Behavioral.Compare {
    /// <summary>
    /// Indicates the property of filed should be campared.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ComparePropertyAttribute: Attribute {
    }
}

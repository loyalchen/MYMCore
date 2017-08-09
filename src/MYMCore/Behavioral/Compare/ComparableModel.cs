using System;
using System.Runtime.Serialization;

namespace MYMCore.Behavioral.Compare {
    [Serializable]
    [DataContract]
    public class ComparableModel {
        /// <summary>
        /// The identity code for the object.
        /// </summary>
        [DataMember(Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// True indicates the object has been logical cancelled;false not logical cancelled.
        /// </summary>
        [DataMember]
        [CompareProperty]
        public bool LCV { get; set; }
    }
}

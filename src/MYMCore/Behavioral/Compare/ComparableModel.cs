using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MYMCore.Behavioral.Compare {
    [Serializable]
    [DataContract]
    public class ComparableModel {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember]
        [CompareProperty]
        public bool LCV { get; set; }
    }
}

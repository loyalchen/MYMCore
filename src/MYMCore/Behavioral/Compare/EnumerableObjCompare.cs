using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MYMCore.Behavioral.Compare {
    public class EnumerableObjCompare<T> : ObjCompare
        where T : ComparableModel, new() {
        private readonly Dictionary<int, string> _originalVersion;
        private readonly Dictionary<int, bool> _originalLCV;
        private readonly BindingFlags _bindingFlags = BindingFlags.Public | BindingFlags.Instance;

        public EnumerableObjCompare() {
            _originalVersion = new Dictionary<int, string>();
            _originalLCV = new Dictionary<int, bool>();
        }

        /// <summary>
        /// Record the original datasouce.
        /// </summary>
        /// <param name="dataSource">A collection of to be recorded</param>
        public void RecordOriginalSourceVersion(IEnumerable<T> dataSource) {
            _originalVersion.Clear();
            _originalLCV.Clear();

            foreach (var item in dataSource) {
                _originalVersion.Add(item.Id, AnalyseOriginalVersion(item, _bindingFlags));
                _originalLCV.Add(item.Id, item.LCV);
            }
        }

        /// <summary>
        /// Item1: Insert
        /// Item2: Delete
        /// Item3: Update
        /// </summary>
        /// <param name="dataSource">The collection need to be compared with original.</param>
        /// <returns></returns>
        public Tuple<IEnumerable<T>, IEnumerable<T>, IEnumerable<T>> AnalyseBatchChange(IEnumerable<T> dataSource) {
            var newRecords = new List<T>();
            var deleteRecords = new List<T>();
            var modifyRecords = new List<T>();

            var dsList = dataSource as IList<T> ?? dataSource.ToList();
            foreach (var item in dsList) {
                if (item.Id < 0) {
                    newRecords.Add(item);
                    continue;
                }
                if (item.LCV && !_originalLCV[item.Id]) {
                    deleteRecords.Add(item);
                    continue;
                }
                if (_originalVersion[item.Id] != AnalyseOriginalVersion(item, _bindingFlags)) {
                    modifyRecords.Add(item);
                }
            }

            deleteRecords.AddRange(from item in _originalVersion
                where dsList.All(c => c.Id != item.Key)
                select new T
                {
                    LCV = true,
                    Id = item.Key
                });
            return Tuple.Create<IEnumerable<T>, IEnumerable<T>, IEnumerable<T>>(newRecords, deleteRecords, modifyRecords);
        }

        /// <summary>
        /// Get the change data compared with origianl data.
        /// </summary>
        /// <param name="dataSource">The collection need to be compared with original.</param>
        /// <returns>The change details list.</returns>
        public IEnumerable<T> AnalyseChange(IEnumerable<T> dataSource) {
            var data = AnalyseBatchChange(dataSource);
            var result = new List<T>();
            result.AddRange(data.Item1);
            result.AddRange(data.Item2);
            result.AddRange(data.Item3);

            return result;
        }
    }
}

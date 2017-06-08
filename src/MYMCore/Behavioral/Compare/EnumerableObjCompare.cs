using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MYMCore.Behavioral.Compare {
    public class EnumerableObjCompare<T> : ObjCompare
        where T : ComparableModel, new() {
        private Dictionary<int, string> _originalVersion;
        private Dictionary<int, bool> _originalLCV;
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
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T>, IEnumerable<T>, IEnumerable<T>> AnalyseBatchChange(IEnumerable<T> dataSource) {
            var newRecords = new List<T>();
            var deleteRecords = new List<T>();
            var modifyRecords = new List<T>();

            foreach (var item in dataSource) {
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

            foreach (var item in _originalVersion) {
                if (!dataSource.Any(c => c.Id == item.Key)) {
                    var unused = new T();
                    unused.LCV = true;
                    unused.Id = item.Key;
                    deleteRecords.Add(unused);
                }
            }
            return Tuple.Create<IEnumerable<T>, IEnumerable<T>, IEnumerable<T>>(newRecords, deleteRecords, modifyRecords);
        }

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

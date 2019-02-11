using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.Interfaces
{
    public interface IMapper : Orleans.IGrainWithGuidKey
    {
        //Function declaration
        Task<AcceloDataPerSec> MapAccelo(string document);
    }

    [Serializable]
    public class AcceloData
    {
        public AcceloData(string key, int count, double average)
        {
            Key = key;
            Count = count;
            Average = average;

        }

        public readonly string Key;
        public int Count { get; set; }
        public double Average { get; set; }

        public override string ToString() =>
                $"{Key}: {Count} {Average/ Count}";
    }

    [Serializable]
    public class AcceloDataPerSec : KeyedCollection<string, AcceloData>
    {
        public AcceloDataPerSec() { }
        public AcceloDataPerSec(IEnumerable<AcceloData> items)
        {
            foreach (var item in items)
            {
                base.Add(item);
            }
        }

        protected override string GetKeyForItem(AcceloData item) =>
            item.Key;

        public void MergeAccelo(AcceloDataPerSec items)
        {
            foreach (var item in items)
            {
                if (Contains(item.Key))
                {
                    var val = base[item.Key];
                    val.Count += item.Count;
                    val.Average += item.Average;
                }
                else
                    base.Add(item);
            }
        }

        public override string ToString() =>
            string.Join("\r\n", this);
    }
}

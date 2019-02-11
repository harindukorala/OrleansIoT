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
        Task<string> SayHello(string greeting);
        Task<Pairs> MapAsync(string document);
        Task<AcceloDataPerSec> MapAccelo(string document);
    }

    [Serializable]
    public class Pair
    {
        public Pair(string key, int value)
        {
            Key = key;
            Value = value;
        }

        public readonly string Key;
        public int Value { get; set; }

        public override string ToString() =>
                $"{Key}: {Value}";
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
    public class Pairs : KeyedCollection<string, Pair>
    {
        public Pairs() { }
        public Pairs(IEnumerable<Pair> items)
        {
            foreach (var item in items)
            {
                base.Add(item);
            }
        }

        protected override string GetKeyForItem(Pair item) =>
            item.Key;

        public void Merge(Pairs items)
        {
            foreach (var item in items)
            {
                Console.WriteLine("\n Inside Merge : State Items", items.ToString());
                Console.WriteLine("\n Inside Merge : State Items",item.ToString());

                if (Contains(item.Key))
                {
                    var val = base[item.Key];
                    val.Value += item.Value;
                }
                else
                    base.Add(item);
            }
        }

        public override string ToString() =>
            string.Join("\r\n", this);
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

using System.Collections.Generic;

namespace cdi
{
    /// <summary>
    /// Random lần lượt các số từ 0 đến count-1
    /// </summary>
    public class SequenceRandom
    {
        List<int> indexes = new List<int>();
        int count;

        public SequenceRandom(int count)
        {
            this.count = count;
            createArray();
        }

        void createArray()
        {
            for (int i = 0; i < count; i++)
            {
                indexes.Add(i);
            }
        }
        public void Reset(int newCount = 0)
        {
            if (newCount > 0) this.count = newCount;
            indexes.Clear();
            createArray();
        }

        public bool IsReady
        {
            get
            {
                return indexes.Count > 0;
            }
        }

        public int RandomPop()
        {
            if (!IsReady) CLog.Err(this, "Popped all items");
            var res = UnityEngine.Random.Range(0, indexes.Count);
            var value = indexes[res];
            indexes.RemoveAt(res);
            return value;
        }
    }
}

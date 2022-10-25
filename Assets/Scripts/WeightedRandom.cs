using System;
using System.Collections.Generic;

namespace Detection
{
    class WeightedRandom<T>
    {
        private struct Element
        {
            public double weight;
            public T item;
        }

        private List<Element> weightedList = new List<Element>();
        private double totalWeight;
        private Random random = new Random();

        public void Add(T item, double weight)
        {
            totalWeight += weight;
            weightedList.Add(new Element { item = item, weight = totalWeight });
        }

        public T GetRandomWeighted()
        {
            double rand = random.NextDouble() * totalWeight;

            foreach (Element entry in weightedList)
            {
                if (entry.weight >= rand)
                {
                    return entry.item;
                }
            }
            return default(T);
        }
    }
}
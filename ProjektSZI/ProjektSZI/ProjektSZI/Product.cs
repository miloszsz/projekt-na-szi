using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjektSZI
{
    class Product
    {
        public readonly int weight;
        public readonly int value;
        public readonly int typeIndex;
        public readonly String type;

        public Product(int typeIndex)
        {
            this.typeIndex = typeIndex;
            this.type = Fuzzy.nazwa[typeIndex];
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            this.value = rnd.Next(1, 100);
            this.weight = rnd.Next(1, 38);
        }

        public String ShelfLabel()
        {
            return type + ", w:" + weight + ", v:" + value;
        }
    }
}

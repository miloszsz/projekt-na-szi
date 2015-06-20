using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ProjektSZI
{
    class Knapsack
    {
        private static readonly int POPULATION_COUNT = 8;
        private static readonly int KNAPSACK_CAPACITY = 80;
        private readonly List<Product> shelf;
        private List<BitArray> newGeneration;
        private List<BitArray> oldGeneration;


        public Knapsack(List<Product> shelf)
        {
            newGeneration = new List<BitArray>(POPULATION_COUNT)
            {
                new BitArray(7),
                new BitArray(7),
                new BitArray(7),
                new BitArray(7),
                new BitArray(7),
                new BitArray(7),
                new BitArray(7),
                new BitArray(7)
            };
            //newGeneration.ForEach(bits => bits = new BitArray(7));
            oldGeneration = new List<BitArray>(POPULATION_COUNT)
            {
                new BitArray(7),
                new BitArray(7),
                new BitArray(7),
                new BitArray(7),
                new BitArray(7),
                new BitArray(7),
                new BitArray(7),
                new BitArray(7)
            };
            //oldGeneration.ForEach(bits => bits = new BitArray(7));
            this.shelf = shelf;
        }

        public List<String> getSolution()
        {
            do
            {
                initEvolution();
            } while (newGeneration.Count < 2);
            evolutionStep();
            evolutionStep();
            List<String> result = new List<String>();
            for (int i = 0; i < newGeneration[0].Count; i++)
            {
                if (newGeneration[0][i])
                {
                    result.Add(i + "." + shelf[i].type);
                }
            }
            return result;
        }

        private void initEvolution()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for(int i = 0; i < newGeneration.Capacity; i++)
            {    
                //for (int j = 0; j < newGeneration[i].Count; j++)
                for (int j = 0; j < 7; j++)
                {
                    newGeneration[i][j] = rnd.NextDouble() < 0.5;
                }
            }
            sortGenerationByValue();
            limitAndRefillGeneration();
        }

        private void evolutionStep()
        {
            for (int i = 0; i < newGeneration.Count; i++)
            {
                oldGeneration[i] = newGeneration[i];
            }
            newGeneration.Clear();
            for (int i = 0; i < oldGeneration.Count - 1; i += 2)
            {
                newGeneration.AddRange(crossoverGenes(oldGeneration[i], oldGeneration[i + 1]));
            }

            sortGenerationByValue();
            limitAndRefillGeneration();
        }

        private void limitAndRefillGeneration()
        {
            newGeneration.RemoveAll(gene => !fitsInKnapsack(gene));
            int counter = 0;
            while (newGeneration.Count < POPULATION_COUNT && counter < newGeneration.Count)
            {
                newGeneration.Add(newGeneration[counter]);
                counter++;
            }
            for (int i = POPULATION_COUNT / 2; i < newGeneration.Count; i++)
            {
                newGeneration[i] = newGeneration[i - POPULATION_COUNT / 2];
            }
        }

        private List<BitArray> crossoverGenes(BitArray gene1, BitArray gene2)
        {
            List<BitArray> result = new List<BitArray>();
            if (gene1.Count != gene2.Count || gene1.Count != 7)
            {
                result.Add(gene1);
                result.Add(gene2);
            }
            else
            {
                BitArray cross1 = new BitArray(gene1.Count);
                BitArray cross2 = new BitArray(gene1.Count);
                for (int i = 0; i < 4; i++)
                {
                    cross1[i] = gene1[i];
                    cross2[i] = gene2[i];
                }
                for (int i = 4; i < 7; i++)
                {
                    cross1[i] = gene2[i];
                    cross2[i] = gene1[i];
                }
                result.Add(cross1);
                result.Add(cross2);
            }
            return result;
        }

        private void sortGenerationByValue()
        {
            newGeneration.Sort(delegate(BitArray gene1, BitArray gene2)
                {
                    return getSolutionValue(gene2).CompareTo(getSolutionValue(gene1));
                });
        }

        private bool fitsInKnapsack(BitArray solution)
        {
            int suma = 0;
            for (int i = 0; i < solution.Count; i++)
            {
                if (solution[i])
                {
                    suma += shelf[i].weight;
                }
            }
            return suma <= KNAPSACK_CAPACITY;
        }

        private int getSolutionValue(BitArray solution)
        {
            int suma = 0;
            for (int i = 0; i < solution.Count; i++)
            {
                suma += solution[i] ? shelf[i].value : 0;
            }
            return suma;
        }
    }
}

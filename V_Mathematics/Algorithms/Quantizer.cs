using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Data;

namespace Vulpine.Core.Calc.Algorithms
{
    public class Quantizer : Algorithm
    {
        private ResultMulti<Vector> KMeans(Int32 dim, IEnumerable<Vector> data, params Vector[] means)
        {
            //creates a KD tree for fast look-up of the mean values
            TreeVector<Int32> current = new TreeKD<Int32>(dim);
            Vector[] update = new Vector[means.Length];

            //NOTE: Should we consider using a ball-tree 
            //for when both K and D are large?

            for (int i = 0; i < means.Length; i++)
            {
                //enshures that all the starting points are valid
                if (means[i].Length != dim) throw new ArgumentException();
                Vector temp = new Vector(means[i]);

                //initialises the current and update data sets
                current.Add(means[i], i);
                update[i] = new Vector(dim);
            }

            Initialise();

            while (true)
            {
                //builds the KD tree structor
                current.Build();
                int count = 0;

                double motion = 0.0;

                //sorts the datapoints by the closest mean
                //and computes the new mean from the datapoints
                foreach (Vector x in data)
                {
                    int i = current.GetNearest(x).Value;
                    Vector delta = x - update[i];

                    count += 1;
                    update[i] += delta / count;
                }

                //computes the amount of motion for each pair of means
                foreach (var pair in current)
                {
                    int i = pair.Value;
                    Vector prev = pair.Location;
                    motion += prev.Dist(update[i]);
                }

                //clears the existing KD-Tree
                current.Clear();

                //we end the process once the means are no longer moving
                if (Step(motion)) break;

                //adds the new means for the next itteration 
                for (int i = 0; i < update.Length; i++)
                {
                    current.Add(update[i], i);
                    update[i] = new Vector(dim);
                }
            }

            //returns the latest set of update mean positons
            return Finish(update.AsEnumerable());
        }
    }
}

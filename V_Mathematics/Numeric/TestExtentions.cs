using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Numeric
{
    public static class TestExtentions
    {
        public static Result<Double> Converge
            (this IEnumerable<Double> source, double tol, int cutoff)
        {
            double error = Double.PositiveInfinity;
            double last = 0.0;
            int step = 0;

            using (var iter = source.GetEnumerator())
            {
                //checks for an empty sequence
                if (!iter.MoveNext()) return default(Result<Double>);

                //sets the last value before itterating
                last = iter.Current;

                while (iter.MoveNext())
                {
                    //increments the step count
                    step = step + 1;

                    //computes the error value
                    double dist = iter.Current - last;
                    dist = dist / last;
                    error = Math.Abs(dist);

                    //checkes if stoping conditions are met
                    if (error < tol || step > cutoff)
                        return new Result<Double>(iter.Current, error);

                    //updates the last value
                    last = iter.Current;
                }
            }

            //returns the last value and error computed
            return new Result<double>(last, error);
        }

        public static IEnumerable<Result<Double>> Calculate
            (this IEnumerable<Double> source, double tol, int cutoff)
        {
            double error = Double.PositiveInfinity;
            double last = 0.0;
            int step = 0;

            using (var iter = source.GetEnumerator())
            {
                //checks for an empty sequence
                if (!iter.MoveNext()) yield break;

                //sets the last value before itterating
                last = iter.Current;

                while (iter.MoveNext())
                {
                    //increments the step count
                    step = step + 1;

                    //computes the error value
                    double dist = iter.Current - last;
                    dist = dist / last;
                    error = Math.Abs(dist);

                    yield return new Result<Double>(iter.Current, error);

                    //checkes if stoping conditions are met
                    if (error < tol || step > cutoff) yield break;

                    //updates the last value
                    last = iter.Current;
                }
            }
        }

        public static IEnumerable<Double> Bisection(VFunc f, double low, double high)
        {
            yield break;
        }

        public static void TestCode()
        {
            VFunc f = x => (x * x) - 3;
            RootFinder rf = new RootFinder();

            //method call using the itterator method
            double result = TestExtentions.Bisection(f, 1.0, 3.0)
                .Calculate(RootFinder.DTOL, 10000).Last();

            //method call using the Algorithim method
            double result2 = rf.Bisection(f, 1.0, 3.0);

            //listing the steps with the itterator method
            var numbers = TestExtentions.Bisection(f, 1.0, 3.0)
                .Calculate(RootFinder.DTOL, 10000);

            foreach (var pair in numbers)
            {
                Console.WriteLine("{0}, Err: {1}", pair.Value, pair.Error);
            }

            //listing the steps with the Algorithim method
            Action<double, double> lister = (val, err) =>
                Console.WriteLine("{0}, Err: {1}", val, err);

            //rf.OnStep += lister;
            double result3 = rf.Bisection(f, 1.0, 3.0);

            

            
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Algorithms
{
    public static class Limits
    {
        public static int MAX = 1024;

        public static IEnumerable<Double> Limit(this IEnumerable<Double> source)
        {
            //calls the expanded method
            return source.LimitRel(VMath.TOL, MAX);
        }

        public static IEnumerable<T> Limit<T>(this IEnumerable<T> source)
            where T : Metrizable<T>
        {
            //calls the expanded method
            return source.LimitRel(VMath.TOL, MAX);
        }

        public static IEnumerable<T> Limit<T>(this IEnumerable<T> source,
            Metric<T> met)
        {
            //calls the expanded method
            return source.LimitAbs(met, VMath.TOL, MAX);
        }

        public static IEnumerable<T> Limit<T>(this IEnumerable<T> source,
            Metric<T> met, T zero)
        {
            //calls the expanded method
            return source.LimitRel(met, zero, VMath.TOL, MAX);
        }




        #region Limit Relitive...

        public static IEnumerable<Double> LimitRel(this IEnumerable<Double> source,
            double tol, int max)
        {
            //checks that we don't have a null source
            if (source == null) throw new ArgumentNullException("source");

            //used in the itterative loop
            double err = Double.PositiveInfinity;
            double curr, last;

            using (var ittr = source.GetEnumerator())
            {
                //if there are no elements, return an empty list
                if (!ittr.MoveNext()) yield break;

                //obtains the last element and return it
                last = ittr.Current;
                yield return last;

                while (ittr.MoveNext())
                {
                    //obtains and returns the next item
                    curr = ittr.Current;
                    yield return curr;

                    //determins if we should break
                    err = VMath.Error(last, curr);
                    if (err <= tol) break;
                }
            }
        }


        public static IEnumerable<T> LimitRel<T>(this IEnumerable<T> source,
            double tol, int max) where T : Metrizable<T>
        {
            //checks that we don't have a null source
            if (source == null) throw new ArgumentNullException("source");

            //used in the itterative loop
            double err = Double.PositiveInfinity;
            T curr, last;

            using (var ittr = source.GetEnumerator())
            {
                //if there are no elements, return an empty list
                if (!ittr.MoveNext()) yield break;

                //obtains the last element and return it
                last = ittr.Current;
                yield return last;

                while (ittr.MoveNext())
                {
                    //obtains and returns the next item
                    curr = ittr.Current;
                    yield return curr;

                    //computes the error value
                    double dist = curr.Dist(last);
                    dist = dist / curr.Norm();
                    err = Math.Abs(dist);

                    //determins if we should break
                    if (err <= tol) break;
                }
            }
        }


        public static IEnumerable<T> LimitRel<T>(this IEnumerable<T> source,
            Metric<T> met, T zero, double tol, int max)
        {
            //checks that we don't have a null source
            if (source == null) throw new ArgumentNullException("source");

            //used in the itterative loop
            double err = Double.PositiveInfinity;
            T curr, last;

            using (var ittr = source.GetEnumerator())
            {
                //if there are no elements, return an empty list
                if (!ittr.MoveNext()) yield break;

                //obtains the last element and return it
                last = ittr.Current;
                yield return last;

                while (ittr.MoveNext())
                {
                    //obtains and returns the next item
                    curr = ittr.Current;
                    yield return curr;

                    //computes the error value
                    double dist = met(curr, last);
                    dist = dist / met(curr, zero);
                    err = Math.Abs(dist);

                    //determins if we should break
                    if (err <= tol) break;
                }
            }
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Limit Absolute...

        public static IEnumerable<Double> LimitAbs(this IEnumerable<Double> source,
            double tol, int max)
        {
            //checks that we don't have a null source
            if (source == null) throw new ArgumentNullException("source");

            //used in the itterative loop
            double err = Double.PositiveInfinity;
            double curr, last;

            using (var ittr = source.GetEnumerator())
            {
                //if there are no elements, return an empty list
                if (!ittr.MoveNext()) yield break;

                //obtains the last element and return it
                last = ittr.Current;
                yield return last;

                while (ittr.MoveNext())
                {
                    //obtains and returns the next item
                    curr = ittr.Current;
                    yield return curr;

                    //computes the error value
                    double dist = curr - last;
                    err = Math.Abs(dist);

                    //determins if we should break
                    if (err <= tol) break;
                }
            }
        }


        public static IEnumerable<T> LimitAbs<T>(this IEnumerable<T> source,
            double tol, int max) where T : Metrizable<T>
        {
            //checks that we don't have a null source
            if (source == null) throw new ArgumentNullException("source");

            //used in the itterative loop
            double err = Double.PositiveInfinity;
            T curr, last;

            using (var ittr = source.GetEnumerator())
            {
                //if there are no elements, return an empty list
                if (!ittr.MoveNext()) yield break;

                //obtains the last element and return it
                last = ittr.Current;
                yield return last;

                while (ittr.MoveNext())
                {
                    //obtains and returns the next item
                    curr = ittr.Current;
                    yield return curr;

                    //computes the error value
                    double dist = curr.Dist(last);
                    err = Math.Abs(dist);

                    //determins if we should break
                    if (err <= tol) break;
                }
            }
        }


        public static IEnumerable<T> LimitAbs<T>(this IEnumerable<T> source,
            Metric<T> met, double tol, int max)
        {
            //checks that we don't have a null source
            if (source == null) throw new ArgumentNullException("source");

            //used in the itterative loop
            double err = Double.PositiveInfinity;
            T curr, last;

            using (var ittr = source.GetEnumerator())
            {
                //if there are no elements, return an empty list
                if (!ittr.MoveNext()) yield break;

                //obtains the last element and return it
                last = ittr.Current;
                yield return last;

                while (ittr.MoveNext())
                {
                    //obtains and returns the next item
                    curr = ittr.Current;
                    yield return curr;

                    //computes the error value
                    double dist = met(curr, last);
                    err = Math.Abs(dist);

                    //determins if we should break
                    if (err <= tol) break;
                }
            }
        }

        #endregion ////////////////////////////////////////////////////////////

        





    }
}

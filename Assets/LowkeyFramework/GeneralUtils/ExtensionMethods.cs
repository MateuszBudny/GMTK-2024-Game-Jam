//using LowkeyFramework.AttributeSaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 Rotate(this Vector2 vector, float eulerAngle)
    {
        float radians = eulerAngle * Mathf.Deg2Rad;
        return new Vector2(
            vector.x * Mathf.Cos(radians) - vector.y * Mathf.Sin(radians),
            vector.x * Mathf.Sin(radians) + vector.y * Mathf.Cos(radians)
        );
    }

    public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    public static Vector2 RotateAroundPivot(Vector2 point, Vector2 pivot, float angle)
    {
        return (point - pivot).Rotate(angle) + pivot;
    }

    public static T GetRandomElement<T>(this List<T> list)
    {
        int chosenElementIndex = UnityEngine.Random.Range(0, list.Count);
        return list[chosenElementIndex];
    }

    public static List<T> Shuffle<T>(this List<T> list)
    {
        List<T> listCopy = new List<T>(list);
        List<T> finalShuffledList = new List<T>();
        for(int i = 0; i < listCopy.Count; i++)
        {
            T elementDrawn = listCopy.GetRandomElement();
            finalShuffledList.Add(elementDrawn);
            listCopy.Remove(elementDrawn);
        }

        return finalShuffledList;
    }

    public static void ForEach<T>(this List<T> list, Action<T, int> forEachDo)
    {
        for(int i = 0; i < list.Count; i++)
        {
            forEachDo(list[i], i);
        }
    }

    public static float RandomRangeFloat(this Vector2 minMax)
    {
        return UnityEngine.Random.Range(minMax.x, minMax.y);
    }

    public static Vector3 RandomRangeVector(this Vector3 minInclusive, Vector3 maxInclusive)
    {
        return new Vector3(UnityEngine.Random.Range(minInclusive.x, maxInclusive.x), UnityEngine.Random.Range(minInclusive.y, maxInclusive.y), UnityEngine.Random.Range(minInclusive.z, maxInclusive.z));
    }

    // MinBy and MaxBy borrowed from: https://github.com/morelinq/MoreLINQ
    /// <summary>
    /// Returns the minimal element of the given sequence, based on
    /// the given projection.
    /// </summary>
    /// <remarks>
    /// If more than one element has the minimal projected value, the first
    /// one encountered will be returned. This overload uses the default comparer
    /// for the projected type. This operator uses immediate execution, but
    /// only buffers a single result (the current minimal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <returns>The minimal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector)
    {
        return source.MinBy(selector, null);
    }

    /// <summary>
    /// Returns the minimal element of the given sequence, based on
    /// the given projection and the specified comparer for projected values.
    /// </summary>
    /// <remarks>
    /// If more than one element has the minimal projected value, the first
    /// one encountered will be returned. This operator uses immediate execution, but
    /// only buffers a single result (the current minimal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <param name="comparer">Comparer to use to compare projected values</param>
    /// <returns>The minimal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
    /// or <paramref name="comparer"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector, IComparer<TKey> comparer)
    {
        if(source == null)
            throw new ArgumentNullException(nameof(source));
        if(selector == null)
            throw new ArgumentNullException(nameof(selector));
        comparer = comparer ?? Comparer<TKey>.Default;

        using(var sourceIterator = source.GetEnumerator())
        {
            if(!sourceIterator.MoveNext())
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }
            var min = sourceIterator.Current;
            var minKey = selector(min);
            while(sourceIterator.MoveNext())
            {
                var candidate = sourceIterator.Current;
                var candidateProjected = selector(candidate);
                if(comparer.Compare(candidateProjected, minKey) < 0)
                {
                    min = candidate;
                    minKey = candidateProjected;
                }
            }
            return min;
        }
    }

    /// <summary>
    /// Returns the maximal element of the given sequence, based on
    /// the given projection.
    /// </summary>
    /// <remarks>
    /// If more than one element has the maximal projected value, the first
    /// one encountered will be returned. This overload uses the default comparer
    /// for the projected type. This operator uses immediate execution, but
    /// only buffers a single result (the current maximal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <returns>The maximal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector)
    {
        return source.MaxBy(selector, null);
    }

    /// <summary>
    /// Returns the maximal element of the given sequence, based on
    /// the given projection and the specified comparer for projected values. 
    /// </summary>
    /// <remarks>
    /// If more than one element has the maximal projected value, the first
    /// one encountered will be returned. This operator uses immediate execution, but
    /// only buffers a single result (the current maximal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <param name="comparer">Comparer to use to compare projected values</param>
    /// <returns>The maximal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
    /// or <paramref name="comparer"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector, IComparer<TKey> comparer)
    {
        if(source == null)
            throw new ArgumentNullException(nameof(source));
        if(selector == null)
            throw new ArgumentNullException(nameof(selector));
        comparer = comparer ?? Comparer<TKey>.Default;

        using(var sourceIterator = source.GetEnumerator())
        {
            if(!sourceIterator.MoveNext())
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }
            var max = sourceIterator.Current;
            var maxKey = selector(max);
            while(sourceIterator.MoveNext())
            {
                var candidate = sourceIterator.Current;
                var candidateProjected = selector(candidate);
                if(comparer.Compare(candidateProjected, maxKey) > 0)
                {
                    max = candidate;
                    maxKey = candidateProjected;
                }
            }
            return max;
        }
    }
}

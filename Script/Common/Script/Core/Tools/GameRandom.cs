using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GameRandom
{
    /// <summary>
    /// get a list of random idx from[include] to[exclude]
    /// </summary>
    /// <param name="from">include</param>
    /// <param name="to">exclude</param>
    /// <param name="num"></param>
    /// <returns></returns>
    public static List<int> GetIndependentRandoms(int from, int to, int num)
    {
        List<int> randoms = new List<int>();
        List<int> range = new List<int>();

        for (int i = from; i < to; ++i)
        {
            range.Add(i);
        }

        int randomCount = num;
        if (randomCount > range.Count)
        {
            randomCount = range.Count;
        }

        for (int i = 0; i < randomCount; ++i)
        {
            int randomIdx = Random.Range(0, range.Count);
            randoms.Add(range[randomIdx]);
            range.RemoveAt(randomIdx);
        }

        return randoms;
    }

    public static List<int> GetTotalRange(int total, int min, int count)
    {
        List<int> randomList = new List<int>();

        int randomCount = count;
        int logicRotal = total;
        while (randomCount > 1)
        {
            int max = logicRotal - (min * randomCount) + min;
            int randomRange1 = min;
            if (max > min)
            {
                randomRange1 = Random.Range(min, max + 1);
            }
            randomList.Add(randomRange1);

            logicRotal -= randomRange1;
            --randomCount;
        }

        randomList.Add(logicRotal);
        return randomList;
    }

    //from 0 to length - 1
    public static int GetRandomLevel(params int[] levelRates)
    {
        int totalRate = 0;
        foreach (int levelRate in levelRates)
        {
            totalRate += levelRate;
        }

        int randomValue = Random.Range(0, totalRate);
        int rateStep = 0;

        for (int i = 0; i < levelRates.Length; ++i)
        {
            rateStep += levelRates[i];
            if (rateStep >= randomValue)
            {
                return i;
            }
        }

        return levelRates.Length - 1;
    }

    public static int GetTotalRandomRate(int totalValue, params int[] levelRates)
    {
        int randomValue = Random.Range(0, totalValue);
        int rateStep = 0;

        for (int i = 0; i < levelRates.Length; ++i)
        {
            rateStep += levelRates[i];
            if (rateStep >= randomValue)
            {
                return i;
            }
        }

        return -1;
    }

    public static int GetRandomLevel(IList<int> levelRates)
    {
        int totalRate = 0;
        foreach (int levelRate in levelRates)
        {
            totalRate += levelRate;
        }

        int randomValue = Random.Range(0, totalRate);
        int rateStep = 0;

        for (int i = 0; i < levelRates.Count; ++i)
        {
            rateStep += levelRates[i];
            if (rateStep >= randomValue)
            {
                return i;
            }
        }

        return levelRates.Count - 1;
    }

    public static bool IsInRate(int rate)
    {
        var random = Random.Range(0, 10001);
        if (random > rate)
            return false;

        return true;
    }
}


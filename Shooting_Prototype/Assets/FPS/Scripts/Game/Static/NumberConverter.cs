using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NumberConverter
{
    static Dictionary<string, long> numberLevels = new Dictionary<string, long>
    {
        {"K", (long)Mathf.Pow(1000, 1)},
        {"M", (long)Mathf.Pow(1000, 2)},
        {"B", (long)Mathf.Pow(1000, 3)},
        {"T", (long)Mathf.Pow(1000, 4)},
        {"Q", (long)Mathf.Pow(1000, 5)},
    };
    public static string FixNumber(float number)
    {
        string result = "";
        long highest = 1;

        foreach (KeyValuePair<string, long> level in numberLevels)
        {
            Debug.Log("fixing: " + number + " By: " + level.Key + ", " + level.Value);
            if (level.Value > highest && level.Value <= number)
            {
                highest = level.Value;
                result = level.Key;
            }
        }


        return (number / highest).ToString() + result;
    }
}

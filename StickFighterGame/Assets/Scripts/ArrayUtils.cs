using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayUtils
{
    public static void Fill<T>(this T[] arr, T value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = value;
        }
    }

    public static void Copy<T>(this T[] arr, T[] arr2)
    {
        int minLength = Mathf.Min(arr.Length, arr2.Length);
        for (int i = 0; i < minLength; i++)
        {
            arr[i] = arr2[i];
        }
    }
}

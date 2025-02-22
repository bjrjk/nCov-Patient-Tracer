﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.DSA
{
    class Algorithm
    {
        public static void swap<T>(ref T v1, ref T v2)
        {
            T tmp = v1;
            v1 = v2;
            v2 = tmp;
        }
        private static int quickSort_partition<T, C>(Vector<T> arr, int l, int r, C comparer) //快速排序——分区函数
           where C : IComparer<T> //C是泛型T的比较器类型
        {
            T pivot = arr[r];
            while (l != r)
            {
                while (comparer.Compare(arr[l], pivot) < 0 && l < r) l++;
                if (l < r) arr[r--] = arr[l];
                while (comparer.Compare(pivot, arr[r]) < 0 && l < r) r--;
                if (l < r) arr[l++] = arr[r];
            }
            arr[l] = pivot;
            return l;
        }
        public static void quickSort<T, C>(Vector<T> arr, int l, int r, C comparer) //快速排序——递归
            where C : IComparer<T>
        {
            if (r <= l) return;
            int pivot = (l + r) >> 1;
            swap(ref arr._arr[pivot], ref arr._arr[r]);
            pivot = quickSort_partition(arr, l, r, comparer);
            quickSort(arr, l, pivot - 1, comparer);
            quickSort(arr, pivot + 1, r, comparer);
        }
        public static void quickSort<T, C>(Vector<T> arr, C comparer) //快速排序——整个数组
             where C : IComparer<T>
        {
            quickSort(arr, 0, arr.size() - 1, comparer);
        }
        public static int lower_bound<T, C>(Vector<T> arr, T v, C comparer)
             where C : IComparer<T> //C是泛型T的比较器类型
        {
            int l = 0, r = arr.size() - 1, mid;
            while (l < r)
            {
                mid = (l + r) / 2;
                //若arr[mid] < v，要将查找范围缩小到右侧子序列，这时候候选位置可能是右边子序列的第一个
                if (comparer.Compare(arr[mid], v) < 0) l = mid + 1;
                //若arr[mid] >= v，要将查找范围缩小到左侧子序列，但有可能arr[mid]处就是最终位置，所以r的位置不做微调，仍然包含在查找范围内
                else r = mid; 
            }
            if (comparer.Compare(v, arr[l]) <= 0) return l;
            else return arr.size();
        }
        public static int upper_bound<T, C>(Vector<T> arr, T v, C comparer)
             where C : IComparer<T>
        {
            int l = 0, r = arr.size() - 1, mid;
            while (l < r)
            {
                mid = (l + r) / 2;
                //若arr[mid] <= v，要将查找范围缩小到右侧子序列，这时候候选位置可能是右边子序列的第一个
                if (comparer.Compare(arr[mid], v) <= 0) l = mid + 1;
                //若arr[mid] > v，要将查找范围缩小到左侧子序列，但有可能arr[mid]处就是最终位置，所以r的位置不做微调，仍然包含在查找范围内
                else r = mid;
            }
            if (comparer.Compare(v, arr[l]) < 0) return l;
            else return arr.size();
        }
        public static bool IsInt(string str) //判断str对应的字符串是否能转换为int
        {
            int vsNum;
            bool isNum;
            isNum = int.TryParse(str, out vsNum);
            return isNum;
        }
        public static bool IsDouble(string str) //判断str对应的字符串是否能转换为double
        {
            double vsNum;
            bool isNum;
            isNum = double.TryParse(str, out vsNum);
            return isNum;
        }
    }
}

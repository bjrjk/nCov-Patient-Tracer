using nCov_Patient_Tracer.DSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace nCov_Patient_Tracer.Tests
{
    class Test
    {
        private class IntComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x.CompareTo(y);
            }
        }
        private static bool testQSort()
        {
            const int itemNum = 1000;
            Vector<int> v = new Vector<int>();
            Random ra = new Random();
            for (int i = 0; i < itemNum; i++)
            {
                v.append(ra.Next());
            }
            Algorithm.quickSort(v, new IntComparer());
            for (int i = 0; i < itemNum - 1; i++)
            {
                if (v[i] > v[i + 1]) return false;
            }
            return true;
        }
        private static bool testBinarySearch()
        {
            const int itemNum = 1000;
            Vector<int> v = new Vector<int>();
            Random ra = new Random();
            for (int i = 0; i < itemNum; i++)
            {
                v.append(ra.Next());
            }
            Algorithm.quickSort(v, new IntComparer());
            int findNum = ra.Next();
            int pos = Algorithm.lower_bound(v, findNum, new IntComparer());
            if (pos == -1)
            {
                if (!(v[v.size() - 1] < findNum)) return false;
            }
            else
            {
                if (!(v[pos] >= findNum && pos == 0 ||
                    v[pos] >= findNum && pos > 0 && v[pos - 1] < findNum)) return false;
            }
            pos = Algorithm.upper_bound(v, findNum, new IntComparer());
            if (pos == -1)
            {
                if (!(v[v.size() - 1] <= findNum)) return false;
            }
            else
            {
                if (!(v[pos] > findNum && pos == 0 ||
                    v[pos] > findNum && pos > 0 && v[pos - 1] <= findNum)) return false;
            }
            return true;
        }
        public static void testMain()
        {
            for (int i = 0; i < 1000; i++)
            {
                if (!testQSort())
                    MessageBox.Show("排序错误！");
                if (!testBinarySearch())
                    MessageBox.Show("二分查找错误！");
            }
        }
    }
}

using nCov_Patient_Tracer.DSA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private class ObjectComparer : IComparer<Object>
        {
            public int Compare(Object x, Object y)
            {
                return x.GetHashCode().CompareTo(y.GetHashCode());
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
            if (pos == v.size())
            {
                if (!(v[v.size() - 1] < findNum)) return false;
            }
            else
            {
                if (!(v[pos] >= findNum && pos == 0 ||
                    v[pos] >= findNum && pos > 0 && v[pos - 1] < findNum)) return false;
            }
            pos = Algorithm.upper_bound(v, findNum, new IntComparer());
            if (pos == v.size())
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
        private class Segment : ISegment
        {
            int s, e;
            public Segment(int s,int e)
            {
                this.s = s;
                this.e = e;
            }
            public int getEndPoint()
            {
                return e;
            }

            public int getStartPoint()
            {
                return s;
            }

            public bool InterSection(ISegment seg)
            {
                return getStartPoint() <= getEndPoint() && getStartPoint() <= seg.getEndPoint() &&
                    seg.getStartPoint() <= getEndPoint() && seg.getStartPoint() <= seg.getEndPoint();
            }

            public int InterSection(int point)
            {
                if (getStartPoint() <= point && point <= getEndPoint()) return 0;
                else if (point < getStartPoint()) return 1;
                else return -1;
            }
        }
        private class SegmentComparer : IComparer<Segment>
        {
            public int Compare(Segment x, Segment y)
            {
                if (x.getStartPoint() != y.getStartPoint()) return x.getStartPoint() - y.getStartPoint();
                else if (x.getEndPoint() != y.getEndPoint()) return x.getEndPoint() - y.getEndPoint();
                else return x.GetHashCode() - y.GetHashCode();
            }
        }
        private static Vector<Segment> InterSection_BruteForce(Vector<Segment> segments, int q)
        {
            Vector<Segment> results = new Vector<Segment>();
            for (int i = 0; i < segments.size(); i++)
            {
                if (segments[i].InterSection(q) == 0) results.append(segments[i]);
            }
            return results;
        }
        private static Vector<Segment> InterSection_IntervalTree(Vector<Segment> segments, int q)
        {
            IntervalTree<Segment> tree = new IntervalTree<Segment>(segments);
            return tree.query(q);
        }
        private static bool testInterSection()
        {
            const int N = 100;
            const int s = -10000, e = 10000;
            Vector<Segment> segs = new Vector<Segment>();
            Random ra = new Random();
            for(int i = 0; i < N; i++)
            {
                int t1 = ra.Next(s, e), t2 = ra.Next(s, e);
                segs.append(new Segment(Math.Min(t1,t2),Math.Max(t1,t2)));
            }
            int q = ra.Next(s, e);
            Vector<Segment> results_BF = InterSection_BruteForce(segs, q);
            Vector<Segment> results_IT = InterSection_IntervalTree(segs, q);
            Debug.Assert(results_BF.size() == results_IT.size());
            Algorithm.quickSort(results_BF, new SegmentComparer());
            Algorithm.quickSort(results_IT, new SegmentComparer());
           for(int i = 0; i < results_BF.size(); i++)
            {
                if (!object.ReferenceEquals(results_BF[i], results_IT[i])) 
                    return false;
            }
            return true;
        }
        public static void testMain()
        {
            return;
            for (int i = 0; i < 1000; i++)
            {
                if (!testQSort())
                    MessageBox.Show("排序错误！");
                if (!testBinarySearch())
                    MessageBox.Show("二分查找错误！");
                if(!testInterSection())
                    MessageBox.Show("线段交集错误！");
            }
        }
    }
}

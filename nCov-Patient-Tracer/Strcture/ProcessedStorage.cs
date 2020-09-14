using nCov_Patient_Tracer.DSA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.Strcture
{
    class ProcessedStorage
    {
        public Storage storage;
        public Vector<Vector<TimeSpan>> TimeSpanSortedByStartHour;
        public Vector<Vector<TimeSpan>> TimeSpanSortedByEndHour;
        public Vector<IntervalTree<TimeSpan>> intervalTrees;
        private class TimeSpanIndexSortByStartTime : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return Global.storage.TimeSpans[x].startHour.CompareTo(
                    Global.storage.TimeSpans[y].startHour);
            }
        }
        public ProcessedStorage(Storage s)
        {
            storage = s;
            for(int i = 0; i < s.Persons.size(); i++) //给每个人的TimeSpan按起始时间排序
            {
                for(int j = 0; j < s.Persons[i].timeSpanCollection.size(); j++)
                {
                    Vector<int> vTS = s.Persons[i].timeSpanCollection;
                    Algorithm.quickSort(vTS, new TimeSpanIndexSortByStartTime());
                }
            }
            Vector<TimeSpan> TimeSpanRef = s.TimeSpans;
            TimeSpanSortedByStartHour = new Vector<Vector<TimeSpan>>();
            TimeSpanSortedByStartHour.reserve(s.Sites.size());
            TimeSpanSortedByEndHour = new Vector<Vector<TimeSpan>>();
            TimeSpanSortedByEndHour.reserve(s.Sites.size());
            intervalTrees = new Vector<IntervalTree<TimeSpan>>();
            intervalTrees.reserve(s.Sites.size());
            for (int i = 0; i < s.Sites.size(); i++)
            {
                TimeSpanSortedByStartHour[i] = new Vector<TimeSpan>();
                TimeSpanSortedByEndHour[i] = new Vector<TimeSpan>();
            }
            for (int i = 0; i < TimeSpanRef.size(); i++)
            {
                TimeSpanSortedByStartHour[TimeSpanRef[i].siteID].append(TimeSpanRef[i]);
                TimeSpanSortedByEndHour[TimeSpanRef[i].siteID].append(TimeSpanRef[i]);
            }
            for (int i = 0; i < s.Sites.size(); i++)
            {
                Algorithm.quickSort(TimeSpanSortedByStartHour[i], new TimeSpanComparerByStartHour());
                Algorithm.quickSort(TimeSpanSortedByEndHour[i], new TimeSpanComparerByEndHour());
                intervalTrees[i] = new IntervalTree<TimeSpan>(TimeSpanSortedByStartHour[i]);
            }
        }
        public Vector<Vector<TimeSpan>> query(Person p)
        {
            Vector<Vector<TimeSpan>> arr = new Vector<Vector<TimeSpan>>();
            arr.reserve(p.timeSpanCollection.size());
            for (int i = 0; i < p.timeSpanCollection.size(); i++)
            {
                HashTable<TimeSpan> hashTable = new HashTable<TimeSpan>();
                arr[i] = new Vector<TimeSpan>();
                TimeSpan t = storage.TimeSpans[p.timeSpanCollection[i]];
                int startHour_s = Algorithm.lower_bound(TimeSpanSortedByStartHour[t.siteID],
                    new TimeSpan(-1, t.startHour, -1, -1, -1, false),
                    new TimeSpanComparerByStartHour());
                int startHour_e = Algorithm.upper_bound(TimeSpanSortedByStartHour[t.siteID],
                    new TimeSpan(-1, t.endHour, -1, -1, -1, false),
                    new TimeSpanComparerByStartHour());
                Debug.Assert(startHour_s != TimeSpanSortedByStartHour[t.siteID].size());
                for(int j = startHour_s; j < startHour_e; j++)
                {
                    if (t.CompareTo(TimeSpanSortedByStartHour[t.siteID][j]) == 0) continue;
                    if (TimeSpanSortedByStartHour[t.siteID][j].isProtected == true) continue;
                    if (hashTable.query(TimeSpanSortedByStartHour[t.siteID][j]) == null)
                    {
                        hashTable.insert(TimeSpanSortedByStartHour[t.siteID][j]);
                        arr[i].append(TimeSpanSortedByStartHour[t.siteID][j]);
                    }
                }
                int endHour_s = Algorithm.lower_bound(TimeSpanSortedByEndHour[t.siteID],
                    new TimeSpan(-1, -1, t.startHour, -1, -1, false),
                    new TimeSpanComparerByEndHour());
                int endHour_e = Algorithm.upper_bound(TimeSpanSortedByEndHour[t.siteID],
                    new TimeSpan(-1, -1, t.endHour, -1, -1, false),
                    new TimeSpanComparerByEndHour());
                Debug.Assert(endHour_s != TimeSpanSortedByEndHour[t.siteID].size());
                for(int j = endHour_s; j < endHour_e; j++)
                {
                    if (t.CompareTo(TimeSpanSortedByEndHour[t.siteID][j]) == 0) continue;
                    if (TimeSpanSortedByEndHour[t.siteID][j].isProtected == true) continue;
                    if (hashTable.query(TimeSpanSortedByEndHour[t.siteID][j]) == null)
                    {
                        hashTable.insert(TimeSpanSortedByEndHour[t.siteID][j]);
                        arr[i].append(TimeSpanSortedByEndHour[t.siteID][j]);
                    }
                }
                Vector<TimeSpan> Results_IT = intervalTrees[t.siteID].query(t.startHour);
                for(int j=0;j< Results_IT.size(); j++)
                {
                    if (t.CompareTo(Results_IT[j]) == 0) continue;
                    if (Results_IT[j].isProtected) continue;
                    if (hashTable.query(Results_IT[j]) == null)
                    {
                        hashTable.insert(Results_IT[j]);
                        arr[i].append(Results_IT[j]);
                    }
                }
            }
            return arr;
        }

        public Vector<Vector<TimeSpan>> queryBruteForce(Person p)
        {
            Vector<Vector<TimeSpan>> result = new Vector<Vector<TimeSpan>>();
            result.reserve(p.timeSpanCollection.size());
            for (int i = 0; i < p.timeSpanCollection.size(); i++)
            {
                TimeSpan t = storage.TimeSpans[p.timeSpanCollection[i]];
                Vector<TimeSpan> arr = TimeSpanSortedByStartHour[t.siteID];
                result[i] = new Vector<TimeSpan>();
                for(int j = 0; j < arr.size(); j++)
                {
                    if (t.InterSection(arr[j]))
                    {
                        if (storage.Persons[arr[j].personID].CompareTo(p) == 0) continue;
                        if (arr[j].isProtected) continue;
                        result[i].append(arr[j]);
                    }
                }
            }
            return result;
        }
    }
}

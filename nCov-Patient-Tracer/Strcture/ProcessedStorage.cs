using nCov_Patient_Tracer.DSA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.Strcture
{
    class ProcessedStorage //预处理后的结果数据存储类
    {
        public Storage storage; //指向原始数据类实例的引用
        public Vector<Vector<TimeSpan>> TimeSpanSortedByStartHour; //对时间段按照开始时间（线段左端点）排序的数组
        public Vector<Vector<TimeSpan>> TimeSpanSortedByEndHour; //对时间段按照结束时间（线段右端点）排序的数组
        public Vector<IntervalTree<TimeSpan>> intervalTrees; //对每个地点都建一个区间树的数组
        private class TimeSpanIndexSortByStartTime : IComparer<int> //时间段开始时间排序比较器类
        {
            public int Compare(int x, int y)
            {
                return Global.storage.TimeSpans[x].startHour.CompareTo(
                    Global.storage.TimeSpans[y].startHour);
            }
        }
        public ProcessedStorage(Storage s) //构造函数
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
            //开始初始化各数组和数据结构，分配内存空间
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
                //加入时间段数据
                TimeSpanSortedByStartHour[TimeSpanRef[i].siteID].append(TimeSpanRef[i]);
                TimeSpanSortedByEndHour[TimeSpanRef[i].siteID].append(TimeSpanRef[i]);
            }
            for (int i = 0; i < s.Sites.size(); i++)
            {
                //对两类数组分别使用开始时间和结束时间进行排序，为第一类情况查询做预处理
                Algorithm.quickSort(TimeSpanSortedByStartHour[i], new TimeSpanComparerByStartHour());
                Algorithm.quickSort(TimeSpanSortedByEndHour[i], new TimeSpanComparerByEndHour());
                //建区间树，为第二类情况查询做预处理
                intervalTrees[i] = new IntervalTree<TimeSpan>(TimeSpanSortedByStartHour[i]);
            }
        }
        public Vector<Vector<TimeSpan>> query(Person p) //综合方法查询
        {
            Vector<Vector<TimeSpan>> arr = new Vector<Vector<TimeSpan>>();
            arr.reserve(p.timeSpanCollection.size());
            for (int i = 0; i < p.timeSpanCollection.size(); i++) //对于病毒感染者每一个去过的地点相应的时间段
            {
                HashTable<TimeSpan> hashTable = new HashTable<TimeSpan>(); //时间段去重哈希表
                arr[i] = new Vector<TimeSpan>();
                TimeSpan t = storage.TimeSpans[p.timeSpanCollection[i]];
                int startHour_s = Algorithm.lower_bound(TimeSpanSortedByStartHour[t.siteID],
                    new TimeSpan(-1, t.startHour, -1, -1, -1, false),
                    new TimeSpanComparerByStartHour()); //二分查找，找以“开始时间”排序的时间段序列的最小下标
                int startHour_e = Algorithm.upper_bound(TimeSpanSortedByStartHour[t.siteID],
                    new TimeSpan(-1, t.endHour, -1, -1, -1, false),
                    new TimeSpanComparerByStartHour()); //二分查找，找以“开始时间”排序的时间段序列的最大下标
                Debug.Assert(startHour_s != TimeSpanSortedByStartHour[t.siteID].size());
                for(int j = startHour_s; j < startHour_e; j++) //遍历全部筛选出的时间段，去重，加入结果集合
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
                    new TimeSpanComparerByEndHour()); //二分查找，找以“结束时间”排序的时间段序列的最小下标
                int endHour_e = Algorithm.upper_bound(TimeSpanSortedByEndHour[t.siteID],
                    new TimeSpan(-1, -1, t.endHour, -1, -1, false),
                    new TimeSpanComparerByEndHour()); //二分查找，找以“结束时间”排序的时间段序列的最大下标
                Debug.Assert(endHour_s != TimeSpanSortedByEndHour[t.siteID].size());
                for(int j = endHour_s; j < endHour_e; j++) //遍历全部筛选出的时间段，去重，加入结果集合
                {
                    if (t.CompareTo(TimeSpanSortedByEndHour[t.siteID][j]) == 0) continue;
                    if (TimeSpanSortedByEndHour[t.siteID][j].isProtected == true) continue;
                    if (hashTable.query(TimeSpanSortedByEndHour[t.siteID][j]) == null)
                    {
                        hashTable.insert(TimeSpanSortedByEndHour[t.siteID][j]);
                        arr[i].append(TimeSpanSortedByEndHour[t.siteID][j]);
                    }
                }
                Vector<TimeSpan> Results_IT = intervalTrees[t.siteID].query(t.startHour); //利用区间树进行穿刺查询
                for(int j=0;j< Results_IT.size(); j++) //遍历全部筛选出的时间段，去重，加入结果集合
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

        public Vector<Vector<TimeSpan>> queryBruteForce(Person p) //暴力方法查询
        {
            Vector<Vector<TimeSpan>> result = new Vector<Vector<TimeSpan>>();
            result.reserve(p.timeSpanCollection.size());
            for (int i = 0; i < p.timeSpanCollection.size(); i++) //对于病毒感染者每一个去过的地点相应的时间段
            {
                TimeSpan t = storage.TimeSpans[p.timeSpanCollection[i]];
                Vector<TimeSpan> arr = TimeSpanSortedByStartHour[t.siteID];
                result[i] = new Vector<TimeSpan>();
                for(int j = 0; j < arr.size(); j++) //暴力求交查询
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

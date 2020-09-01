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
        public ProcessedStorage(Storage s)
        {
            storage = s;
            Vector<TimeSpan> TimeSpanRef = s.TimeSpans;
            TimeSpanSortedByStartHour = new Vector<Vector<TimeSpan>>();
            TimeSpanSortedByStartHour.reserve(s.Sites.size());
            TimeSpanSortedByEndHour = new Vector<Vector<TimeSpan>>();
            TimeSpanSortedByEndHour.reserve(s.Sites.size());
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
            }
        }
        public Vector<Person> query(Person p)
        {
            HashTable<Person> PersonTable = new HashTable<Person>();
            Vector<Person> arr = new Vector<Person>();
            for (int i = 0; i < p.timeSpanCollection.size(); i++)
            {
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
                    if (PersonTable.query(
                        storage.Persons[TimeSpanSortedByStartHour[t.siteID][j].personID]) == null)
                    {
                        PersonTable.insert(
                            storage.Persons[TimeSpanSortedByStartHour[t.siteID][j].personID]);
                        arr.append(storage.Persons[TimeSpanSortedByStartHour[t.siteID][j].personID]);
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
                    if(PersonTable.query(
                        storage.Persons[TimeSpanSortedByEndHour[t.siteID][j].personID]) == null)
                    {
                        PersonTable.insert(
                            storage.Persons[TimeSpanSortedByEndHour[t.siteID][j].personID]);
                        arr.append(storage.Persons[TimeSpanSortedByEndHour[t.siteID][j].personID]);
                    }
                }
            }

            return arr;
        }

        public Vector<Person> queryBruteForce(Person p)
        {
            HashTable<Person> PersonTable = new HashTable<Person>();
            Vector<Person> result = new Vector<Person>();
            for(int i = 0; i < p.timeSpanCollection.size(); i++)
            {
                TimeSpan t = storage.TimeSpans[p.timeSpanCollection[i]];
                Vector<TimeSpan> arr = TimeSpanSortedByStartHour[t.siteID];
                for(int j = 0; j < arr.size(); j++)
                {
                    if (t.InterSection(arr[j]))
                    {
                        if (storage.Persons[arr[j].personID].CompareTo(p) == 0) continue;
                        if (PersonTable.query(storage.Persons[arr[j].personID]) == null)
                        {
                            PersonTable.insert(storage.Persons[arr[j].personID]);
                            result.append(storage.Persons[arr[j].personID]);
                        }
                    }
                }
            }
            return result;
        }
    }
}

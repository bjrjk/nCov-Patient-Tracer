using nCov_Patient_Tracer.DSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.Strcture
{
    class ProcessedStorage
    {
        public Vector<TimeSpan> TimeSpanSortedByStartHour;
        public Vector<TimeSpan> TimeSpanSortedByEndHour;
        public HashTable<TimeSpan> Table;
        public ProcessedStorage(Storage s)
        {
            TimeSpanSortedByStartHour = new Vector<TimeSpan>();
            TimeSpanSortedByEndHour = new Vector<TimeSpan>();
            Table = new HashTable<TimeSpan>();
        }
    }
}

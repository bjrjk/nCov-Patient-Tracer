using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.Strcture
{
    class TimeSpan:IComparable<TimeSpan>
    {
        public int ID;
        public int startHour, endHour;
        public int personID, siteID;
        public bool isProtected;
        public TimeSpan(int ID, int startHour, int endHour, int personID, int siteID, bool isProtected)
        {
            this.ID = ID;
            this.startHour = startHour;
            this.endHour = endHour;
            this.personID = personID;
            this.siteID = siteID;
            this.isProtected = isProtected;
        }
        public static TimeSpan read(System.IO.BinaryReader reader)
        {
            int ID, startHour, endHour, personID, siteID;
            bool isProtected;
            ID = reader.ReadInt32();
            startHour = reader.ReadInt32();
            endHour = reader.ReadInt32();
            personID = reader.ReadInt32();
            siteID = reader.ReadInt32();
            isProtected = reader.ReadBoolean();
            return new TimeSpan(ID, startHour, endHour, personID, siteID, isProtected);
        }
        public void write(System.IO.BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(startHour);
            writer.Write(endHour);
            writer.Write(personID);
            writer.Write(siteID);
            writer.Write(isProtected);
        }
        public int CompareTo(TimeSpan other)
        {
            return ID.CompareTo(other.ID);
        }
        public bool InterSection(TimeSpan other)
        {
            return startHour <= endHour && startHour <= other.endHour &&
                other.startHour <= endHour && other.startHour <= other.endHour;
        }
    }
    class TimeSpanComparerByStartHour : IComparer<TimeSpan>
    {
        public int Compare(TimeSpan x, TimeSpan y)
        {
            return x.startHour.CompareTo(y.startHour);
        }
    }
    class TimeSpanComparerByEndHour : IComparer<TimeSpan>
    {
        public int Compare(TimeSpan x, TimeSpan y)
        {
            return x.endHour.CompareTo(y.endHour);
        }
    }
    class TimeSpanComparerByContent : IComparer<TimeSpan>
    {
        public int Compare(TimeSpan x, TimeSpan y)
        {
            return x.ID.CompareTo(y.ID);
        }
    }
}

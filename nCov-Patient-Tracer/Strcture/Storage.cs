using nCov_Patient_Tracer.DSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.Strcture
{
    class Storage
    {
        public int timespanIncCnt, personIncCnt, siteIncCnt; //时间段总数，人员总数，地点总数
        public Vector<TimeSpan> TimeSpans; //存储时间段的数组
        public Vector<Person> Persons; //存储人员的数组
        public Vector<Site> Sites; //存储地点的数组
        public Storage()
        {
            timespanIncCnt = personIncCnt = siteIncCnt = 0;
            TimeSpans = new Vector<TimeSpan>();
            Persons = new Vector<Person>();
            Sites = new Vector<Site>();
        }
        public Storage(int timespanIncCnt, int personIncCnt, int siteIncCnt,
            Vector<TimeSpan> TimeSpans, Vector<Person> Persons, Vector<Site> Sites)
        {
            this.timespanIncCnt = timespanIncCnt;
            this.personIncCnt = personIncCnt;
            this.siteIncCnt = siteIncCnt;
            this.TimeSpans = TimeSpans;
            this.Persons = Persons;
            this.Sites = Sites;
        }
        public static Storage read(System.IO.BinaryReader reader)
        {
            int timespanIncCnt, personIncCnt, siteIncCnt;
            Vector<TimeSpan> TimeSpans;
            Vector<Person> Persons;
            Vector<Site> Sites;
            timespanIncCnt = reader.ReadInt32();
            personIncCnt = reader.ReadInt32();
            siteIncCnt = reader.ReadInt32();
            TimeSpans = VectorHelper.readTimeSpans(reader);
            Persons = VectorHelper.readPersons(reader);
            Sites = VectorHelper.readSites(reader);
            return new Storage(timespanIncCnt, personIncCnt, siteIncCnt, TimeSpans, Persons, Sites);
        }
        public void write(System.IO.BinaryWriter writer)
        {
            writer.Write(timespanIncCnt);
            writer.Write(personIncCnt);
            writer.Write(siteIncCnt);
            VectorHelper.writeTimeSpans(writer, TimeSpans);
            VectorHelper.writePersons(writer, Persons);
            VectorHelper.writeSites(writer, Sites);
        }
    }
}

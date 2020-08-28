using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.Strcture
{
    class TimeSpan
    {
        public int ID;
        public int startHour, endHour;
        public int personID;
        public TimeSpan(int ID, int startHour, int endHour, int personID)
        {
            this.ID = ID;
            this.startHour = startHour;
            this.endHour = endHour;
            this.personID = personID;
        }
        public static TimeSpan read(System.IO.BinaryReader reader)
        {
            int ID, startHour, endHour, personID;
            ID = reader.ReadInt32();
            startHour = reader.ReadInt32();
            endHour = reader.ReadInt32();
            personID = reader.ReadInt32();
            return new TimeSpan(ID, startHour, endHour, personID);
        }
        public void write(System.IO.BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(startHour);
            writer.Write(endHour);
            writer.Write(personID);
        }
    }
}

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
        public bool isProtected;
        public TimeSpan(int ID, int startHour, int endHour, int personID, bool isProtected)
        {
            this.ID = ID;
            this.startHour = startHour;
            this.endHour = endHour;
            this.personID = personID;
            this.isProtected = isProtected;
        }
        public static TimeSpan read(System.IO.BinaryReader reader)
        {
            int ID, startHour, endHour, personID;
            bool isProtected;
            ID = reader.ReadInt32();
            startHour = reader.ReadInt32();
            endHour = reader.ReadInt32();
            personID = reader.ReadInt32();
            isProtected = reader.ReadBoolean();
            return new TimeSpan(ID, startHour, endHour, personID, isProtected);
        }
        public void write(System.IO.BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(startHour);
            writer.Write(endHour);
            writer.Write(personID);
            writer.Write(isProtected);
        }
    }
}

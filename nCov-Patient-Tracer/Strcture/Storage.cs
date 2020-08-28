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
        public int coordIncCnt, personIncCnt, siteIncCnt;
        public Vector<Coordinate> Coordinates;
        public Vector<Person> Persons;
        public Vector<Site> Sites;
        public Storage()
        {
            coordIncCnt = personIncCnt = siteIncCnt = 0;
            Coordinates = new Vector<Coordinate>();
            Persons = new Vector<Person>();
            Sites = new Vector<Site>();
        }
        public Storage(int coordIncCnt, int personIncCnt, int siteIncCnt,
            Vector<Coordinate> Coordinates, Vector<Person> Persons, Vector<Site> Sites)
        {
            this.coordIncCnt = coordIncCnt;
            this.personIncCnt = personIncCnt;
            this.siteIncCnt = siteIncCnt;
            this.Coordinates = Coordinates;
            this.Persons = Persons;
            this.Sites = Sites;
        }
        public static Storage read(System.IO.BinaryReader reader)
        {
            int coordIncCnt, personIncCnt, siteIncCnt;
            Vector<Coordinate> Coordinates;
            Vector<Person> Persons;
            Vector<Site> Sites;
            coordIncCnt = reader.ReadInt32();
            personIncCnt = reader.ReadInt32();
            siteIncCnt = reader.ReadInt32();
            Coordinates = VectorHelper.readCoordinates(reader);
            Persons = VectorHelper.readPersons(reader);
            Sites = VectorHelper.readSites(reader);
            return new Storage(coordIncCnt, personIncCnt, siteIncCnt, Coordinates, Persons, Sites);
        }
        public void write(System.IO.BinaryWriter writer)
        {
            writer.Write(coordIncCnt);
            writer.Write(personIncCnt);
            writer.Write(siteIncCnt);
            VectorHelper.writeCoordinates(writer, Coordinates);
            VectorHelper.writePersons(writer, Persons);
            VectorHelper.writeSites(writer, Sites);
        }
    }
}

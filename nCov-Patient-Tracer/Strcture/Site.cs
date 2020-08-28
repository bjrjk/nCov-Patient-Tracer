using nCov_Patient_Tracer.DSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.Strcture
{
    class Site
    {
        public int ID;
        public Coordinate coordinate;
        public string name;
        public Vector<int> timeSpanCollection;
        public Site(int ID, Coordinate coordinate,string name)
        {
            this.ID = ID;
            this.coordinate = coordinate;
            this.name = name;
            this.timeSpanCollection = new Vector<int>();
        }
        public Site(int ID, Coordinate coordinate, string name, Vector<int> timeSpanCollection)
        {
            this.ID = ID;
            this.coordinate = coordinate;
            this.name = name;
            this.timeSpanCollection = timeSpanCollection;
        }
        public static Site read(System.IO.BinaryReader reader)
        {
            int ID;
            Coordinate coordinate;
            string name;
            Vector<int> timeSpanCollection;
            ID = reader.ReadInt32();
            coordinate = Coordinate.read(reader);
            name = reader.ReadString();
            timeSpanCollection = VectorHelper.readInts(reader);
            return new Site(ID, coordinate, name, timeSpanCollection);
        }
        public void write(System.IO.BinaryWriter writer)
        {
            writer.Write(ID);
            coordinate.write(writer);
            writer.Write(name);
            VectorHelper.writeInts(writer, timeSpanCollection);
        }
    }
}

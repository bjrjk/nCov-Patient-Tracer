using nCov_Patient_Tracer.DSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.Strcture
{
    class Site //地点类
    {
        public int ID; //地点ID
        public Coordinate coordinate; //地点经纬度
        public string name; //地点名称
        public Vector<int> timeSpanCollection; //当前地点对应的全部访问时间段
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
        public static string getRiskLevel(int peopleNumber)
        {
            if (peopleNumber <= 2) return "green";
            else if (peopleNumber <= 10) return "yellow";
            else return "red";
        }
    }
}

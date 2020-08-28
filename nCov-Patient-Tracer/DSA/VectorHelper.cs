using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nCov_Patient_Tracer.Strcture;

namespace nCov_Patient_Tracer.DSA
{
    class VectorHelper
    {
        public static Vector<int> readInts(System.IO.BinaryReader reader)
        {
            Vector<int> v = new Vector<int>();
            int num = reader.ReadInt32();
            v.reserve(num);
            for (int i = 0; i < num; i++)
            {
                v[i] = reader.ReadInt32();
            }
            return v;
        }
        public static void writeInts(System.IO.BinaryWriter writer,Vector<int> v)
        {
            writer.Write(v.size());
            for (int i = 0; i < v.size(); i++)
            {
                writer.Write(v[i]);
            }
        }
        public static Vector<Coordinate> readCoordinates(System.IO.BinaryReader reader)
        {
            Vector<Coordinate> v = new Vector<Coordinate>();
            int num = reader.ReadInt32();
            v.reserve(num);
            for (int i = 0; i < num; i++)
            {
                v[i] = Coordinate.read(reader);
            }
            return v;
        }
        public static void writeCoordinates(System.IO.BinaryWriter writer, Vector<Coordinate> v)
        {
            writer.Write(v.size());
            for (int i = 0; i < v.size(); i++)
            {
                v[i].write(writer);
            }
        }
        public static Vector<Person> readPersons(System.IO.BinaryReader reader)
        {
            Vector<Person> v = new Vector<Person>();
            int num = reader.ReadInt32();
            v.reserve(num);
            for (int i = 0; i < num; i++)
            {
                v[i] = Person.read(reader);
            }
            return v;
        }
        public static void writePersons(System.IO.BinaryWriter writer, Vector<Person> v)
        {
            writer.Write(v.size());
            for (int i = 0; i < v.size(); i++)
            {
                v[i].write(writer);
            }
        }
        public static Vector<Site> readSites(System.IO.BinaryReader reader)
        {
            Vector<Site> v = new Vector<Site>();
            int num = reader.ReadInt32();
            v.reserve(num);
            for (int i = 0; i < num; i++)
            {
                v[i] = Site.read(reader);
            }
            return v;
        }
        public static void writeSites(System.IO.BinaryWriter writer, Vector<Site> v)
        {
            writer.Write(v.size());
            for (int i = 0; i < v.size(); i++)
            {
                v[i].write(writer);
            }
        }
    }
}

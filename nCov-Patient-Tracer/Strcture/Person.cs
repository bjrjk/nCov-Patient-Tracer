using nCov_Patient_Tracer.DSA;
using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.Strcture
{
    class Person
    {
        public int ID;
        public string name, company, address, telephone;
        public Vector<int> timeSpanCollection;
        public Person(int ID, string name, string company, string address,
            string telephone)
        {
            this.ID = ID;
            this.name = name;
            this.company = company;
            this.address = address;
            this.telephone = telephone;
            this.timeSpanCollection = new Vector<int>();
        }
        public Person(int ID, string name, string company, string address,
            string telephone, Vector<int> timeSpanCollection)
        {
            this.ID = ID;
            this.name = name;
            this.company = company;
            this.address = address;
            this.telephone = telephone;
            this.timeSpanCollection = timeSpanCollection;
        }
        public static Person read(System.IO.BinaryReader reader)
        {
            int ID;
            string name, company, address, telephone;
            Vector<int> timeSpanCollection;
            ID = reader.ReadInt32();
            name = reader.ReadString();
            company = reader.ReadString();
            address = reader.ReadString();
            telephone = reader.ReadString();
            timeSpanCollection = VectorHelper.readInts(reader);
            return new Person(ID, name , company, address,telephone,timeSpanCollection);
        }
        public void write(System.IO.BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(name);
            writer.Write(company);
            writer.Write(address);
            writer.Write(telephone);
            VectorHelper.writeInts(writer, timeSpanCollection);
        }
    }
}

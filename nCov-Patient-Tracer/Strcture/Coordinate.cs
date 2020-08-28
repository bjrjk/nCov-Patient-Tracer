using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.Strcture
{
    class Coordinate
    {
        public double longitude, latitude;
        public Coordinate(double longitude,double latitude)
        {
            this.longitude = longitude;
            this.latitude = latitude;
        }
        public static Coordinate read(System.IO.BinaryReader reader)
        {
            double longitude, latitude;
            longitude = reader.ReadDouble();
            latitude = reader.ReadDouble();
            return new Coordinate(longitude, latitude);
        }
        public void write(System.IO.BinaryWriter writer)
        {
            writer.Write(longitude);
            writer.Write(latitude);
        }
    }
}

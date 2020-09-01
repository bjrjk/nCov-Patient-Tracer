using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nCov_Patient_Tracer.Strcture;

namespace nCov_Patient_Tracer
{
    class Global
    {
        public const string WebURL = "https://ncov-patient-tracer.renjikai.com/";
        public static Storage storage = new Storage();
        public static ProcessedStorage processedStorage;
        public static string configPath;
    }
}

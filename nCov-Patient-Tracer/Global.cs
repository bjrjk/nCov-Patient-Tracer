using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nCov_Patient_Tracer.DSA;
using nCov_Patient_Tracer.Strcture;
using TimeSpan = nCov_Patient_Tracer.Strcture.TimeSpan;

namespace nCov_Patient_Tracer
{
    class Global
    {
        public const string WebURL = "https://ncov-patient-tracer.renjikai.com/"; //浏览器网站地址
        public static Storage storage = new Storage(); //原始数据存储
        public static ProcessedStorage processedStorage; //处理数据存储
        public static string configPath; //配置文件路径
        public static Vector<Person> personArr; //地图展示用人员数组
        public static Vector<Vector<Vector<TimeSpan>>> timeSpanArr; //地图展示用时间段数组
    }
}

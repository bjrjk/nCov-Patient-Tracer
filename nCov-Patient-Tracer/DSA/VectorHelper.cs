﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nCov_Patient_Tracer.Strcture;

namespace nCov_Patient_Tracer.DSA
{
    class VectorHelper
    {
        public static Vector<int> readInts(System.IO.BinaryReader reader) //读入int数组
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
        public static void writeInts(System.IO.BinaryWriter writer,Vector<int> v) //写入int数组
        {
            writer.Write(v.size());
            for (int i = 0; i < v.size(); i++)
            {
                writer.Write(v[i]);
            }
        }
        public static Vector<Coordinate> readCoordinates(System.IO.BinaryReader reader) //读入坐标
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
        public static void writeCoordinates(System.IO.BinaryWriter writer, Vector<Coordinate> v) //写入坐标
        {
            writer.Write(v.size());
            for (int i = 0; i < v.size(); i++)
            {
                v[i].write(writer);
            }
        }
        public static Vector<Person> readPersons(System.IO.BinaryReader reader) //读入人员
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
        public static void writePersons(System.IO.BinaryWriter writer, Vector<Person> v) //写入人员
        {
            writer.Write(v.size());
            for (int i = 0; i < v.size(); i++)
            {
                v[i].write(writer);
            }
        }
        public static Vector<Site> readSites(System.IO.BinaryReader reader) //读入地点
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
        public static void writeSites(System.IO.BinaryWriter writer, Vector<Site> v) //写入地点
        {
            writer.Write(v.size());
            for (int i = 0; i < v.size(); i++)
            {
                v[i].write(writer);
            }
        }
        public static Vector<Strcture.TimeSpan> readTimeSpans(System.IO.BinaryReader reader) //读入时间段
        {
            Vector<Strcture.TimeSpan> v = new Vector<Strcture.TimeSpan>();
            int num = reader.ReadInt32();
            v.reserve(num);
            for (int i = 0; i < num; i++)
            {
                v[i] = Strcture.TimeSpan.read(reader);
            }
            return v;
        }
        public static void writeTimeSpans(System.IO.BinaryWriter writer, Vector<Strcture.TimeSpan> v) //写入时间段
        {
            writer.Write(v.size());
            for (int i = 0; i < v.size(); i++)
            {
                v[i].write(writer);
            }
        }
        public static Vector<int> Str2IntVector(string s) //字符串转int数组
        {
            if (s.Trim() == "") return new Vector<int>();
            string[] arr = s.Trim().Split();
            Vector<int> v = new Vector<int>();
            v.reserve(arr.Length);
            for(int i = 0; i < arr.Length; i++)
            {
                v[i] = int.Parse(arr[i]);
            }
            return v;
        }
        public static string IntVector2Str(Vector<int> v) //int数组转字符串
        {
            string s = "";
            for (int i = 0; i < v.size(); i++)
            {
                s += v[i].ToString() + " ";
            }
            return s;
        }
    }
}

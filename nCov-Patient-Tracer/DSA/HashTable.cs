using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.DSA
{

    class HashTable<T>
        where T : IComparable<T>
    {
        private Vector<Vector<T>> arr;
        public HashTable(int size=65537)
        {
            arr = new Vector<Vector<T>>();
            arr.reserve(size);
            for (int i = 0; i < size; i++)
            {
                arr[i] = new Vector<T>();
            }
        }
        public void insert(T value)
        {
            int pos = value.GetHashCode() % arr.size();
            arr[pos].append(value);
        }
        public T query(T value)
        {
            int pos = value.GetHashCode() % arr.size();
            for(int i = 0; i < arr[pos].size(); i++)
            {
                if (value.CompareTo(arr[pos][i]) == 0) return arr[pos][i];
            }
            return default(T);
        }
    }
}

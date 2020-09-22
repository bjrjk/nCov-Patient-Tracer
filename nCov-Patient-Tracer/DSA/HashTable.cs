using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.DSA
{

    class HashTable<T>
        where T : IComparable<T> //类型T需要实现可比较接口(IComparable)
    {
        private Vector<Vector<T>> arr; //采用拉链法实现的哈希表数组
        public HashTable(int size=65537) //构造函数
        {
            arr = new Vector<Vector<T>>();
            arr.reserve(size);
            for (int i = 0; i < size; i++)
            {
                arr[i] = new Vector<T>();
            }
        }
        public void insert(T value) //哈希表中插入元素
        {
            int pos = value.GetHashCode() % arr.size();
            arr[pos].append(value);
        }
        public T query(T value) //哈希表中查询元素
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

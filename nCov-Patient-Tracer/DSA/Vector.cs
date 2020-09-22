using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.DSA
{
    class Vector<T>
    {
        public T[] _arr; //内部数组指针
        private int _size; //记录数组元素个数
        private int _capacity; //记录数组容量
        public Vector() //构造函数
        {
            _capacity = 16;
            _arr = new T[_capacity];
            _size = 0;
        }
        public T this[int i] //重载[]运算符，C#中访问器
        {
            get { return _arr[i]; }
            set { _arr[i] = value; }
        }
        private void _resize(int size) //内部函数，调整数组元素个数
        {
            T[] _newArr = new T[size];
            for (int i = 0; i < Math.Min(_size, size); i++)
                _newArr[i] = _arr[i];
            _arr = _newArr;
            _capacity = size;
        }
        public int size() //获得当前数组元素个数
        {
            return _size;
        }
        public void reserve(int size) //公开函数，调整数组元素个数到size
        {
            if (size <= _capacity)
            {
                _size = size;
                return;
            }
            _resize(size);
            _size = size;
        }
        public void clear() //数组元素清零
        {
            reserve(0);
        }
        public void append(T value) //在数组末尾添加元素value
        {
            if (_size >= _capacity) _resize(2 * _capacity);
            _arr[_size] = value;
            _size++;
        } 
        public bool remove(int index) //删除下标index处的元素
        {
            if (index < 0 || index >= _size) return false;
            for (int i = index; i < _size; i++) _arr[i] = _arr[i + 1];
            _size--;
            return true;
        }
    }
}

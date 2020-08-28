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
        private T[] _arr;
        private int _size;
        private int _capacity;
        public Vector()
        {
            _capacity = 16;
            _arr = new T[_capacity];
            _size = 0;
        }
        public T this[int i]
        {
            get { return _arr[i]; }
            set { _arr[i] = value; }
        }
        private void _resize(int size)
        {
            T[] _newArr = new T[size];
            for (int i = 0; i < Math.Min(_size, size); i++)
                _newArr[i] = _arr[i];
            _arr = _newArr;
            _capacity = size;
        }
        public int size()
        {
            return _size;
        }
        public void reserve(int size)
        {
            if (size <= _capacity)
            {
                _size = size;
                return;
            }
            _resize(size);
            _size = size;
        }
        public void clear()
        {
            reserve(0);
        }
        public void append(T value)
        {
            if (_size >= _capacity) _resize(2 * _capacity);
            _arr[_size] = value;
            _size++;
        } 
        
    }
}

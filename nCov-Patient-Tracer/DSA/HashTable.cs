using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.DSA
{

    class HashTable<T>
        where T:IHashable
    {
        private Vector<Vector<T>> arr;
        public HashTable()
        {

        }
    }
    interface IHashable
    {
        int Hash(int MOD);
    }
}

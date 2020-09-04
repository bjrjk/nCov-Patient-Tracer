using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.DSA
{
    class IntervalTree<T>where T:ISegment
    {
        struct Point
        {
            int pos;

            T obj;
        }
    }
    interface ISegment
    {
        int getStartPoint();
        int getEndPoint();
        bool InterSection(ISegment seg);
        bool InterSection(int point);
    }
}

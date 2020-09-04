using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.DSA
{
    class IntervalTree<T> where T : ISegment
    {
        private class Point : IComparable<Point>
        {
            public int pos;
            public bool isStart;
            public T obj;
            public Point(int pos, bool isStart, T obj)
            {
                this.pos = pos;
                this.isStart = isStart;
                this.obj = obj;
            }
            public int CompareTo(Point other)
            {
                return pos.CompareTo(other.pos);
            }
        }
        private class PointComparer : IComparer<Point>
        {
            public int Compare(Point x, Point y)
            {
                return x.pos.CompareTo(y.pos);
            }
        }
        private class Node
        {
            public Node lc, rc;
            public int Xmid;
            public Vector<Point> leftList, rightList;
            public Node()
            {
                lc = rc = null;
                Xmid = -1;
                leftList = new Vector<Point>();
                rightList = new Vector<Point>();
            }
        }
        private Node root;
        public IntervalTree(Vector<T> arr)
        {
            //初始化线段端点数组
            Vector<Point> segmentEndpointArr = new Vector<Point>();
            for (int i = 0; i < arr.size(); i++)
            {
                segmentEndpointArr.append(new Point(arr[i].getStartPoint(), true, arr[i]));
                segmentEndpointArr.append(new Point(arr[i].getEndPoint(), false, arr[i]));
            }
            //线段端点数组排序
            //需要注意的是，此处排序一次，建树时做的操作不会影响端点的顺序性，所以之后无需重新排序
            Algorithm.quickSort(segmentEndpointArr, new PointComparer());
            //建树
            root = construct(segmentEndpointArr);
        }
        private Node construct(Vector<Point> segmentEndpointArr)
        {
            //平凡情况下，叶子节点为空
            if (segmentEndpointArr.size() == 0) return null;
            //创建节点
            Node node = new Node();
            //找中位数Median点位置
            int medianPointPos = (segmentEndpointArr.size() - 1) / 2;
            //确定中位数点坐标
            node.Xmid = segmentEndpointArr[medianPointPos].pos;
            //分配S{left},S{right}端点的数组
            Vector<Point> lSegmentEndpointArr = new Vector<Point>();
            Vector<Point> rSegmentEndpointArr = new Vector<Point>();
            //遍历全部端点
            for (int i = 0; i < segmentEndpointArr.size(); i++)
            {
                //取交结果
                int intersectionResult = segmentEndpointArr[i].obj.InterSection(node.Xmid);
                if (intersectionResult < 0) //线段在点左侧
                {
                    //分配到S{left}集合做递归处理
                    lSegmentEndpointArr.append(segmentEndpointArr[i]);
                }
                else if (intersectionResult > 0) //线段在点右侧
                {
                    //分配到S{right}集合做递归处理
                    rSegmentEndpointArr.append(segmentEndpointArr[i]);
                }
                else //线段与点相交
                {
                    if (segmentEndpointArr[i].isStart) //是线段的左端点
                    {
                        //加入左关联列表
                        node.leftList.append(segmentEndpointArr[i]);
                    }
                    else //是线段的右端点
                    {
                        //加入右关联列表
                        node.rightList.append(segmentEndpointArr[i]);
                    }
                }
            }
            node.lc = construct(lSegmentEndpointArr);
            node.rc = construct(rSegmentEndpointArr);
            return node;
        }
        public Vector<T> query(int Qx)
        {
            Vector<T> results = new Vector<T>();
            query(root, Qx, results);
            return results;
        }
        private void query(Node node, int Qx, Vector<T> results)
        {
            if (node == null) return;
            if (Qx < node.Xmid) //查询的坐标在Xmid左侧
            {
                //遍历左关联列表
                for(int i = 0; i < node.leftList.size(); i++)
                {
                    //不交时跳出
                    if (node.leftList[i].obj.InterSection(Qx) != 0) break;
                    //相交时报告
                    results.append(node.leftList[i].obj);
                }
                //继续查询S{left}集合
                query(node.lc, Qx, results);
            }
            else if (node.Xmid < Qx) //查询的坐标在Xmid右侧
            {
                //遍历右关联列表
                for (int i = node.rightList.size()-1; i >=0; i--)
                {
                    //不交时跳出
                    if (node.rightList[i].obj.InterSection(Qx) != 0) break;
                    //相交时报告
                    results.append(node.rightList[i].obj);
                }
                //继续查询S{left}集合
                query(node.rc, Qx, results);
            }
            else //查询的坐标与Xmid一致
            {
                //遍历任意关联列表并全部报告
                for (int i = 0; i < node.leftList.size(); i++)
                {
                    results.append(node.leftList[i].obj);
                }
            }
        }
    }
    interface ISegment
    {
        int getStartPoint();
        int getEndPoint();
        bool InterSection(ISegment seg);
        int InterSection(int point); //小于0线段在点左侧，等于0相交，大于0线段在点右侧
    }
}

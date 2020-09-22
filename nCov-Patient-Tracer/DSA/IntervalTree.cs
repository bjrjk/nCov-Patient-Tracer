using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nCov_Patient_Tracer.DSA
{
    class IntervalTree<T> where T : ISegment //要求必须继承“线段”接口
    {
        private class Point : IComparable<Point> //内部“端点”类，需要是可比较的
        {
            public int pos; //端点在X轴上的位置
            public bool isStart; //是线段的左端点还是右端点，左端点为true，右端点为false
            public T obj; //指回原线段的引用
            public Point(int pos, bool isStart, T obj) //端点的构造函数
            {
                this.pos = pos;
                this.isStart = isStart;
                this.obj = obj;
            }
            public int CompareTo(Point other) //比较函数接口实现
            {
                return pos.CompareTo(other.pos);
            }
        }
        private class PointComparer : IComparer<Point> //端点的比较器类
        {
            public int Compare(Point x, Point y) //比较函数接口实现
            {
                return x.pos.CompareTo(y.pos);
            }
        }
        private class Node //区间树节点定义
        {
            public Node lc, rc; //左孩子节点引用，右孩子节点引用
            public int Xmid; //当前节点的穿刺点在X轴上的坐标
            public Vector<Point> leftList, rightList; //左关联列表和右关联列表
            public Node() //构造函数
            {
                lc = rc = null;
                Xmid = -1;
                leftList = new Vector<Point>();
                rightList = new Vector<Point>();
            }
        }
        private Node root; //区间树根节点引用
        public IntervalTree(Vector<T> arr) //区间树的公开构造函数，要求arr是实现了线段接口ISegment的类型的实例集合
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
        private Node construct(Vector<Point> segmentEndpointArr) //区间树的私有构造函数，其中segmentEndpointArr为分配到当前节点的线段端点集合
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
        public Vector<T> query(int Qx) //区间树的公开查询函数，Qx为穿刺点在X轴上的坐标，返回包含穿刺点的线段的引用集合
        {
            Vector<T> results = new Vector<T>();
            query(root, Qx, results);
            return results;
        }
        private void query(Node node, int Qx, Vector<T> results) //区间树的私有查询函数，node是当前查询的区间树节点，Qx为穿刺点在X轴上的坐标，results是包含穿刺点的线段的引用集合
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
    interface ISegment //“线段”接口，只有实现了该接口的类才能够调用区间树进行穿刺查询
    {
        int getStartPoint(); //获得线段左端点坐标
        int getEndPoint(); //获得线段右端点坐标
        bool InterSection(ISegment seg); //查询与另一线段是否有交
        int InterSection(int point); //查询与另一点是否有交，小于0线段在点左侧，等于0相交，大于0线段在点右侧
    }
}

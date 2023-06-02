using Firmament.Module;
using Firmament.Utils.QuardTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Firmament.Utils.QuardTree
{
    // 定义四叉树
    public class QuadTree
    {
        public  QuadTreeNode root;
        public QuadTree(double x, double y, double width , double height)
        {
            root = new QuadTreeNode(new Rect(x,y,width,height));
        }

        // 将对象插入四叉树
        public void Insert(Ball obj)
        {
            InsertObject(root, obj);
        }

        // 递归插入对象到指定节点或子节点
        private void InsertObject(QuadTreeNode node, Ball obj)
        {
            // 检查对象是否在节点范围内
            if (!IsObjectInBounds(node.Bounds, obj))
            {
                return;
            }

            // 如果当前节点没有子节点，将对象添加到当前节点
            if (node.Children[0] == null)
            {
                node.Objects.Add(obj);
                node.IsLeaf = true;
            }
            else
            {
                node.IsLeaf = false;
                // 递归插入对象到子节点
                foreach (var child in node.Children)
                {
                    InsertObject(child, obj);
                }
            }

            // 检查当前节点的对象数量，如果超过阈值，则划分子节点
            if (node.Objects.Count > 10 && node.Children[0] == null)
            {
                SplitNode(node);
            }
        }

        // 划分节点为四个子节点
        private void SplitNode(QuadTreeNode node)
        {
            double subWidth = node.Bounds.Width / 2;
            double subHeight = node.Bounds.Height / 2;
            double x = node.Bounds.X;
            double y = node.Bounds.Y;

            node.Children[0] = new QuadTreeNode(new Rect { X = x + subWidth, Y = y, Width = subWidth, Height = subHeight });
            node.Children[1] = new QuadTreeNode(new Rect { X = x, Y = y, Width = subWidth, Height = subHeight });
            node.Children[2] = new QuadTreeNode(new Rect { X = x, Y = y + subHeight, Width = subWidth, Height = subHeight });
            node.Children[3] = new QuadTreeNode(new Rect { X = x + subWidth, Y = y + subHeight, Width = subWidth, Height = subHeight });
            // 将当前节点的对象重新插入到子节点
            foreach (var obj in node.Objects)
            {
                InsertObject(node, obj);
            }

            node.Objects.Clear();
        }

        // 检查对象是否在边界框内
        private bool IsObjectInBounds(Rect bounds, Ball obj)
        {
            return obj.x >= bounds.X &&
                   obj.y >= bounds.Y &&
                   obj.x + obj.Width <= bounds.X + bounds.Width &&
                   obj.y + obj.Height <= bounds.Y + bounds.Height;
        }


        public List<Ball> Query(Ball obj)
        {
            List<Ball> results = new List<Ball>();
            QueryObjects(root, obj, results);
            return results;
        }

        private void QueryObjects(QuadTreeNode node, Ball obj, List<Ball> results)
        {
            if (node == null)
            {
                return;
            }
            // 递归查询子节点
            if (!node.IsLeaf)
            {
                foreach (var child in node.Children)
                {
                    QueryObjects(child, obj, results);
                }
                
            }
            else {
                // 检查当前节点的边界框是否与查询对象相交
                if (IsObjectInBounds(node.Bounds, obj))
                {
                    // 将当前节点中的对象添加到结果列表
                    foreach (var objectInNode in node.Objects)
                    {
                        results.Add(objectInNode);
                    }
                }
            }
            
           
        }

        /// <summary>
        /// 更新节点状态
        /// </summary>
        public void Update()
        {
            ClearTree(root);
            RebuildTree(root);
        }

        private void ClearTree(QuadTreeNode node)
        {
            node.Objects.Clear();

            if (!node.IsLeaf)
            {
                foreach (QuadTreeNode child in node.Children)
                {
                    ClearTree(child);
                }

                node.IsLeaf = true;
            }
        }

        private void RebuildTree(QuadTreeNode node)
        {
            foreach (Ball obj in node.Objects)
            {
                InsertObject(node,obj);
            }

            if (!node.IsLeaf)
            {
                foreach (QuadTreeNode child in node.Children)
                {
                    RebuildTree(child);
                }
            }
        }

        //private List<GameObject> FindObjectsInBounds(Rect bounds)
        //{
        //    // 根据你的场景中的对象存储方式，实现一个根据边界查找对象的方法
        //    // 返回与给定边界相交的所有游戏对象的列表
        //    // 你可以使用 Physics2D.OverlapAreaAll 或其他适当的方法来实现这个功能
        //}


        /// <summary>
        /// 删除节点
        /// </summary>
        public void delete(Ball rec) {
            deleteNode(root, rec);
        }

        public void deleteNode(QuadTreeNode node, Ball rec)
        {
            foreach (Ball rect in node.Objects)
            {
                if (rec == rect)
                {
                    root.Objects.Remove(rect);
                    return;
                }
            }
            foreach (QuadTreeNode qrNode in node.Children) {

                if (qrNode != null) {
                    if (qrNode.Objects.Count == 0)
                    {
                        qrNode.Children = null;
                    }
                    else {
                        deleteNode(qrNode, rec);
                    }
                   
                }
              
            }
           
        }



    }
}

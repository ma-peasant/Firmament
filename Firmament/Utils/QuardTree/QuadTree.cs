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
        //public List<Ball> ballList = new List<Ball>();

        public  QuadTreeNode root;
        public int MaxCount = 10;
        //用于给节点设置flag
        public int flag = 0;
        public QuadTree(Rect rect ,int maxCount)
        {
            this.MaxCount = maxCount;
            root = new QuadTreeNode(rect,flag++);
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
            if (node.Children.Count== 0)
            {
                obj.flag = node.flag;
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
            if (node.Objects.Count > MaxCount && node.Children.Count == 0)
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

            node.Children.Add(new QuadTreeNode(new Rect { X = x + subWidth, Y = y, Width = subWidth, Height = subHeight }, this.flag++));
            node.Children.Add(new QuadTreeNode(new Rect { X = x, Y = y, Width = subWidth, Height = subHeight }, flag++));
            node.Children.Add(new QuadTreeNode(new Rect { X = x, Y = y + subHeight, Width = subWidth, Height = subHeight }, flag++));
            node.Children.Add(new QuadTreeNode(new Rect { X = x + subWidth, Y = y + subHeight, Width = subWidth, Height = subHeight }, flag++));
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

        /// <summary>
        /// 更新节点状态
        /// </summary>
        public void Update(List<Ball> balls)
        {
            flag = 0;
            //1、清理四叉树节点
            ClearTree(root);
            //2、重新插入节点
            RebuildTree(root, balls);
        }

        private void ClearTree(QuadTreeNode node)
        {
            node.Clear(true);
        }

        private void RebuildTree(QuadTreeNode node, List<Ball> balls)
        {
            foreach (Ball obj in balls)
            {
                InsertObject(node,obj);
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

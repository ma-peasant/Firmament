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

namespace Firmament.Utils
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
        public void Insert(Rectangle obj)
        {
            InsertObject(root, obj);
        }

        // 递归插入对象到指定节点或子节点
        private void InsertObject(QuadTreeNode node, Rectangle obj)
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
            }
            else
            {
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
        private bool IsObjectInBounds(Rect bounds, Rectangle obj)
        {
            return Canvas.GetLeft(obj) >= bounds.X &&
                   Canvas.GetTop(obj) >= bounds.Y &&
                   Canvas.GetLeft(obj) + obj.Width <= bounds.X + bounds.Width &&
                   Canvas.GetTop(obj) + obj.Height <= bounds.Y + bounds.Height;
        }


        public List<Rectangle> Query(Rectangle obj)
        {
            List<Rectangle> results = new List<Rectangle>();
            QueryObjects(root, obj, results);
            return results;
        }

        private void QueryObjects(QuadTreeNode node, Rectangle obj, List<Rectangle> results)
        {
            if (node == null)
            {
                return;
            }

            // 检查当前节点的边界框是否与查询对象相交
            if (IsIntersecting(node.Bounds, obj))
            {
                // 将当前节点中的对象添加到结果列表
                foreach (var objectInNode in node.Objects)
                {
                    results.Add(objectInNode);
                }
                // 递归查询子节点
                foreach (var child in node.Children)
                {
                    QueryObjects(child, obj, results);
                }
            }
        }

        private bool IsIntersecting(Rect a, Rectangle b)
        {
            //GeneralTransform transform = b.TransformToAncestor((Canvas)b.Parent); // 'this' 表示矩形的父元素
            //Point position = transform.Transform(new Point(0, 0));

            //// 创建一个新的 Rect 对象
            //Rect rect = new Rect(position, new Size(b.ActualWidth, b.ActualHeight));

            return a.Right > Canvas.GetLeft(b) && a.Left < Canvas.GetLeft(b)+b.Width && a.Bottom > Canvas.GetTop(b) && a.Top < Canvas.GetTop(b)+b.Height;
        }

        private bool IsContaining(Rect a, Rect b)
        {
            return a.Left <= b.Left && a.Top <= b.Top && a.Right >= b.Right && a.Bottom >= b.Bottom;
        }

        /// <summary>
        /// 更新节点状态
        /// </summary>
        public void UpdateState() {
            
        }

       
    }
}

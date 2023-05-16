using Firmament.Utils.QuardTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firmament.Utils
{
    // 定义四叉树
    public class QuadTree
    {
        private QuadTreeNode root;

        public QuadTree(BoundingBox bounds)
        {
            root = new QuadTreeNode(bounds);
        }

        // 将对象插入四叉树
        public void Insert(BoundingBox obj)
        {
            InsertObject(root, obj);
        }

        // 递归插入对象到指定节点或子节点
        private void InsertObject(QuadTreeNode node, BoundingBox obj)
        {
            // 检查对象是否在节点范围内
            //if (!IsObjectInBounds(node.Bounds, obj))
            //{
            //    return;
            //}

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
            float subWidth = node.Bounds.Width / 2;
            float subHeight = node.Bounds.Height / 2;
            float x = node.Bounds.X;
            float y = node.Bounds.Y;

            node.Children[0] = new QuadTreeNode(new BoundingBox { X = x + subWidth, Y = y, Width = subWidth, Height = subHeight });
            node.Children[1] = new QuadTreeNode(new BoundingBox { X = x, Y = y, Width = subWidth, Height = subHeight });
            node.Children[2] = new QuadTreeNode(new BoundingBox { X = x, Y = y + subHeight, Width = subWidth, Height = subHeight });
            node.Children[3] = new QuadTreeNode(new BoundingBox { X = x + subWidth, Y = y + subHeight, Width = subWidth, Height = subHeight });

            // 将当前节点的对象重新插入到子节点
            foreach (var obj in node.Objects)
            {
                InsertObject(node, obj);
            }

            node.Objects.Clear();
        }

        // 检查对象是否在边界框内
        private bool IsObjectInBounds(BoundingBox bounds, BoundingBox obj)
        {
            return obj.X >= bounds.X &&
                   obj.Y >= bounds.Y &&
                   obj.X + obj.Width <= bounds.X + bounds.Width &&
                   obj.Y + obj.Height <= bounds.Y + bounds.Height;
        }


        public List<BoundingBox> Query(BoundingBox obj)
        {
            List<BoundingBox> results = new List<BoundingBox>();
            QueryObjects(root, obj, results);
            return results;
        }

        private void QueryObjects(QuadTreeNode node, BoundingBox obj, List<BoundingBox> results)
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

        private bool IsIntersecting(BoundingBox a, BoundingBox b)
        {
            return a.X < b.X + b.Width &&
                   a.X + a.Width > b.X &&
                   a.Y < b.Y + b.Height &&
                   a.Y + a.Height > b.Y;
        }

        /// <summary>
        /// 更新节点状态
        /// </summary>
        public void UpdateState() {
            
        }

       
    }
}

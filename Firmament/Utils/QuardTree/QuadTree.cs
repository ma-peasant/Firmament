﻿using Firmament.Module;
using System.Collections.Generic;
using System.Windows;
namespace Firmament.Utils.QuardTree
{
    // 定义四叉树
    public class QuadTree<T> where T : IBaseElement
    {
        public  QuadTreeNode<T> root;
        public int MaxCount = 10;  //默认是10
        //用于给节点设置flag
        public int flag = 0;
        public QuadTree(Rect rect ,int maxCount)
        {
            this.MaxCount = maxCount;
            root = new QuadTreeNode<T>(rect,flag++);
        }

        // 将对象插入四叉树
        public void Insert(T obj)
        {
            InsertObject(root, obj);
        }

        // 递归插入对象到指定节点或子节点
        private void InsertObject(QuadTreeNode<T> node, T obj)
        {
            // 检查对象是否在节点范围内
            if (!IsObjectInBounds(node.Bounds, obj))
            {
                return;
            }

            // 如果当前节点没有子节点，将对象添加到当前节点
            if (node.Children.Count== 0)
            {
                obj.Flag = node.flag;
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
        private void SplitNode(QuadTreeNode<T> node)
        {
            double subWidth = node.Bounds.Width / 2;
            double subHeight = node.Bounds.Height / 2;
            double x = node.Bounds.X;
            double y = node.Bounds.Y;

            node.Children.Add(new QuadTreeNode<T>(new Rect { X = x + subWidth, Y = y, Width = subWidth, Height = subHeight }, this.flag++));
            node.Children.Add(new QuadTreeNode<T>(new Rect { X = x, Y = y, Width = subWidth, Height = subHeight }, flag++));
            node.Children.Add(new QuadTreeNode<T>(new Rect { X = x, Y = y + subHeight, Width = subWidth, Height = subHeight }, flag++));
            node.Children.Add(new QuadTreeNode<T>(new Rect { X = x + subWidth, Y = y + subHeight, Width = subWidth, Height = subHeight }, flag++));
            // 将当前节点的对象重新插入到子节点
            foreach (var obj in node.Objects)
            {
                InsertObject(node, obj);
            }

            node.Objects.Clear();
        }

        // 检查对象是否在边界框内
        private bool IsObjectInBounds(Rect bounds, T obj)
        {
            return obj.X >= bounds.X &&
                   obj.Y >= bounds.Y &&
                   obj.X + obj.Width <= bounds.X + bounds.Width &&
                   obj.Y + obj.Height <= bounds.Y + bounds.Height;
        }

        /// <summary>
        /// 更新节点状态
        /// </summary>
        public void Update(List<T> list)
        {
            flag = 0;
            //1、清理四叉树节点
            ClearTree(root);
            //2、重新插入节点
            RebuildTree(root, list);
        }

        private void ClearTree(QuadTreeNode<T> node)
        {
            node.Clear(true);
        }

        private void RebuildTree(QuadTreeNode<T> node, List<T> list)
        {
            foreach (T obj in list)
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
        public void delete(T rec)
        {
            deleteNode(root, rec);
        }

        public void deleteNode(QuadTreeNode<T> node, T rec)
        {
            foreach (T rect in node.Objects)
            {
                if (EqualityComparer<T>.Default.Equals(rec, rect))
                {
                    root.Objects.Remove(rect);
                    return;
                }
            }
            foreach (QuadTreeNode<T> qrNode in node.Children)
            {

                if (qrNode != null)
                {
                    if (qrNode.Objects.Count == 0)
                    {
                        qrNode.Children = null;
                    }
                    else
                    {
                        deleteNode(qrNode, rec);
                    }
                }
            }
        }
    }
}

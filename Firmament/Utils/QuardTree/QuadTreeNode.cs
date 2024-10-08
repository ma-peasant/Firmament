﻿using Firmament.Module;
using System.Collections.Generic;
using System.Windows;

namespace Firmament.Utils.QuardTree
{
    // 定义四叉树节点
    public class QuadTreeNode<T> where T : IBaseElement
    {
        //自身的边界
        public Rect Bounds { get; set; }
        public bool IsLeaf { get; set; }   //叶子节点，  没有子节点的节点称为叶子节点

        public int flag { get; set; }    //节点的标记，处于该节点下的objects的flag保持一致


        private List<T> objectList;
        //自身存在的矩形个数
        public List<T> Objects {
            get{ return objectList; }
            set {
                objectList = value;
            }
        }

        //子节点分区
        public List<QuadTreeNode<T>> Children { get; set; }

        public QuadTreeNode(Rect bounds ,int flag)
        {
            Bounds = bounds;
            Objects = new List<T>();
            Children = new List<QuadTreeNode<T>>();
            IsLeaf = true;
            this.flag = flag;
        }
        public void Clear(bool isRoot){
            if (!isRoot) {
                Objects = null;
            }
            foreach(QuadTreeNode<T> node in Children){
                node.Clear(false);
            }
            Children.Clear();
            IsLeaf = true;
        }

       
    }
}

using Firmament.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Firmament.Utils.QuardTree
{
    // 定义四叉树节点
    public class QuadTreeNode
    {
        //自身的边界
        public Rect Bounds { get; set; }
        public bool IsLeaf { get; set; }   //叶子节点，  没有子节点的节点称为叶子节点

        public int flag { get; set; }    //节点的标记，处于该节点下的objects的flag保持一致


        private List<Ball> objectList;
        //自身存在的矩形个数
        public List<Ball> Objects {
            get{ return objectList; }
            set {
                objectList = value;
            }
        }

        //子节点分区
        public List<QuadTreeNode> Children { get; set; }

        public QuadTreeNode(Rect bounds ,int flag)
        {
            Bounds = bounds;
            Objects = new List<Ball>();
            Children = new List<QuadTreeNode>();
            IsLeaf = true;
            this.flag = flag;
        }
        public void Clear(bool isRoot){
            if (!isRoot) {
                Objects = null;
            }
            foreach(QuadTreeNode node in Children){
                node.Clear(false);
            }
            Children.Clear();
            IsLeaf = true;
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firmament.Utils.QuardTree
{
    // 定义四叉树节点
    public class QuadTreeNode
    {
        //自身的边界
        public BoundingBox Bounds { get; set; }

        //自身存在的矩形个数
        public List<BoundingBox> Objects { get; set; }

        //子节点分区
        public QuadTreeNode[] Children { get; set; }

        public QuadTreeNode(BoundingBox bounds)
        {
            Bounds = bounds;
            Objects = new List<BoundingBox>();
            Children = new QuadTreeNode[4];
        }

       
    }
}

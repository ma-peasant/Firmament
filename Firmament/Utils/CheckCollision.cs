using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firmament.Utils
{
    //检测碰撞
    class CheckCollision
    {
        public void CheckCollison()
        {
            // 创建四叉树
            QuadTree quadTree = new QuadTree(sceneBounds);

            // 插入对象到四叉树
            foreach (var obj in gameObjects)
            {
                quadTree.Insert(obj);
            }

            // 进行碰撞检测
            foreach (var obj in gameObjects)
            {
                // 查询与当前对象可能发生碰撞的其他对象
                List<BoundingBox> potentialCollisions = quadTree.Query(obj);

                // 对查询到的对象进行碰撞检测
                foreach (var collisionObj in potentialCollisions)
                {
                    if (IsColliding(obj, collisionObj))
                    {
                        // 发生碰撞，处理碰撞逻辑
                        HandleCollision(obj, collisionObj);
                    }
                }
            }
        }
    }
}

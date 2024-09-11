using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QuardTreeProject.Module
{
   public interface IBaseElement 
    {

        double X { get; set; }  // X 坐标
        double Y { get; set; }  // Y 坐标
        double Width { get; set; }  // X 坐标
        double Height { get; set; }  // Y 坐标
        double XSpeed { get; set; }  // X移动速度
        double YSpeed { get; set; }  // Y移动速度
        bool HitState { get; set; }  // 是否相撞
        int Flag { get; set; }  //碰撞检测需要用到的， 同节点的flag相等
    }
}

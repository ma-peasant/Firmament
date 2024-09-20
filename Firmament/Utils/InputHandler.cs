using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Firmament.Utils
{
    public class InputHandler
    {
        // 定义按键处理委托和事件
        public delegate void KeyPressedHandler(KeyEventArgs key,string type = "DOWN");
        public event KeyPressedHandler OnKeyPressed;

        // 处理按键按下事件（WPF 的 KeyDown 事件）
        public void HandleKeyDown(KeyEventArgs e)
        {
            Key key = (e.Key == Key.ImeProcessed ? e.ImeProcessedKey : e.Key);
            // 将按键转换成字符串，并触发按键事件
            OnKeyPressed?.Invoke(e);
        }

        // 处理按键释放事件（WPF 的 KeyUp 事件）
        public void HandleKeyUp(KeyEventArgs e)
        {
            Key key = (e.Key == Key.ImeProcessed ? e.ImeProcessedKey : e.Key);
            // 你可以在这里处理按键释放逻辑，如果需要
            OnKeyPressed?.Invoke(e, "UP");
        }
    }
}

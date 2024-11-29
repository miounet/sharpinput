using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Core.Comm
{
    public class Cache
    {
        /// <summary>
        /// key按键队列
        /// </summary>
        public static Queue<KeysQueue> KeyQueue = new Queue<KeysQueue>();

        public static KeyboardHookStruct VDown = new KeyboardHookStruct();
        public static KeyboardHookStruct VUp = new KeyboardHookStruct();
    }

    //声明键盘钩子的封送结构类型 
    [StructLayout(LayoutKind.Sequential)]
    public class KeyboardHookStruct
    {
        public int vkCode;   //表示一个在1到254间的虚似键盘码 
        public int scanCode;   //表示硬件扫描码 
        public int flags;
        public int time;
        public int dwExtraInfo;
    }

    public class KeysQueue
    {
        public Keys KeyData { get; set; } = new Keys();
        public DateTime DataTime { get; set; } = DateTime.Now;
        public KeyboardHookStruct MyKeyboardHookStruct { get; set; } = new KeyboardHookStruct();
    }
}

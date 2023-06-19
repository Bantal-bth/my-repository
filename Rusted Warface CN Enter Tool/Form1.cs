using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rusted_Warface_CN_Enter_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Location = new Point(0, 0);
            textBox1.Width = this.Width;
            textBox1.Height = this.Height;
            
            startListen();
        }
        private KeyboardHook k_hook = new KeyboardHook();
        private KeyEventHandler myKeyEventHandeler = null;

        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            //  这里写具体实现
            if (e.KeyCode.Equals(Keys.F2))
            {
                if (this.Visible == true)
                    this.Visible = false;
                else
                {
                    this.Visible = true;
                    this.Activate();
                }

            }
            //if (e.KeyCode.Equals(Keys.T))
            //{

            //    if (this.Visible == false)
            //    {
            //        this.Visible = true;
            //        this.Activate();
            //    }
            //}
            if (e.KeyCode.Equals(Keys.Enter))
            {
                if (this.Visible == true)
                {
                    Clipboard.SetDataObject(textBox1.Text, true);
                    //Thread.Sleep(500);
                    this.Visible = false;
                    SendKeys.Send("{ENTER}");
                    //SendKeys.Send("{ENTER}");

                    SendKeys.Send("^V");
                    //SendKeys.Send("^V");
                    //SendKeys.Send("{ENTER}");
                    /*再写复制粘贴发出去
                     * 
                     * 还没写 撒泡尿标记一下
                     * 
                     * 写好了
                     * */
                }

            }
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        public void startListen()
        {
            myKeyEventHandeler = new KeyEventHandler(hook_KeyDown);
            k_hook.KeyDownEvent += myKeyEventHandeler;//钩住键按下
            k_hook.Start();//安装键盘钩子
        }

        /// <summary>
        /// 结束监听
        /// </summary>
        public void stopListen()
        {
            if (myKeyEventHandeler != null)
            {
                k_hook.KeyDownEvent -= myKeyEventHandeler;//取消按键事件
                myKeyEventHandeler = null;
                k_hook.Stop();//关闭键盘钩子
            }
        }
        

        [DllImport("user32")]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        [DllImport("user32")]
        static extern bool AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, bool fAttach);
        [DllImport("user32")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32")]
        static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        [DllImport("kernel32")]
        static extern IntPtr GetCurrentThreadId();

        void BringWindowToFront()
        {

            IntPtr currentHwnd = GetForegroundWindow();
            IntPtr intPtr = this.Handle;
            if (currentHwnd != intPtr)
            {
                IntPtr currentActiveThreadId = GetWindowThreadProcessId(currentHwnd, IntPtr.Zero);
                IntPtr thisThreadId = GetCurrentThreadId();

                AttachThreadInput(thisThreadId, currentActiveThreadId, true);  //将线程的输入处理机制绑定在当前激活的窗口上
                SwitchToThisWindow(intPtr, true);
                AttachThreadInput(thisThreadId, currentActiveThreadId, false);  //解除绑定
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            textBox1.Text = "";

        }
    }
}

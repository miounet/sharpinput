using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Core.Win
{
    public partial class BJNumFrm : Form
    {
        public BJNumFrm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.textBox1.Text = WinInput.keybjnum.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.textBox1.Text = WinInput.keybjnum.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WinInput.keybjnum = 0;
        }
    }

 

}

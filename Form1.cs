using System;
using System.Windows.Forms;

namespace CSharpHttpListener
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var network = new Network();
            await network.ConnectHttp();
        }
    }
}

using ServerForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerForm
{
    public partial class Form1 : Form
    {
        private Server _server;

        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _server = new Server(12345);
            _server.LogUpdated += OnLogUpdated;
            _server.Start();
        }

        private void OnLogUpdated(string message)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(OnLogUpdated), new object[] { message });
                return;
            }

            listBox1.Items.Add(message);
        }
    }
}

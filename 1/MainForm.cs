using System;
using System.Windows.Forms;

namespace RemoteDesktopApp
{
    public partial class MainForm : Form
    {
        private RemoteDesktopServer server;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            server = new RemoteDesktopServer("127.0.0.1", 12345);
        }
    }
}

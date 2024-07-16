using System;
using System.Windows.Forms;

namespace RemoteDesktopApp
{
    public partial class ClientForm : Form
    {
        private RemoteDesktopClient client;

        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            // Thay đổi IP và port theo cấu hình server của bạn
            client = new RemoteDesktopClient("127.0.0.1", 12345, pictureBoxDisplay);
        }
    }
}

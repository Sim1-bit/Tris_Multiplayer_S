using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tris_Multiplayer_S
{
    public partial class Form1 : Form
    {
        public static Server server;
        public static SemaphoreSlim inizialization = new SemaphoreSlim(0);

        public Form1()
        {
            InitializeComponent();
            server = new Server(1024);
            Aux();
        }

        private async void Aux()
        {
            inizialization.WaitAsync();
            await server.StartAsync();
        }
    }
}

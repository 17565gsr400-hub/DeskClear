using DeskClear.Core;

namespace DeskClear
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCloseAll_Click(object sender, EventArgs e)
        {
            WindowCloser.CloseAll();
        }
    }
}
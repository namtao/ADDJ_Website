using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GQKN.Archive.Core;
using System.Data.SqlClient;

namespace GQKN.Archive
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            try
            {

                Helper.GhiLogs("Logsx", "Nội dung{0}", DateTime.Now);
            }
            catch (Exception ex)
            {

                Helper.GhiLogs(ex);
            }
        }
    }
}

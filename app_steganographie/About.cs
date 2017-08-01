using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace app_steganographie
{
    public partial class About : Form
    {
        // constr.
        public About()
        {
            InitializeComponent();
        }

        // event. click on 'OK' boutton
        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // event. form_load/load of this form
        private void About_Load(object sender, EventArgs e)
        {
            infoTxtBox.Text = "Développeur:\r\nAXeL" + "\r\n\r\n"
                            + "Conception:\r\nLilya" + "\r\n\r\n"
                            + "Compatibilité:\r\nWindows XP/7/++";
        }
    }
}

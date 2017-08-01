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
    public partial class MethodeChoose : Form
    {
        // constr.
        public MethodeChoose()
        {
            InitializeComponent();
        }

        // event. click on 'Ok' boutton
        private void okBtn_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                Acceuil.methode = 1;
            else if (radioButton2.Checked)
                Acceuil.methode = 2;
            else if (radioButton3.Checked)
                Acceuil.methode = 3;
            else
                Acceuil.methode = 4;

            Acceuil.nombreDeBit = Convert.ToInt16(comboBox1.Text);

            this.Close(); // fermeture de la fenetre
        }

        // event. form_laod of this form
        private void MethodeChoose_Load(object sender, EventArgs e)
        {
            if (Acceuil.methode == 1)
                radioButton1.Checked = true;
            else if (Acceuil.methode == 2)
                radioButton2.Checked = true;
            else if (Acceuil.methode == 3)
                radioButton3.Checked = true;
            else
                radioButton4.Checked = true;

            comboBox1.Text = Acceuil.nombreDeBit.ToString();
        }

        // event. CheckedChanged of 'LSB' radio boutton
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                radioButton1.Checked = radioButton2.Checked = radioButton3.Checked = false;
        }

        // event. CheckedChanged of 'Fitness et voisinage' radio boutton
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked && radioButton4.Checked)
                radioButton4.Checked = false;
        }

        // event. CheckedChanged of 'En boucle' radio boutton
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked && radioButton4.Checked)
                radioButton4.Checked = false;
        }

        // event. CheckedChanged of 'Aléatoire' radio boutton
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked && radioButton4.Checked)
                radioButton4.Checked = false;
        }
    }
}

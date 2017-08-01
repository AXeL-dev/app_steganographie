using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics; // for Process class

namespace app_steganographie
{
    public partial class CompareAudioFiles : Form
    {
        // constr.
        public CompareAudioFiles()
        {
            InitializeComponent();
        }

        // event. click on 1st 'Parcourir' boutton
        private void wavFileChooseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Choisir un fichier audio .wav";
            openFileDialog1.DefaultExt = "wav";
            openFileDialog1.Filter = "audio files (*.wav)|*.wav";
            if (openFileDialog1.ShowDialog() == DialogResult.OK && file1TxtBox.Text != openFileDialog1.FileName)
            {
                // show file path
                file1TxtBox.Text = openFileDialog1.FileName;

                // create graph/spectrum
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    chart1.Series.Clear();
                    chart1.Series.Add("wave");
                    chart1.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                    chart1.Series["wave"].ChartArea = "ChartArea1";

                    NAudio.Wave.WaveChannel32 wave = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(openFileDialog1.FileName));

                    byte[] buffer = new byte[16384];
                    int read = 0;

                    while (wave.Position < wave.Length)
                    {
                        read = wave.Read(buffer, 0, 16384);

                        for (int i = 0; i < read / 4; i++)
                        {
                            chart1.Series["wave"].Points.Add(BitConverter.ToSingle(buffer, i * 4));
                        }
                    } // fin while

                    wave.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                this.Cursor = Cursors.Default;
            } // fin if
        }

        // event. click on 2nd 'Parcourir' boutton
        private void wavFileChooseBtn2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Choisir un fichier audio .wav";
            openFileDialog1.DefaultExt = "wav";
            openFileDialog1.Filter = "audio files (*.wav)|*.wav";
            if (openFileDialog1.ShowDialog() == DialogResult.OK && file2TxtBox.Text != openFileDialog1.FileName)
            {
                // show file path
                file2TxtBox.Text = openFileDialog1.FileName;

                // create graph/spectrum
                this.Cursor = Cursors.WaitCursor;
                
                try
                {
                    chart2.Series.Clear();
                    chart2.Series.Add("wave");
                    chart2.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                    chart2.Series["wave"].ChartArea = "ChartArea1";

                    // NAudio is a reference to NAudio dll
                    NAudio.Wave.WaveChannel32 wave = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(openFileDialog1.FileName));

                    byte[] buffer = new byte[16384];
                    int read = 0;

                    while (wave.Position < wave.Length)
                    {
                        read = wave.Read(buffer, 0, 16384);

                        for (int i = 0; i < read / 4; i++)
                        {
                            chart2.Series["wave"].Points.Add(BitConverter.ToSingle(buffer, i * 4));
                        }
                    } // fin while

                    wave.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                this.Cursor = Cursors.Default;
            } // fin if
        }

        // event. click on 1st 'Ouvrir avec HxD' boutton
        private void openHxD1_Click(object sender, EventArgs e)
        {
            try
            {
                if (file1TxtBox.Text != "")
                    Process.Start(@"C:\Program Files\HxD\HxD.exe", "\"" + file1TxtBox.Text + "\""); // le "\"" sert a accepter les chemins qui contient des espaces
                else
                    throw new Exception("Aucun fichier audio choisi !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        // event. click on 2nd 'Ouvrir avec HxD' boutton
        private void openHxD2_Click(object sender, EventArgs e)
        {
            try
            {
                if (file2TxtBox.Text != "")
                    Process.Start(@"C:\Program Files\HxD\HxD.exe", "\"" + file2TxtBox.Text + "\"");
                else
                    throw new Exception("Aucun fichier audio choisi !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}

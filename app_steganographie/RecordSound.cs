using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace app_steganographie
{
    public partial class RecordSound : Form
    {
        // attributs
		private long countSamplesRequired = 0; // How many samples do we need to hide the message using the specified key?
        private long countSamplesRecorded = 0; // How man samples do we have recorded yet?
        private MemoryStream recordedData = new MemoryStream(); // Empty stream to receive the recorded samples
        private WaveInRecorder waveRecorder; // The recorder will do the WaveIn work
        private WaveFormat format = new WaveFormat(11025, BytesPerSample * 8, 2); // Format of the new wave: 16 bit, stereo
        private const int BytesPerSample = 2; // Again: 16 bit
        private Stream recordedStream; // Header + recorded samples

        // constr.
        public RecordSound()
        {
            InitializeComponent();
        }

        // constr. surchargé
        public RecordSound(long countSamplesRequired)
        {
            InitializeComponent();

            this.countSamplesRequired = countSamplesRequired;
            lblSamplesRequired.Text = this.countSamplesRequired.ToString();
        }

        // event. click on 'Enregistrer' boutton
        private void RecordStopBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (RecordStopBtn.Text == "      Enregistrer")
                {
                    Start();
                }
                else
                {
                    Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        /// <summary>Start recording</summary>
        private void Start()
        {
            countSamplesRecorded = 0; // rénitialisation pr les prochains enregistrement
            waveRecorder = new WaveInRecorder(-1, format, 16384, 3, new BufferDoneEventHandler(WaveDataArrived));
            RecordStopBtn.Text = "      Stop";
            RecordStopBtn.Image = app_steganographie.Properties.Resources.arreter;
            if (countSamplesRequired > 0)
                RecordStopBtn.Enabled = false;
        }

        /// <summary>Stop recording, add a header to the sound data</summary>
        private void Stop()
        {
            waveRecorder.Dispose();
            recordedStream = WaveStream.CreateStream(recordedData, format);

            // demande d'enregistrement du fichier
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Enregistrer l'enregistrement audio";
            saveFileDialog1.DefaultExt = "wav";
            saveFileDialog1.Filter = "audio files (*.wav)|*.wav";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream outputFileStream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                recordedStream.CopyTo(outputFileStream);
                outputFileStream.Close();
                MessageBox.Show("Fichier audio enregistré !", "App. stéganographie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            RecordStopBtn.Text = "      Enregistrer";
            RecordStopBtn.Image = app_steganographie.Properties.Resources.play;
        }

        /// <summary>Callback method - copy a buffer of recorded audio data</summary>
        /// <param name="data">Pointer to the raw data</param>
        /// <param name="size">Size of the data</param>
        private void WaveDataArrived(IntPtr data, int size)
        {
            byte[] recBuffer = new byte[size];
            System.Runtime.InteropServices.Marshal.Copy(data, recBuffer, 0, size);
            recordedData.Write(recBuffer, 0, recBuffer.Length);

            countSamplesRecorded += size / BytesPerSample;
            MethodInvoker invoker = new MethodInvoker(SetSamplesRecordedLabelText);
            lblSamplesRecorded.Invoke(invoker);

            if (countSamplesRequired > 0 && (countSamplesRecorded >= countSamplesRequired))
            {
                //enough samples arrived - allow the user to stop recording
                invoker = new MethodInvoker(SetStopButtonEnabled);
                RecordStopBtn.Invoke(invoker);
            }
        }

        private void SetStopButtonEnabled()
        {
            RecordStopBtn.Enabled = true;
        }

        private void SetSamplesRecordedLabelText()
        {
            lblSamplesRecorded.Text = countSamplesRecorded.ToString();
        }
    }
}

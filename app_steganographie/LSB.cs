using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO; // to read/write files
using System.Diagnostics; // for StopWatch Class
using System.Threading;

namespace app_steganographie
{
    public partial class LSB : Form
    {
        // constr.
        public LSB()
        {
            InitializeComponent();
        }

        // event. click on 'Parcourir' boutton
        private void wavFileChooseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Choisir un fichier audio .wav";
            openFileDialog1.DefaultExt = "wav";
            openFileDialog1.Filter = "audio files (*.wav)|*.wav";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                audioFileTxtBox.Text = openFileDialog1.FileName;
        }

        // event. click on 1st 'Lire' boutton
        private void playAudioBtn_Click(object sender, EventArgs e)
        {
            if (audioFileTxtBox.Text != "")
            {
                // changement de l'image + disable du boutton 'Lire'
                playAudioBtn.Image = app_steganographie.Properties.Resources.loader;
                playAudioBtn.Enabled = false;

                // lecture du fichier audio
                ToolsAndFunctions.playAudioFile(audioFileTxtBox.Text);

                // changement de l'image + enable du boutton 'Lire'
                playAudioBtn.Image = app_steganographie.Properties.Resources.media_play_icon;
                playAudioBtn.Enabled = true;

                // focus sur l'onglet actuel
                this.tabControl1.SelectedTab.Focus();
            }
            else
                MessageBox.Show("Aucun fichier audio choisi !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        // event. click on 1st 'Insérer..' boutton
        private void insererBtn1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Insérer depuis un fichier texte";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "text files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                cleTxtBox.Text = File.ReadAllText(openFileDialog1.FileName); // lecture/utilisation du contenu du fichier
        }

        // event. click on 2nd 'Insérer..' boutton
        private void insererBtn2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Insérer depuis un fichier texte";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "text files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                txtAcacherTab1.Text = File.ReadAllText(openFileDialog1.FileName); // lecture/utilisation du contenu du fichier
        }

        // event. click on 'Effacer' boutton
        private void effacerBtn_Click(object sender, EventArgs e)
        {
            txtAcacherTab1.Text = "";
        }

        // event. click on 'Cacher' boutton
        private void cacherBtn_Click(object sender, EventArgs e)
        {
            // Désactivation du boutton 'Lire' du fichier audio de sortie + 'Extraction Rapide..'
            playOutputBtn.Enabled = ExtractionRapideBtn.Enabled = false;

            // remise à zéro de la barre de chargement
            progressBar1.Value = 0;

            // 1 - vérification du fichier audio d'entrée + la clé + le texte à cacher
            if (audioFileTxtBox.Text == "")
            {
                MessageBox.Show("Aucun fichier audio choisi !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (cleTxtBox.Text == "")
            {
                MessageBox.Show("Aucune clé choisie !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (txtAcacherTab1.Text == "")
            {
                MessageBox.Show("Aucun texte à cacher !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2 - demande d'emplacement d'enregistrement du fichier audio de sortie
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Enregistrer le fichier audio de sortie dans";
            saveFileDialog1.DefaultExt = "wav";
            saveFileDialog1.Filter = "audio files (*.wav)|*.wav";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) // si emplacement choisi
            {
                // Let's hide the message/text !

                // on commence le calcul du temp d'éxecution
                Stopwatch sw = new Stopwatch();
                sw.Start();

                // create streams
                Stream sourceStream = null;
                FileStream destinationStream = null;
                WaveStream audioStream = null;

                // create a stream that contains the message, preceeded by its length
                Stream messageStream = ToolsAndFunctions.stringToStream(txtAcacherTab1.Text);
                // create a stream that contains the key
                Stream keyStream = ToolsAndFunctions.stringToStream(cleTxtBox.Text);

                long countSamplesRequired = 0; // var to count samples required (we use it in catch() so this is the best place)

                try
                {
                    //how man samples do we need?
                    countSamplesRequired = WaveUtility.CheckKeyForMessage(keyStream, messageStream.Length);

                    if (countSamplesRequired > Int32.MaxValue)
                    {
                        throw new Exception("Message too long, or bad key! This message/key combination requires " + countSamplesRequired + " samples, only " + Int32.MaxValue + " samples are allowed.");
                    }

                    // open source stream
                    sourceStream = new FileStream(audioFileTxtBox.Text, FileMode.Open);
                    
                    // create destination stream
                    destinationStream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);

                    //copy the carrier file's header
                    audioStream = new WaveStream(sourceStream, destinationStream);
                    if (audioStream.Length <= 0)
                    {
                        throw new Exception("Invalid WAV file");
                    }

                    //are there enough samples in the carrier wave?
                    if (countSamplesRequired > audioStream.CountSamples)
                    {
                        String errorReport = "The carrier file is too small for this message and key!\r\n"
                            + "Samples available: " + audioStream.CountSamples + "\r\n"
                            + "Samples needed: " + countSamplesRequired + "\r\n\r\n"
                            + "\tOuvrir la fenêtre d'enregistrement audio ?";
                        throw new Exception(errorReport);
                    }

                    //hide the message
                    this.Cursor = Cursors.WaitCursor;
                    WaveUtility utility = new WaveUtility(audioStream, destinationStream);
                    utility.Hide(messageStream, keyStream);
                    
                    // set output audio file path
                    outputAudioTxtBox.Text = saveFileDialog1.FileName;

                    // on arrête le calcul du temp d'éxecution + on l'affiche
                    sw.Stop();
                    execTime.Text = sw.ElapsedMilliseconds.ToString() + " ms";

                    // activate 'Lire' boutton + 'Extraction Rapide..'
                    playOutputBtn.Enabled = ExtractionRapideBtn.Enabled = true;

                    // chargement complet de la barre de chargement
                    progressBar1.Value = 100;
                }
                catch (Exception ex)
                {
                    //this.Cursor = Cursors.Default;
                    // if Message contains "Samples needed" , mean that wav file is not enought
                    if (ex.Message.Contains("Samples needed"))
                    {
                        if (MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == System.Windows.Forms.DialogResult.Yes)
                        {
                            Form fen = new RecordSound(countSamplesRequired);
                            fen.ShowDialog();
                        }
                    }
                    else
                        MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    if (keyStream != null) { keyStream.Close(); }
                    if (messageStream != null) { messageStream.Close(); }
                    if (audioStream != null) { audioStream.Close(); }
                    if (sourceStream != null) { sourceStream.Close(); }
                    if (destinationStream != null) { destinationStream.Close(); }
                    this.Cursor = Cursors.Default;
                    if (sw.IsRunning) { sw.Stop(); }
                } // fin try
            } // fin if
        }

        // event. click on 2nd 'Lire' boutton
        private void playOutputBtn_Click(object sender, EventArgs e)
        {
            if (outputAudioTxtBox.Text != "")
            {
                // changement de l'image + disable du boutton 'Lire'
                playOutputBtn.Image = app_steganographie.Properties.Resources.loader;
                playOutputBtn.Enabled = false;

                // lecture du fichier audio
                ToolsAndFunctions.playAudioFile(outputAudioTxtBox.Text);

                // changement de l'image + enable du boutton 'Lire'
                playOutputBtn.Image = app_steganographie.Properties.Resources.media_play_icon;
                playOutputBtn.Enabled = true;

                // focus sur l'onglet actuel
                this.tabControl1.SelectedTab.Focus();
            }
            else
                MessageBox.Show("Aucun fichier audio de sortie !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        // event. click on 'Extraire' boutton
        private void extraireBtn_Click(object sender, EventArgs e)
        {
            // vidage de la zone qui contiendra le message caché
            txtCacherTab2.Text = "";

            // remise à zéro de la barre de chargement
            progressBar1.Value = 0;

            // 1 - vérification du fichier audio d'entrée + la clé
            if (audioFileTxtBox.Text == "")
            {
                MessageBox.Show("Aucun fichier audio choisi !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (cleTxtBox.Text == "")
            {
                MessageBox.Show("Aucune clé choisie !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Let's Extract the message !

            // on commence le calcul du temp d'éxecution
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // create streams
            FileStream sourceStream = null;
            WaveStream audioStream = null;
            
            //create an empty stream to receive the extracted message
            MemoryStream messageStream = new MemoryStream();
            
            // create a stream that contains the key
            Stream keyStream = ToolsAndFunctions.stringToStream(cleTxtBox.Text);
            
            try
            {
                //open the carrier file
                sourceStream = new FileStream(audioFileTxtBox.Text, FileMode.Open);
                audioStream = new WaveStream(sourceStream);
                WaveUtility utility = new WaveUtility(audioStream);

                //exctract the message from the carrier wave
                this.Cursor = Cursors.WaitCursor;
                Thread thread = new Thread(() => { utility.Extract(messageStream, keyStream); });
                thread.Start();

                int timeout = (int)TimeoutNumericUpDown.Value * 1000; // conversion en milliseconde
                if (!thread.Join(timeout)) // if timeout
                {
                    thread.Abort();
                    throw new Exception("[Timeout !]\r\nIl est probable que le fichier audio ou que la clé soit erronée !");
                }

                // show message
                if (messageStream.Length > 0)
                {
                    messageStream.Seek(0, SeekOrigin.Begin);
                    txtCacherTab2.Text = new StreamReader(messageStream).ReadToEnd();
                }

                // on arrête le calcul du temp d'éxecution + on l'affiche
                sw.Stop();
                execTime.Text = sw.ElapsedMilliseconds.ToString() + " ms";

                // chargement complet de la barre de chargement
                progressBar1.Value = 100;
            }
            catch (Exception ex)
            {
                //this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                if (keyStream != null) { keyStream.Close(); }
                if (messageStream != null) { messageStream.Close(); }
                if (audioStream != null) { audioStream.Close(); }
                if (sourceStream != null) { sourceStream.Close(); }
                this.Cursor = Cursors.Default;
                if (sw.IsRunning) { sw.Stop(); }
            }
        }

        // event. click on 'Extraction Rapide..' boutton
        private void ExtractionRapideBtn_Click(object sender, EventArgs e)
        {
            // changement du fichier audio
            audioFileTxtBox.Text = outputAudioTxtBox.Text;
            // changement de l'onglet actuel
            this.tabControl1.SelectedTab = this.tabPage2;
            // focus sur le boutton extraire
            extraireBtn.Focus();
        }

        // event. click on 'Enregistrer le contenu dans un fichier texte' boutton du tabPage2
        private void saveAsTextFile_Click(object sender, EventArgs e)
        {
            if (txtCacherTab2.Text != "")
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Enregistrer le contenu dans un fichier texte";
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog1.FileName, txtCacherTab2.Text);
                    MessageBox.Show("Contenu enregistré !", "LSB", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Aucun contenu à enregistrer !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        // event. formClosed of this form
        private void LSB_FormClosed(object sender, FormClosedEventArgs e)
        {
            Acceuil.methode = 1; // reset methode to 1
        }
    }
}

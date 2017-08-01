/**
 * App. stéganographie
 * 
 * version : 1.0
 * 
 * Date de création : 14/05/2015
 * 
 * Date de la dernière modification : 17/05/2015
 *  
 * Auteur : AXeL
 * 
 * Contact : albatrosx95@gmail.com
 * 
 * Idées d'amélioration : - les méthodes/procédures copier/couper ne sont pas encore finalisés, il faut gérer la séléction/copie de par exemple juste une partie du texte.
 * 
 * 
 **/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO; // to read/save files
using System.Diagnostics; // for StopWatch class
//using System.Threading; // to use Threads

namespace app_steganographie
{
    public partial class Acceuil : Form
    {
        // attributs/variables
        public static int methode = 1;
        public static int nombreDeBit = 4;

        // constr.
        public Acceuil()
        {
            InitializeComponent();
        }

        // event. click on 'Codage' in MenuStrip1
        private void customizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = this.tabPage1;
        }

        // event. click on 'Décodage' in MenuStrip1
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = this.tabPage2;
        }

        // event. click on 'About' in MenuStrip1
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBoxHelper.PrepToCenterMessageBoxOnForm(this); // to center MessageBox (no need after set proprietie 'StartPosition' of the form to 'CenterScreen')
            //MessageBox.Show("App. stéganographie\nby : AXeL\ncontact : albatrosx95@gmail.com", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Form fen = new About();
            fen.ShowDialog();
        }

        // function who return the current focused textBox
        private TextBox getFocusedTextBox()
        {
            // we test all textboxs until found focused one
            if (txtAcacherTab1.Focused)
                return txtAcacherTab1;
            else if (txtCacherTab2.Focused)
                return txtCacherTab2;
            else if (mdpTab1.Focused)
                return mdpTab1;
            else if (mdpTab2.Focused)
                return mdpTab2;
            else if (cleStegoTab1.Focused)
                return cleStegoTab1;
            else if (cleStegoTab2.Focused)
                return cleStegoTab2;
            else
                return null;
        }

        // event. click on 'Couper' in MenuStrip1
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBox focusedTextBox = getFocusedTextBox();
            if (focusedTextBox != null && focusedTextBox.Text.Length > 0) // si textbox on vide
            {
                // copie du texte dans le clipboard + vidage du textbox
                Clipboard.SetText(focusedTextBox.Text);
                focusedTextBox.Text = "";
            }
        }

        // event. click on 'Copier' in MenuStrip1
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBox focusedTextBox = getFocusedTextBox();
            if (focusedTextBox != null && focusedTextBox.Text.Length > 0)
                Clipboard.SetText(focusedTextBox.Text); // copie du texte
        }

        // event. click on 'Coller' in MenuStrip1
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBox focusedTextBox = getFocusedTextBox();
            if (focusedTextBox != null && focusedTextBox != cleStegoTab1) // si il y'a un textBox qui a le focus, + ça ne doit pas etre le textBox en lecture seule
            {
                if (focusedTextBox.SelectionStart == 0) // si texte séléctionner
                {
                    if (focusedTextBox.SelectionLength == focusedTextBox.Text.Length) // si tout le texte est séléctionné 
                        focusedTextBox.Text = Clipboard.GetText(); // collage du texte et effacement de l'ancien
                    else // si nn remplacement de la séléction seulement
                    {
                        focusedTextBox.Text = Clipboard.GetText() + focusedTextBox.Text.Substring(focusedTextBox.SelectionLength, focusedTextBox.Text.Length - focusedTextBox.SelectionLength);
                        // on séléctionne la partie qu'on vien d'insérer
                        focusedTextBox.SelectionStart = 0;
                        focusedTextBox.SelectionLength = Clipboard.GetText().Length;
                        return;
                    }
                }
                else
                    focusedTextBox.Text += Clipboard.GetText(); // collage/ajout du texte
                // move cursor to end/on bouge le curseur à la fin du texte
                focusedTextBox.SelectionStart = focusedTextBox.Text.Length;
                focusedTextBox.SelectionLength = focusedTextBox.Text.Length;
            }
        }

        // event. click on 'Tout Sélectionner' in MenuStrip1
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBox focusedTextBox = getFocusedTextBox();
            if (focusedTextBox != null && focusedTextBox.Text.Length > 0) // si le textbox contient du texte
            {
                // sélection du texte
                focusedTextBox.SelectionStart = 0;
                focusedTextBox.SelectionLength = focusedTextBox.Text.Length;
            }
        }

        // event. click on 'Fermer' in MenuStrip1
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); // Fermeture de l'application
        }

        // event. click on 'Couper/Cut' in toolStrip1
        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            cutToolStripMenuItem_Click(sender, e); // appel de la méthode click sur le boutton 'Couper' du Menu
        }

        // event. click on 'Copier/Copy' in toolStrip1
        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            copyToolStripMenuItem_Click(sender, e); // appel de la méthode click sur le boutton 'Copier' du Menu
        }

        // event. click on 'Coller/Paste' in toolStrip1
        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            pasteToolStripMenuItem_Click(sender, e); // appel de la méthode click sur le boutton 'Coller' du Menu
        }

        // event. click on 'Help' in toolStrip1
        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            aboutToolStripMenuItem_Click(sender, e); // appel de la méthode click sur le boutton 'About' du Menu
        }

        // event. click on 'Parcourir' boutton du tabPage1
        private void wavFileChooseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Choisir un fichier audio .wav";
            openFileDialog1.DefaultExt = "wav";
            openFileDialog1.Filter = "audio files (*.wav)|*.wav";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (this.tabControl1.SelectedTab == this.tabPage1) // si tab1 (Codage)
                    wavFileLabelTab1.Text = openFileDialog1.FileName;
                else // si nn (tab 2 - Décodage)
                    wavFileLabelTab2.Text = openFileDialog1.FileName;
                // si vous vous demandez pq faire ça içi, bah c tout simple, je compte appeler/utiliser cette procédure pr le boutton 'Parcourir' du tab2, sans devoir réecrire le même code
            }
        }

        // event. click on 'Parcourir' boutton du tabPage2
        private void wavFileChooseBtn2_Click(object sender, EventArgs e)
        {
            wavFileChooseBtn_Click(sender, e);
        }

        // event. click on 'Effacer contenu' boutton du tabPage1
        private void button1_Click(object sender, EventArgs e)
        {
            txtAcacherTab1.Text = "";
        }

        // event. click on 'utiliser le contenu d'un fichier texte' boutton du tabPage1
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Insérer depuis un fichier texte";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "text files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                txtAcacherTab1.Text = File.ReadAllText(openFileDialog1.FileName); // lecture/utilisation du contenu du fichier
        }

        // event. click on 'Coder' boutton du tabPage1
        private void coderBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // remise à zéro de la barre de chargement
                progressBar1.Value = 0;

                // Désactivation des bouttons 'Relire..' et 'Décodage Rapide..' + save
                replayAudioBtn.Enabled = decodageRapideBtn.Enabled = saveCleStegoBtn.Enabled = false;

                // 1 - lecture du fichier audio en bytes
                if (wavFileLabelTab1.Text == "Aucun fichier séléctionné.")
                {
                    MessageBox.Show("Aucun fichier audio séléctionné !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                byte[] fileBytes = File.ReadAllBytes(wavFileLabelTab1.Text);

                // 2 - Conversion du texte a cacher en bytes
                if (txtAcacherTab1.Text == "")
                {
                    MessageBox.Show("Aucun texte à cacher !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                byte[] textBytes = System.Text.Encoding.ASCII.GetBytes(txtAcacherTab1.Text);

                // si mot de passe de cryptage AES non saisi
                if (mdpTab1.Text == "")
                {
                    MessageBox.Show("Aucun mot de passe (AES) saisi !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3 - Toutes les vérifications ont été faites, Let's Go !

                // on commence le calcul du temp d'éxecution
                Stopwatch sw = new Stopwatch();
                sw.Start();
            
                // conversion du fichier audio en block de 2/4 bits
                string[] fileBitBlocks = ToolsAndFunctions.bytesToBlocks(fileBytes, nombreDeBit);

                // conversion du text à cacher en block de 2/4 bits
                string[] textBitBlocks = ToolsAndFunctions.bytesToBlocks(textBytes, nombreDeBit);

                // Recherche Locale
                int[] positions = new int[textBytes.Length * (8 / nombreDeBit)]; // 8(bits) / 4 (bits par block) = 2 (positions pour chaque 8bit)

                if (methode == 1) // si methode Fitness et voisinage
                {
                    int[] fitness = new int[textBitBlocks.Length]; // pr chaque bloc une fitness
                    int[] randomPos = new int[textBitBlocks.Length]; // pr chaque bloc une position aléatoire
                    Random rnd = new Random();

                    // Etape 1 : Position aléatoire
                    
                    // on parcours les blocks du texte à cacher
                    for (int i = 0; i < textBitBlocks.Length; i++)
                    {
                        // pr chaque bloc de texte on génère une position aléatoire dans les blocs audio
                        randomPos[i] = rnd.Next(0, fileBitBlocks.Length - 1); // -1 car le tableau commence de 0
                        // on calcule la fitness du bloc
                        fitness[i] = ToolsAndFunctions.fitness(textBitBlocks[i], fileBitBlocks[randomPos[i]]);
                        //MessageBox.Show(fitness[i].ToString());
                    }
                    
                    // Etape 2 : Vérification de la fitness de chaque bloc, puis recherche au voisinage si fitness != 0

                    // on parcours les blocks du texte à cacher
                    for (int i = 0; i < textBitBlocks.Length; i++)
                    {
                        if (fitness[i] != 0)
                        {
                            // recherche au voisinage de l'ancienne randomPos[i]
                            int k = randomPos[i]; // initialisation avec la randomPos[i]
                            // 1 - recherche au voisinage droit (1er bloc a droite)
                            for (int j = randomPos[i] + 1; j < fileBitBlocks.Length; j++) // depuis randomPos[i] + 1
                            {
                                // on recalcule la fitness du bloc
                                fitness[i] = ToolsAndFunctions.fitness(textBitBlocks[i], fileBitBlocks[j]);
                                //MessageBox.Show(i.ToString() + " - voisinage droite - " + j.ToString());
                                // si la fitness != 0 tj
                                if (fitness[i] != 0)
                                {
                                    // 2 - recherche au voisinage gauche (1er bloc a gauche), içi on vérifie juste 1 seul bloc, pr repasser la main a la 1ere boucle for
                                    if (k > 0) // si on peu décrémenter k (=> y'a encore du voisinage gauche)
                                    {
                                        k--;
                                        // on recalcule la fitness du bloc
                                        fitness[i] = ToolsAndFunctions.fitness(textBitBlocks[i], fileBitBlocks[k]);
                                        //MessageBox.Show(i.ToString() + " - voisinage gauche - " + k.ToString());
                                        // si fitness == 0, c'est bon, on sauvegarde la position et on sort de la boucle (1ere boucle for)
                                        if (fitness[i] == 0)
                                        { // ça veux dire qu'on a trouvé le bloc au voisinage gauche
                                            positions[i] = k;
                                            break;
                                        }
                                    }
                                }
                                else // si nn => fitness == 0, c'est bon, on sauvegarde la position et on sort de la boucle (1ere boucle for)
                                { // ça veux dire qu'on a trouvé le bloc au voisinage droit
                                    positions[i] = j;
                                    break;
                                }
                            }

                            // si jamais la boucle du voisinage à droite prend fin alors que la fitness != 0, on n'ai pas sur alors que la boucle du voisinage à gauche a fini aussi !
                            if (fitness[i] != 0)
                            {
                                // 3 - Finition de la recherche au voisinage gauche
                                for (int f = k; f >= 0; f--)
                                {
                                    // on recalcule la fitness du bloc
                                    fitness[i] = ToolsAndFunctions.fitness(textBitBlocks[i], fileBitBlocks[f]);
                                    //MessageBox.Show(i.ToString() + " - voisinage gauche (boucle) - " + f.ToString());
                                    // si fitness == 0, c'est bon, on sauvegarde la position et on sort de la boucle
                                    if (fitness[i] == 0)
                                    {
                                        positions[i] = f;
                                        break;
                                    }
                                }

                                // si jamais apres tt ça la fitness != 0 tj, alors qu'on a parcouru toute la boucle, ça veux dire que le bloc est introuvable
                                if (fitness[i] != 0)
                                    throw new Exception("Le bloc : '" + textBitBlocks[i] + "' est introuvable dans les blocs du fichier audio !");
                            } // fin if
                        }
                        else // si fitness == 0, autrement dit, si les blocs (texte et audio) se ressemble, on passe au prochain bloc
                            positions[i] = randomPos[i];
                    } // fin for => Etape 2
                }
                else // si nn (les 2 autres méthodes)
                {
                    int lastPosition;
                    Random rnd = new Random();
                    if (methode == 2) // si recherche en boucle
                        lastPosition = 0; // le nom de la variable ne reflète pas le role qu'elle joue , je sais..
                    else // si nn (recherche aléatoire, car si methode == 3 on ne rentrera jamais içi), on genère un nombre aléatoire
                        lastPosition = rnd.Next(0, fileBitBlocks.Length - 2); // - 2 pour avoir au moin un block suivant (car le tableau de block commence de 0 et fini en fileBitBlocks.Length - 1)

                    // chargement à 30% de la barre de chargement
                    //progressBar1.Value = 30; // ça ne s'applique pas, forcément car, on entre apres dans des boucles => on bloque la GUI (redessinage..)

                    // on parcours les blocks du texte à cacher
                    for (int i = 0; i < textBitBlocks.Length; i++)
                    {
                        bool positionOk = false;
                        while (!positionOk)
                        {
                            // on parcours les blocks du fichier audio
                            for (int j = lastPosition; j < fileBitBlocks.Length; j++)
                            {
                                //MessageBox.Show(textBitBlocks[i] + "-" + fileBitBlocks[j] + "-" + i.ToString() + "-" + j.ToString(), "");

                                // si on trouve un block de texte dans le fichier audio
                                if (textBitBlocks[i] == fileBitBlocks[j])
                                {
                                    // on sauvegarde la position dans la clé stégo/tableau de positions
                                    positions[i] = j;
                                    //lastPosition = ++j; // ou j + 1
                                    positionOk = true;
                                    break; // on sort de la boucle sur les blocks du fichier audio pour passer au prochain block de texte
                                }
                            } // fin 2eme for

                            if (methode == 3) // si méthode de recherche aléatoire
                            {
                                if (!positionOk) // si on n'a pas trouvé la position du block avec la valeur aléatoire
                                {
                                    // on vérifie si le block existe déja dans le fichier audio et on retourne sa position, si nn la fonction retourne -1
                                    lastPosition = ToolsAndFunctions.isBlockExists(textBitBlocks[i], fileBitBlocks);
                                    if (lastPosition == -1)
                                        throw new Exception("Le bloc : '" + textBitBlocks[i] + "' est introuvable dans les blocs du fichier audio !");
                                    // si nn si position trouvé la boucle va continuer son travail (meme si on l'a aider pr avoir la position + c'est plus rapide comme ça meme si ce n'est plus completement aléatoire)
                                }
                                else
                                    lastPosition = rnd.Next(0, fileBitBlocks.Length - 1); // on regénère un nombre aléatoire
                            }
                            else // si nn, methode en boucle
                            {
                                if (!positionOk) // si jamais on ne trouve pas le bloc apres avoir parcouru tout le fichier, positionOk sera = à false (c peu probable, mais si ça arrive, ça provequera une boucle infini, 'mieux vaut prévenir que guérir !')
                                    throw new Exception("Le bloc : '" + textBitBlocks[i] + "' est introuvable dans les blocs du fichier audio !");
                                else
                                    break; // on sort de la boucle while, pour passer au bloc suivant
                            }

                        } // fin while
                    } // fin 1er for
                } // fin else

                // traitement de la clé stégo => Ajout du Hash du fichier audio
                string cleStegoString = ToolsAndFunctions.GetFileHash(fileBytes) + "|";

                for (int i = 0; i < positions.Length; i++) // Ajout des positions des blocks
                    cleStegoString += positions[i].ToString() + "|"; // conversion en chaine de caractère

                //MessageBox.Show(cleStegoString, "Clé stégo en clair", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // cryptage de la clé stégo + affichage
                cleStegoTab1.Text = Crypt.EncryptStringAES(cleStegoString, mdpTab1.Text);

                // on arrête le calcul du temp d'éxecution + on l'affiche
                sw.Stop();
                execTimeTab1.Text = sw.ElapsedMilliseconds.ToString() + " ms";

                // chargement complet de la barre de chargement
                progressBar1.Value = 100;

                // Activation des bouttons 'Relire..' et 'Décodage Rapide..' + save
                replayAudioBtn.Enabled = decodageRapideBtn.Enabled = saveCleStegoBtn.Enabled = true;

            } // fin try
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // event. click on Mdp 'Aléatoire' boutton du tabPage1
        private void aleatoireBtn_Click(object sender, EventArgs e)
        {
            mdpTab1.Text = ToolsAndFunctions.createPassword(8);
        }

        // event. click on 'Décodage Rapide..' boutton du tabPage1
        private void decodageRapideBtn_Click(object sender, EventArgs e)
        {
            // changement d'onglet (vers onglet de décodage)
            this.tabControl1.SelectedTab = this.tabPage2;
            // copie du nom du fichier wav
            wavFileLabelTab2.Text = wavFileLabelTab1.Text;
            // copie de la clé stégo
            cleStegoTab2.Text = cleStegoTab1.Text;
            // copie du mot de passe (AES)
            mdpTab2.Text = mdpTab1.Text;
            // vidage de la zone du texte caché
            txtCacherTab2.Text = "";
            // remise à zéro de la barre de chargement
            progressBar2.Value = 0;
            // focus sur le boutton 'Décoder'
            decoderBtn.Focus();
        }

        // event. click on 'Décoder' boutton du tabPage2
        private void decoderBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // remise à zéro de la barre de chargement
                progressBar2.Value = 0;

                // vidage de la zone/textBox du texte caché
                txtCacherTab2.Text = "";

                // 1 - lecture du fichier audio en bytes
                if (wavFileLabelTab2.Text == "Aucun fichier séléctionné.")
                {
                    MessageBox.Show("Aucun fichier audio séléctionné !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                byte[] fileBytes = File.ReadAllBytes(wavFileLabelTab2.Text);

                // 2 - si clé stégo non saisi
                if (cleStegoTab2.Text == "")
                {
                    MessageBox.Show("Aucune clé stégo saisie !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3 - si mot de passe de décryptage AES non saisi
                if (mdpTab2.Text == "")
                {
                    MessageBox.Show("Aucun mot de passe (AES) saisi !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3 - Toutes les vérifications ont été faites, Let's decrypt this !

                // on commence le calcul du temp d'éxecution
                Stopwatch sw = new Stopwatch();
                sw.Start();

                // conversion du fichier audio en block de 2/4 bits
                string[] fileBitBlocks = ToolsAndFunctions.bytesToBlocks(fileBytes, nombreDeBit);

                // décryptage de la clé stégo
                string cleStegoString = Crypt.DecryptStringAES(cleStegoTab2.Text, mdpTab2.Text);

                // on récupère le hash du fichier audio et les positions des blocks
                string[] positions = cleStegoString.Split('|');

                //MessageBox.Show(cleStegoString + "," + fileBitBlocks[Convert.ToInt32(positions[0])], "");

                // vérification du Hash du fichier audio avec celui contenu dans la clé
                string fileHash = positions[0];

                if (fileHash != ToolsAndFunctions.GetFileHash(fileBytes)) // si les 2 hash ne sont pas identiques
                {
                    MessageBox.Show("Fichier audio erroné !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // Recherche dans les blocks du fichier audio
                string texteCachee = "";
                
                // on parcours toutes les positions pour reconstituer le texte caché
                for (int i = 1; i < positions.Length - 1; i += (8 / nombreDeBit)) // de 1 => première position contient le hash, 8(bits) / 4(bits par block) = 2 , -1 du dernier '|'
                {
                    string Blocks = ""; // contiendra 2 bloc si codage sur (4bit) ou 4 bloc si codage sur (2bit)
                    for (int j = 0; j < (8 / nombreDeBit); j++)
                        Blocks += fileBitBlocks[Convert.ToInt32(positions[i+j])];
                    
                    //MessageBox.Show(positions.Length.ToString() + "," + i.ToString() + "," + texteCachee);
                    texteCachee += ToolsAndFunctions.BlocksToString(Blocks);
                }

                // on arrête le calcul du temp d'éxecution + on l'affiche
                sw.Stop();
                execTimeTab2.Text = sw.ElapsedMilliseconds.ToString() + " ms";

                // affichage du texte caché
                txtCacherTab2.Text = texteCachee;

                // chargement complet de la barre de chargement
                progressBar2.Value = 100;

            } // fin try
            catch (Exception ex)
            {
                MessageBox.Show("[Erreur de décodage : " + ex.Message + "]\nIl se peut que le mot de passe ou la clé stégo soit incorrecte !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                    MessageBox.Show("Contenu enregistré !", "App. stéganographie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Aucun contenu à enregistrer !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        // event. click on 'Reset' boutton du toolStrip1
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            // reset wavFile Labels
            wavFileLabelTab1.Text = wavFileLabelTab2.Text = "Aucun fichier séléctionné.";
            // reset all TextBoxs
            txtAcacherTab1.Text = txtCacherTab2.Text = mdpTab1.Text = mdpTab2.Text = cleStegoTab1.Text = cleStegoTab2.Text = "";
            // reset progressBars
            progressBar1.Value = progressBar2.Value = 0;
            // Disable replay & décodage rapide buttons & save
            replayAudioBtn.Enabled = decodageRapideBtn.Enabled = saveCleStegoBtn.Enabled = false;
            // reset execTime labels
            execTimeTab1.Text = execTimeTab2.Text = "-";
        }

        // event. click on 'Relire le fichier audio' boutton
        private void replayAudioBtn_Click(object sender, EventArgs e)
        {
            // lecture du fichier audio en utilisant la procédure click du boutton 'Lire'
            playAudioBtn_Click(sender, e);
        }

        // event. click on 'Lire' boutton du tabPage1
        private void playAudioBtn_Click(object sender, EventArgs e)
        {
            if (wavFileLabelTab1.Text != "Aucun fichier séléctionné.")
            {
                // changement de l'image + disable du boutton 'Lire'
                playAudioBtn.Image = app_steganographie.Properties.Resources.loader;
                playAudioBtn.Enabled = false;

                // changement de l'image + disable du boutton 'Relire..'
                bool replayOn = false;
                if (replayAudioBtn.Enabled)
                {
                    replayOn = true;
                    replayAudioBtn.Image = app_steganographie.Properties.Resources.loader;
                    replayAudioBtn.Enabled = false;
                }

                // lecture du fichier audio
                ToolsAndFunctions.playAudioFile(wavFileLabelTab1.Text);

                // Thread de lecture (pour eviter le plantage de l'application)
                /*
                Thread playAudioThread = new Thread(new ParameterizedThreadStart(ToolsAndFunctions.playAudioFile));
                playAudioThread.Start(wavFileLabelTab1.Text);
                */

                // changement de l'image + enable du boutton 'Lire'
                playAudioBtn.Image = app_steganographie.Properties.Resources.media_play_icon;
                playAudioBtn.Enabled = true;

                // changement de l'image + enable du boutton 'Relire..'
                if (replayOn)
                {
                    replayAudioBtn.Image = app_steganographie.Properties.Resources.media_play_icon;
                    replayAudioBtn.Enabled = true;
                }

                // focus sur l'onglet actuel
                this.tabControl1.SelectedTab.Focus();
            }
            else
                MessageBox.Show("Aucun fichier audio séléctionné !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        // event. click on 'Parcourir..' in MenuStrip1 
        private void parcourirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wavFileChooseBtn_Click(sender, e);
        }

        // event. click on 'Lire' in MenuStrip1 
        private void lireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabPage1)
                playAudioBtn_Click(sender, e);
            else
                playAudioBtn2_Click(sender, e);
        }

        // event. click on 'Lire' boutton du tabPage2
        private void playAudioBtn2_Click(object sender, EventArgs e)
        {
            if (wavFileLabelTab2.Text != "Aucun fichier séléctionné.")
            {
                // changement de l'image + disable du boutton 'Lire'
                playAudioBtn2.Image = app_steganographie.Properties.Resources.loader;
                playAudioBtn2.Enabled = false;

                // lecture du fichier audio
                ToolsAndFunctions.playAudioFile(wavFileLabelTab2.Text);

                // Thread de lecture (pour eviter le plantage de l'application)
                /*
                Thread playAudioThread = new Thread(new ParameterizedThreadStart(ToolsAndFunctions.playAudioFile));
                playAudioThread.Start(wavFileLabelTab2.Text);
                */

                // changement de l'image + enable du boutton 'Lire'
                playAudioBtn2.Image = app_steganographie.Properties.Resources.media_play_icon;
                playAudioBtn2.Enabled = true;

                // focus sur l'onglet actuel
                this.tabControl1.SelectedTab.Focus();
            }
            else
                MessageBox.Show("Aucun fichier audio séléctionné !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        // event. shown of this form
        private void Acceuil_Shown(object sender, EventArgs e)
        {
            méthodeDeRechercheToolStripMenuItem_Click(sender, e);
        }

        // event. click on 'Méthode à utiliser' in MenuStrip1
        private void méthodeDeRechercheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form fen = new MethodeChoose(); // pas grave si l'objet est Form car la class MethodeChosse hérite de Form (l'inverse nn!)
            fen.ShowDialog();

            if (methode == 4)
            {
                Form fen2 = new LSB();
                fen2.ShowDialog();
            }

            nbrBitLbl.Text = nombreDeBit.ToString();
        }

        // event. click on save boutton in tabPage1
        private void saveCleStegoBtn_Click(object sender, EventArgs e)
        {
            if (cleStegoTab1.Text != "")
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Enregistrer la clé stégo dans un fichier texte";
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog1.FileName, cleStegoTab1.Text);
                    MessageBox.Show("Clé stégo enregistrée !", "App. stéganographie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Aucune clé stégo à enregistrer !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        // event. click on open cle stego text file boutton in tabPage2
        private void openCleStegoTexteBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Utiliser une clé stégo enregistrée dans un fichier texte";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "text files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                cleStegoTab2.Text = File.ReadAllText(openFileDialog1.FileName); // lecture/utilisation du contenu du fichier
        }

        // event. click on 'Enregistrer audio..' in MenuStrip1
        private void enregistrerUnAudiowavToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form fen = new RecordSound();
            fen.ShowDialog();
        }

        // event. click on 'Comparer 2 fichiers..' in MenuStrip1
        private void comparer2FichierAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form fen = new CompareAudioFiles();
            fen.ShowDialog();
        }

        // event. click on 'Afficher la clé stégo..' boutton
        private void afficherCleStegoBtn_Click(object sender, EventArgs e)
        {
            // 1 - si clé stégo non saisi
            if (cleStegoTab2.Text == "")
            {
                MessageBox.Show("Aucune clé stégo saisie !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2 - si mot de passe de décryptage AES non saisi
            if (mdpTab2.Text == "")
            {
                MessageBox.Show("Aucun mot de passe (AES) saisi !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                MessageBox.Show(Crypt.DecryptStringAES(cleStegoTab2.Text, mdpTab2.Text), "Clé stégo en clair", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //this.Cursor = Cursors.Default;
                MessageBox.Show("[Erreur de décryptage : " + ex.Message + "]\nIl se peut que le mot de passe ou la clé stégo soit incorrecte !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}

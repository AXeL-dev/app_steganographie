﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Media; // to play .wav files
using System.Windows.Forms; // to show MessageBox
using System.IO;
using NAudio.Wave;

namespace app_steganographie
{
    class ToolsAndFunctions
    {
        // attributes
        private static WaveOutEvent outputDevice;
        private static AudioFileReader audioFile;
        private static string audioFilePath;

        // events
        public static event EventHandler audioPlaybackStop;

        // function to generate random password
        public static string createPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        // function to convert byte array to block string array
        public static string[] bytesToBlocks(byte[] bytesArray, int bitsPerBlock)
        {
            // array of blocks
            string[] BlockStrArray = new string[bytesArray.Length * (8 / bitsPerBlock)];

            int currentBlock = 0; // current string array position

            // for all values in bytesArray
            for (int i = 0; i < bytesArray.Length; i++)
            {
                // this string will countain the binary (8bit) of one byte/caracter
                string binaryString = Convert.ToString(bytesArray[i], 2).PadLeft(8, '0');

                // creating blocks
                for (int b = 0; b < 8; b += bitsPerBlock) // b => block
                    BlockStrArray[currentBlock++] = binaryString.Substring(b, bitsPerBlock);
            }

            return BlockStrArray;
        }

        // function to convert blocks binary string to ASCII string
        public static string BlocksToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }

            return Encoding.ASCII.GetString(byteList.ToArray());
        }

        // function to test if a block exists in a block table
        public static int isBlockExists(string block, string[] blockTable)
        {
            for (int i = 0; i < blockTable.Length; i++)
            {
                if (blockTable[i] == block)
                    return i;
            }

            return -1;
        }

        // function to get file hash/signature (64 caracter)
        public static string GetFileHash(byte[] fileBytes)
        {
            return BitConverter.ToString(SHA256Managed.Create().ComputeHash(fileBytes)).Replace("-", "").ToLower();
        }

        // function to play audio wav files
        public static void playAudioFile(/*object*/ string filePath)
        {
            try
            {
                // On vérifie si on a pas déjà le chemin du fichier à lire
                if (audioFilePath == null || audioFilePath != filePath)
                {
                    audioFilePath = filePath;
                }

                // On vérifie si le lecteur existe déjà
                if (outputDevice == null)
                {
                    // On initialise le lecteur
                    outputDevice = new WaveOutEvent();
                    outputDevice.PlaybackStopped += (s, e) => onAudioPlaybackStop();
                }

                // On initialise le stream du fichier audio à lire
                audioFile = new AudioFileReader(audioFilePath);
                outputDevice.Init(audioFile);

                // lecture
                outputDevice.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // function to stop audio wav files
        public static void stopAudioFile()
        {
            try
            {
                // si le lecteur exist on l'arrête
                if (outputDevice != null)
                    outputDevice.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // function to execute at audio playback stop
        private static void onAudioPlaybackStop()
        {
            // On execute le handler si défini
            if (audioPlaybackStop != null)
                audioPlaybackStop(outputDevice, new EventArgs());
            // On libère les resources
            if (outputDevice != null)
            {
                outputDevice.Dispose();
                outputDevice = null;
            }
            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }
        }

        // function to convert string to stream
        public static Stream stringToStream(string str)
        {
            BinaryWriter bnw = new BinaryWriter(new MemoryStream());

            bnw.Write(str.Length);
            bnw.Write(Encoding.ASCII.GetBytes(str));
            bnw.Seek(0, SeekOrigin.Begin);

            return bnw.BaseStream;
        }

        // function to calculate fitness
        public static int fitness(string blocTexte, string blocAudio)
        {
            for (int i = 0; i < blocTexte.Length; i++)
            {
                if (blocTexte[i] != blocAudio[i]) // if blocks are not similar
                    return i + 1; // return pos where we have prob + 1 (because pos 0 mean all's good)
            }

            return 0; // all fine , blocks are equal, return 0 to confirm
        }
    }
}

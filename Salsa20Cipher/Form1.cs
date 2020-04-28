﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Salsa20Cipher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void rb32_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void rbAuto_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rb16_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rb10_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            byte[] key = Encoding.ASCII.GetBytes(tbKey.Text);
            byte[] nonce = Encoding.ASCII.GetBytes(tbNonce.Text);
            int keyLength = 0;
            if (rb32.Checked) keyLength = Salsa20Cipher.Salsa20cipher.KEY_32BYTE;
            if (rb16.Checked) keyLength = Salsa20Cipher.Salsa20cipher.KEY_16BYTE;
            if (rb10.Checked) keyLength = Salsa20Cipher.Salsa20cipher.KEY_10BYTE;

            if (tabControl1.SelectedIndex == 0)
            {
                byte[] inMsg = Encoding.Default.GetBytes(tbIn.Text);

                var watch = System.Diagnostics.Stopwatch.StartNew();
                byte[] outMsg = Salsa20Cipher.Salsa20cipher.crypt(key,nonce,keyLength,inMsg);
                watch.Stop();
                tbTime.Text = watch.ElapsedMilliseconds.ToString();

                tbOut.Text = Encoding.Default.GetString(outMsg);
            }
            else
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                FileStream inFs = new FileStream(tbInPath.Text, FileMode.Open, FileAccess.Read);
                FileStream outFs = new FileStream(tbOutPath.Text, FileMode.Create);
                byte[] bufferIn = new byte[64];
                int fileOffset = 0;
                ulong i = 0;
                pbar.Value = 0;

                while (fileOffset < inFs.Length)
                {
                    //pbar.Value = (int)(fileOffset / inFs.Length) * 100;
                    pbar.Value = (int)(fileOffset / inFs.Length) * 100;
                    label10.Text = ((fileOffset / inFs.Length) * 100).ToString();
                    inFs.Seek(fileOffset, SeekOrigin.Begin);
                    bufferIn = new byte[64];
                    int bytesRead = inFs.Read(bufferIn, 0, 64);
                    outFs.Write(Salsa20cipher.cryptBlock(key, nonce, i, bufferIn), 0, bytesRead);
                    fileOffset += bytesRead;
                    i++;
                }
                inFs.Close();
                outFs.Close(); watch.Stop();
                tbTime.Text = watch.ElapsedMilliseconds.ToString();
            }

        }

        private void btnIn_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                fd.FilterIndex = 2;
                fd.RestoreDirectory = true;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    tbInPath.Text = fd.FileName;
                }
            }


        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog fd = new SaveFileDialog())
            {
                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                fd.FilterIndex = 2;
                fd.RestoreDirectory = true;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    tbOutPath.Text = fd.FileName;
                }
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}

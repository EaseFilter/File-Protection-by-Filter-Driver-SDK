///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2011 EaseFilter Technologies
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
//    NOTE:  THIS MODULE IS UNSUPPORTED SAMPLE CODE
//
//    This module contains sample code provided for convenience and
//    demonstration purposes only,this software is provided on an 
//    "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
//     either express or implied.  
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using EaseFilter.CommonObjects;

namespace FileProtector
{
    public partial class EncryptedFileForm : Form
    {
        FilterAPI.EncryptType encryptType = FilterAPI.EncryptType.Encryption;

        public EncryptedFileForm(string formName, FilterAPI.EncryptType encryptType)
        {
            InitializeComponent();
            this.Text = formName;
            this.encryptType = encryptType;
        }

        private void button_Start_Click(object sender, EventArgs e)
        {

            string passPhrase = textBox_PassPhrase.Text.Trim();
            string fileName = textBox_FileName.Text.Trim();
            string lastError = string.Empty;
            bool retVal = false;

            if (passPhrase.Length == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Password phrase can't be empty.", "Encryption", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (fileName.Length == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("File name can't be empty.", "Encryption", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] key = Utils.GetKeyByPassPhrase(passPhrase);
            byte[] iv = Utils.GetRandomIV();

            if (checkBox_UseDefaultIV.Checked)
            {
                iv = FilterAPI.DEFAULT_IV_TAG;
            }
            

            if (encryptType == FilterAPI.EncryptType.Encryption)
            {
                retVal = FilterAPI.AESEncryptFile(fileName, (uint)key.Length, key, (uint)iv.Length, iv, true);
            }
            else
            {
                if (!checkBox_UseDefaultIV.Checked)
                {
                    uint accessFlags = 0;
                    retVal = FilterAPI.GetIVTag(fileName, ref iv,ref accessFlags, out lastError);

                    if (!retVal)
                    {
                        MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                        MessageBox.Show(" Get file " + fileName + " IV tag failed with error:" + lastError, encryptType.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                retVal = FilterAPI.AESDecryptFile(fileName, (uint)key.Length, key, (uint)iv.Length, iv);

            }


            if (!retVal)
            {
                string errorMessage = FilterAPI.GetLastErrorMessage();

                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show(encryptType.ToString() + " file " + fileName + " failed with error:" + errorMessage, encryptType.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            else
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show(encryptType.ToString() + " file " + fileName + " succeeded.", encryptType.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void checkBox_DisplayPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_DisplayPassword.Checked)
            {
                textBox_PassPhrase.UseSystemPasswordChar = false;
            }
            else
            {
                textBox_PassPhrase.UseSystemPasswordChar = true;
            }
        }

        private void button_OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_FileName.Text = openFileDialog.FileName;
            }
        }
    }
}

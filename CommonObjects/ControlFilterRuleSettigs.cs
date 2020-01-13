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
using System.Windows.Forms;

namespace EaseFilter.CommonObjects
{
    public partial class ControlFilterRuleSettigs : Form
    {
        public FilterRule filterRule = new FilterRule();

        public ControlFilterRuleSettigs()
        {
        }

        public ControlFilterRuleSettigs(FilterRule _filterRule)
        {
            InitializeComponent();
            filterRule = _filterRule;

            textBox_FileAccessFlags.Text = filterRule.AccessFlag.ToString();
            textBox_PassPhrase.Text = filterRule.EncryptionPassPhrase;
            textBox_HiddenFilterMask.Text = filterRule.HiddenFileFilterMasks;
            textBox_ReparseFileFilterMask.Text = filterRule.ReparseFileFilterMasks;
            textBox_ControlIO.Text = filterRule.ControlIO.ToString();
            checkBox_EnableProtectionInBootTime.Checked = filterRule.IsResident;

            textBox_ProcessRights.Text = filterRule.ProcessRights;
            textBox_ProcessIdRights.Text = filterRule.ProcessIdRights;

            textBox_UserRights.Text = filterRule.UserRights;

            comboBox_EncryptMethod.Items.Clear();

            //General infomation
            foreach (FilterAPI.EncryptionMethod item in Enum.GetValues(typeof(FilterAPI.EncryptionMethod)))
            {
                comboBox_EncryptMethod.Items.Add(item.ToString());

                if ((int)item == filterRule.EncryptMethod)
                {
                    comboBox_EncryptMethod.SelectedItem = item.ToString();
                }
            }

            SetCheckBoxValue();
        }

        private void SetCheckBoxValue()
        {

            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE) > 0 )
            {
                checkBox_Encryption.Checked = true;
                comboBox_EncryptMethod.Enabled = true;
                textBox_PassPhrase.ReadOnly = false;
            }
            else
            {
                comboBox_EncryptMethod.Enabled = false;
                checkBox_Encryption.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK) > 0)
            {
                checkBox_AllowRemoteAccess.Checked = true;
            }
            else
            {
                checkBox_AllowRemoteAccess.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE) > 0)
            {
                checkBox_AllowDelete.Checked = true;
            }
            else
            {
                checkBox_AllowDelete.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME) > 0)
            {
                checkBox_AllowRename.Checked = true;
            }
            else
            {
                checkBox_AllowRename.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0
                && (accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION) > 0)
            {
                checkBox_AllowChange.Checked = true;
            }
            else
            {
                checkBox_AllowChange.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS) > 0)
            {
                checkBox_AllowNewFileCreation.Checked = true;
            }
            else
            {
                checkBox_AllowNewFileCreation.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_MEMORY_MAPPED) > 0)
            {
                checkBox_AllowMemoryMapped.Checked = true;
            }
            else
            {
                checkBox_AllowMemoryMapped.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS) > 0)
            {
                checkBox_AllowSetSecurity.Checked = true;
            }
            else
            {
                checkBox_AllowSetSecurity.Checked = false;
            }


            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_ALL_SAVE_AS) > 0)
            {
                checkBox_AllowSaveAs.Checked = true;
            }
            else
            {
                checkBox_AllowSaveAs.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT) > 0)
            {
                checkBox_AllowCopyOut.Checked = true;
            }
            else
            {
                checkBox_AllowCopyOut.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES) > 0)
            {
                checkBox_AllowReadEncryptedFiles.Checked = true;
            }
            else
            {
                checkBox_AllowReadEncryptedFiles.Checked = false;
            }
        }

        private void button_SaveControlSettings_Click(object sender, EventArgs e)
        {
            string encryptionPassPhrase = string.Empty;
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);
             
            filterRule.EncryptMethod = comboBox_EncryptMethod.SelectedIndex;

            if (checkBox_Encryption.Checked)
            {
                if (filterRule.EncryptMethod == (uint)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_IV
                    || filterRule.EncryptMethod == (uint)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_UNIQUE_IV)
                {
                    if (textBox_PassPhrase.Text.Trim().Length == 0)
                    {
                        MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                        MessageBox.Show("The passphrase can't be empty for this encrypt method: " + ((FilterAPI.EncryptionMethod)filterRule.EncryptMethod).ToString(),
                            "SaveControlSettings", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                }

                encryptionPassPhrase = textBox_PassPhrase.Text;

                //enable encryption for this filter rule.
                accessFlags = accessFlags | (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;
            }

            if (textBox_HiddenFilterMask.Text.Trim().Length > 0)
            {
                //enable hidden filter mask for this filter rule.
                accessFlags = accessFlags | (uint)FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING;
            }


            filterRule.EncryptionPassPhrase = encryptionPassPhrase;
            filterRule.HiddenFileFilterMasks = textBox_HiddenFilterMask.Text;
            filterRule.ReparseFileFilterMasks = textBox_ReparseFileFilterMask.Text;
            filterRule.AccessFlag = accessFlags;
            filterRule.ControlIO = uint.Parse(textBox_ControlIO.Text);
            filterRule.IsResident = checkBox_EnableProtectionInBootTime.Checked;
            filterRule.UserRights = textBox_UserRights.Text;
            filterRule.ProcessRights = textBox_ProcessRights.Text;
            filterRule.ProcessIdRights = textBox_ProcessIdRights.Text;


           

            
        }


        private void button_FileAccessFlags_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.Access_Flag, textBox_FileAccessFlags.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (optionForm.AccessFlags > 0)
                {
                    textBox_FileAccessFlags.Text = optionForm.AccessFlags.ToString();
                }
                else
                {
                    //if the accessFlag is 0, it is exclude filter rule,this is not what we want, so we need to include this flag.
                    textBox_FileAccessFlags.Text = ((uint)FilterAPI.AccessFlag.LEAST_ACCESS_FLAG).ToString();
                }

                SetCheckBoxValue();
            }
        }


        private void button_RegisterControlIO_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.Register_Request, textBox_ControlIO.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_ControlIO.Text = optionForm.RequestRegistration.ToString();
            }
        }

        private void button_AddProcessRights_Click(object sender, EventArgs e)
        {
            Form_AccessRights accessRightsForm = new Form_AccessRights(Form_AccessRights.AccessRightType.ProcessNameRight);

            if (accessRightsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (textBox_ProcessRights.Text.Trim().Length > 0)
                {
                    textBox_ProcessRights.Text += ";" + accessRightsForm.accessRightText;
                }
                else
                {
                    textBox_ProcessRights.Text = accessRightsForm.accessRightText;
                }
            }
        }

        private void button_AddProcessIdRights_Click(object sender, EventArgs e)
        {
            Form_AccessRights accessRightsForm = new Form_AccessRights(Form_AccessRights.AccessRightType.ProccessIdRight);

            if (accessRightsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (textBox_ProcessIdRights.Text.Trim().Length > 0)
                {
                    textBox_ProcessIdRights.Text += ";" + accessRightsForm.accessRightText;
                }
                else
                {
                    textBox_ProcessIdRights.Text = accessRightsForm.accessRightText;
                }
            }

        }   


        private void button_AddUserRights_Click(object sender, EventArgs e)
        {
            Form_AccessRights accessRightsForm = new Form_AccessRights(Form_AccessRights.AccessRightType.UserNameRight);

            if (accessRightsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (textBox_UserRights.Text.Trim().Length > 0)
                {
                    textBox_UserRights.Text += ";" + accessRightsForm.accessRightText;
                }
                else
                {
                    textBox_UserRights.Text = accessRightsForm.accessRightText;
                }
            }
        }


        private void checkBox_Encryption_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (checkBox_Encryption.Checked)
            {
                textBox_PassPhrase.ReadOnly = false;
                comboBox_EncryptMethod.Enabled = true;
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE);
            }
            else
            {
                comboBox_EncryptMethod.Enabled = false;
                textBox_PassPhrase.ReadOnly = true;
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowDelete_CheckedChanged(object sender, EventArgs e)
        {

            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowDelete.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_FILE_DELETE);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowChange_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);
            if (!checkBox_AllowChange.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) & ((uint)~FilterAPI.AccessFlag.ALLOW_SET_INFORMATION);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) | ((uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowRename_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowRename.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_FILE_RENAME);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowRemoteAccess_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowRemoteAccess.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowNewFileCreation_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowNewFileCreation.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowMemoryMapped_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowMemoryMapped.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_FILE_MEMORY_MAPPED);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_FILE_MEMORY_MAPPED);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowSetSecurity_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowSetSecurity.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }


        private void checkBox_AllowSaveAs_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowSaveAs.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_ALL_SAVE_AS);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_ALL_SAVE_AS);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowCopyOut_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowCopyOut.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowReadEncryptedFiles_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_AllowReadEncryptedFiles.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void textBox_FileAccessFlags_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("This is the access control flags,uncheck the access right will deny the associated access.", textBox_FileAccessFlags);
        }

        private void textBox_ProcessRights_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("You can set the specific processes access rights,to authorize or deny the file access to these processes.", textBox_FileAccessFlags);
        }

        private void textBox_UserRights_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("You can set the specific users access rights,to authorize or deny the file access to these users.", textBox_UserRights);
        }

        private void textBox_ControlIO_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("This is the block and wait IO notification with the user name, process name, file name and detail IO data information, you can modify, allow or block the IO access here.", textBox_ControlIO);
        }

        private void textBox_ReparseFileFilterMask_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("You can reparse the file open to another file path with this setting.", textBox_ReparseFileFilterMask);
        }

        private void textBox_HiddenFilterMask_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("You can hide the file names when users browse the folder with this setting.", textBox_HiddenFilterMask);
        }

        private void textBox_PassPhrase_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Enable the transparent file encryption with this passphrase when the accessFlag encryption was enabled.", textBox_PassPhrase);
        }

        private void button_Info_Click(object sender, EventArgs e)
        {
            string information = "ENCRYPT_FILE_WITH_SAME_KEY_AND_IV: Use the same encryption key and iv from the filter rule for all files, there are no reparse point tag data added to encrypted file,it supports all file systems.You can't identify if the file is encrypted or not by API.\r\n\r\n";
            information += "ENCRYPT_FILE_WITH_SAME_KEY_AND_UNIQUE_IV: Use the same encryption key from the filter rule for all files, use unique iv key per file, the iv key will be attached to the encyrpted file as a reparse point extended attribute, it only supports NTFS, ReFS.You can identify if the file is encrypted by checking the tagdata.\r\n\r\n";
            information += "ENCRYPT_FILE_WITH_KEY_AND_IV_FROM_SERVICE: Use the encryption key and iv from user mode service, your own service can determine if the file can be encrypted or the encrypted can be opened,you also can control how to use encryption key and iv for files, there are no reparse point tag data added to encrypted file.You can't identify if the file is encrypted or not by API.\r\n\r\n";
            information += "ENCRYPT_FILE_WITH_KEY_IV_AND_TAGDATA_FROM_SERVICE: Use the encryption key and iv from user mode service, your own service can determine if the file can be encrypted or the encrypted can be opened,you also can control how to use encryption key and iv for files, you can add your own customized tag data to the encrypted file.You can identify if the file is encrypted by checking the tagdata.\r\n\r\n"; 

            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            MessageBox.Show(information,"Encryption Methods", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

      
    }
}

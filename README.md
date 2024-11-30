# File Protection by Filter Driver SDK
A Windows file system control filter driver library, for you to develop Windows file access control application with the SDK.A file system filter driver is an optional driver that adds value to or modifies the behavior of a file system. A file system filter driver is a kernel-mode component that runs as part of the Windows executive.A file system filter driver intercepts requests targeted at a file system or another file system filter driver. By intercepting the request before it reaches its intended target, the filter driver can extend or replace functionality provided by the original target of the request.
 
What can you do with the File Control Filter Driver SDK
1.Block the new file creation via configuring the access control flag of the filter rule.

Example:
Block the new file creation in folder c:\test:
AddFileFilterRule(ALLOW_MAX_RIGHT_ACCESS&(~ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS), L"c:\\test\\*", 1);

2.Prevent your sensitive files from being copied out of your protected folder

Example:
Prevent the files in folder c:\test from being copied out.
AddFileFilterRule(ALLOW_MAX_RIGHT_ACCESS&(~ALLOW_COPY_PROTECTED_FILES_OUT), L"c:\\test\\*", 1);
     
3.Prevent your sensitive files from being modified, renamed or deleted

Example:
Prevent the file from being modified, renamed or deleted in folder c:\test:
AddFileFilterRule(ALLOW_MAX_RIGHT_ACCESS&(~(ALLOW_WRITE_ACCESS|ALLOW_FILE_RENAME|ALLOW_FILE_DELETE), L"c:\\test\\*", 1);

4.Prevent your sensitive files from being accessed from the network computer

Example:
Protect the files in folder c:\test, block the file access from the network.

AddFileFilterRule(ALLOW_MAX_RIGHT_ACCESS&(~ALLOW_FILE_ACCESS_FROM_NETWORK), L"c:\\test\\*", 1);

5.Hide your sensitive files to the specific processes or users
  
Example:
Hide the files in folder c:\test for process "explorer.exe"

AddFileFilterRule(ALLOW_MAX_RIGHT_ACCESS|HIDE_FILES_IN_DIRECTORY_BROWSING, L"c:\\test\\*", 1);
AddIncludeProcessNameToFilterRule(L"c:\\test\\*",L"explorer.exe");
AddHiddenFileMaskToFilterRule(L"c:\\test\\*",L"*.*");

6.Reparse your file open from one location to another location.

Example:
Reparse the file open in folder c:\test to another folder c:\reparseFolder"

AddFileFilterRule(ALLOW_MAX_RIGHT_ACCESS|REPARSE_FILE_OPEN, L"c:\\test\\*", 1);
AddReparseFileMaskToFilterRule(L"c:\\test\\*",L"c:\\reparseFolder\\*");

7.Allow or deny the specific file I/O operation via registering the specific I/O callback routine based on the process name, user name or the file I/O information.

Example:
Register the PRE_CREATE, PRE_SETINFORMATION I/O for folder c:\test, you can allow or deny the file opern, creation, deletion, rename in the callback routine.

AddFileFilterRule(ALLOW_MAX_RIGHT_ACCESS, L"c:\\test\\*", 1);
RegisterControlToFilterRule(L"c:\\test\\*",PRE_CREATE|PRE_SET_INFORMATION);

8.Authorize or De-authorize the file access rights (read,write,rename,delete..) to the specific processes or users.
Example:
Set the full access rights to the process "notepad.exe", set the readonly access rights to the process "wordpad.exe", remove all the access rights to other processes.

AddFileFilterRule(LEAST_ACCESS_FLAG, L"c:\\test\\*", 1);
AddProcessRightsToFilterRule(L"c:\\test\\*",L"notepad.exe",ALLOW_MAX_RIGHT_ACCESS);
AddProcessRightsToFilterRule(L"c:\\test\\*",L"wordpad.exe",ALLOW_MAX_RIGHT_ACCESS&(~(ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS|ALLOW_WRITE_ACCESS|ALLOW_FILE_RENAME|ALLOW_FILE_DELETE|ALLOW_SET_INFORMATION));


## EaseFilter File System Filter Driver SDK Reference
| Product Name | Description |
| --- | --- |
| [File Monitor SDK](https://www.easefilter.com/kb/file-monitor-filter-driver-sdk.htm) | EaseFilter File Monitor Filter Driver SDK Introduction. |
| [File Control SDK](https://www.easefilter.com/kb/file-control-file-security-sdk.htm) | EaseFilter File Control Filter Driver SDK Introduction. |
| [File Encryption SDK](https://www.easefilter.com/kb/transparent-file-encryption-filter-driver-sdk.htm) | EaseFilter Transparent File Encryption Filter Driver SDK Introduction. |
| [Registry Filter SDK](https://www.easefilter.com/kb/registry-filter-drive-sdk.htm) | EaseFilter Registry Filter Driver SDK Introduction. |
| [Process Filter SDK](https://www.easefilter.com/kb/process-filter-driver-sdk.htm) | EaseFilter Process Filter Driver SDK Introduction. |
| [Storage Tiering SDK](https://www.easefilter.com/cloud/storage-tiering-sdk.htm) | EaseFilter Storage Tiering Filter Driver SDK Introduction. |
| [EaseFilter SDK Programming](https://www.easefilter.com/kb/programming.htm) | EaseFilter Filter Driver SDK Programming. |

## EaseFilter SDK Sample Projects
| Sample Project | Description |
| --- | --- |
| [Auto File DRM Encryption](https://www.easefilter.com/kb/auto-file-drm-encryption-tool.htm) | Auto file encryption with DRM data embedded. |
| [Transparent File Encrypt](https://www.easefilter.com/kb/AutoFileEncryption.htm) | Transparent on access file encryption. |
| [Secure File Sharing with DRM](https://www.easefilter.com/kb/DRM_Secure_File_Sharing.htm) | Secure encrypted file sharing with digital rights management. |
| [File Monitor Example](https://www.easefilter.com/kb/file-monitor-demo.htm) | Monitor file system I/O in real time, tracking file changes. |
| [File Protector Example](https://www.easefilter.com/kb/file-protector-demo.htm) | Prevent sensitive files from being accessed by unauthorized users or processes. |
| [FolderLocker Example](https://www.easefilter.com/kb/FolderLocker.htm) | Lock file automatically in a FolderLocker. |
| [Process Monitor](https://www.easefilter.com/kb/Process-Monitor.htm) | Monitor the process creation and termination, block unauthorized process running. |
| [Registry Monitor](https://www.easefilter.com/kb/RegMon.htm) | Monitor the Registry activities, block the modification of the Registry keys. |
| [Secure Sandbox Example](https://www.easefilter.com/kb/Secure-Sandbox.htm) |A secure sandbox example, block the processes accessing the files out of the box. |
| [FileSystemWatcher Example](https://www.easefilter.com/kb/FileSystemWatcher.htm) | File system watcher, logging the file I/O events. |

## Filter Driver Reference

* Understand MiniFilter Driver: https://www.easefilter.com/kb/understand-minifilter.htm
* Understand File I/O: https://www.easefilter.com/kb/File_IO.htm
* Understand I/O Request Packets(IRPs):https://www.easefilter.com/kb/understand-irps.htm
* Filter Driver Developer Guide: https://www.easefilter.com/kb/DeveloperGuide.htm
* MiniFilter Filter Driver Framework: https://www.easefilter.com/kb/minifilter-framework.htm
* Isolation Filter Driver: https://www.easefilter.com/kb/Isolation_Filter_Driver.htm

## Support
If you have questions or need help, please contact support@easefilter.com 

[Home](https://www.easefilter.com/) | [Solution](https://www.easefilter.com/solutions.htm) | [Download](https://www.easefilter.com/download.htm) | [Demos](https://www.easefilter.com/online-fileio-test.aspx) | [Blog](https://blog.easefilter.com/) | [Programming](https://www.easefilter.com/kb/programming.htm)

//===============================================================================
// Copyright © Serhiy Perevoznyk.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Karna.Compression
{
    /// <summary>
    /// KarnaZip is a class for dealing with zip archives that can add, freshen, or move files 
    /// in an archive. For extracting files from zip archive see <see cref="Karna.Compression.KarnaUnzip"/> class.
    /// </summary>
    public class KarnaZip
    {
        #region Private members
        private string comment;
        private string password;
        private string fileName;

        private ZipUserFunctions zuf;
        private ASCIIEncoding Ascii = new ASCIIEncoding();

        private string rootFolder = string.Empty;
        private string temporaryFolder = string.Empty;
        private bool systemFilesIgnored;
        private bool volumeLabelStored;
        private bool extraAttributesExcluded;
        private bool dirEntriesIgnored;
        private bool verboseMessages;
        private bool quietMessages;
        private bool growZipFile;
        private Recurse recurse;
        private Repair repair;
        private CompressionLevel compressionLevel;
        private bool setLatestTime;
        private bool usePrivileges;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="KarnaZip"/> class.
        /// </summary>
        public KarnaZip()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KarnaZip"/> class.
        /// </summary>
        /// <param name="fileName">Name of the zip file.</param>
        public KarnaZip(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// Set zip file time to time of latest file in it
        /// </summary>
        /// <value><c>true</c> if zip file time is modified; otherwise, <c>false</c>.</value>
        public bool SetLatestTime
        {
            get { return setLatestTime; }
            set { setLatestTime = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use privileges (if granted) 
        /// to obtain all aspects of WinNT security
        /// </summary>
        /// <value><c>true</c> if use security privilege; otherwise, <c>false</c>.</value>
        public bool UsePrivileges
        {
            get { return usePrivileges; }
            set { usePrivileges = value; }
        }

        /// <summary>
        /// Gets or sets the recurse into subfolders level.
        /// </summary>
        /// <value>The recurse level.</value>
        /// <seealso cref="Karna.Compression.Recurse"/>
        public Recurse Recurse
        {
            get { return recurse; }
            set { recurse = value; }
        }

        /// <summary>
        /// Gets or sets the repair arhive options.
        /// </summary>
        /// <value>The repair archive options.</value>
        /// <seealso cref="Karna.Compression.Repair"/>
        public Repair Repair
        {
            get { return repair; }
            set { repair = value; }
        }

        /// <summary>
        /// Gets or sets the archive compression level.
        /// </summary>
        /// <value>The archive compression level.</value>
        ///<seealso cref="Karna.Compression.CompressionLevel"/>
        public CompressionLevel CompressionLevel
        {
            get { return compressionLevel; }
            set { compressionLevel = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the volume label is stored.
        /// </summary>
        /// <value><c>true</c> if volume label is stored; otherwise, <c>false</c>.</value>
        public bool VolumeLabelStored
        {
            get { return volumeLabelStored; }
            set { volumeLabelStored = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether system and hidden files are ignored.
        /// </summary>
        /// <value><c>true</c> if system and hidden files are ignored; otherwise, <c>false</c>.</value>
        public bool SystemFilesIgnored
        {
            get { return systemFilesIgnored; }
            set { systemFilesIgnored = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether extra file attributes are excluded.
        /// </summary>
        /// <value>
        ///	<c>true</c> if extra file attributes are excluded; otherwise, <c>false</c>.
        /// </value>
        public bool ExtraAttributesExcluded
        {
            get { return extraAttributesExcluded; }
            set { extraAttributesExcluded = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether directory entries ignored.
        /// </summary>
        /// <value><c>true</c> if ignoring directory entries; otherwise, <c>false</c>.</value>
        public bool DirEntriesIgnored
        {
            get { return dirEntriesIgnored; }
            set { dirEntriesIgnored = value; }
        }

        /// <summary>
        /// Mention oddities in zip file structure
        /// </summary>
        /// <value><c>true</c> if additional messages has to be processed during archive oprations; otherwise, <c>false</c>.</value>
        public bool VerboseMessages
        {
            get { return verboseMessages; }
            set { verboseMessages = value; }
        }

        /// <summary>
        /// Quiet mode;  eliminate informational messages and comment prompts.
        /// </summary>
        /// <value><c>true</c> if archive oprations are performed in the quiet mode; otherwise, <c>false</c>.</value>
        public bool QuietMessages
        {
            get { return quietMessages; }
            set { quietMessages = value; }
        }

        /// <summary>
        /// Allows appending to a zip file
        /// </summary>
        /// <value><c>true</c> if allow appending to zip file; otherwise, <c>false</c>.</value>
        public bool GrowZipFile
        {
            get { return growZipFile; }
            set { growZipFile = value; }
        }


        #region Private Methods
        /// <summary>
        /// Creates the zip options.
        /// </summary>
        /// <returns></returns>
        private ZipOptions CreateZipOptions()
        {
            ZipOptions zopt = new ZipOptions();

            zopt.Date = string.Empty;
            zopt.szRootDir = rootFolder;
            zopt.szTempDir = temporaryFolder;
            zopt.fTemp = (string.IsNullOrEmpty(temporaryFolder)) ? 0 : 1;
            zopt.fEncrypt = (string.IsNullOrEmpty(password)) ? 0 : 1;
            zopt.fSystem = InternalHelper.BoolToInt(SystemFilesIgnored);
            zopt.fVolume = InternalHelper.BoolToInt(VolumeLabelStored);
            zopt.fExtra = InternalHelper.BoolToInt(ExtraAttributesExcluded);
            zopt.fNoDirEntries = InternalHelper.BoolToInt(DirEntriesIgnored);
            zopt.fVerbose = InternalHelper.BoolToInt(VerboseMessages);
            zopt.fQuiet = InternalHelper.BoolToInt(QuietMessages);

            //Do not translate CR/LF to LF 
            zopt.fCRLF_LF = 0;
            //Do not translate LF to CR/LF
            zopt.fLF_CRLF = 0;

            zopt.fGrow = InternalHelper.BoolToInt(GrowZipFile);
            zopt.fLatestTime = InternalHelper.BoolToInt(setLatestTime);
            zopt.fComment = (string.IsNullOrEmpty(comment)) ? 0 : 1;
            zopt.fPrivilege = InternalHelper.BoolToInt(usePrivileges);
            zopt.fRecurse = (int)(recurse);
            zopt.fLevel = (byte)CompressionLevel;

            zopt.fJunkDir = 0;
            zopt.fExcludeDate = 0;
            zopt.fIncludeDate = 0;
            zopt.fForce = 0;
            zopt.fOffsets = 0;
            zopt.fSuffix = 0;
            zopt.fJunkSFX = 0;
            zopt.fEncryption = 0;
            return zopt;
        }

        /// <summary>
        /// Creates the zip options for delete files opration.
        /// </summary>
        /// <returns>ZipOptions structure prepared for deleting files opration</returns>
        private ZipOptions CreateZipOptionsForDelete()
        {
            ZipOptions zopt = CreateZipOptions();
            zopt.fDeleteEntries = 1;
            return zopt;
        }

        /// <summary>
        /// Creates the zip options for update files in zip archive.
        /// </summary>
        /// <returns>ZipOptions structure, prepared for files update operation</returns>
        private ZipOptions CreateZipOptionsForUpdate()
        {
            ZipOptions zopt = CreateZipOptions();
            zopt.fUpdate = 1;
            return zopt;
        }

        private ZipOptions CreateZipOptionsForFreshen()
        {
            ZipOptions zopt = CreateZipOptions();
            zopt.fFreshen = 1;
            return zopt;
        }

        private ZipOptions CreateZipOptionsForMove()
        {
            ZipOptions zopt = CreateZipOptions();
            zopt.fMove = 1;
            return zopt;
        }

        #endregion

        #region Events
        /// <summary>
        /// Occurs when print message from Info-ZIP engine is received
        /// </summary>
        public event EventHandler<CompressionEventArgs> PrintMessage;
        /// <summary>
        /// Occurs when the service message from Info-ZIP engine is received.
        /// It provides the application with the name of the name of the archive 
        /// member it has just processed, as well as it's original size.
        /// </summary>
        public event EventHandler<CompressionServiceEventArgs> ServiceMessage;
        #endregion

        /// <summary>
        /// Raises the <see cref="PrintMessage"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Karna.Compression.CompressionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPrintMessage(CompressionEventArgs e)
        {
            if (PrintMessage != null)
                PrintMessage(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Karna.Compression.KarnaUnzip.ServiceMessage"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Karna.Compression.CompressionServiceEventArgs"/> instance containing the event data.</param>
        protected virtual void OnReceiveServiceMessage(CompressionServiceEventArgs e)
        {
            if (ServiceMessage != null)
                ServiceMessage(this, e);
        }

        #region Callback Functions


        /// <summary>
        /// The zip32.dll passes activity messages to the declared byte array.  
        /// Decode the byte array to see the message
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <param name="x">The length of the message.</param>
        /// <returns></returns>
        internal int ProcessPrintCallback(ref CallbackString msg, uint x)
        {

            string s = InternalHelper.PCharToString(msg.Buffer);
            CompressionEventArgs e = new CompressionEventArgs(s);
            OnPrintMessage(e);

            return 0;
        }

        /// <summary>
        /// Processes the service message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        internal int ProcessServiceMessage(ref CallbackString msg, uint x)
        {

            string s = InternalHelper.PCharToString(msg.Buffer);
            CompressionServiceEventArgs e = new CompressionServiceEventArgs(s, (int)x);
            OnReceiveServiceMessage(e);

            return 0;
        }

        /// <summary>
        /// Processes the password callback.
        /// </summary>
        /// <param name="passwordBuffer">The password buffer.</param>
        /// <param name="n">The n.</param>
        /// <param name="textMessage">The text message.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        internal int ProcessPasswordCallback(IntPtr passwordBuffer, int n,  string textMessage,  string name)
        {
            if (string.IsNullOrEmpty(password))
                return 1;


            try
            {
                byte[] pwdCopy = new byte[n];

                for (int i = 0; i <= n - 1; i++)
                    pwdCopy[i] = 0x0;

                Ascii.GetBytes(password, 0, password.Length, pwdCopy, 0);

                Marshal.Copy(pwdCopy, 0, passwordBuffer, n);
            }
            catch (ArgumentOutOfRangeException)
            {
                return 1;
            }
            catch (ArgumentNullException)
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Processes the comment callback. 
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns></returns>
        internal int ProcessCommentCallback(ref CallbackString msg)
        {

            if (string.IsNullOrEmpty(comment))
                return 1;

            //clear the byte array
            for (int i = 0; i <= msg.Buffer.Length - 1; i++)
                msg.Buffer[i] = 0;

            //set the comment in the zip file
            Ascii.GetBytes(comment, 0, comment.Length, msg.Buffer, 0);

            return 0;
        }

        #endregion


        /// <summary>
        /// Deletes the specified files from the archive.
        /// </summary>
        /// <param name="fileMask">The file mask.
        /// The wild card characters '*' and '?' are allowed in file mask
        /// </param>
        /// <returns>Returns <c>true</c> if successful, or <c>false</c> otherwise.</returns>
        public bool DeleteFiles(string[] fileMask)
        {
            ZipOptions zopt = CreateZipOptionsForDelete();
            return ProcessFiles(fileMask, zopt);
        }

        /// <summary>
        /// Deletes the specified files from the archive.
        /// </summary>
        /// <param name="fileMask">The file mask.
        /// The wild card characters '*' and '?' are allowed in file mask
        /// </param>
        /// <returns>Returns <c>true</c> if successful, or <c>false</c> otherwise.</returns>
        public bool DeleteFiles(string fileMask)
        {
            string[] fileNames = { fileMask };
            return DeleteFiles(fileNames);
        }

        /// <summary>
        /// Updates the specified files in the archive
        /// </summary>
        /// <param name="fileMask">The file mask.
        /// The wild card characters '*' and '?' are allowed in file mask
        /// </param>
        /// <returns>Returns <c>true</c> if successful, or <c>false</c> otherwise.</returns>
        public bool UpdateFiles(string[] fileMask)
        {
            ZipOptions zopt = CreateZipOptionsForUpdate();
            return ProcessFiles(fileMask, zopt);
        }

        /// <summary>
        /// Updates the specified files in the archive
        /// </summary>
        /// <param name="fileMask">The file mask.
        /// The wild card characters '*' and '?' are allowed in file mask
        /// </param>
        /// <returns>Returns <c>true</c> if successful, or <c>false</c> otherwise.</returns>
        public bool UpdateFiles(string fileMask)
        {
            string[] fileNames = { fileMask };
            return UpdateFiles(fileNames);
        }

        /// <summary>
        /// Freshens the specified files in the archive.
        /// </summary>
        /// <param name="fileMask">The file mask.
        /// The wild card characters '*' and '?' are allowed in file mask
        /// </param>
        /// <returns>Returns <c>true</c> if successful, or <c>false</c> otherwise.</returns>
        public bool FreshenFiles(string[] fileMask)
        {
            ZipOptions zopt = CreateZipOptionsForFreshen();
            return ProcessFiles(fileMask, zopt);
        }

        /// <summary>
        /// Freshens the specified files in the archive.
        /// </summary>
        /// <param name="fileMask">The file mask.
        /// The wild card characters '*' and '?' are allowed in file mask
        /// </param>
        /// <returns>Returns <c>true</c> if successful, or <c>false</c> otherwise.</returns>
        public bool FreshenFiles(string fileMask)
        {
            string[] fileNames = { fileMask };
            return FreshenFiles(fileNames);
        }

        /// <summary>
        /// Moves the specified files to ZIP archive.
        /// </summary>
        /// <param name="fileMask">The file name mask. 
        /// The wild card characters '*' and '?' are allowed in file mask</param>
        /// <returns>Returns <c>true</c> if successful, or <c>false</c> otherwise.</returns>
        public bool MoveFiles(string[] fileMask)
        {
            ZipOptions zopt = CreateZipOptionsForMove();
            return ProcessFiles(fileMask, zopt);
        }

        /// <summary>
        /// Moves the specified files to ZIP archive.
        /// </summary>
        /// <param name="fileMask">The file mask. 
        /// The wild card characters '*' and '?' are allowed in file mask</param>
        /// <returns>Returns <c>true</c> if successful, or <c>false</c> otherwise.</returns>
        public bool MoveFiles(string fileMask)
        {
            string[] fileNames = { fileMask };
            return MoveFiles(fileNames);
        }

        /// <summary>
        /// Adds the files that match file mask to the archive.
        /// </summary>
        /// <param name="fileMask">The file mask.
        /// The wild card characters '*' and '?' are allowed in file mask
        /// </param>
        /// <returns>Returns <c>true</c> if successful, or <c>false</c> otherwise.</returns>
        public bool AddFiles(string fileMask)
        {
            string[] fileNames = { fileMask };
            return AddFiles(fileNames);
        }

        /// <summary>
        /// Prepares the call back.
        /// </summary>
        /// <returns>ZipUserFunctions stucture</returns>
        internal ZipUserFunctions PrepareCallBack()
        {
            zuf = new ZipUserFunctions();
            zuf.PrintCallbackFunction = new PrintCallbackDelegate(ProcessPrintCallback);
            zuf.ServiceCallbackFunction = new ServiceCallbackDelegate(ProcessServiceMessage);
            zuf.PasswordCallbackFunction = new PasswordCallbackDelegate(ProcessPasswordCallback);
            zuf.CommentCallbackFunction = new CommentCallbackDelegate(ProcessCommentCallback);
            return zuf;
        }

        /// <summary>
        /// Adds the files that match file mask to the archive.
        /// </summary>
        /// <param name="fileMask">The file name mask, for example <c>"*.txt"</c>
        /// The wild card characters '*' and '?' are allowed in file mask
        /// </param>
        /// <returns><c>true</c>, if files successfully added to the archive</returns>
        /// <example>
        /// <code>
        /// string[] content = { "*.jpg" };
        /// KarnaZip zip = new KarnaZip();
        /// zip.PrintMessage += new EventHandler&lt;CompressionEventArgs&gt;(zip_PrintMessage);
        /// zip.ServiceMessage += new EventHandler&lt;CompressionServiceEventArgs&gt;(zip_ServiceMessage);
        /// zip.FileName = "test.zip";
        /// zip.Password = "password";
        /// zip.Comment = "This is just a test archive";
        /// zip.AddFiles(content);
        /// </code>
        /// </example>
        public bool AddFiles(string[] fileMask)
        {
            ZipOptions zopt = CreateZipOptions();
            return ProcessFiles(fileMask, zopt);
        }

        /// <summary>
        /// Processes the files. The opration is defined by ZipOptions (add, move or delete)
        /// </summary>
        /// <param name="fileMask">The file name mask.
        /// The wild card characters '*' and '?' are allowed in file mask
        /// </param>
        /// <param name="zopt">Compression options</param>
        /// <returns><c>true</c> if files successfully processed; otherwise, <c>false</c> </returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        internal bool ProcessFiles(string[] fileMask, ZipOptions zopt)
        {
            int iret;
            ZipError zipError;

            if (string.IsNullOrEmpty(FileName))
                return false;

            if (fileMask.Length == 0)
                return false;

            ZipUserFunctions zuf = PrepareCallBack();

            try
            {
                iret = NativeMethods.ZpInit(ref zuf);
            }
            catch (DllNotFoundException)
            {
                return false;
            }

            if (iret == 0)
                return false;


            try
            {
                zipError = NativeMethods.ZpSetOptions(ref zopt);
                zipError = NativeMethods.ZpArchive(fileMask.Length, fileName, fileMask);
            }
            catch (DllNotFoundException)
            {
                return false;
            }
            return (zipError == ZipError.ZE_OK);
        }

        /// <summary>
        /// Gets or sets the root folder of the archive
        /// </summary>
        /// <value>The archive root folder.</value>
        public string RootFolder
        {
            get { return rootFolder; }
            set { rootFolder = value; }
        }

        /// <summary>
        /// Gets or sets the temporary folder name.
        /// </summary>
        /// <value>The temporary folder name.</value>
        public string TemporaryFolder
        {
            get { return temporaryFolder; }
            set { temporaryFolder = value; }
        }

        /// <summary>
        /// Gets or sets the archive password. 
        /// In case if you set the password the archive will be encrypted.
        /// </summary>
        /// <value>The archive password.</value>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// Gets or sets the archive comment.
        /// </summary>
        /// <value>The archive comment text.</value>
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        /// <summary>
        /// Gets or sets the name of the archive ZIP file.
        /// </summary>
        /// <value>The name of the archive ZIP file.</value>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

    }
}

//===============================================================================
// Copyright © Serhiy Perevoznyk.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Security.Permissions;


namespace Karna.Compression
{

    /// <summary>
    /// Extract files from ZIP archive to the hard disk or memory.
    /// For creating ZIP archives, adding, updating or moving files to ZIP archive
    /// see <see cref="Karna.Compression.KarnaZip"/> class
    /// </summary>
    public class KarnaUnzip
    {

        private string fileName;
        private string outputFolder;
        private string password;
        private bool useSystemSound;



        /// <summary>
        /// Initializes a new instance of the <see cref="KarnaUnzip"/> class.
        /// </summary>
        public KarnaUnzip()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KarnaUnzip"/> class.
        /// </summary>
        /// <param name="fileName">Name of the zip file.</param>
        public KarnaUnzip(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// Occurs when print message from Info-ZIP engine is received.
        /// Usually the message contains the name of the processed file.
        /// </summary>
        public event EventHandler<CompressionEventArgs> PrintMessage;
        /// <summary>
        /// Occurs when Info-ZIP engine asks the application to play a sound.
        /// This event occurs only when <see cref="UseSystemSound"/> is <c>false</c>,
        /// otherwise the Windows system sound will be used.
        /// </summary>
        public event EventHandler PlaySound;
        /// <summary>
        /// Occurs when the service message from Info-ZIP engine is received.
        /// It provides the application with the name of the name of the archive 
        /// member it has just processed, as well as it's original size.
        /// </summary>
        public event EventHandler<CompressionServiceEventArgs> ServiceMessage;
        /// <summary>
        /// Occurs when the appication message from Info-ZIP engine is received.
        /// Info-ZIP uses this message for displaying information about specific files
        /// in the archive, for example for listing the contents of an archive.
        /// </summary>
        public event EventHandler<CompressionServiceEventArgs> ApplicationMessage;
        /// <summary>
        /// Occurs when file with given name alrady exists on the hard drive.
        /// You can specify what action will be performed. Seee <see cref="FileReplaceOption"/> 
        /// for available options.
        /// </summary>
        public event EventHandler<FileReplaceEventArgs> FileReplace;

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
        /// Raises the <see cref="PlaySound"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnPlaySound(EventArgs e)
        {
            if (PlaySound != null)
                PlaySound(this, e);
            else
            {
                if (useSystemSound)
                {
                    Console.Beep();
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="ServiceMessage"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Karna.Compression.CompressionServiceEventArgs"/> instance containing the event data.</param>
        protected virtual void OnReceiveServiceMessage(CompressionServiceEventArgs e)
        {
            if (ServiceMessage != null)
                ServiceMessage(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ApplicationMessage"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Karna.Compression.CompressionServiceEventArgs"/> instance containing the event data.</param>
        protected virtual void OnReceiveApplicationMessage(CompressionServiceEventArgs e)
        {
            if (ApplicationMessage != null)
                ApplicationMessage(this, e);
        }

        /// <summary>
        /// Raises the <see cref="FileReplace"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Karna.Compression.FileReplaceEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileReplace(FileReplaceEventArgs e)
        {
            if (FileReplace != null)
                FileReplace(this, e);
        }

        #region Callback Functions

        /// <summary>
        /// Processes the sound callback.
        /// </summary>
        internal void ProcessSoundCallback()
        {
            OnPlaySound(EventArgs.Empty);
        }


        /// <summary>
        /// Processes the service message.
        /// </summary>
        /// <param name="msg">The text of the service message.</param>
        /// <param name="x">The lenght of the message</param>
        /// <returns></returns>
        internal int ProcessServiceMessageCallback(ref CallbackString msg, uint x)
        {
            string s = InternalHelper.PCharToString(msg.Buffer);

            CompressionServiceEventArgs e = new CompressionServiceEventArgs(s, (int)x);
            OnReceiveServiceMessage(e);

            return 0;
        }

        /// <summary>
        /// Processes the application message callback.
        /// </summary>
        /// <param name="msg">The text of the application message.</param>
        /// <param name="x">The lenght of the message</param>
        /// <returns></returns>
        internal int ProcessApplicationMessageCallback(ref CallbackString msg, uint x)
        {
            string s = InternalHelper.PCharToString(msg.Buffer);

            CompressionServiceEventArgs e = new CompressionServiceEventArgs(s, (int)x);
            OnReceiveApplicationMessage(e);

            return 0;
        }

        /// <summary>
        /// Processes the password callback.
        /// </summary>
        /// <param name="passwordBuffer">The password buffer.</param>
        /// <param name="n">The lenght of the password buffer.</param>
        /// <param name="textMessage">The text message.</param>
        /// <param name="name">The file name.</param>
        /// <returns></returns>
        internal int ProcessPasswordCallback(IntPtr passwordBuffer, int n, string textMessage, string name)
        {
            if (string.IsNullOrEmpty(password))
                return 1;


            try
            {
                byte[] pwdCopy = new byte[n];

                for (int i = 0; i <= n - 1; i++)
                    pwdCopy[i] = 0x0;

                ASCIIEncoding Ascii = new ASCIIEncoding();
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
        /// Processes the print callback.
        /// </summary>
        /// <param name="msg">The message</param>
        /// <param name="x">The lenght of the message</param>
        /// <returns></returns>
        internal int ProcessPrintCallback(ref CallbackString msg, uint x)
        {
            string s = InternalHelper.PCharToString(msg.Buffer);
            CompressionEventArgs e = new CompressionEventArgs(s);
            OnPrintMessage(e);

            return 0;
        }

        /// <summary>
        /// Processes the replace callback.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <param name="x">The length of the message text.</param>
        /// <returns>
        /// Return code that tells to unzip what to do when the file already exists
        /// </returns>
        internal int ProcessReplaceCallback(ref CallbackString msg, uint x)
        {
            string s = InternalHelper.PCharToString(msg.Buffer);
            FileReplaceEventArgs e = new FileReplaceEventArgs(s, FileReplaceOption.OverwriteFile);
            OnFileReplace(e);
            return InternalHelper.ReplaceOptionToCode(e.Option);
        }

        #endregion

        /// <summary>
        /// Prepares the call back functions.
        /// </summary>
        /// <returns>Unzip callback functions structure</returns>
        internal UnzipUserFunctions PrepareCallBack()
        {
            UnzipUserFunctions unzipUserFunctions = new UnzipUserFunctions();
            unzipUserFunctions.PrintCallbackFunction = new PrintCallbackDelegate(ProcessPrintCallback);
            unzipUserFunctions.ReplaceCallbackFunction = new ServiceCallbackDelegate(ProcessReplaceCallback);
            unzipUserFunctions.SoundCallbackFunction = new SoundCallbackDelegate(ProcessSoundCallback);
            unzipUserFunctions.PasswordCallbackFunction = new PasswordCallbackDelegate(ProcessPasswordCallback);
            unzipUserFunctions.ServiceCallbackFunction = new ServiceCallbackDelegate(ProcessServiceMessageCallback);
            unzipUserFunctions.SendApplicationMessage = new ServiceCallbackDelegate(ProcessApplicationMessageCallback);
            return unzipUserFunctions;
        }

        /// <summary>
        /// Prepares the options and flags.
        /// </summary>
        /// <returns>Unzip options and flags structure</returns>
        internal UnzipOptionsFlags PrepareOptionsFlags()
        {
            UnzipOptionsFlags opts = new UnzipOptionsFlags();
            opts.ExtractOnlyNewer = 0;
            opts.SpaceToUnderscore = 0;
            opts.PromptToOverwrite = 1;
            opts.fQuiet = 0;
            opts.ncflag = 0;
            opts.ntflag = 0;
            opts.nvflag = 0;
            opts.nfflag = 0;
            opts.nzflag = 0;
            opts.ndflag = 1;
            opts.noflag = 1;
            opts.naflag = 0;
            opts.nZIflag = 0;
            opts.C_flag = 1;
            opts.fPrivilege = 0;

            opts.ExtractDir = outputFolder;
            opts.ZipFN = fileName;
            return opts;
        }

        /// <summary>
        /// Extracts one file from the archive to memory.
        /// </summary>
        /// <param name="fileToExtract">The file to extract.</param>
        /// <returns>The byte array with the file content</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public byte[] ExtractToMemory(string fileToExtract)
        {

            UnzipUserFunctions unzipUserFunctions = PrepareCallBack();

            UnzipMemoryBuffer buf = new UnzipMemoryBuffer();

            // note: on successfull memory allocation and initialization,
            // Wiz_UnzipToMemory() returns a Longbool instead of a return code.
            // therefore the return value needs to be translated if < PK_BADERR. 
            UnZipError res = NativeMethods.Wiz_UnzipToMemory(fileName, fileToExtract, ref unzipUserFunctions, ref buf);

            if (res == UnZipError.PK_WARN)
                res = UnZipError.PK_OK;
            else
                if (res == UnZipError.PK_OK)
                    res = UnZipError.PK_ERR;

            if (res != UnZipError.PK_OK)
            {
                try
                {
                    NativeMethods.UzpFreeMemBuffer(ref buf);
                }
                catch
                {
                }
                return null;
            }

            if (buf.TotalSize > 0)
            {
                try
                {
                    byte[] result = new byte[buf.TotalSize];
                    Marshal.Copy(buf.Buffer, result, 0, buf.TotalSize);
                    NativeMethods.UzpFreeMemBuffer(ref buf);
                    return result;
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }

        /// <summary>
        /// Extracts the files from archive to the hard disk.
        /// </summary>
        /// <param name="filesToExtract">The files to extract.</param>
        /// <returns><c>true</c> if files successfully extracted from the archive; otherwise <c>false</c></returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool ExtractFiles(string[] filesToExtract)
        {
            if (filesToExtract == null)
                return false;

            if (filesToExtract.Length == 0)
                return false;

            UnzipOptionsFlags opts = PrepareOptionsFlags();
            UnzipUserFunctions unzipUserFunctions = PrepareCallBack();

            try
            {
                UnZipError ret = NativeMethods.Wiz_SingleEntryUnzip(filesToExtract.Length, filesToExtract, 0, null, ref opts, ref unzipUserFunctions);
                return (ret == UnZipError.PK_OK);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extracts all files from the archive.
        /// </summary>
        /// <returns>Returns <c>true</c> if successful, or <c>false</c> otherwise.</returns>
        /// <example>
        /// <code>
        /// KarnaUnzip unzip = new KarnaUnzip("test.zip");
        /// unzip.Password = "password";
        /// unzip.PrintMessage += new EventHandler&lt;CompressionEventArgs&gt;(zip_PrintMessage);
        /// unzip.ExtractArchive();
        /// </code>
        /// </example>
        public bool ExtractArchive()
        {
            string[] m = { "*.*" };
            return ExtractFiles(m);
        }

        /// <summary>
        /// Gets or sets the output folder.
        /// </summary>
        /// <value>The output folder.</value>
        public string OutputFolder
        {
            get { return outputFolder; }
            set { outputFolder = value; }
        }

        /// <summary>
        /// Gets or sets the name of the zip file.
        /// </summary>
        /// <value>The name of the zip file.</value>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// Gets or sets the archive password. If archive is encrypted you have to provide the
        /// valid password in order to extract files from encrypted archive.
        /// </summary>
        /// <value>The archive password. Leave it empty if archive is not encrypted</value>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the system sound is used
        /// </summary>
        /// <value><c>true</c> if Windows system sound is used; otherwise, <c>false</c>.</value>
        public bool UseSystemSound
        {
            get { return useSystemSound; }
            set { useSystemSound = value; }
        }

    }

}
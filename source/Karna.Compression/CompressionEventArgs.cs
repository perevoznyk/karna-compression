using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Karna.Compression
{
    /// <summary>
    /// Provides data for the <see cref="Karna.Compression.KarnaUnzip.PrintMessage"/> event 
    /// </summary>
    public class CompressionEventArgs : EventArgs
    {
        private readonly string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompressionEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message text.</param>
        public CompressionEventArgs(string message)
        {
            this.message = message;
        }

        /// <summary>
        /// Gets the message text.
        /// </summary>
        /// <value>The message text.</value>
        public string Message
        {
            get { return message; }
        }
    }

    /// <summary>
    /// Provides data for the <see cref="Karna.Compression.KarnaUnzip.FileReplace"/> event
    /// </summary>
    public class FileReplaceEventArgs : EventArgs
    {
        private readonly string message;
        private FileReplaceOption option;

        /// <summary>
        /// Gets or sets the file replace option in case if the file with the same name
        /// already exists on the hard drive. See <see cref="FileReplaceOption"/> for more details.
        /// </summary>
        /// <value>The chosen file replace option.</value>
        public FileReplaceOption Option
        {
            get { return option; }
            set { option = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReplaceEventArgs"/> class.
        /// </summary>
        /// <param name="message">The callback message received from Info-ZIP engine.</param>
        /// <param name="option">The file replace option.</param>
        public FileReplaceEventArgs (string message, FileReplaceOption option)
        {
            this.message = message;
            this.option = option;
        }

        /// <summary>
        /// Gets the text of the callback message.
        /// </summary>
        /// <value>The text of the callback message.</value>
        public string Message
        {
            get { return message; }
        }

        

    }

    /// <summary>
    /// Provides data for <see cref="Karna.Compression.KarnaUnzip.ServiceMessage"/> and <see cref="Karna.Compression.KarnaUnzip.ApplicationMessage"/> events
    /// </summary>
    public class CompressionServiceEventArgs : EventArgs
    {
        private readonly string fileName;
        private readonly int fileSize;

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get { return fileName; }
        }


        /// <summary>
        /// Gets the size of the file.
        /// </summary>
        /// <value>The size of the file.</value>
        public int FileSize
        {
            get { return fileSize; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CompressionServiceEventArgs"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileSize">Size of the file.</param>
        public CompressionServiceEventArgs(string fileName, int fileSize)
        {
            this.fileName = fileName;
            this.fileSize = fileSize;
        }
    }
}

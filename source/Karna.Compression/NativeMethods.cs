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

    #region Delegates

    /// <summary>
    /// Callback function for printing messages
    /// </summary>
    internal delegate int PrintCallbackDelegate(ref CallbackString m, uint size);
    /// <summary>
    /// Service callback function
    /// </summary>
    internal delegate int ServiceCallbackDelegate(ref CallbackString m, uint size);
    /// <summary>
    /// Password callback function
    /// </summary>
    internal delegate int PasswordCallbackDelegate(IntPtr passwordBuffer, int n,  string textmessage,  string name);
    /// <summary>
    /// Comment callback function
    /// </summary>
    internal delegate int CommentCallbackDelegate(ref CallbackString m);
    /// <summary>
    /// Sound callback function
    /// </summary>
    internal delegate void SoundCallbackDelegate();

    #endregion

    /// <summary>
    /// Native methods from zip32.dll and unzip32.dll
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Exctract file(s) from the archive
        /// </summary>
        /// <param name="zipcnt">number of file names being passed. 
        /// If all files are to be extracted, then this can be zero</param>
        /// <param name="zipnames">file names to be unarchived. Wildcard patterns are recognized 
        /// and expanded. If all files are to be extracted, then this can be null</param>
        /// <param name="zipncnt2">number of "file names to be excluded from processing" being
        /// passed. If all files are to be extracted, set this to zero.</param>
        /// <param name="zipnames2">file names to be excluded from the unarchiving process. Wildcard
        /// characters are allowed and expanded. If all files are to be
        /// extracted, set this argument to NULL.</param>
        /// <param name="opts">pointer to a structure with the flags for setting the 
        /// various options, as well as the zip file name.</param>
        /// <param name="zuf">pointer to a structure that contains pointers to functions
        /// in the calling application, as well as sizes passed back to
        /// the calling application etc.</param>
        /// <returns>Error code</returns>
        [DllImport("unzip32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static extern UnZipError Wiz_SingleEntryUnzip(int zipcnt, string[] zipnames, int zipncnt2, string[] zipnames2, ref UnzipOptionsFlags opts, ref UnzipUserFunctions zuf);

        /// <summary>
        /// Unzip file from archive to memory
        /// </summary>
        /// <param name="zip">The name of the zip archive</param>
        /// <param name="file">The name of the file to extract</param>
        /// <param name="zuf">The unzip user functions</param>
        /// <param name="retstr">The umnagaged memory block with a resulting file</param>
        /// <returns>Error code</returns>
        [DllImport("unzip32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static extern UnZipError Wiz_UnzipToMemory(string zip, string file, ref UnzipUserFunctions zuf, ref UnzipMemoryBuffer retstr);

        /// <summary>
        /// Free memory buffer, allocated during the extraction of the file from the archive to memory
        /// </summary>
        /// <param name="buf">The buffer</param>
        [DllImport("unzip32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static extern void UzpFreeMemBuffer(ref UnzipMemoryBuffer buf);

        /// <summary>
        /// Set Zip Archive Function callback
        /// </summary>
        /// <param name="zuf">Zip user functions</param>
        /// <returns>Error code</returns>
        [DllImport("zip32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static extern int ZpInit(ref ZipUserFunctions zuf);

        /// <summary>
        /// set archiving options
        /// </summary>
        /// <param name="zopts">The zip engine options to set</param>
        /// <returns>Error code</returns>
        [DllImport("zip32.dll", SetLastError = true, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static extern ZipError ZpSetOptions(ref ZipOptions zopts);


        /// <summary>
        /// Perform archiving
        /// </summary>
        /// <param name="argc">The number of the arguments</param>
        /// <param name="funame">The archive file name</param>
        /// <param name="zipnames">The list of the files included into archive.</param>
        /// <returns>Error code</returns>
        [DllImport("zip32.dll", SetLastError = true, CallingConvention=CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static extern ZipError ZpArchive(int argc, string funame, string[] zipnames);

    }
}

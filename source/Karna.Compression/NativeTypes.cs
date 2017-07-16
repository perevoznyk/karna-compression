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

namespace Karna.Compression
{

    /// <summary>
    /// File replace codes for unzip replace callback
    /// </summary>
    internal enum FileReplaceCode : int
    {
        /// <summary>
        /// skip this file
        /// </summary>
        IDM_REPLACE_NO = 100,    
        /// <summary>
        /// overwrite this file
        /// </summary>
        IDM_REPLACE_YES = 102,    
        /// <summary>
        /// always overwrite files
        /// </summary>
        IDM_REPLACE_ALL = 103,    
        /// <summary>
        /// never overwrite files
        /// </summary>
        IDM_REPLACE_NONE = 104,    
        /// <summary>
        /// auto rename file
        /// </summary>
        IDM_REPLACE_RENAME = 105   
    };

    /// <summary>
    /// File replace options, supported by KarnaUnzip class
    /// </summary>
    public enum FileReplaceOption
    {
        /// <summary>
        /// Skip the file if exists
        /// </summary>
        SkipFile,
        /// <summary>
        /// Overwrite existsing file
        /// </summary>
        OverwriteFile,
        /// <summary>
        /// Overwrite all files
        /// </summary>
        AlwaysOverwrite,
        /// <summary>
        /// Do not overwrite existing files
        /// </summary>
        NeverOverwrite,
        /// <summary>
        /// Autorename files
        /// </summary>
        AutoRenameFile
    }

    /// <summary>
    /// Unzip functions return codes
    /// </summary>
    internal enum UnZipError
    {
        /// <summary>
        /// no error
        /// </summary>
        PK_OK = 0,
        /// <summary>
        /// no error
        /// </summary>
        PK_COOL = 0,
        /// <summary>
        /// warning error
        /// </summary>
        PK_WARN = 1,
        /// <summary>
        /// error in zipfile
        /// </summary>
        PK_ERR = 2,
        /// <summary>
        /// severe error in zipfile
        /// </summary>
        PK_BADERR = 3,
        /// <summary>
        /// insufficient memory (during initialization)
        /// </summary>
        PK_MEM = 4,
        /// <summary>
        /// insufficient memory (password failure)
        /// </summary>
        PK_MEM2 = 5,
        /// <summary>
        /// insufficient memory (file decompression)
        /// </summary>
        PK_MEM3 = 6,
        /// <summary>
        /// insufficient memory (memory decompression)
        /// </summary>
        PK_MEM4 = 7,
        /// <summary>
        /// insufficient memory (not yet used)
        /// </summary>
        PK_MEM5 = 8,
        /// <summary>
        /// zipfile not found
        /// </summary>
        PK_NOZIP = 9,
        /// <summary>
        /// bad or illegal parameters specified
        /// </summary>
        PK_PARAM = 10,
        /// <summary>
        /// no files found
        /// </summary>
        PK_FIND = 11,
        /// <summary>
        /// disk full
        /// </summary>
        PK_DISK = 50,
        /// <summary>
        /// unexpected EOF
        /// </summary>
        PK_EOF = 51
    }


    /// <summary>
    /// Zip functions return codes
    /// </summary>
    internal enum ZipError
    {
        /// <summary>
        /// used by procname(), zipbare()
        /// </summary>
        ZE_MISS = -1,
        /// <summary>
        /// success
        /// </summary>
        ZE_OK = 0,
        /// <summary>
        /// unexpected end of zip file
        /// </summary>
        ZE_EOF = 2,
        /// <summary>
        /// zip file structure error
        /// </summary>
        ZE_FORM = 3,
        /// <summary>
        /// out of memory
        /// </summary>
        ZE_MEM = 4,
        /// <summary>
        /// internal logic error
        /// </summary>
        ZE_LOGIC = 5,       
        /// <summary>
        /// entry too large to split, read, or write
        /// </summary>
        ZE_BIG = 6,
        /// <summary>
        /// invalid comment format
        /// </summary>
        ZE_NOTE = 7,
        /// <summary>
        /// zip test (-T) failed or out of memory
        /// </summary>
        ZE_TEST = 8,
        /// <summary>
        /// user interrupt or termination
        /// </summary>
        ZE_ABORT = 9,
        /// <summary>
        /// error using a temp file
        /// </summary>
        ZE_TEMP = 10,
        /// <summary>
        /// read or seek error
        /// </summary>
        ZE_READ = 11,      
        /// <summary>
        /// nothing to do
        /// </summary>
        ZE_NONE = 12,
        /// <summary>
        /// missing or empty zip file
        /// </summary>
        ZE_NAME = 13,
        /// <summary>
        /// error writing to a file
        /// </summary>
        ZE_WRITE = 14,      
        /// <summary>
        /// couldn't open to write
        /// </summary>
        ZE_CREAT = 15,
        /// <summary>
        /// bad command line
        /// </summary>
        ZE_PARMS = 16,
        /// <summary>
        /// could not open a specified file to read
        /// </summary>
        ZE_OPEN = 18,
        /// <summary>
        /// error in compilation options
        /// </summary>
        ZE_COMPERR = 19,
        /// <summary>
        /// Zip64 not supported
        /// </summary>
        ZE_ZIP64 = 20      
    }

    /// <summary>
    /// Unzip flags structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct UnzipOptionsFlags
    {
        /// <summary>
        /// extract only newer/new files, without queries
        /// </summary>
        public int ExtractOnlyNewer;
        /// <summary>
        /// true if convert space to underscore
        /// </summary>
        public int SpaceToUnderscore;
        /// <summary>
        /// true if prompt to overwrite is wanted
        /// </summary>
        public int PromptToOverwrite;
        /// <summary>
        /// quiet flag:
        /// 0 = all messages, 1 = few messages, 2 = no messages
        /// </summary>
        public int fQuiet;
        /// <summary>
        /// write to stdout if true
        /// </summary>
        public int ncflag;
        /// <summary>
        /// test zip file
        /// </summary>
        public int ntflag;
        /// <summary>
        /// verbose listing
        /// </summary>
        public int nvflag;
        /// <summary>
        /// "freshen" (replace existing files by newer versions)
        /// </summary>
        public int nfflag;
        /// <summary>
        /// display zip file comment
        /// </summary>
        public int nzflag;
        /// <summary>
        /// controls (sub)directory recreation during extraction
        /// 0 = junk paths from filenames
        /// 1 = "safe" usage of paths in filenames (skip "../")
        /// 2 = allow also unsafe path components (dir traversal)
        /// </summary>
        public int ndflag;
        /// <summary>
        /// true if you are to always overwrite existing files
        /// </summary>
        public int noflag;
        /// <summary>
        /// do end-of-line translation
        /// </summary>
        public int naflag;
        /// <summary>
        /// get ZipInfo if TRUE
        /// </summary>
        public int nZIflag;
        /// <summary>
        /// be case insensitive if TRUE
        /// </summary>
        public int C_flag;
        /// <summary>
        /// 1 => restore ACLs in user mode,
        /// 2 => try to use privileges for restoring ACLs
        /// </summary>
        public int fPrivilege;
        /// <summary>
        /// zip file name
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string ZipFN;
        /// <summary>
        /// directory to extract to. This should be NULL if you
        /// are extracting to the current directory.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string ExtractDir;
    }

    /// <summary>
    /// Unzip user functions
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct UnzipUserFunctions
    {
        /// <summary>
        /// A pointer to the application's print routine
        /// </summary>
        public PrintCallbackDelegate PrintCallbackFunction;
        /// <summary>
        /// a pointer to the application's sound routine. This can be NULL if your application 
        /// doesn't use sound.
        /// </summary>
        public SoundCallbackDelegate SoundCallbackFunction;
        /// <summary>
        /// A pointer to the application's replace routine
        /// </summary>
        public ServiceCallbackDelegate ReplaceCallbackFunction;
        /// <summary>
        /// A pointer to the application's password routine
        /// </summary>
        public PasswordCallbackDelegate PasswordCallbackFunction;
        /// <summary>
        /// A pointer to the application's routine
        /// for displaying information about specific files
        /// in the archive. Used for listing the contents of
        /// an archive.
        /// </summary>
        public ServiceCallbackDelegate SendApplicationMessage;
        /// <summary>
        /// Callback function designed to be used for
        /// allowing the application to process Windows messages,
        /// or canceling the operation, as well as giving the
        /// option of a progress indicator. If this function
        /// returns a non-zero value, then it will terminate
        /// what it is doing. It provides the application with
        /// the name of the name of the archive member it has
        /// just processed, as well as it's original size.
        /// </summary>
        public ServiceCallbackDelegate ServiceCallbackFunction;
        /// <summary>
        /// Value to be filled in by the dll for the
        /// compressed total size of the archive. Note this
        /// value does not include the size of the archive
        /// header and central directory list.
        /// </summary>
        public int TotalSizeComputed;
        /// <summary>
        /// Value to be filled in by the dll for the total
        /// size of all files in the archive.
        /// </summary>
        public int TotalSize;
        /// <summary>
        /// Value to be filled in by the dll for the overall
        /// compression factor. This could actually be computed
        /// from the other values, but it is available.
        /// </summary>
        public int CompressionFactor;
        /// <summary>
        /// Total number of files in the archive
        /// </summary>
        public int MembersNumber;
        /// <summary>
        /// Flag to be set if archive has a comment
        /// </summary>
        public short CommentPresent;

    }

    /// <summary>
    /// Zip User functions
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct ZipUserFunctions
    {
        /// <summary>
        /// Callback ZIP32.DLL Print Function
        /// </summary>
        public PrintCallbackDelegate PrintCallbackFunction;
        /// <summary>
        /// Callback ZIP32.DLL Comment Function
        /// </summary>
        public CommentCallbackDelegate CommentCallbackFunction;
        /// <summary>
        /// Callback ZIP32.DLL Password Function
        /// </summary>
        public PasswordCallbackDelegate PasswordCallbackFunction;
        /// <summary>
        /// Callback ZIP32.DLL Service Function
        /// </summary>
        public ServiceCallbackDelegate ServiceCallbackFunction;
    }

    /// <summary>
    /// Memory buffer for extracting one file from the archive to memory
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct UnzipMemoryBuffer
    {
        /// <summary>
        /// Total size of the extracted file
        /// </summary>
        public int TotalSize;
        /// <summary>
        /// Unmanaged memory buffer where the content of the extracted file is stored
        /// </summary>
        public IntPtr Buffer;
    }


    /// <summary>
    /// Callback string.  This is a byte array 4K long
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct CallbackString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096, ArraySubType = UnmanagedType.U1)]
        public byte[] Buffer;
    }


    /// <summary>
    /// ZPOPT Is Used To Set The Options In The ZIP32.DLL
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ZipOptions
    {
        /// <summary>
        /// US Date (8 Bytes Long) "12/31/98"
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Date;
        /// <summary>
        /// Root Directory Pathname (Up To 256 Bytes Long)
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string szRootDir;
        /// <summary>
        /// Temp Directory Pathname (Up To 256 Bytes Long)
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string szTempDir;
        /// <summary>
        /// 1 If Temp dir Wanted, Else 0
        /// </summary>
        public int fTemp;
        /// <summary>
        /// Include Suffixes (Not Yet Implemented!)
        /// </summary>
        public int fSuffix;
        /// <summary>
        /// 1 If Encryption Wanted, Else 0
        /// </summary>
        public int fEncrypt;
        /// <summary>
        /// 1 To Include System/Hidden Files, Else 0
        /// </summary>
        public int fSystem;
        /// <summary>
        /// 1 If Storing Volume Label, Else 0
        /// </summary>
        public int fVolume;
        /// <summary>
        /// 1 If Excluding Extra Attributes, Else 0
        /// </summary>
        public int fExtra;
        /// <summary>
        /// 1 If Ignoring Directory Entries, Else 0
        /// </summary>
        public int fNoDirEntries;
        /// <summary>
        /// 1 If Excluding Files Earlier Than Specified Date, Else 0 
        /// </summary>
        public int fExcludeDate;
        /// <summary>
        /// 1 If Including Files Earlier Than Specified Date, Else 0
        /// </summary>
        public int fIncludeDate;
        /// <summary>
        /// 1 If Full Messages Wanted, Else 0
        /// </summary>
        public int fVerbose;
        /// <summary>
        /// 1 If Minimum Messages Wanted, Else 0
        /// </summary>
        public int fQuiet;
        /// <summary>
        /// 1 If Translate CR/LF To LF, Else 0
        /// </summary>
        public int fCRLF_LF;
        /// <summary>
        /// 1 If Translate LF To CR/LF, Else 0
        /// </summary>
        public int fLF_CRLF;
        /// <summary>
        /// 1 If Junking Directory Names, Else 0
        /// </summary>
        public int fJunkDir;
        /// <summary>
        /// 1 If Allow Appending To Zip File, Else 0
        /// </summary>
        public int fGrow;
        /// <summary>
        /// 1 If Making Entries Using DOS File Names, Else 0
        /// </summary>
        public int fForce;
        /// <summary>
        /// 1 If Deleting Files Added Or Updated, Else 0
        /// </summary>
        public int fMove;
        /// <summary>
        /// 1 If Files Passed Have To Be Deleted, Else 0
        /// </summary>
        public int fDeleteEntries;
        /// <summary>
        /// 1 If Updating Zip File-Overwrite Only If Newer, Else 0
        /// </summary>
        public int fUpdate;
        /// <summary>
        /// 1 If Freshing Zip File-Overwrite Only, Else 0
        /// </summary>
        public int fFreshen;
        /// <summary>
        /// 1 If Junking SFX Prefix, Else 0
        /// </summary>
        public int fJunkSFX;
        /// <summary>
        /// 1 If Setting Zip File Time To Time Of Latest File In Archive, Else 0
        /// </summary>
        public int fLatestTime;
        /// <summary>
        /// 1 If Putting Comment In Zip File, Else 0
        /// </summary>
        public int fComment;
        /// <summary>
        /// 1 If Updating Archive Offsets For SFX Files, Else 0
        /// </summary>
        public int fOffsets;
        /// <summary>
        /// 1 If Not Saving Privileges, Else 0
        /// </summary>
        public int fPrivilege;
        /// <summary>
        /// Read Only Property!!!
        /// </summary>
        public int fEncryption;
        /// <summary>
        /// 1 (-r), 2 (-R) If Recursing Into Sub-Directories, Else 0
        /// </summary>
        public int fRecurse;
        /// <summary>
        /// 1 = Fix Archive, 2 = Try Harder To Fix, Else 0
        /// </summary>
        public int fRepair;
        /// <summary>
        /// Compression Level - 0 = Stored 6 = Default 9 = Max
        /// </summary>
        public byte fLevel;
    }

    /// <summary>
    /// Compression level. Regulate  the  speed of compression using the specified digit
    /// where -0 indicates no compression (store all  files),  -1  indicates  the  fastest  
    /// compression speed (less compression) and -9 indicates the slowest compression  speed
    /// he default compression level is -6.
    /// </summary>
    public enum CompressionLevel
    {
        /// <summary>
        /// Store only
        /// </summary>
        Level0 = 48,
        /// <summary>
        /// Fastest compression, better performance
        /// </summary>
        Level1,
        /// <summary>
        /// Fast compression
        /// </summary>
        Level2,
        /// <summary>
        /// Fast compression
        /// </summary>
        Level3,
        /// <summary>
        /// Fast compression
        /// </summary>
        Level4,
        /// <summary>
        /// Fast compression
        /// </summary>
        Level5,
        /// <summary>
        /// Default compression level
        /// </summary>
        Level6,
        /// <summary>
        /// Slower compression
        /// </summary>
        Level7,
        /// <summary>
        /// Slow compression, better compression level
        /// </summary>
        Level8,
        /// <summary>
        /// Slowest compression, best compression level
        /// </summary>
        Level9
    };

    /// <summary>
    /// Recurse into subdirectories
    /// </summary>
    public enum Recurse 
    {
        /// <summary>
        /// Do not recurse into subdirectories
        /// </summary>
        None,
        /// <summary>
        /// Travel the directory structure recursively
        /// </summary>
        Level1,
        /// <summary>
        /// Travel  the directory structure recursively starting at the current directory
        /// </summary>
        Level2 
    };

    /// <summary>
    /// Repair archive options
    /// </summary>
    public enum Repair 
    {
        /// <summary>
        /// Do not repair archive
        /// </summary>
        None,
        /// <summary>
        /// Fix the zip archive
        /// </summary>
        Fix,
        /// <summary>
        /// Fix the zip archive if the archive is too damaged or the end has been truncated
        /// </summary>
        DeepFix 
    };

}

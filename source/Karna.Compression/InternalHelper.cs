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

namespace Karna.Compression
{
    /// <summary>
    /// Internal helper functions
    /// </summary>
    internal static class InternalHelper
    {

        /// <summary>
        /// Converts boolean value to integer
        /// </summary>
        /// <param name="b">if set to <c>true</c> [b].</param>
        /// <returns></returns>
        public static int BoolToInt(bool b)
        {
            if (b)
                return 1;
            else
                return 0;
        }


        /// <summary>
        /// Converts C++ pchar to string
        /// </summary>
        /// <param name="ch">The char array representing unmanaged pointer to string</param>
        /// <returns></returns>
        public static string PCharToString(byte[] ch)
        {
            ASCIIEncoding Ascii = new ASCIIEncoding();
            string s = string.Empty;
            int i = 0;

            for (i = 0; i <= ch.Length; i++)
                if (ch[i] == 0) break;
            s = Ascii.GetString(ch, 0, i);

            return s;
        }

        /// <summary>
        /// Convert file replace option to integer code
        /// </summary>
        /// <param name="option">The replace option.</param>
        /// <returns>Integer code</returns>
        public static int ReplaceOptionToCode(FileReplaceOption option)
        {
            int res;
            switch (option)
            {
                case FileReplaceOption.AlwaysOverwrite:
                    res = (int)FileReplaceCode.IDM_REPLACE_ALL;
                    break;
                case FileReplaceOption.AutoRenameFile :
                    res = (int)FileReplaceCode.IDM_REPLACE_RENAME;
                    break;
                case FileReplaceOption.NeverOverwrite:
                    res = (int)FileReplaceCode.IDM_REPLACE_NONE;
                    break;
                case FileReplaceOption.OverwriteFile :
                    res = (int)FileReplaceCode.IDM_REPLACE_YES;
                    break;
                case FileReplaceOption.SkipFile :
                    res = (int)FileReplaceCode.IDM_REPLACE_NO;
                    break;
                default :
                    res = (int)FileReplaceCode.IDM_REPLACE_ALL;
                    break;

            }

            return res;
        }
    }
}

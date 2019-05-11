using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace UtilityLib
{
    public static class ZipHelper
    {
        public static string ZipFolder(string sourcePath, string zipPath)
        {
            string Result = "0";
            try
            {
                FastZip z = new FastZip();

                z.CreateEmptyDirectories = true;
                z.CreateZip(zipPath, sourcePath, true, "");
            }
            catch (Exception ex)
            {
                Result = ex.ToString();
            }

            return Result;
        }
    }
}

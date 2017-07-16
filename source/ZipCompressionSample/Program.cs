//Security Note
//
//This sample code is provided to illustrate a concept and should not be used in applications, 
//as it may not illustrate the safest coding practices. 
//Author assumes no liability for incidental or consequential damages should the sample code be 
//used for purposes other than as intended.


using System;
using System.Collections.Generic;
using System.Text;
using Karna.Compression;

namespace ZipCompressionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] content = { "*.jpg" };
            KarnaZip zip = new KarnaZip();
            zip.PrintMessage += new EventHandler<CompressionEventArgs>(zip_PrintMessage);
            zip.ServiceMessage += new EventHandler<CompressionServiceEventArgs>(zip_ServiceMessage);
            zip.FileName = "test.zip";
            zip.Password = "password";
            zip.Comment = "This is just a test archive";
            zip.AddFiles(content);
            
            //Update file
            //zip.UpdateFiles(content);

            //Move file to the archive
            //zip.MoveFiles(content);

            //Delete file from the archive
            //zip.DeleteFiles(content);

            KarnaUnzip unzip = new KarnaUnzip("test.zip");
            unzip.Password = "password";
            unzip.PrintMessage += new EventHandler<CompressionEventArgs>(zip_PrintMessage);
            unzip.ExtractArchive();
        }

        static void zip_ServiceMessage(object sender, CompressionServiceEventArgs e)
        {
            //Console.WriteLine(e.FileName);
            //Do something here
        }

        static void zip_PrintMessage(object sender, CompressionEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

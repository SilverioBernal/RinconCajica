using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Utilities.AzureStorageFileUploader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["AzureStorageAccount"].ConnectionString;


                //Get a reference to the storage account
                CloudStorageAccount sa = CloudStorageAccount.Parse(connString);
                CloudBlobClient bc = sa.CreateCloudBlobClient();

                string sourceFolder = "";
                string destinyContainer = "";

                for (int i = 0; i < 3; i++)
                {
                    switch (i)
                    {
                        case 0:
                            sourceFolder = ConfigurationManager.AppSettings["SourceFolderUF"];
                            destinyContainer = ConfigurationManager.AppSettings["DestinyContainerUF"];
                            break;
                        case 1:
                            sourceFolder = ConfigurationManager.AppSettings["SourceFolderPC"];
                            destinyContainer = ConfigurationManager.AppSettings["DestinyContainerPC"];
                            break;
                        case 2:
                            sourceFolder = ConfigurationManager.AppSettings["SourceFolderAS"];
                            destinyContainer = ConfigurationManager.AppSettings["DestinyContainerAS"];
                            break;
                        default:
                            break;
                    }

                    //Get a reference to the container
                    CloudBlobContainer container = bc.GetContainerReference(destinyContainer);

                    string[] fileEntries = Directory.GetFiles(sourceFolder);

                    foreach (string filePath in fileEntries)
                    {
                        string fileName = Path.GetFileName(filePath);
                        Console.WriteLine(string.Format("Subiendo el archivo {0}", fileName));

                        CloudBlockBlob cbb = container.GetBlockBlobReference(fileName);

                        using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            cbb.UploadFromStream(fileStream);
                        }                                       
                    }                    
                }
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }        
    }
}

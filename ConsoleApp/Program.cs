using Google.Apis.Drive.v3;
using GoogleDrive;

namespace ConsoleApp
{
    class Program
    {
        static readonly string folderPath = @"C:\Users\vojta\Documents\GoogleSync";
        static async Task Main(string[] args)
        {
            try
            {
                var service = Drive.InitializeDriveService("C:\\Users\\vojta\\source\\repos\\GoogleSync\\GoogleSync\\client_secret.json").Result;

                var folderId = await Folder.GetFolderId(service, "Test");
                Console.WriteLine("Folder ID: " + folderId);

                //var trashFolderId = await Folder.GetFolderId(service, "Trash");
                //Console.WriteLine("Folder ID: " + trashFolderId);

                DateTime date = await Folder.GetLatestModificationDate(service, folderId);
                Console.WriteLine("Latest modification date: " + date);

                //var pageToken =  Drive.GetPageToken(service);
                //Console.WriteLine("Page token: " + pageToken);

                //pageToken = Drive.FetchChanges(service, pageToken);

                //load files in local folder
                var localFiles = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f)).ToList();

                //delete local files
                foreach (var file in localFiles)
                {
                    System.IO.File.Delete(folderPath + "/" + file);
                }

                //download files from Google Drive
                await GoogleDrive.File.DownloadFiles(service, folderId, folderPath);

                localFiles = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f)).ToList();

                foreach (var file in localFiles)
                {
                    await GoogleDrive.File.Update(service, file, folderId);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR]" + e.Message);
            }
        }

    }
}
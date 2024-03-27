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

                DateTime date = await Folder.GetLatestModificationDate(service, folderId);
                Console.WriteLine("Latest modification date: " + date);




                var pageToken =  Drive.GetPageToken(service);
                Console.WriteLine("Page token: " + pageToken);

                pageToken = Drive.FetchChanges(service, pageToken);

            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR]" + e.Message);
            }
        }

    }
}
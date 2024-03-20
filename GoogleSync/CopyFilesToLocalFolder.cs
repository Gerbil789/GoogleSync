using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSync
{
    public class CopyFilesToLocalFolder
    {
        private readonly DriveService _driveService;

        public CopyFilesToLocalFolder(DriveService driveService)
        {
            _driveService = driveService;
        }

        public void Execute(string folderId, string localFolderPath)
        {
            try
            {
                // Define the request parameters
                var listRequest = _driveService.Files.List();
                listRequest.Q = $"'{folderId}' in parents"; // Filter by folder ID
                listRequest.Fields = "files(id, name)";

                // Execute the request
                var result = listRequest.Execute();

                // Check if any files were found
                if (result.Files != null && result.Files.Count > 0)
                {
                    // Iterate through each file
                    foreach (var file in result.Files)
                    {
                        // Check if the file already exists in the local folder
                        var fileName = file.Name;
                        var localFilePath = Path.Combine(localFolderPath, fileName);

                        if (File.Exists(localFilePath))
                        {
                            Console.WriteLine($"File '{fileName}' already exists in the local folder.");
                            continue;
                        }

                        // Download the file from Google Drive
                        var fileId = file.Id;

                        using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                        {
                            _driveService.Files.Get(fileId).Download(fileStream);
                        }

                        Console.WriteLine($"File '{fileName}' downloaded successfully.");
                    }
                }
                else
                {
                    Console.WriteLine("No files found in the Google Drive folder.");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

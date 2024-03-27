using Google.Apis.Drive.v3;

namespace GoogleDrive
{
    public static class File
    {
        public static async Task DownloadFile(DriveService service, string fileId, string localFilePath)
        {
            try
            {
                var request = service.Files.Get(fileId);
                var stream = new MemoryStream();
                await request.DownloadAsync(stream);
                System.IO.File.WriteAllBytes(localFilePath, stream.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR][DOWNLOAD FILE]: " + e.Message);
            }
        }

        public static async Task DownloadFiles(DriveService service, string folderId, string localFilePath)
        {
            try
            {
                var request = service.Files.List();
                request.Q = $"'{folderId}' in parents"; // Filter by parent folder ID
                request.Fields = "files(id)";
                var stream = new MemoryStream();
                foreach (var file in request.Execute().Files)
                {
                    await DownloadFile(service, file.Id, localFilePath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR][DOWNLOAD FILES]: " + e.Message);
            }
        }

        public static async Task Upload(DriveService service, string localFilePath, string folderId)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(localFilePath),
                    Parents = new List<string>() { folderId },
                    Properties = new Dictionary<string, string>()
                    {
                        { "modifiedTime", DateTime.Now.ToString() }
                    }
                };

                using (var stream = new FileStream(localFilePath, FileMode.Open))
                {
                    var request = service.Files.Create(fileMetadata, stream, "application/octet-stream");
                    var response = await request.UploadAsync();

                    if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                        throw response.Exception;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR][UPLOAD FILE]: " + e.Message);
            }
        }

        public static async Task Update(DriveService service, string fileId, string localFilePath)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(localFilePath),
                    Properties = new Dictionary<string, string>()
                    {
                        { "modifiedTime", DateTime.Now.ToString() }
                    }
                };

                using (var stream = new FileStream(localFilePath, FileMode.Open))
                {
                    var request = service.Files.Update(fileMetadata, fileId, stream, "application/octet-stream");
                    var response = await request.UploadAsync();

                    if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                        throw response.Exception;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR][UPDATE FILE]: " + e.Message);
            }
        }
    }
}

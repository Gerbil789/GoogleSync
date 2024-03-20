using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace GoogleSync
{
    public class UploadFile
    {
        private readonly DriveService _service;

        public UploadFile(DriveService service)
        {
            _service = service;
        }
        public async Task Execute(string folderId, string localFilePath)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(localFilePath),
                    Parents = new List<string>() { folderId }
                };

                using (var stream = new FileStream(localFilePath, FileMode.Open))
                {
                    var request = _service.Files.Create(fileMetadata, stream, "application/octet-stream");
                    var response = await request.UploadAsync();

                    if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                        throw response.Exception;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred in UploadFile: " + e.Message);
            }
            
        }
    }
}

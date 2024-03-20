using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Drive.v3;

namespace GoogleSync
{
    public class UpdateFile
    {
        private readonly DriveService _service;

        public UpdateFile(DriveService service)
        {
            _service = service;
        }
        public async Task Execute(string fileId, string localFilePath)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(localFilePath)
                };
                FilesResource.UpdateMediaUpload request;
                using (var stream = new FileStream(localFilePath, FileMode.Open))
                {
                    request = _service.Files.Update(fileMetadata, fileId, stream, "application/octet-stream");
                    request.Fields = "id";
                    await request.UploadAsync();
                }
                var file = request.ResponseBody;
                Console.WriteLine("File ID: " + fileId);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred in UpdateFile: " + e.Message);
            }
            
        }
    }
}

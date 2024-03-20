using Google.Apis.Drive.v3;

namespace GoogleSync
{
    public class DownloadFile
    {
        private readonly DriveService _service;

        public DownloadFile(DriveService service)
        {
            _service = service;
        }
        public async Task Execute(string fileId, string localFilePath)
        {
            try
            {
                var request = _service.Files.Get(fileId);
                var stream = new MemoryStream();
                await request.DownloadAsync(stream);
                File.WriteAllBytes(localFilePath, stream.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred in DownloadFile: " + e.Message);
            }
        }
    }
}

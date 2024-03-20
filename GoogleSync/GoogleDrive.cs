using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GoogleSync
{
    //1082463448755-fuqfcrsf2hk2rermvjcda87uuhfbuvef.apps.googleusercontent.com
    public class GoogleDrive
    {
        string[] Scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile };
        private readonly DriveService _service;

        public GoogleDrive(string credentialsFilePath)
        {
            UserCredential credential;
            using (var stream = new FileStream(credentialsFilePath, FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, false)).Result;

                Console.WriteLine("Credential file saved to: " + credPath);
            }

            _service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        }

        public DriveService GetDriveService()
        {
            return _service;
        }

        public string GetFolderId(string folderName)
        {
            return new GetFolderId(_service).Execute(folderName);
        }

        public void CopyFilesToLocalFolder(string folderId, string localFolderPath)
        {
            new CopyFilesToLocalFolder(_service).Execute(folderId, localFolderPath);
        }

        public async Task Synchronize(string folderId, string localFolderPath)
        {
            FilesResource.ListRequest listRequest = _service.Files.List();
            listRequest.Q = $"'{folderId}' in parents";
            listRequest.Fields = "files(id, name, parents, modifiedTime, trashed)";
            var driveFiles = listRequest.Execute().Files;


            //sync drive files to local files
            foreach(var file in driveFiles)
            {
                var localFilePath = Path.Combine(localFolderPath, file.Name);
                if (System.IO.File.Exists(localFilePath))
                {
                    var lastModifiedLocal = Directory.GetLastWriteTime(localFilePath);
                    var lastModifiedDrive = file.ModifiedTime;

                    if (lastModifiedLocal < lastModifiedDrive)
                    {
                        Console.WriteLine($"File {file.Name} is newer on Google Drive. Downloading...");
                        await new DownloadFile(_service).Execute(file.Id, localFilePath);
                    }
                    else if (lastModifiedLocal > lastModifiedDrive)
                    {
                        Console.WriteLine($"File {file.Name} is newer locally. Updating...");
                        await new UpdateFile(_service).Execute(file.Id, localFilePath);
                    }
                }
                else
                {
                    Console.WriteLine($"File {file.Name} does not exist locally. Downloading...");
                    await new DownloadFile(_service).Execute(file.Id, localFilePath);
                }
            }

            //sync local files to drive files
            string[] localFiles = Directory.GetFiles(localFolderPath);
            foreach (var localFile in localFiles)
            {
                var fileName = Path.GetFileName(localFile);
                var driveFile = driveFiles.FirstOrDefault(f => f.Name == fileName);
                if (driveFile == null)
                {
                    Console.WriteLine($"File {fileName} does not exist on Google Drive. Uploading...");
                    await new UploadFile(_service).Execute(folderId, localFile);
                }
            }

        }
    }

}

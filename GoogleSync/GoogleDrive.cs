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

        public async Task Synchronize(string folderId, string localFolderPath)
        {
            await new Synchronize(_service).Execute(folderId, localFolderPath);
        }
        

           
    }

}

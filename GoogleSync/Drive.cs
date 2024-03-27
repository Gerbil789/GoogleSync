using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleDrive
{
    //1082463448755-fuqfcrsf2hk2rermvjcda87uuhfbuvef.apps.googleusercontent.com
    public static class Drive
    {
        public static Task<DriveService> InitializeDriveService(string credentialsFilePath)
        {
            try
            {
                string[] Scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile };

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

                DriveService service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });

                return Task.FromResult(service);
             
            }
            catch(Exception e)
            {
                Console.WriteLine("[ERROR][INITIALIZE DRIVE SERVICE]: " + e.Message);
                return null;
            }
        }

        public static string GetPageToken(DriveService service)
        {
            try
            {
                var res = service.Changes.GetStartPageToken().Execute();
                return res.StartPageTokenValue;
            }
            catch(Exception e)
            {
                Console.WriteLine("[ERROR][GET PAGE TOKEN]: " + e.Message);
                return null;
            }
        }

        public static string FetchChanges(DriveService service, string savedStartPageToken)
        {
            try
            {
                string pageToken = savedStartPageToken;
                while (pageToken != null)
                {
                    var request = service.Changes.List(pageToken);
                    request.Spaces = "drive";
                    //request.RestrictToMyDrive = true;
                    request.IncludeRemoved = true;
                    var changes = request.Execute();
                    foreach (var change in changes.Changes)
                    {
                        // Process change
                        Console.WriteLine("Change found for file: " + change.FileId);
                    }

                    if (changes.NewStartPageToken != null)
                    {
                        // Last page, save this token for the next polling interval
                        savedStartPageToken = changes.NewStartPageToken;
                    }
                    pageToken = changes.NextPageToken;
                }
                return savedStartPageToken;
            }
            catch (Exception e)
            {
                if (e is AggregateException)
                {
                    Console.WriteLine("[ERROR][GET CHANGES]: Credential Not found");
                }
                else
                {
                    Console.WriteLine("[ERROR][GET CHANGES]: " + e.Message);
                    throw;
                }
            }
            return null;
        }
    }

}

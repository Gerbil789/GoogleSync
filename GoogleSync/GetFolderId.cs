using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSync
{
    public class GetFolderId
    {
        private readonly DriveService _service;

        public GetFolderId(DriveService service)
        {
            _service = service;
        }
        public string Execute(string folderName)
        {
            // Define the request parameters
            var request = _service.Files.List();
            request.Q = $"mimeType='application/vnd.google-apps.folder' and name='{folderName}'"; // Filter by folder name
            request.Fields = "files(id)";

            // Execute the request
            var result = request.Execute();

            if (result.Files != null && result.Files.Count > 0)
            {
                // Return the ID of the first matching folder
                return result.Files[0].Id;
            }
            else
            {
                return null;
            }
        }
    }
}

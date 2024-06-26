﻿using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive
{
    public static class Folder
    {
        public static async Task<string?> GetFolderId(DriveService service, string folderName)
        {
            try
            {
                var request = service.Files.List();
                request.Q = $"mimeType='application/vnd.google-apps.folder' and name='{folderName}'"; // Filter by folder name
                request.Fields = "files(id)";

                // Execute the request
                var result = await request.ExecuteAsync();

                if (result.Files != null && result.Files.Count > 0)
                {
                    // Return the ID of the first matching folder
                    return result.Files[0].Id;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR][GET FOLDER ID]: " + e.Message);
                return null;
            }
        }

        public static async Task<DateTime> GetLatestModificationDate(DriveService service, string folderId)
        {
            try
            {
                FilesResource.ListRequest request = service.Files.List();
                request.Q = $"'{folderId}' in parents";
                request.Fields = "files(id, name, parents, modifiedTime, trashed, sha1Checksum, createdTime, trashedTime, capabilities, properties)";
                request.PageSize = 100; // Adjust as needed
                request.OrderBy = "modifiedTime desc";

                var response = await request.ExecuteAsync();

                var trashedfiles = response.Files.Where(f => f.Trashed ?? false).ToList();

                if (!string.IsNullOrEmpty(response.NextPageToken))
                {
                    Console.WriteLine("[WARNING][GET LATEST MODIFICATION DATE]: There are more than 100 files in the folder. Only the first 100 files are considered.");
                }

                var latestCreationDate = response.Files.Max(f => f.CreatedTimeDateTimeOffset?.DateTime) ?? DateTime.MinValue;
                var latestModificationDate = response.Files.Max(f => f.ModifiedTimeDateTimeOffset?.DateTime) ?? DateTime.MinValue;
                var latestTrashedDate = response.Files.Max(f => f.TrashedTimeDateTimeOffset?.DateTime) ?? DateTime.MinValue; // <-- [FIX] RETURN NULL ALWAYS!!! 

                var latestChangeTime = DateTime.MinValue;
                if(latestChangeTime < latestCreationDate) latestChangeTime = latestCreationDate;
                if(latestChangeTime < latestModificationDate) latestChangeTime = latestModificationDate;
                if(latestChangeTime < latestTrashedDate) latestChangeTime = latestTrashedDate;

                return latestChangeTime;
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR][GET LATEST MODIFICATION DATE]: " + e.Message);
                return DateTime.MinValue;
            }

            
        }

    }
}

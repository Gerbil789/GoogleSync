using Google.Apis.Drive.v3;
using System.IO;

using System.Security.Cryptography;
using System;

namespace GoogleDrive
{
    public class Synchronize
    {
        private readonly DriveService _service;

        public Synchronize(DriveService service)
        {
            _service = service;
        }

        //public async Task Execute(string folderId, string localFolderPath)
        //{
        //    try
        //    {
        //        FilesResource.ListRequest listRequest = _service.Files.List();
        //        listRequest.Q = $"'{folderId}' in parents";
        //        listRequest.Fields = "files(id, name, parents, modifiedTime, trashed, sha1Checksum)";
        //        var driveFiles = listRequest.Execute().Files;
        //        var localFiles = Directory.GetFiles(localFolderPath).Select(f => Path.GetFileName(f)).ToList();

        //        //UPLOAD
        //        var filesToUpload = localFiles.Except(driveFiles.Select(f => f.Name)).ToList();

        //        //DOWNLOAD
        //        var filesToDownload = driveFiles.Where(f => (!f.Trashed ?? false) && !localFiles.Contains(f.Name)).Select(f => f.Name).ToList();

        //        //UPDATE
        //        var filesToUpdateOnDrive = new List<string>();
        //        var filesToUpdateOnLocally = new List<string>();
        //        foreach (var driveFile in driveFiles)
        //        {
        //            var localFilePath = Path.Combine(localFolderPath, driveFile.Name);
        //            if (File.Exists(localFilePath))
        //            {
        //                var localModifiedTime = File.GetLastWriteTime(localFilePath);
        //                var driveModifiedTime = driveFile.ModifiedTime ?? DateTime.MinValue;

        //                if (localModifiedTime > driveModifiedTime)
        //                {
        //                    // Change occurred locally
                            
        //                    var localFileHash = CalculateSHA1Checksum(localFilePath);
        //                    if (localFileHash != driveFile.Sha1Checksum)
        //                    {
        //                        filesToUpdateOnDrive.Add(driveFile.Name);
        //                    }
        //                    // Otherwise, local file was updated but content is the same, no action needed
        //                }
        //                else if (localModifiedTime < driveModifiedTime)
        //                {
        //                    // Change occurred on Drive, download the updated file
        //                    var localFileHash = CalculateSHA1Checksum(localFilePath);
        //                    if (localFileHash != driveFile.Sha1Checksum)
        //                    {
        //                        filesToUpdateOnLocally.Add(driveFile.Name);
        //                    }
                            
        //                }
        //            }
        //        }



        //        //DELETE
        //        var filesToDeleteLocally = driveFiles.Where(f => f.Trashed == true && localFiles.Contains(f.Name)).Select(f => f.Name).ToList();

        //        //log counts
        //        Console.WriteLine($"Files to upload: {filesToUpload.Count}");
        //        Console.WriteLine($"Files to download: {filesToDownload.Count}");
        //        Console.WriteLine($"Files to update on Drive: {filesToUpdateOnDrive.Count}");
        //        Console.WriteLine($"Files to update locally: {filesToUpdateOnLocally.Count}");
        //        Console.WriteLine($"Files to delete locally: {filesToDeleteLocally.Count}");

        //        foreach (var file in filesToUpload)
        //        {
        //            var localFilePath = Path.Combine(localFolderPath, file);
        //            Console.WriteLine($"File {file} does not exist on Google Drive. Uploading...");
        //            await new UploadFile(_service).Execute(folderId, localFilePath);
        //        }

        //        foreach (var file in filesToDownload)
        //        {
        //            var driveFile = driveFiles.First(f => f.Name == file);
        //            var localFilePath = Path.Combine(localFolderPath, file);
        //            Console.WriteLine($"File {file} is newer on Google Drive. Downloading...");
        //            await new DownloadFile(_service).Execute(driveFile.Id, localFilePath);
        //        }

        //        foreach (var file in filesToUpdateOnDrive)
        //        {
        //            var localFilePath = Path.Combine(localFolderPath, file);
        //            Console.WriteLine($"File {file} is newer locally. Updating...");
        //            await new UpdateFile(_service).Execute(driveFiles.First(f => f.Name == file).Id, localFilePath);
        //        }

        //        foreach (var file in filesToUpdateOnLocally)
        //        {
        //            var driveFile = driveFiles.First(f => f.Name == file);
        //            var localFilePath = Path.Combine(localFolderPath, file);
        //            Console.WriteLine($"File {file} is newer on Google Drive. Downloading...");
        //            await new DownloadFile(_service).Execute(driveFile.Id, localFilePath);
        //        }

        //        foreach (var file in filesToDeleteLocally)
        //        {
        //            var localFilePath = Path.Combine(localFolderPath, file);
        //            Console.WriteLine($"File {file} is trashed on Google Drive. Deleting locally...");
        //            File.Delete(localFilePath);
        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("An error occurred in Synchronize: " + e.Message);
        //    }
        //}

        //static string CalculateSHA1Checksum(string filePath)
        //{
        //    using (var stream = File.OpenRead(filePath))
        //    {
        //        using (var sha1 = SHA1.Create())
        //        {
        //            byte[] checksum = sha1.ComputeHash(stream);
        //            return BitConverter.ToString(checksum).Replace("-", "").ToLowerInvariant();
        //        }
        //    }
        //}

    }
}

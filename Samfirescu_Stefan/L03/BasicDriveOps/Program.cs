using System;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using System.Collections.Generic;


namespace L03
{
    class Program
    {
        private static DriveService _service;

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Drive API .NET Quickstart";

        private static string _token;

        static void Main(string[] args)
        {
            Initialize();
        }

        static void Initialize()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("./credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

             // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            GetAllFiles(credential, service);
            UploadTxtFile(service);
        }

        static void GetAllFiles(UserCredential credential, DriveService service)
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 100;
            listRequest.Fields = "nextPageToken, files(id, name)";
            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
            // end
            Console.Read();
        }
        static async void UploadTxtFile(DriveService service)
        {
            string filepath = @"C:\Users\sstef\Documents\Faculta\datc\laborator\Samfirescu_Stefan\L03\BasicDriveOps\text.txt";
            if (System.IO.File.Exists(filepath.ToString()))
            {
                using var uploadStream = System.IO.File.OpenRead(filepath);

                // Open the file to read from.
                using (StreamReader sr = File.OpenText(filepath))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }

                // Create the File resource to upload.
                Google.Apis.Drive.v3.Data.File driveFile = new Google.Apis.Drive.v3.Data.File
                {
                    Name = "text.txt"
                };
                // Get the media upload request object.
                FilesResource.CreateMediaUpload insertRequest = service.Files.Create(
                    driveFile, uploadStream, "text/plain");

                Console.WriteLine(insertRequest.Body.Id);

                // Add handlers which will be notified on progress changes and upload completion.
                // Notification of progress changed will be invoked when the upload was started,
                // on each upload chunk, and on success or failure.
                insertRequest.ProgressChanged += Upload_ProgressChanged;
                insertRequest.ResponseReceived += Upload_ResponseReceived;

                await insertRequest.UploadAsync();
            }
        }
        static void Upload_ProgressChanged(Google.Apis.Upload.IUploadProgress progress) =>
                    Console.WriteLine(progress.Status + " " + progress.BytesSent);

        static void Upload_ResponseReceived(Google.Apis.Drive.v3.Data.File file) =>
                Console.WriteLine(file.Name + " was uploaded successfully");
    }
}
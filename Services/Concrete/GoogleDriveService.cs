using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace EduSat.TestSeries.Service.Services.Concrete
{
    public static class GoogleDriveService
    {
        private static string[] Scopes = { DriveService.Scope.DriveFile };
        private static string ApplicationName = "EduSat";

        private static DriveService GetDriveService()
        {
            GoogleCredential credential;

            // Load the service account key from the JSON file
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(DriveService.Scope.DriveFile);
            }

            // Create the Drive API service
            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        public static string UploadFile(string filePath, string mimeType, string name)
        {
            var service = GetDriveService();

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = $"{name}_{DateTime.Now.ToString("yyyyMMddHHmmss")}_{Guid.NewGuid()}",
            };

            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, mimeType);
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;
            return file.Id;
        }

        public static string GetShareableLink(string fileId)
        {
            var service = GetDriveService();

            var permission = new Google.Apis.Drive.v3.Data.Permission()
            {
                Role = "reader",
                Type = "anyone"
            };

            service.Permissions.Create(permission, fileId).Execute();

            return $"https://drive.google.com/uc?export=download&id={fileId}";
        }
    }
}

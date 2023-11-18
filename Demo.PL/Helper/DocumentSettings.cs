namespace Demo.PL.Helper
{
    public static class DocumentSettings
    {
        // Upload
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Located Folder Path 
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", folderName);

            // 2. Get File Name and Make it Unique
            var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(file.FileName)}";

            // 3. Get File Path [FolderPath + FileName]
            var filePath = Path.Combine(folderPath, fileName);

            // 4. Save File As Stream
            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            
            // 5. Return File Name
            return fileName;
        }

        // Delete
        public static void DeleteFile(string fileName, string folderName)
        {
            // 1. Get File Path
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", folderName, fileName);

            // 2. Check if FIle Exit Or No
            if (File.Exists(FilePath))
            {
                // If Exists Remove It
                File.Delete(FilePath);
            }

        }
    }
}

namespace BizLand.Utilities.Extentions
{
    public static class FileValidator
    {
        public static bool IsValid(this IFormFile file, string fileFormat = "image/")
        {
            if (file.ContentType.Contains(fileFormat)) return true;
            return false;
        }
        public static bool LimitSize(this IFormFile file, int limitSize = 10)
        {
            if (file.Length <= limitSize * 1024 * 1024) return true;
            return false;
        }
        public static string GetGuidName(string fullFileNAme)
        {
            int score = fullFileNAme.LastIndexOf("_");
            if(score > 0) return fullFileNAme.Substring(0, score);
            return fullFileNAme;
        }
        public static string GetFileFormat(string fullFileNAme)
        {
            int score = fullFileNAme.LastIndexOf(".");
            if (score > 0) return fullFileNAme.Substring(score);
            return fullFileNAme;
        }
        public static async Task<string> CreateFileAsync(this IFormFile file, string root, params string[] folders)
        {
            string orginalName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string finalName = GetGuidName(orginalName) + GetFileFormat(orginalName);

            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, finalName);

            using (FileStream stream = new FileStream (path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return finalName;
        }
        public static async void DeleteFileAsync(this string finalName, string root, params string[] folders)
        {
            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, finalName);

            if (File.Exists(path)) File.Delete(path);
        }
    }
}

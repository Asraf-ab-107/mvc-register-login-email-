using Pronia_Tekrar_1.Model;

namespace Pronia_Tekrar_1.Helpers
{
    public static class FileExtension
    {
        public static string Upload(this IFormFile file, string rootpath, string Foldername)
        {
            string filename = file.FileName;
            if (filename.Length > 64)
            {
                filename = filename.Substring(filename.Length - 64, 64);
            }
            filename = Guid.NewGuid() + filename;

            string path = Path.Combine(rootpath, Foldername, filename);

            using (FileStream Stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(Stream);
            }
            return filename;
        }

        public static bool DeleteFile(string rootpath, string foldername, string filename)
        {
            string path = Path.Combine(rootpath, foldername, filename);
            if (!File.Exists(path))
            {
                return false;
            }
            File.Delete(path);
            return true;
        }

    }
}

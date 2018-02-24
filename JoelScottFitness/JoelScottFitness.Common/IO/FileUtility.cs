using log4net;
using System;
using System.IO;
using System.Web;

namespace JoelScottFitness.Common.IO
{
    public static class FileUtility
    {
        private static ILog logger = LogManager.GetLogger(typeof(FileUtility));

        public static bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool CreateDirectory(string path)
        {
            bool success = false;
            try
            {
                Directory.CreateDirectory(path);
                success = true;
            }
            catch (Exception ex)
            {
                logger.Warn($"Failed to create directory '{path}', error details: '{ex.Message}'.");
            }

            return success;
        }

        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public static UploadResult UploadFile(HttpPostedFileBase file, string directory, string name = null)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException("String is null or empty", nameof(directory));

            UploadResult uploadResult = new UploadResult();
            try
            {
                string path = HttpContext.Current.Server.MapPath($"~/{directory}/");
                if (!DirectoryExists(path))
                {
                    if (!CreateDirectory(path))
                    {
                        throw new Exception($"Failed to create directory '{path}'.");
                    }
                }

                string fileName = !string.IsNullOrEmpty(name)
                                    ? name
                                    : GetFileName(file.FileName);

                file.SaveAs(path + fileName);
                uploadResult.Success = true;
                uploadResult.UploadPath = $"/{directory.Trim('/')}/{fileName.Trim('/')}";
            }
            catch (Exception ex)
            {
                logger.Warn($"An exception occured attempting to upload a file, error details: '{ex.Message}'.");
            }

            return uploadResult;
        }

        public static string MapPath(string file)
        {
            return HttpContext.Current.Server.MapPath(file);
        }
    }
}

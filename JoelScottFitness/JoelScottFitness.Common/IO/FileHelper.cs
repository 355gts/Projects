using System.Web;

namespace JoelScottFitness.Common.IO
{
    public class FileHelper : IFileHelper
    {
        public bool FileExists(string fileName)
        {
            return FileUtility.FileExists(fileName);
        }

        public bool DirectoryExists(string path)
        {
            return FileUtility.DirectoryExists(path);
        }

        public bool CreateDirectory(string path)
        {
            return FileUtility.CreateDirectory(path);
        }

        public string GetFileName(string path)
        {
            return FileUtility.GetFileName(path);
        }

        public UploadResult UploadFile(HttpPostedFileBase postedFile, string directory, string name = null)
        {
            return FileUtility.UploadFile(postedFile, directory, name);
        }
    }
}

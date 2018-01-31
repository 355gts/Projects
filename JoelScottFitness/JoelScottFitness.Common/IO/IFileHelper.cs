using System.Web;

namespace JoelScottFitness.Common.IO
{
    public interface IFileHelper
    {
        bool CreateDirectory(string path);
        bool DirectoryExists(string path);
        bool FileExists(string fileName);
        string GetFileName(string path);
        UploadResult UploadFile(HttpPostedFileBase postedFile, string directory, string name = null);
    }
}
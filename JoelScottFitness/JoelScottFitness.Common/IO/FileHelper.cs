using System.IO;

namespace JoelScottFitness.Common.IO
{
    public class FileHelper : IFileHelper
    {
        public bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }
    }
}

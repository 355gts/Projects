using JoelScottFitness.Common.IO;
using Ninject.Modules;

namespace JoelScottFitness.Common.Modules
{
    public class CommonModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileHelper>().To<FileHelper>();
        }
    }
}

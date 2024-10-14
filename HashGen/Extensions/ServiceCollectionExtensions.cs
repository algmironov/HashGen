using HashGen.Services;
using HashGen.ViewModels;

using Microsoft.Extensions.DependencyInjection;

namespace HashGen.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddTransient<IFileService, FileService>();
            collection.AddTransient<MainViewModel>();
        }
    }
}

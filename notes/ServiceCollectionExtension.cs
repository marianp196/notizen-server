using System;
using Microsoft.Extensions.DependencyInjection;

namespace notizen_web_api.notes
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddNotesCached(this IServiceCollection serviceCollection, string basePath, TimeSpan cachingTime)
        {
            serviceCollection.AddSingleton<FilterNoteService>();
            serviceCollection.AddSingleton(typeof(INotesService), sp => {
                var filter = sp.GetService<FilterNoteService>();
                var fileNoteService = new FileNoteService(filter, basePath, ".mcpnote");
                return new CashedNoteService(fileNoteService, filter, cachingTime);
            });

            return serviceCollection;
        }
    }
}
using Microsoft.AspNetCore.Builder;

namespace Codity.WebApi.ExtensionMethods
{
    public static class ApplicationBuilderExtensionMethods
    {
        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CODITY API V1");
            });
        }
    }
}

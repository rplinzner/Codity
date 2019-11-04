using Microsoft.AspNetCore.Builder;

namespace Twitter.WebApi.ExtensionMethods
{
    public static class ApplicationBuilderExtensionMethods
    {
        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NOT A TWITTER API V1");
            });
        }
    }
}

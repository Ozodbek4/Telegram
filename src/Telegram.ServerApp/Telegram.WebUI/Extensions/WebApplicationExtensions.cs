using Telegram.WebUI.Hubs;

namespace Telegram.WebUI.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseWebApplicationMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHub<ChatHub>("chat-hub");

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors("AllowAllOrigins");

        app.UseWebSockets();

        app.MapControllers();

        return app;
    }
}
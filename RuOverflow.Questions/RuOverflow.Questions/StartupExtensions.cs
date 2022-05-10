using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace RuOverflow.Questions;

public class StartupExtensions
{
    public static void ConfigureAuthWithGraphQl(IRequestExecutorBuilder builder)
    {
        builder.AddAuthorization();
        builder.AddHttpRequestInterceptor(async (ctx, executor, builder, ct) =>
        {
            var authenticateResult = await ctx.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (authenticateResult.Succeeded)
            {
                ctx.User = authenticateResult.Principal;
            }
        });
    }
}

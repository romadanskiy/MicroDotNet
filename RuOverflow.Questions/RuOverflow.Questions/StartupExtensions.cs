using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using RuOverflow.Questions.EF;


namespace RuOverflow.Questions;

public static class StartupExtensions
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
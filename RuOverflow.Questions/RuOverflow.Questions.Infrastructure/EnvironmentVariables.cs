﻿namespace RuOverflow.Questions.Infrastructure;

public static class EnvironmentVariables
{
    public static string ConnectionString =
        Environment.GetEnvironmentVariable("RuOverFlow_Question_ConnectionString") ??
        throw new SystemException("Environment does not contains connection string variable");
}

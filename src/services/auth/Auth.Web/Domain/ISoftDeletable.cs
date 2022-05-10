using System;

namespace AuthorizationServer.Web.Domain;

public interface ISoftDeletable
{
    DateOnly? DeletionDateTime { get; }

    void SoftDelete();
}

public static class Dates
{
    public static DateOnly Now() => DateOnly.FromDateTime(DateTime.Now);
}
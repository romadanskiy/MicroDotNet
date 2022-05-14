using System.Collections.Concurrent;
using Background.Services.QuestionService.Models;

namespace Background.Services.QuestionService
{
    public static class QuestionStore
    {
        public static readonly ConcurrentDictionary<Guid, Question> Questions = new();
    }
}

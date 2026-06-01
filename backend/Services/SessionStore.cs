using System.Collections.Concurrent;
using InterviewCoach.Models;

namespace InterviewCoach.Services;

/// <summary>
/// Thread-safe in-memory store for active interview sessions.
/// In production, swap this for Redis or a DB-backed store.
/// </summary>
public class SessionStore
{
    private readonly ConcurrentDictionary<string, InterviewSession> _sessions = new();

    public InterviewSession Create(string resume, string jd, string candidateName)
    {
        var session = new InterviewSession
        {
            Resume = resume,
            JobDescription = jd,
            CandidateName = candidateName
        };
        _sessions[session.Id] = session;
        return session;
    }

    public InterviewSession? Get(string id) =>
        _sessions.TryGetValue(id, out var s) ? s : null;

    public void Remove(string id) => _sessions.TryRemove(id, out _);

    // Clean sessions older than 1 hour (call via background service in prod)
    public void Evict() =>
        _sessions
            .Where(kv => kv.Value.CreatedAt < DateTime.UtcNow.AddHours(-1))
            .ToList()
            .ForEach(kv => _sessions.TryRemove(kv.Key, out _));
}

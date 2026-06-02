# Interview Coach — Backend

This is the backend API for Interview Coach, a mock interview service that uses the Google Generative Language (Gemini) API to run simulated technical interviews.

Features
- Start an interview by providing a resume and job description (plain text or PDF).
- Submit answers and receive feedback plus the next question.
- List available Gemini models (raw JSON) for inspection.

Getting started
1. Configure your Gemini API key in appsettings.json:

```json
{
  "Gemini": {
	"ApiKey": "YOUR_API_KEY"
  }
}
```

2. Run the API:

   dotnet run

API Endpoints
- POST /api/interview/start — start a session (JSON body: Resume, JobDescription, CandidateName)
- POST /api/interview/start-form — start a session using multipart/form-data to upload PDFs
- POST /api/interview/answer — submit an answer (JSON body: SessionId, Answer)
- DELETE /api/interview/{sessionId} — abandon a session
- GET /api/interview/models — list models from the Generative Language API

Development notes
- The service uses an in-memory SessionStore. Replace with Redis or a DB for production.
- The Gemini-related DTOs live in Models/Models.cs.
- Swagger UI is available in development at /swagger.

License
MIT

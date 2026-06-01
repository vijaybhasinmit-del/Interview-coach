using InterviewCoach.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<ClaudeService>();
builder.Services.AddSingleton<SessionStore>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // Redirect root to Swagger UI so visiting the base URL doesn't return 404
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.UseCors("VueFrontend");
app.UseAuthorization();
app.MapControllers();
app.Run();

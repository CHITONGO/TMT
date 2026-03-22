using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI
builder.Services.AddOpenApi();

// đăng ký Neo4j service
builder.Services.AddSingleton<Neo4jService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing","Bracing","Chilly","Cool","Mild","Warm","Balmy","Hot","Sweltering","Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )).ToArray();

    return forecast;
})
.WithName("GetWeatherForecast");


// ==========================
// API Neo4j
// ==========================

// // tạo user
// app.MapPost("/users", async (string name, Neo4jService neo4j) =>
// {
//     await neo4j.CreateUser(name);
//     return Results.Ok("User created");
// });

// // lấy danh sách user
// app.MapGet("/users", async (Neo4jService neo4j) =>
// {
//     var users = await neo4j.GetUsers();
//     return Results.Ok(users);
// });

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
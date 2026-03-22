using Neo4j.Driver;

public class Neo4jService
{
    private readonly IDriver _driver;

    public Neo4jService(IConfiguration config)
    {
        var uri = config["Neo4j:Uri"];
        var user = config["Neo4j:User"];
        var password = config["Neo4j:Password"];

        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
    }

    public async Task CreateUser(string name)
    {
        var session = _driver.AsyncSession();

        var query = "CREATE (u:User {name:$name}) RETURN u";

        await session.RunAsync(query, new { name });

        await session.CloseAsync();
    }

    public async Task<List<string>> GetUsers()
    {
        var session = _driver.AsyncSession();

        var result = await session.RunAsync(
            "MATCH (u:User) RETURN u.name AS name"
        );

        var users = new List<string>();

        await result.ForEachAsync(record =>
        {
            users.Add(record["name"].As<string>());
        });

        await session.CloseAsync();

        return users;
    }
}
using MongoDB.Driver;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var mongoClient = new MongoClient("mongodb+srv://testminhaj:yzsKdGz9zfWwhNu8@owncluster.4skch.mongodb.net/?retryWrites=true&w=majority&appName=OwnCluster");
var database = mongoClient.GetDatabase("unishare");

app.MapGet("/", async () =>
{
    try
    {
        var collection = database.GetCollection<BsonDocument>("courses");
        var documents = await collection.Find(new BsonDocument()).ToListAsync();

        // Convert all BsonDocuments to clean JSON objects
        var jsonList = documents.Select(doc => BsonTypeMapper.MapToDotNetValue(doc)).ToList();

        return Results.Json(new { courses = jsonList });
    }
    catch (Exception ex)
    {
        return Results.Json(new { error = ex.Message }, statusCode: 500);
    }
});

app.Run();


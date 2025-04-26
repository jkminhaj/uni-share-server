using MongoDB.Driver;
using MongoDB.Bson;
using UniShare.Services ;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

// app.MapGet("/", async () =>
// {
//     try
//     {
//         var collection = database.GetCollection<BsonDocument>("courses");
//         var documents = await collection.Find(new BsonDocument()).ToListAsync();

//         var jsonList = documents.Select(doc =>
//         {
//             // Convert the BsonDocument to a Dictionary for manipulation
//             var dict = doc.ToDictionary();

//             // Convert _id from ObjectId to string
//             if (dict.ContainsKey("_id") && dict["_id"] is ObjectId oid)
//             {
//                 dict["_id"] = oid.ToString();
//             }

//             return dict;
//         }).ToList();

//         return Results.Json(new { courses = jsonList });
//     }
//     catch (Exception ex)
//     {
//         return Results.Json(new { error = ex.Message }, statusCode: 500);
//     }
// });

app.Run();


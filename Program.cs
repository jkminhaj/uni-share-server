using MongoDB.Driver;
using MongoDB.Bson;
using UniShare.Services ;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run();
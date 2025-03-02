// using Microsoft.EntityFrameworkCore;
// using TodoApi;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<ToDoDbContext>(option =>
//      option.UseMySql(
//         builder.Configuration.GetConnectionString("ToDoDB"),
//      new MySqlServerVersion(new Version(9,0,0))));

// // טיפול בבעית הקורס
// builder.Services.AddCors(x => x.AddPolicy("all", a => a.AllowAnyHeader()
// .AllowAnyMethod().AllowAnyOrigin()));

// builder.Services.AddSwaggerGen();
// // builder.Services.AddEndpointsApiExplorer();

// var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
// app.UseCors("all");

// // ToDoDbContext db = new();

// app.MapGet("/", () => "Hello World!");

// // שליפת כל המשימות
// app.MapGet("/items", async (ToDoDbContext db) =>
// {
//     var items = await db.Items.ToListAsync();
//     return Results.Ok(items);
// });
// // הוספת משימה
// app.MapPost("/items", async (string name, ToDoDbContext db) =>
// {
//     Item newItem = new Item();
//     newItem.Name = name;
//     newItem.IsComplete = false;
//     db.Add(newItem);
//     await db.SaveChangesAsync();
//     return Results.Ok(newItem);
// });
// // עדכון משימה
// app.MapPatch("/items", async (int id, bool isComplete, ToDoDbContext db) =>
// {
//     var item = await db.Items.FindAsync(id);
//     if (item == null)
//         return null;
//     item.IsComplete = isComplete;
//     await db.SaveChangesAsync();
//     return Results.Ok(item);
// });
// // מחיקת משימה
// app.MapDelete("/items", async (int id, ToDoDbContext db) =>
// {
//     var item = await db.Items.FindAsync(id);
//     if (item == null)
//         return null;
//     db.Remove(item);
//     await db.SaveChangesAsync();
//     return Results.Ok(item);
// });

// app.Run();
using Microsoft.EntityFrameworkCore;
using TodoApi;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);
// service
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
     new MySqlServerVersion(new Version(9, 0, 0))));
// cors
builder.Services.AddCors(options => options.AddPolicy("All", policy =>
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
// 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (builder.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
app.MapControllers();

app.UseCors("All");

app.MapGet("/", () => "Hello World!");

// get
app.MapGet("/items", async (ToDoDbContext db) =>
{
    var item = await db.Items.ToListAsync();
    return Results.Ok(item);
});
// post
app.MapPost("/items", async (ToDoDbContext db, string name) =>
{
    Item item = new Item();
    item.Name = name;
    item.IsComplete = false;
    db.Add(item);
    await db.SaveChangesAsync();
    return Results.Ok(item);
});
// patch
app.MapPatch("/items", async (ToDoDbContext db, int id, bool isComplete) =>
{
    var item = await db.Items.FindAsync(id);
    if (item != null)
    {
        item.IsComplete = isComplete;
    }
    await db.SaveChangesAsync();
    return Results.Ok(item);

});
// delete
app.MapDelete("/items", async (ToDoDbContext db, int id) =>
{
    var item = await db.Items.FindAsync(id);
    if (item != null)
    {
        db.Remove(item);
    }
    await db.SaveChangesAsync();
    return Results.Ok(item);
});
app.Run();
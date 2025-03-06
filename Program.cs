
// using Microsoft.EntityFrameworkCore;
// using TodoApi;
// using Microsoft.OpenApi.Models;
// var builder = WebApplication.CreateBuilder(args);
// // service
// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
//      new MySqlServerVersion(new Version(9, 0, 0))));
// // cors
// builder.Services.AddCors(options => options.AddPolicy("All", policy =>
//     policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
// // 
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddSwaggerGen(options =>
// {
//     options.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Version = "v1",
//         Title = "ToDo API",
//         Description = "An ASP.NET Core Web API for managing ToDo items",
//         TermsOfService = new Uri("https://example.com/terms"),
//         Contact = new OpenApiContact
//         {
//             Name = "Example Contact",
//             Url = new Uri("https://example.com/contact")
//         },
//         License = new OpenApiLicense
//         {
//             Name = "Example License",
//             Url = new Uri("https://example.com/license")
//         }
//     });
// });

// var app = builder.Build();
// // if (app.Environment.IsDevelopment())
// // {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// // }
// if (builder.Environment.IsDevelopment())
// {
//     app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
//     {
//         options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
//         options.RoutePrefix = string.Empty;
//     });
// }
// app.MapControllers();

// app.UseCors("All");

// app.MapGet("/", () => "Server API is running!");

// // get
// app.MapGet("/items", async (ToDoDbContext db) =>
// {
//     var item = await db.Items.ToListAsync();
//     return Results.Ok(item);
// });
// // post
// app.MapPost("/items", async (ToDoDbContext db, string name) =>
// {
//     Item item = new Item();
//     item.Name = name;
//     item.IsComplete = false;
//     db.Add(item);
//     await db.SaveChangesAsync();
//     return Results.Ok(item);
// });
// // patch
// app.MapPatch("/items", async (ToDoDbContext db, int id, bool isComplete) =>
// {
//     var item = await db.Items.FindAsync(id);
//     if (item != null)
//     {
//         item.IsComplete = isComplete;
//     }
//     await db.SaveChangesAsync();
//     return Results.Ok(item);

// });
// // delete
// app.MapDelete("/items", async (ToDoDbContext db, int id) =>
// {
//     var item = await db.Items.FindAsync(id);
//     if (item != null)
//     {
//         db.Remove(item);
//     }
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
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), new MySqlServerVersion(new Version(8, 0, 25))));
// cors
builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy =>
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
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }
if (builder.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
app.MapControllers();

app.UseCors("AllowAll");

app.MapGet("/", () => "Server API is running!");
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
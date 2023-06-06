using Platform;
using Platform.Models;
using Microsoft.EntityFrameworkCore;
using Platform.Services;
using Microsoft.Extensions.Caching.SqlServer;

var builder = WebApplication.CreateBuilder(args);
/*
builder.Services.AddDistributedMemoryCache(opts => {
opts.SizeLimit = 200;
});
*/
builder.Services.AddDistributedSqlServerCache(opts => {
    opts.ConnectionString =builder.Configuration["ConnectionStrings:CacheConnection"];
    opts.SchemaName = "dbo";
    opts.TableName = "DataCache";
});
builder.Services.AddResponseCaching();
builder.Services.AddSingleton<IResponseFormatter,HtmlResponseFormatter>();



builder.Services.AddDbContext<CalculationContext>(options => {
    options.UseSqlServer(builder.Configuration["ConnectionStrings:CalcConnection"]!);
    //options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

// Add services to the container.
builder.Services.AddTransient<SeedData>();
var app = builder.Build();


app.UseResponseCaching();

app.MapEndpoint<SumEndpoint>("sum/{count:int=1000000000}");

 bool cmdLineInıt=(app.Configuration["INITDB"]??"false").ToLower()=="true";
 if(app.Environment.IsDevelopment()||cmdLineInıt){
     using var scope=app.Services.CreateScope();
     var seedData=scope.ServiceProvider.GetRequiredService<SeedData>();
     seedData.SeedDatabase();
 }if(!cmdLineInıt){
    app.Run();
 }





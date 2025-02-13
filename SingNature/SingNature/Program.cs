using SingNature.Data;
using authorization.Data; 


Console.WriteLine("Application Starting...");
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5056")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

builder.WebHost.ConfigureKestrel(options =>
{

    if (builder.Environment.IsDevelopment())
    {
        options.ListenAnyIP(5075);
        options.ListenAnyIP(5076, listenOptions => listenOptions.UseHttps());
    }
    else 
    {
        var port = Environment.GetEnvironmentVariable("PORT") ?? "8000";    // Cloud port
        options.ListenAnyIP(int.Parse(port));
    }
});

// Explicitly set URLs for Docker (Overrides Kestrel)
var dockerPort = Environment.GetEnvironmentVariable("DOCKER_PORT") ?? "8000";
builder.WebHost.UseUrls($"http://0.0.0.0:{dockerPort}"); // Force HTTP only

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserDao>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

Console.WriteLine("Configuring Middleware...");
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseSession();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    // .WithStaticAssets(); 

Console.WriteLine("Application Running...");
app.Run();

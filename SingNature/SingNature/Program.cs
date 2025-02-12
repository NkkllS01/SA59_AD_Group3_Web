using SingNature.Data;
using authorization.Data; 


Console.WriteLine("Application Starting...");
var builder = WebApplication.CreateBuilder(args);

// // Secure CORS Policy for Local & Cloud (required for frontend APIs like React)
// val allowedOrigins = builder.Environment.IsDevelopment()
//     ? new[] { "http://localhost:3000", "http://localhost:5173" }
//     : new[] { "https://yourdomain.com" };

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigins",
//         builder => builder
//             .WithOrigins(allowedOrigins)    // Uses different origins for local vs cloud
//             .AllowAnyMethod()
//             .AllowAnyHeader());
// });

// Configure Kestrel for Local & Cloud Deployment
builder.WebHost.ConfigureKestrel(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.ListenAnyIP(5075);
        options.ListenAnyIP(5076, listenOptions => listenOptions.UseHttps());
    }
    else 
    {
        var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";    // Cloud port
        options.ListenAnyIP(int.Parse(port));
    }
});

// Explicitly set URLs for Docker (Overrides Kestrel)
var dockerPort = Environment.GetEnvironmentVariable("DOCKER_PORT") ?? "8080";
builder.WebHost.UseUrls($"http://+:{dockerPort}"); // Force HTTP only

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserDao>();

var app = builder.Build();

// app.UseCors("AllowSpecificOrigins");

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

Console.WriteLine("Configuring Middleware...");
app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    // .WithStaticAssets(); 

Console.WriteLine($"Application Running in {app.Environment.EnvironmentName} mode...");
app.Run();

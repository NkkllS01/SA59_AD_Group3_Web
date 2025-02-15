using Amazon.Runtime;
using Amazon.S3;
using SingNature.Data;
using authorization.Data;
using SingNature.Services;


Console.WriteLine("Application Starting...");
var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.GetSection("DigitalOcean");
var accessKey = config["AccessKey"];
var secretKey = config["SecretKey"];
var endpointUrl = config["EndpointUrl"];

if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(endpointUrl))
{
    throw new Exception("❌ DigitalOcean Spaces 配置缺失! 请检查 appsettings.json.");
}

Console.WriteLine($"✅ DigitalOcean ServiceURL: {endpointUrl}");

var s3Config = new AmazonS3Config
{
    ServiceURL = endpointUrl, // ✅ DigitalOcean 需要 `ServiceURL`
    ForcePathStyle = true     // ✅ 兼容 DigitalOcean Spaces
};

// ✅ 统一注册 `IAmazonS3`，防止 `ServiceURL` 丢失
builder.Services.AddScoped<IAmazonS3>(_ => new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), s3Config));

// ✅ `S3Service` 复用 `IAmazonS3`，避免重复创建
builder.Services.AddScoped<S3Service>();

if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(endpointUrl))
{
    throw new Exception("DigitalOcean Spaces Config error：AccessKey, SecretKey or EndpointUrl is missing!");
}

builder.Services.AddHttpClient();
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") 
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");


if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Database connection string is missing! Check environment variables or appsettings.json.");
}

// Print connection string for debugging (REMOVE in production)
Console.WriteLine($"Using Database Connection: {connectionString}");


    
    builder.Services.AddScoped<UserDao>();
	builder.Services.AddScoped<SightingsDAO>();
	builder.Services.AddScoped<SpeciesDAO>();
	builder.Services.AddScoped<ParkDAO>();
	builder.Services.AddScoped<WarningDAO>();
    builder.Services.AddScoped<CategoryDAO>();


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
        var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";    // Cloud port
        options.ListenAnyIP(int.Parse(port));
    }
});

// Explicitly set URLs for Docker (Overrides Kestrel)
var dockerPort = Environment.GetEnvironmentVariable("DOCKER_PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{dockerPort}"); // Force HTTP only

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// Register DAO as a service (IMPORTANT)
builder.Services.AddScoped<UserDao>();
builder.Services.AddTransient<WarningService>();
builder.Services.AddScoped<SightingsDAO>();
builder.Services.AddScoped<SpeciesDAO>();
builder.Services.AddScoped<ParkDAO>();
builder.Services.AddScoped<WarningDAO>();
builder.Services.AddScoped<CategoryDAO>();

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

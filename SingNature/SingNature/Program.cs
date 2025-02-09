using SingNature.Data;
using authorization.Data; 


Console.WriteLine("Application Starting...");
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    
    options.ListenAnyIP(5075); 
    options.ListenAnyIP(5076, listenOptions => listenOptions.UseHttps());
});

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
app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets(); 

Console.WriteLine("Application Running...");
app.Run();

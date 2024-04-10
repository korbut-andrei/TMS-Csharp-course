using EventsTestProject;
using TrainingProjectFile;
using TransientServiceWithServiceFactory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHostedService<BackgroundTickerService>();
builder.Services.AddSingleton<TickerService>();
builder.Services.AddTransient<TransientService>();
builder.Services.AddTransient<ITickerServiceFactory, TickerServiceFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

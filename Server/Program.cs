using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5218")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Register Services & Controllers
builder.Services.AddScoped<StudentService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();


    var csMajor = context.Majors.FirstOrDefault(m => m.Name == "Computer Science");
    if (csMajor != null && !csMajor.IsRegistrationOpen)
    {
        csMajor.IsRegistrationOpen = true;
        context.SaveChanges();
    }

    var validCourseIds = context.Courses.Select(c => c.Id).ToList();
    var orphanedSchedules = context.CourseSchedules
        .Where(cs => !validCourseIds.Contains(cs.CourseId));

    if (orphanedSchedules.Any())
    {
        context.CourseSchedules.RemoveRange(orphanedSchedules);
        context.SaveChanges();
    }
}

app.UseRouting();
app.UseCors("AllowBlazorClient");
app.UseAuthorization();
app.MapControllers();

app.Run();
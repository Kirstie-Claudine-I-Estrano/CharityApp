using CharityApp.Data;  // Ensure this namespace is correct for your DbContext
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Step 1: Register the ApplicationDbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Step 2: Add services for both MVC and API
builder.Services.AddControllersWithViews();  // Supports both MVC and API
builder.Services.AddEndpointsApiExplorer();  // Enables minimal API documentation
builder.Services.AddSwaggerGen();            // Adds Swagger for API testing

var app = builder.Build();

// Step 3: Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();   // Enable Swagger for API documentation in development
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Step 4: Default route for the Home controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Step 5: Custom route for Donation
app.MapControllerRoute(
    name: "donationList",
    pattern: "donations",
    defaults: new { controller = "Donations", action = "Index" });

// Step 6: Enable API routes
app.MapControllers();  // Register API controllers to handle API routes

// Step 7: Run the application
app.Run();

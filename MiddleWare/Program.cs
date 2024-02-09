var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.Use(async (context, _next) => {
        
    var username = context.Request.Query["username"];
    var password = context.Request.Query["password"];

    // enter localhost:5244/?username=user1&password=password1 in search bar 
    if (username != "user1" || password != "password1") {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Not authorized.");
    } else {
        await _next(context);
    }

});

app.UseAuthorization();

app.MapRazorPages();

app.Run();

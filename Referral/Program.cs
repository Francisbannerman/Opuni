using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Referral;
using Referral.Repositories;
using Referral.Services;
using Referral.Settings;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);
//Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<ReferralCodeService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<ApplicationDbContext>(Options =>
    Options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<AuthorizationSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://dev-rbh6jkmgi8r73h4d.us.auth0.com/";
    options.Audience = "https://localhost:7144";
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.MapControllers();

app.Run();


//
// // Add services to the container.
// builder.Services.AddControllers();
//
// // Add DbContext and services
// builder.Services.AddDbContext<ApplicationDbContext>(Options =>
//     Options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// builder.Services.AddScoped<ReferralCodeService>();
// builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// builder.Services.AddScoped<AuthorizationSettings>();
//
// // // Configure JWT Bearer authentication
// // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// //     .AddJwtBearer(options =>
// //     {
// //         options.Authority = "https://dev-rbh6jkmgi8r73h4d.us.auth0.com/";
// //         options.Audience = "https://localhost:7144/"; // Make sure this matches the identifier of your API
// //     });
//
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//
// var app = builder.Build();
//
// // Configure middleware pipeline
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
//     app.UseDeveloperExceptionPage();
// }
//
// app.UseExceptionHandler("/Home/Error");
// app.UseStaticFiles();
// // app.UseAuthentication(); // Ensure this middleware is configured before authorization
// app.UseHttpsRedirection();
// app.UseRouting();
// // app.UseAuthorization();
// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers();
// });
// app.Run();

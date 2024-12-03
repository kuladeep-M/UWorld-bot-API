using ChatHubSignalR;
using ChatHubSignalR.Repository;
using Google;
using Google.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UWorldSupportBot;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ChatContext>(options
 =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),b=> b.MigrationsAssembly("UWorldSupportBot")));
builder.Services.AddDialogFlowService();
builder.Services.AddSignalR();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

           // Adding Jwt Bearer
           .AddJwtBearer(options =>
           {
               options.SaveToken = true;
               options.RequireHttpsMetadata = false;
               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidAudience = builder.Configuration["Jwt:Audience"],
                   ValidIssuer = builder.Configuration["Jwt:Issuer"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
               };
           });
        ;

var app = builder.Build();
app.UseCors(builder=> builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub"); // Map the SignalR Hub


app.Run();

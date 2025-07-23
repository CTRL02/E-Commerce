using E_Commerce;
using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using E_Commerce.Repository;
using E_Commerce.Interfaces;
using E_Commerce.DbInitliazer;
using Microsoft.EntityFrameworkCore.Internal;
using E_Commerce.Context;

var builder = WebApplication.CreateBuilder(args);

//hello this is for azure for testing

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // You can also add other providers (e.g., Debug, EventSource)

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<IVariationService, VariationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce API", Version = "v1" });

    // Add JWT Bearer Authorization
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: \"Bearer abcdefgh123456\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddDbContext<ECommerceDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<ECommerceDbContext>().AddDefaultTokenProviders();

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
builder.Services.AddSingleton(jwtOptions);
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Use cookies as the default scheme
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
    };
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme) // Register cookie authentication
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Store the results in cookies
});


builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
builder.Services.AddTransient<ISMsService, SmsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//SeedDatabase();

app.MapControllers();

app.Run();

//void SeedDatabase()
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var dbIntializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
//        dbIntializer.Initialize();
//    }
//}

using CourseProjectItems.Data;
using CourseProjectItems.Interfaces;
using CourseProjectItems.Repositories;
using CourseProjectItems.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.StaticFiles;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllersWithViews();


builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Lockout.AllowedForNewUsers = false;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

var cloudinarySettings = new Account(
    builder.Configuration["CloudinarySettings:CloudName"],
    builder.Configuration["CloudinarySettings:ApiKey"],
    builder.Configuration["CloudinarySettings:ApiSecret"]
);
var cloudinary = new Cloudinary(cloudinarySettings);
builder.Services.AddSingleton(cloudinary);

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
	new CultureInfo("ru-RU")
};
var localizationOptions = new RequestLocalizationOptions
{
	DefaultRequestCulture = new RequestCulture("ru-RU"),
	SupportedCultures = supportedCultures,
	SupportedUICultures = supportedCultures
};

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc()
	.AddViewLocalization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<ITicketsRepository, TicketsRepository>();
    
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "AAAAAAAAAAAAAAAAAAAAMACARENA";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

// ������������ ��������� HTTP-��������
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // �������� HSTS �� ��������� � 30 ����. �� ������ �������� ��� �������� ��� ������� ���������, ��. https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization(localizationOptions);



app.UseHttpsRedirection();

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".unityweb"] = "application/octet-stream";
provider.Mappings[".data"] = "application/octet-stream";
provider.Mappings[".wasm"] = "application/wasm";


app.UseStaticFiles(CustomStaticFileOptions.GetOptions());

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "jira",
    pattern: "jira/{action=Index}/{id?}",
    defaults: new { controller = "Jira", action = "Index" });

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedAdminAsync(userManager, roleManager);
}

app.UseSession();

app.Run();

static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    var adminEmail = "admin.net@owner.com"; 
    var adminPassword = "#P@ssw0rdXpl0r3r2024!"; 

    if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
    {
        await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
    }
    if (!await roleManager.RoleExistsAsync(UserRoles.User))
    {
        await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
    }
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true, ApiToken = String.Empty};
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
        }
    }
}


public static class CustomStaticFileOptions
{
	private class CustomContentTypeProvider : IContentTypeProvider
	{
		private const string GZIP_EXTENSION = ".gz";
		private const string BROTLI_EXTENSION = ".br";

		public static readonly IReadOnlyDictionary<string, string> COMPRESSION_ENCODINGS = new Dictionary<string, string>()
		{
			{ GZIP_EXTENSION, "gzip" },
			{ BROTLI_EXTENSION, "br" }
		};

		private readonly FileExtensionContentTypeProvider fileTypeProvider = new();

		public CustomContentTypeProvider()
		{
			fileTypeProvider.Mappings[".data"] = "application/octet-stream";
			fileTypeProvider.Mappings[".wasm"] = "application/wasm";
			fileTypeProvider.Mappings[".unityweb"] = "application/octet-stream";
		}

		public bool TryGetContentType(string filePath, out string contentType)
		{
			var extension = Path.GetExtension(filePath);
			if (extension == GZIP_EXTENSION || extension == BROTLI_EXTENSION)
			{
				filePath = filePath[..^extension.Length];
				extension = Path.GetExtension(filePath);
			}

			return fileTypeProvider.TryGetContentType(filePath, out contentType);
		}
	}

	public static StaticFileOptions GetOptions()
	{
		var fileTypeProvider = new CustomContentTypeProvider();
		return new StaticFileOptions
		{
			ContentTypeProvider = fileTypeProvider,
			OnPrepareResponse = context =>
			{
				if (CustomContentTypeProvider.COMPRESSION_ENCODINGS.TryGetValue(Path.GetExtension(context.File.Name), out var encoding))
				{
					context.Context.Response.Headers["Content-Encoding"] = encoding;
				}
			}
		};
	}
}
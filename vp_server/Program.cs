using vp_server.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();
deleleteTransactions dt = new deleleteTransactions();
dt.delete();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}"
    );


app.UseStaticFiles();
app.Run();
//Scaffold-DbContext "Data Source=(local);Initial Catalog=vapeshop;Integrated Security=True;Encrypt=False" Microsoft.EntityFrameworkCore.SqlServer (-Force для обновлений)

//Scaffold-DbContext "server=localhost;Port=3306;user=root;password=Passw0rd;database=vapeshop" Pomelo.EntityFrameworkCore.MySql
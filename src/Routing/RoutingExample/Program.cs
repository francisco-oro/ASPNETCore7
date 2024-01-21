using RoutingExample.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint));
});
var app = builder.Build();


app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.Map("files/{filename}.{extension}", async context =>
    {
        string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
        await context.Response.WriteAsync($"In files -  {fileName} - {extension}");
    });


    endpoints.Map("employee/profile/{employeeName:length(3,7):alpha=francisco}", async context =>
    {
        string? employeeName = Convert.ToString(context.Request.RouteValues["employeeName"]);
        await context.Response.WriteAsync($"In employee profile - {employeeName}");
    });


    // Eg: products/details/
    endpoints.Map("products/details/{id:int:range(1,1000)?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product details - {id}");
        }
        else
        {
            await context.Response.WriteAsync($"Products details - id is not supplied"); 
        }
    });

    // Eg: daily-digest-report/{reportDate}
    endpoints.Map("daily-digest-report/{reportdate:datetime}", async context =>
    {
        DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"In daily-digest-report - {reportDate.ToShortDateString()}");
    });

    // Eg: cities/cityid
    endpoints.Map("cities/{cityid:guid}", async context =>
    {
        Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"]));
        await context.Response.WriteAsync($"City information - {cityId}");
    });

    // sales-report/2020/apr
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        if (month == "apr" || month == "jul" || month == "oct" || month == "jan")
        {
            await context.Response.WriteAsync($"sales report - {year} - {month}");
        }
        else
        {
            await context.Response.WriteAsync($"Month  {month} is not allowed"); 
        }
    });

    // sales-report/2024/jan
    endpoints.Map("sales-report/2024/jan", async context =>
    {
        await context.Response.WriteAsync("Sales report exclusively for 2024 - jan");
    });
}); 
app.Run(async context =>
{
    await context.Response.WriteAsync($"No route matched at " +
                                      $"{context.Request.Path}");
});
app.Run();

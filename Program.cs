using Oracle.ManagedDataAccess.Client;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing.Constraints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:8080") // или ваш домен
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

var app = builder.Build();

app.UseRouting();

app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/AddData", ([FromBody]Data data) =>
    {
        try
        {
            OracleConnection connection = new OracleConnection("PERSIST SECURITY INFO=True;Connection Timeout=60;Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = HOME-PC)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = orcl))); User Id=SYS;Password=wAYTEK123;DBA Privilege=SYSDBA");

            connection.Open();
        
            Console.WriteLine(data.DateOfReport);
            
            OracleCommand cmd = connection.CreateCommand();

            cmd.CommandText = $"Insert into Datas (ffmin, ffmax, flmin, flmax, ptmin, ptmax, lfmin, lfmax, llmin, llmax, tpmin, tpmax, ttmin, ttmax, fflitmin, fflitmax, fllitmin, fllitmax, ptlitmin, ptlitmax, lflitmin, lflitmax, lllitmin, lllitmax, tplitmin, tplitmax, ttlitmin, ttlitmax, dateofreport) values({data.ffMin}, {data.ffMax}, {data.flMin}, {data.flMax},{data.ptMin}, {data.ptMax},{data.lfMin}, {data.lfMax},{data.llMin}, {data.llMax},{data.tpMin}, {data.tpMax},{data.ttMin}, {data.ttMax},{data.fflitMin}, {data.fflitMax},{data.fllitMin}, {data.fllitMax},{data.ptlitMin}, {data.ptlitMax},{data.lflitMin}, {data.lflitMax},{data.lllitMin}, {data.lllitMax},{data.tplitMin}, {data.tplitMax},{data.ttlitMin}, {data.ttlitMax}, '{data.DateOfReport.Date:dd-MM-yyyy}')";

            cmd.ExecuteNonQuery();
        
            connection.Close();

            Console.WriteLine("Успех");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Провал");
        }
    })
    .WithName("AddData")
    .WithOpenApi();

app.MapGet("/GetData", (DateTime date) =>
    {
        try
        {
            OracleConnection connection = new OracleConnection("PERSIST SECURITY INFO=True;Connection Timeout=60;Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = HOME-PC)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = orcl))); User Id=SYS;Password=wAYTEK123;DBA Privilege=SYSDBA");

            connection.Open();
        
            OracleCommand cmd = connection.CreateCommand();

            cmd.CommandText = $"Select * from datas where dateOfReport = '{date.Date:dd-MM-yyyy}'";

            OracleDataReader reader = cmd.ExecuteReader();

            reader.Read();

            Data gettedData = new Data();

            gettedData.ffMin = Convert.ToInt32(reader["ffmin"]);
            gettedData.ffMax = Convert.ToInt32(reader["ffmax"]);
            gettedData.flMin = Convert.ToInt32(reader["flmin"]);
            gettedData.flMax = Convert.ToInt32(reader["flmax"]);
            gettedData.ptMin = Convert.ToInt32(reader["ptmin"]);
            gettedData.ptMax = Convert.ToInt32(reader["ptmax"]);
            gettedData.lfMin = Convert.ToInt32(reader["lfmin"]);
            gettedData.lfMax = Convert.ToInt32(reader["lfmax"]);
            gettedData.llMin = Convert.ToInt32(reader["llmin"]);
            gettedData.llMax = Convert.ToInt32(reader["llmax"]);
            gettedData.tpMin = Convert.ToInt32(reader["tpmin"]);
            gettedData.tpMax = Convert.ToInt32(reader["tpmax"]);
            gettedData.ttMin = Convert.ToInt32(reader["ttmin"]);
            gettedData.ttMax = Convert.ToInt32(reader["ttmax"]);
            gettedData.fflitMin = Convert.ToInt32(reader["fflitmin"]);
            gettedData.fflitMax = Convert.ToInt32(reader["fflitmax"]);
            gettedData.fllitMin = Convert.ToInt32(reader["fllitmin"]);
            gettedData.fllitMax = Convert.ToInt32(reader["fllitmax"]);
            gettedData.ptlitMin = Convert.ToInt32(reader["ptlitmin"]);
            gettedData.ptlitMax = Convert.ToInt32(reader["ptlitmax"]);
            gettedData.lflitMin = Convert.ToInt32(reader["lflitmin"]);
            gettedData.lflitMax = Convert.ToInt32(reader["lflitmax"]);
            gettedData.lllitMin = Convert.ToInt32(reader["lllitmin"]);
            gettedData.lllitMax = Convert.ToInt32(reader["lllitmax"]);
            gettedData.tplitMin = Convert.ToInt32(reader["tplitmin"]);
            gettedData.tplitMax = Convert.ToInt32(reader["tplitmax"]);
            gettedData.ttlitMin = Convert.ToInt32(reader["ttlitmin"]);
            gettedData.ttlitMax = Convert.ToInt32(reader["ttlitmax"]);
            
            connection.Close();
            
            Console.WriteLine("Успех");

            return Results.Ok(gettedData);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);

            return Results.NotFound();
        }
    })
    .WithName("GetData")
    .WithOpenApi();

app.Run();

public class Data
{
    public int ffMin { get; set; }
    public int ffMax { get; set; }
    public int flMin { get; set; }
    public int flMax { get; set; }
    public int ptMin { get; set; }
    public int ptMax { get; set; }
    public int lfMin { get; set; }
    public int lfMax { get; set; }
    public int llMin { get; set; }
    public int llMax { get; set; }
    public int tpMin { get; set; }
    public int tpMax { get; set; }
    public int ttMin { get; set; }
    public int ttMax { get; set; }
    public int fflitMin { get; set; }
    public int fflitMax { get; set; }
    public int fllitMin { get; set; }
    public int fllitMax { get; set; }
    public int ptlitMin { get; set; }
    public int ptlitMax { get; set; }
    public int lflitMin { get; set; }
    public int lflitMax { get; set; }
    public int lllitMin { get; set; }
    public int lllitMax { get; set; }
    public int tplitMin { get; set; }
    public int tplitMax { get; set; }
    public int ttlitMin { get; set; }
    public int ttlitMax { get; set; }
    public DateTime DateOfReport { get; set; }
}


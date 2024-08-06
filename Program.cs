using API_QCode.Context;
using API_QCode.Servicios;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var connection_string = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddDbContext<DbContext_QCode>(options => options.UseSqlServer(connection_string));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

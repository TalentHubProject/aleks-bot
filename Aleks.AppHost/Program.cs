using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddSqlServerContainer("mysql");

var aleksDb = mysql.AddDatabase("aleks");

builder.AddProject<Aleks_Worker>("aleks.worker")
    .WithReference(aleksDb);

builder.Build().Run();
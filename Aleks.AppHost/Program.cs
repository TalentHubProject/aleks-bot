var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Aleks_Worker>("aleks.worker");

builder.Build().Run();

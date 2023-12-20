var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.RemoraDiscordBot_Worker>("remoradiscordbot.worker");

builder.Build().Run();

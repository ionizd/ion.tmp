﻿using Ion.Extensions;

namespace Ion.MicroServices.Job.Demo.Services;

public class JobService2 : IHostedJobService
{
    private readonly ILogger<JobService2> logger;

    public JobService2(ILogger<JobService2> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var rand = new Random(DateTime.UtcNow.Millisecond);

        return TaskEx.TryWaitUntil(() => true == false,
            500.Milliseconds(),
            rand.Next(3000, 10000).Milliseconds(),
            () =>
            {
                logger.LogInformation($"{nameof(JobService2)} doing work ...");
            });
    }
}
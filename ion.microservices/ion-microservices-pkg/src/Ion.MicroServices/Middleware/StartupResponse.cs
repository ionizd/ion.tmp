﻿using System.Text.Json.Serialization;

namespace Ion.Middleware;

public class StartupResponse : MiddlewareResponse
{
    public StartupResponse(IMicroService service)
    : base(service)
    {
        Started = service.IsStarted;
    }

    public StartupResponse()
    {
    }

    [JsonPropertyName("started")]
    public bool Started { get; set; }
}
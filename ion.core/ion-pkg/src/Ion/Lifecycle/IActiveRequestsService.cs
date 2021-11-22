﻿namespace Ion.Lifecycle;

public interface IActiveRequestsService
{
    long Counter { get; }

    bool HasActiveRequests { get; }

    void Decrement();

    void Increment();
}

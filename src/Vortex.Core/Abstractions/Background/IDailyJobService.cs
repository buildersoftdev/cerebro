﻿namespace Vortex.Core.Abstractions.Background
{
    public interface IDailyJobService
    {
        void OnDailyJob_Callback(object state);
    }
}

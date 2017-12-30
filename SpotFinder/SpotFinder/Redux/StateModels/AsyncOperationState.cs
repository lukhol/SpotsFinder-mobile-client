using SpotFinder.Core.Enums;
using System;

namespace SpotFinder.Redux.StateModels
{
    public class AsyncOperationState<TValue, TTrigger>
    {
        public Status Status { get; private set; }
        public Exception Error { get; private set; }
        public TValue Value { get; private set; }
        public TTrigger TriggerValue { get; private set; }

        public AsyncOperationState() { }

        public AsyncOperationState(Status status, Exception error, TValue value, TTrigger triggerValue)
        {
            Status = status;
            Error = error;
            Value = value;
            TriggerValue = triggerValue;
        }
    }
}

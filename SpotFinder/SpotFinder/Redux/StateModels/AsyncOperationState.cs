using SpotFinder.Core.Enums;

namespace SpotFinder.Redux.StateModels
{
    public class AsyncOperationState<TValue, TTrigger>
    {
        public Status Status { get; private set; }
        public string ErrorMessage { get; private set; }
        public TValue Value { get; private set; }
        public TTrigger TriggerValue { get; private set; }

        public AsyncOperationState() { }

        public AsyncOperationState(Status status, string errorMessage, TValue value, TTrigger triggerValue)
        {
            Status = status;
            ErrorMessage = errorMessage;
            Value = value;
            TriggerValue = triggerValue;
        }
    }
}

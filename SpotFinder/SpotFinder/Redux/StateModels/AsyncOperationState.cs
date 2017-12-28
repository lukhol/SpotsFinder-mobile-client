using SpotFinder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpotFinder.Redux.StateModels
{
    public class AsyncOperationState<T>
    {
        public Status Status { get; private set; }
        public string ErrorMessage { get; private set; }
        public T Value { get; private set; }

        public AsyncOperationState() { }

        public AsyncOperationState(Status status, string errorMessage, T value)
        {
            Status = status;
            ErrorMessage = errorMessage;
            Value = value;
        }
    }
}

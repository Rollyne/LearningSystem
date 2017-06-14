namespace LearningSystem.Services.Tools.Generic
{
    public class ExecutionResult<TResult> : ExecutionResult, IExecutionResult<TResult>
    {
        public TResult Result { get; set; }
    }
}

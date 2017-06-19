namespace LearningSystem.Services.Tools.Generic
{
    public interface IExecutionResult<TResult> : IExecutionResult
    {
        TResult Result { get; set; }
    }
}
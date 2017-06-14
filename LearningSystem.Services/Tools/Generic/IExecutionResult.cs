namespace LearningSystem.Services.Tools.Generic
{
    public interface IExecutionResult<TResult>
    {
        TResult Result { get; set; }
    }
}
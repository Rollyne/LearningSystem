namespace LearningSystem.Services.Tools
{
    public interface IExecutionResult
    {
        string Message { get; set; }
        bool Succeded { get; set; }
    }
}
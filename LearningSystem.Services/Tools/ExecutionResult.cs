namespace LearningSystem.Services.Tools
{
    public class ExecutionResult : IExecutionResult
    {
        public bool Succeded { get; set; }

        public string Message { get; set; }
    }
}

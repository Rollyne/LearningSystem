namespace LearningSystem.Services.Tools.Messages
{
    public static class CrudMessages
    {
        public static string SuccessfulCreationOf(string item = "item") => $"The {item} was successfuly created.";

        public static string UnsuccessfulCreationOf(string item = "item")
            => $"The {item} wasn't be created successfully. There is a server problem.";

        public static string NotFound(string item = "item") => $"This {item} was not found.";
    }
}
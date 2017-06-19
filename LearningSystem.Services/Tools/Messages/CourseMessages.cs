namespace LearningSystem.Services.Tools.Messages
{
    public static class CourseMessages
    {
        public static string SuccessfulSignUp(string courseName = "") => $"You successfully signed up to the course {courseName}";

        public static string SuccessfulSignOut(string courseName = "") => $"You successfully signed out of the course {courseName}";

        public static string UnsuccessfulSignUpDbError() => "Sorry there was a problem signing you up to the course.";

        public static string UnsuccessfulSignUpPassedStartDate() => "You cannot sign up to this course because it's start date has passed";

        public static string UnsuccessfulSignOutDbError() => "Sorry there was a problem signing you out of the course.";

        public static string AlreadySignedUp() => "You have already signed up to this course.";

        public static string NotSignedUp() => "You are not signed up to this course.";

        public static string NotFound() => "Couldn't find the course you are looking for.";

        public static string CannotGrade()
            => "You cannot grade students from this course because you are not a trainer.";

        public static string SuccessfullyGraded() => "You successfully graded this student.";
    }
}

using System;
namespace ModelLibrary
{
    public static class clsEnum
    {
        public enum LoginResault
        {
            SuccessfullyAuthenticated = 0,
            WorngUsernameOrPassword = 1,
            PasswordExpired = 2,
            AccountLocked = 3,
            ErrorOccurred = 9
        }

        public enum RequestType
        {
            Projects = 1
        }

        public enum ErrorType
        {
            ErrorOccurred = 0,
            InvalidToken = 1,
            InvalidRequestType = 2

        }
    }
}

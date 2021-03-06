﻿using System;
namespace ModelLibrary
{
    public static class npcc_types
    {
        public enum inf_login_result
        {
            SuccessfullyAuthenticated = 0,
            WorngUsernameOrPassword = 1,
            PasswordExpired = 2,
            AccountLocked = 3,
            ErrorOccurred = 9
        }

        public enum inf_request
        {
            Projects = 1
        }

        public enum inf_method
        {
            Get = 1,
            Post =2 
        }

        public enum inf_error
        {
            ErrorOccurred = 0,
            InvalidToken = 1,
            InvalidRequestType = 2

        }
    }
}

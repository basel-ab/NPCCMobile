using System;
using System.Runtime.Serialization;

namespace ModelLibrary
{
    public  enum LoginResault
    {
        SuccessfullyAuthenticated = 0,
        WorngUsernameOrPassword = 1,
        PasswordExpired =2,
        AccountLocked = 3,
        ErrorOccurred = 9
    }

    public class LoginInfo
    {
        public LoginResault Authenticated { get; set; }
        public string Token { get; set; }
    }
}

using System;
using System.Runtime.Serialization;
using static ModelLibrary.clsEnum;

namespace ModelLibrary
{
    public class clsLoginInfo
    {
        public LoginResault Authenticated { get; set; }
        public string Token { get; set; }
    }
}

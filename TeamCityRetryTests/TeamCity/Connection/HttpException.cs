﻿using System;
using System.Net;

namespace TeamCityRetryTests.TeamCity.Connection
{
    public class HttpException : Exception
    {
        public HttpStatusCode ResponseStatusCode { get; }
        public string StatusDescription { get; }

        public HttpException(HttpStatusCode responseStatusCode, string message) : base(message)
        {
            ResponseStatusCode = responseStatusCode;
            StatusDescription = message;
        }
    }
}
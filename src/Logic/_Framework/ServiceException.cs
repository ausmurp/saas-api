using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace SaaSApi.Logic.Framework
{

    /// <summary>
    /// Throws service exception to catch by client and expose as an effort for one-spot validation.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="status">Status code to match up with HTTP response codes</param>
    /// <param name="errors">var errors = new Dictionary<string, string[]>() { { "Key", new[] { "Error 1.", "Error 2." } } };</param>
    /// <returns></returns>
    public class ServiceException : Exception
    {
        public int Status { get; private set; }
        public IDictionary<string, string[]> Errors { get; set; }

        public ServiceException(string message, int status = 500, IDictionary<string, string[]> errors = null) : base(message)
        {
            this.Status = status;
            this.Errors = errors;
        }

        public ServiceException(string message, IDictionary<string, string[]> errors = null) : this(message, 500, errors)
        {
        }
    }

    /// <summary>
    /// Throws bad request (400) exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="errors">var errors = new Dictionary<string, string[]>() { { "Key", new[] { "Error 1.", "Error 2." } } };</param>
    /// <returns></returns>
    public class ServiceValidationException : ServiceException
    {
        public ServiceValidationException(string message, IDictionary<string, string[]> errors = null) : base(message, 400, errors)
        {
        }
    }
}
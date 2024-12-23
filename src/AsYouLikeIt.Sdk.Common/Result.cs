
namespace AsYouLikeIt.Sdk.Common
{
    using AsYouLikeIt.Sdk.Common.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AsYouLikeIt.Sdk.Common.Models;
    using System.Text.Json.Serialization;

    public enum MessageTypeOption
    {
        InfoMessage = 0,
        WarningMessage = 2,
        ErrorMessage = 3
    }

    /// <summary>
    /// A common feedback object for returning data to users and present information in the UI.
    /// NOTE: Should always be safe to present to the user and display in the UI.
    /// </summary>
    public class Result
    {
        public bool Success { get; set; }

        /// <summary>
        /// Safe to display in the UI to the public or clients.
        /// </summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ObjectType { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object ReturnKey { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ReturnReferenceKey { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? StatusCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? StatusSubCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string HelpLink { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Optional value used as a correlation to logs using a unique value.
        /// Can be helpful to assign with system info, origination, datetime stamping and other values as part of a derived key scheme.
        /// </summary>
	    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ReferenceId { get; set; }

        [JsonIgnore]
        public Exception Exception { get; set; }

        private bool _hasError;

        public bool HasError
        {
            get
            {
                if (this.Exception != null)
                {
                    return true;
                }
                return _hasError;
            }
            set
            {
                _hasError = value;
            }
        }

        public bool HasErrorShouldSerialize()
        {
            return this.HasError;
        }

        /// <summary>
        /// If completed function without an exception that was
        /// handled in the BLL
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>
        /// The process on the UI should stop and back up 
        /// if this is true
        /// </remarks>
        [JsonIgnore]
        public bool HasHandledException
        {
            get
            {
                if (!this.HasError && this.Exception == null)
                {
                    return false;
                }
                else if (((this.Exception != null)) & (!this.ExceptionHandled))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [JsonIgnore]
        public bool ExceptionHandled { get; set; }

        private ICollection<Result> _subResults;

        /// <summary>
        /// If the action executes multiple statements
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<Result> SubResults
        {
            get
            {
                if (_subResults == null)
                {
                    _subResults = new List<Result>();
                }
                return _subResults;
            }
            set { _subResults = value; }
        }

        public bool SubResultsShouldSerialize()
        {
            return this.SubResults != null && this.SubResults.Any();
        }

        private ICollection<ErrorDetail> _errors;

        /// <summary>
        /// If the action executes multiple statements
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<ErrorDetail> Errors
        {
            get
            {
                if (_errors == null)
                {
                    _errors = new List<ErrorDetail>();
                }
                return _errors;
            }
            set { _errors = value; }
        }

        public bool ErrorsShouldSerialize()
        {
            return this.Errors != null && this.Errors.Any();
        }

        #region .ctors

        public Result()
        {
        }

        public Result(string message, bool hasError = false)
        {
            this.Message = message;
            this.HasError = hasError;
        }

        public Result(string message, bool hasError, int statusCode, int? statusSubCode = null, object returnKey = null)
        {
            this.Message = message;
            this.HasError = hasError;
            this.StatusCode = statusCode;
            this.StatusSubCode = statusSubCode;
            this.ReturnKey = returnKey;
        }

        public Result(Exception ex)
        {
            this.HasError = true;
            this.Exception = ex;

            if (ex is FriendlyException fex)
            {
                if (fex.FriendlyResult != null)
                {
                    this.Message = fex.FriendlyResult.Message;
                }
                else
                {
                    this.Message = ex.Message;
                }
            }
            this.Exception = ex;
        }


        #endregion
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        #region .ctrors

        public Result()
        {
        }

        public Result(string message, T returnObject, bool hasError = false) : base(message, hasError)
        {
            this.Data = returnObject;
        }

        public Result(string message, bool hasError, int statusCode, int? statusSubCode = null, object returnKey = null) : base(message, hasError, statusCode, statusSubCode, returnKey)
        {
        }

        public Result(Exception ex) : base(ex)
        {
        }

        #endregion
    }
}

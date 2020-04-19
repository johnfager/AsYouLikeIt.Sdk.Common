
namespace Sdk.Common
{
    using Newtonsoft.Json;
    using Sdk.Common.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ObjectType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object ReturnKey { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ReturnReferenceKey { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? StatusCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? StatusSubCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HelpLink { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Optional value used as a correlation to logs using a unique value.
        /// Can be helpful to assign with system info, origination, datetime stamping and other values as part of a derived key scheme.
        /// </summary>
	    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
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
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
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
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
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
            //this.ReturnKey = returnKey;
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

            if (object.ReferenceEquals(ex.GetType(), typeof(FriendlyException)))
            {
                var fex = (FriendlyException)ex;
                if ((fex.FriendlyResult != null))
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
        [JsonIgnore]
        [Obsolete("Use 'Data' instead.")]
        public T ReturnObject { get; set; }

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

    public class ErrorDetail
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FieldName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ErrorLine { get; set; }

        public ErrorDetail()
        {
        }

        public ErrorDetail(string fieldName)
        {
            this.FieldName = fieldName;
        }

        public ErrorDetail(string fieldName, string error)
        {
            this.FieldName = fieldName;
            this.Error = error;
        }

        public ErrorDetail(string fieldName, string error, int? errorLine)
        {
            this.FieldName = fieldName;
            this.Error = error;
            this.ErrorLine = errorLine;
        }

    }
}

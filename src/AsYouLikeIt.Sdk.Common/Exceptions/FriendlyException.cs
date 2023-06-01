using System;

namespace AsYouLikeIt.Sdk.Common.Exceptions
{
    [Serializable]
    public class FriendlyException : Exception
    {
        public virtual Result FriendlyResult { get; set; }

        public virtual int? StatusCode => this.FriendlyResult?.StatusCode;

        public virtual int? StatusSubCode => this.FriendlyResult?.StatusSubCode;

        protected string _message;

        public override string Message
        {
            get
            {
                if (this.StatusCode.HasValue && this.StatusSubCode.HasValue)
                {
                    return _message + $" --[{this.StatusCode.Value}.{this.StatusSubCode.Value}]";
                }
                else if (this.StatusCode.HasValue)
                {
                    return _message + $" --[{this.StatusCode.Value}]";
                }
                else
                {
                    return _message;
                }
            }
        }

        private Exception _actualException;
        public Exception ActualException
        {
            get => _actualException;
            set
            {
                this._message = value.Message;
                _actualException = value;
            }
        }

        public FriendlyException() : base()
        {
            this.FriendlyResult = new Result("An unknown error has occurred.", hasError: true);
        }

        public FriendlyException(string message) : base(message)
        {
            this.FriendlyResult = new Result(message, true);
            this._message = message;
        }

        public FriendlyException(Result friendlyResult) : base(friendlyResult?.Message)
        {
            this.FriendlyResult = friendlyResult ?? throw new ArgumentNullException(nameof(friendlyResult));
            this.FriendlyResult.HasError = true;
            _message = this.FriendlyResult.Message;
        }

        public FriendlyException(string message, int statusCode, int? statusSubCode = null) : base(message)
        {
            this.FriendlyResult = new Result(message, true, statusCode, statusSubCode)
            {
                HasError = true
            };
            this._message = message;
        }
    }
}

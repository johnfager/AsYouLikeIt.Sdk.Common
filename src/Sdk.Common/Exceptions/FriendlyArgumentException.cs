
namespace Sdk.Common.Exceptions
{
    using Sdk.Common.Models;
    using System;

    [Serializable]
    public class FriendlyArgumentException : FriendlyException
    {
        public FriendlyArgumentException()
        {
        }

        public FriendlyArgumentException(string fieldName, int? statusSubCode = null) :
            base(new Result($"'{fieldName}' is not valid.", true) { HasError = true, StatusCode = 400, StatusSubCode = statusSubCode })
        {
        }

        public FriendlyArgumentException(string fieldName, string error, int? statusSubCode = null) :
            base(new Result(Dialog.Messages.FriendlyArgumentException, true) { HasError = true, StatusCode = 400, StatusSubCode = statusSubCode })
        {
            base.FriendlyResult.Errors = new[] { new ErrorDetail(fieldName, error) };
        }

        public FriendlyArgumentException(ErrorDetail[] errors, int? statusSubCode = null) :
            base(new Result(Dialog.Messages.FriendlyArgumentException, true) { HasError = true, StatusCode = 400, StatusSubCode = statusSubCode })
        {
            base.FriendlyResult.Errors = errors;
        }
    }
}

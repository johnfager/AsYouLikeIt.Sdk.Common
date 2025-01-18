using System;
using System.Collections.Generic;
using System.Text;

namespace AsYouLikeIt.Sdk.Common.Exceptions
{
    public class ForbiddenAccessException : FriendlyException
    {
        /// <summary>
        /// Generates a 403 with a standard dialog message for the user and a status code.
        /// </summary>
		public ForbiddenAccessException() : base(Dialog.Messages.ForbiddenAccessMessage)
        {
            base.FriendlyResult = new Result(Dialog.Messages.ForbiddenAccessMessage, true) { StatusCode = 403 };
        }

        /// <summary>
        /// Generates a 403 with a standard dialog message for the user and a status code, but with an internal message for logging purposes.
        /// </summary>
        /// <param name="message">Internal message attached to the underlying exception for logging purposes.</param>
		public ForbiddenAccessException(string message) : base(message)
        {
            base.FriendlyResult = new Result(Dialog.Messages.ForbiddenAccessMessage, true) { StatusCode = 403 };
        }

    }
}

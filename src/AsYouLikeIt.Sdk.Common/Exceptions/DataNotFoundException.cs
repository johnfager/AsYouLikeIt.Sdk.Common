
namespace AsYouLikeIt.Sdk.Common.Exceptions
{
    using System;

    /// <summary>
    /// Thrown when a 404 response is desired. Generic information passed in the result to the user while the underlying exception can have an internal message.
    /// </summary>
    [Serializable]
    public class DataNotFoundException : FriendlyException
    {
        /// <summary>
        /// Generates a 404 with a standard dialog message for the user and a 404 status code.
        /// </summary>
		public DataNotFoundException() : base(Dialog.Messages.DataNotFoundMessage)
        {
            base.FriendlyResult = new Result(Dialog.Messages.DataNotFoundMessage, true) { StatusCode = 404 };
        }

        /// <summary>
        /// Generates a 404 with a standard dialog message for the user and a 404 status code, but with an internal message for logging purposes.
        /// </summary>
        /// <param name="message">Internal message attached to the underlying exception for logging purposes.</param>
		public DataNotFoundException(string message) : base(message)
        {
            base.FriendlyResult = new Result(Dialog.Messages.DataNotFoundMessage, true) { StatusCode = 404 };
        }
    }
}


namespace AsYouLikeIt.Sdk.Common
{

    /// <summary>
    /// Contains class names for displaying system messages
    /// </summary>
    /// <remarks></remarks>
    public class Dialog
    {

        /// <summary>
        /// Holds a number of message strings to standardize feedback
        /// </summary>
        /// <remarks></remarks>
        public class Messages
        {

            public static readonly string Duplicate = "Duplicate entry";

            public static readonly string Success = "Success";

            public static readonly string PoisonMessage = "The data could not be processed immediately and was added to a log of failed messages.";

            public static readonly string DataNotFoundMessage = "The requested record could not be located.";

            public static readonly string FriendlyArgumentException = "The request contains format errors.";

            //Update messages
            public static readonly string UpdateInvalidRecord = "Cannot update because the record does not exist in the database";

            public static readonly string UpdateSucceeded = "Record successfully saved";

            //Voided messages
            public static readonly string Voided = "Record successfully voided";

            //Delete messages
            public static readonly string DeleteHasDependenciesError = "Cannot delete record because other items rely on this entry";
            public static readonly string DeleteSucceeded = "Record successfully deleted";

            public static readonly string DeleteBelow = "Click delete below to delete this record";

            //Destroy messages
            public static readonly string DestroySucceeded = "Record successfully removed from database";

            //Cancel message
            public static readonly string CancelSucceeded = "Successfully cancelled";

            //Reported error
            public static readonly string ReportedError = "An error has occurred. Details have been sent the development team so that the issue can be addressed in the future.";

            //Files
            public static readonly string FileSaved = "File successfully saved";

            public static readonly string FileDeleted = "File successfully deleted";

            //Folders
            public static readonly string FolderCreated = "Folder successfully created";

            public static readonly string FolderDeleted = "Folder successfully deleted";

            public static readonly string NoDataFound = "The requested item does not exist.";

        }

        public static string GetStandardMessageFromStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 400:
                    return "Bad request";
                case 401:
                    return "Unauthorized";
                case 402:
                    return "Payment required";
                case 403:
                    return "Forbidden";
                case 404:
                    return "Not found";
                case 405:
                    return "Not implemented";
                case 410:
                    return "Gone";
                case 429:
                    return "Too many requests";
                case 451:
                    return "Unavailable for legal reasons";
                case 500:
                    return "Server error";
                default:
                    return "An unknown error has occurred.";
            }
        }

    }
}

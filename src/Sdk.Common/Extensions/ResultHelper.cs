namespace Sdk.Common.Extensions
{
    using Sdk.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ResultHelper
    {


        /// <summary>
        /// Copies over all data to the more simple ApiResponse that is a returnable POCO.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static ApiResponse ToApiResponse(this Result result)
        {
            var apiReponse = new ApiResponse();
            CopyFromResultToApiResponse(result, apiReponse);
            return apiReponse;
        }

        /// <summary>
        /// Copies over all data to the more simple ApiResponse that is a returnable POCO.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static ApiResponse<T> ToApiResponseWithData<T>(this Result<T> result) where T : new()
        {
            var apiReponse = new ApiResponse<T>()
            {
                Data = result.Data
            };
            CopyFromResultToApiResponse(result, apiReponse);
            return apiReponse;
        }

        public static void CopyFromResultToApiResponse(Result result, ApiResponse apiResponse)
        {
            if (result == null)
            {
                throw new ArgumentException(nameof(result));
            }

            if (apiResponse == null)
            {
                throw new ArgumentException(nameof(apiResponse));
            }

            apiResponse.Succeeded = result.Success;

            if (result.Message != null && result.Message.Contains(Environment.NewLine)) // no new line items can be in the response
            {
                apiResponse.Message = result.Message.Replace(Environment.NewLine, "\n");
            }
            else
            {
                apiResponse.Message = result.Message;
            }
            apiResponse.Succeeded = !result.HasError;
            if (!string.IsNullOrEmpty(result.ObjectType))
            {
                apiResponse.ObjectType = result.ObjectType;
            }
            //apiResponse.RequestReferenceKey = string.IsNullOrWhiteSpace(result.ReturnReferenceKey) ? null : result.ReturnReferenceKey;
            if (result.ReturnKey != null)
            {
                var str = result.ReturnKey.ToString();
                apiResponse.ReturnKey = string.IsNullOrWhiteSpace(str) ? null : str;
            }

            if (result.HasError)
            {
                if (!result.StatusCode.HasValue || result.StatusCode.Value == 0)
                {
                    apiResponse.ErrorCode = 400;
                    apiResponse.Succeeded = false;
                }
                else
                {
                    apiResponse.ErrorCode = result.StatusCode;
                }
                if (result.StatusSubCode.HasValue && result.StatusSubCode.Value != 0)
                {
                    apiResponse.ErrorSubCode = result.StatusSubCode.Value;
                }
            }

            // pass along the links
            apiResponse.RedirectUrl = result.RedirectUrl;
            apiResponse.HelpLink = result.HelpLink;

            if (result.Errors != null && result.Errors.Any())
            {
                // translate to the model error
                var list = new List<ErrorDetail>();
                result.Errors.ToList().ForEach(x => list.Add(new ErrorDetail(x.FieldName, x.Error, x.ErrorLine)));
                apiResponse.Errors = list;
            }

            // handle any subresults
            if (result.SubResults != null && result.SubResults.Any())
            {
                var list = result.SubResults.ToList();
                var arr = new ApiResponse[result.SubResults.Count];
                int length = result.SubResults.Count;
                for (int i = 0; i < length; i++)
                {
                    // We don't copy over the data attribute at this time, but could in the future
                    arr[i] = list[i].ToApiResponse();
                }
                apiResponse.SubResponses = arr;
            }
        }
    }
}

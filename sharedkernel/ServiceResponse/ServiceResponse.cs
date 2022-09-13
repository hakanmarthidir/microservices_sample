using System;
using sharedkernel.Interfaces;

namespace sharedkernel.ServiceResponse
{
    public class ServiceResponse : IServiceResponse
    {
        internal ServiceResponse(string message)
        {
            Status = ResponseStatus.Success;
            Message = message;
        }

        internal ServiceResponse(string message, ErrorCodes error)
        {
            Status = ResponseStatus.Failed;
            Message = message;
            ErrorCode = error;
        }

        public ResponseStatus Status { get; set; }

        public string Message { get; set; }

        public ErrorCodes ErrorCode { get; set; } = ErrorCodes.NO_ERROR;

        public static ServiceResponse Success(string message = "Successfully completed.")
        {
            return new ServiceResponse(message);
        }

        public static ServiceResponse Failure(ErrorCodes error, string message = "Unknown error occurred.")
        {
            return new ServiceResponse(message, error);
        }

    }

    public class ServiceResponse<T> : IServiceResponse<T>
    {
        internal ServiceResponse(T data, string message)
        {
            Status = ResponseStatus.Success;
            Message = message;
            Data = data;
        }

        internal ServiceResponse(T data, string message, ErrorCodes error)
        {
            Status = ResponseStatus.Success;
            Message = message;
            Data = data;
            ErrorCode = error;
        }

        public ErrorCodes ErrorCode { get; set; } = ErrorCodes.NO_ERROR;

        public ResponseStatus Status { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }


        public static ServiceResponse<T> Success(T data, string message = "Successfully completed.")
        {
            return new ServiceResponse<T>(data, message);
        }

        public static ServiceResponse<T> Failure(T data, ErrorCodes error, string message = "An unknown error has occurred while executing the request.")
        {
            return new ServiceResponse<T>(data, message, error);
        }

    }

}


namespace sharedkernel.ServiceResponse
{
    public enum ErrorCodes : int
    {
        NO_ERROR = -1,
        UNKNOWN_ERROR = 0,
        NULL_ARGUMENT = 1,
        ARGUMENT_WITH_WRONGVALUES = 2,
        ARGUMENTS_NOT_MATCH = 3,
        OBJECT_NOT_FOUND = 4,
        MAPPER_OBJECT_NOT_FOUND = 5,
        FILE_NOT_FOUND = 6,
        MULTI_QUERY_RESULT = 7,
        SERVICE_NOT_AVAILABLE = 8,
        INVALID_TOKEN = 9,
        INVALID_REQUEST = 10
    }

}


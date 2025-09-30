namespace Domain.Exceptions;

public class MaxItemQuantityExceededException(string message) : Exception(message);

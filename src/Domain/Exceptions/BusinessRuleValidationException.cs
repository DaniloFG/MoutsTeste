namespace Domain.Exceptions;

public class BusinessRuleValidationException(string message) : Exception(message);

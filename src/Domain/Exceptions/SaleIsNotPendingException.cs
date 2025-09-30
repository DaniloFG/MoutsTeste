namespace Domain.Exceptions;

public class SaleIsNotPendingException(string message) : Exception(message);

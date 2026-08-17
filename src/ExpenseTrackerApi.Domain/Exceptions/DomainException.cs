namespace ExpenseTrackerApi.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}

public class InvalidBudgetOperationException : Exception
{
    public InvalidBudgetOperationException(string message) : base(message) { }

    public InvalidBudgetOperationException(string message, Exception innerException)
        : base(message, innerException) { }
}

public class DuplicateBudgetException : Exception
{
    public DuplicateBudgetException(string message) : base(message) { }
}

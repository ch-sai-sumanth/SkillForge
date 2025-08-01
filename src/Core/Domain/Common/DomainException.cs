namespace Domain.Common;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class InvalidDomainOperationException : DomainException
{
    public InvalidDomainOperationException(string message) : base(message) { }
}

public class BusinessRuleViolationException : DomainException
{
    public BusinessRuleViolationException(string rule, string message) : base($"Business rule violation: {rule}. {message}")
    {
        Rule = rule;
    }

    public string Rule { get; }
}
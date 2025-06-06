using FluentValidation.Results;

namespace Core.Commons;

public class Output
{
    private readonly List<string> _messages = new();
    private List<string> _errorMessages = new();

    public IReadOnlyCollection<string>? ErrorMessages => _errorMessages?.AsReadOnly();
    public bool IsValid { get; private set; }
    public IReadOnlyCollection<string>? Messages => _messages?.AsReadOnly();
    public object? Result { get; private set; }

    public Output() => IsValid = true;

    public Output(object result)
    {
        IsValid = true;
        AddResult(result);
    }

    public Output(ValidationResult validationResult) => ProcessValidationResults(validationResult);
    public Output(IEnumerable<ValidationResult> validationResults) => ProcessValidationResults(validationResults.ToArray());

    private void ProcessValidationResults(params ValidationResult[] validationResults)
    {
        foreach (ValidationResult validationResult in validationResults)
            AddValidationResult(validationResult);

        VerifyValidty();
    }

    private void VerifyErrorMessages(ValidationResult validationResult)
    {
        _errorMessages ??= new List<string>();
        _errorMessages.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));
    }

    private void VerifyValidty()
    {
        IsValid = ErrorMessages?.Count == 0;
    }

    public void AddErrorMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentException("message is null");

        _errorMessages.Add(message);
        VerifyValidty();
    }

    public void AddErrorMessages(params string[] messages)
    {
        foreach (var text in messages)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("message is null");

            _errorMessages.Add(text);
        }

        VerifyValidty();
    }

    public void AddMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentException("message is null");

        _messages.Add(message);
    }

    public void AddMessages(params string[] messages)
    {
        foreach (var text in messages)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("message is null");

            _messages.Add(text);
        }
    }

    public void AddResult(object result)
    {
        Result = result ?? throw new NullReferenceException();
        IsValid = true;
    }

    public void AddValidationResult(ValidationResult validationResult)
    {
        VerifyErrorMessages(validationResult);
    }

    public T? GetResult<T>() => (T?)Result;

    public void SetToInvalid() => IsValid = false;
}

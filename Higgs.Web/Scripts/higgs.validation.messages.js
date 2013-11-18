// TODO: Generate from default validation message.
(function()
{
    var messages =
    {
        email_error : '\'{PropertyName}\' is not a valid email address.',
        greaterthanorequal_error : '\'{PropertyName}\' must be greater than or equal to \'{ComparisonValue}\'.',
        greaterthan_error : '\'{PropertyName}\' must be greater than \'{ComparisonValue}\'.',
        length_error : '\'{PropertyName}\' must be between {MinLength} and {MaxLength} characters. You entered {TotalLength} characters.',
        lessthanorequal_error : '\'{PropertyName}\' must be less than or equal to \'{ComparisonValue}\'.',
        lessthan_error : '\'{PropertyName}\' must be less than \'{ComparisonValue}\'.',
        notempty_error : '\'{PropertyName}\' should not be empty.',
        notequal_error : '\'{PropertyName}\' should not be equal to \'{PropertyValue}\'.',
        notnull_error : '\'{PropertyName}\' must not be empty.',
        predicate_error : 'The specified condition was not met for \'{PropertyName}\'.',
        regex_error : '\'{PropertyName}\' is not in the correct format.',
        equal_error : '\'{PropertyName}\' should be equal to \'{PropertyValue}\'.',
        exact_length_error : '\'{PropertyName}\' must be {MaxLength} characters in length. You entered {TotalLength} characters.',
        inclusivebetween_error : '\'{PropertyName}\' must be between {From} and {To}. You entered {Value}.',
        exclusivebetween_error : '\'{PropertyName}\' must be between {From} and {To} (exclusive). You entered {Value}.',
        range_error: '{PropertyName} should has value betwen {MinValue} - {MaxValue}.'
    };
    
    window.ValidationMessage = messages;
})();
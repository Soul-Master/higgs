/// <reference path="../jquery-1.7.1-vsdoc.js"/>
/// <reference path="../jquery-cookie-1.0.js" />
/// <reference path="Higgs.core.js" />
/// <reference path="higgs.validation.messages.js" />

function iValidation()
{
    this.errorMessageFn = function (control)
    {
        throw 'Error message is not be defined.';
    };
    this.isValid = function ()
    {
        throw 'Validation function is not be defined.';
    };
    iValidation.prototype.errorMsg = function (msg)
    {
        ///	<summary>
        ///		 Set error message for current validation.
        ///	</summary>
        ///	<param name="msg" type="String">
        ///		Error message to be displayed when error occurs.
        ///	</param>

        if (msg)
        {
            this.errorMessageFn = function (control)
            {
                return msg;
            };

            return this;
        }
    };
}

requiredValidation.prototype = new iValidation();
function requiredValidation()
{
    this.isValid = function (bindedProperty, obj)
    {
        obj = $(obj);
        var currentValue = obj.value();

        return currentValue !== null && currentValue !== '';
    };
    this.errorMessageFn = function (control)
    {
        return ValidationMessage.notnull_error;
    };
}

notEmptyValidation.prototype = new iValidation();
function notEmptyValidation()
{
    this.isValid = function (bindedProperty, obj)
    {
        obj = $(obj);
        var currentValue = obj.value();

        if (typeof currentValue === 'string')
        {
            return currentValue !== null && currentValue !== '' && currentValue !== emptyGuid;
        }
        else if (typeof currentValue === 'number')
        {
            return currentValue !== 0;
        }
        else if (typeof currentValue === 'boolean')
        {
            return currentValue;
        }
        else
        {
            return currentValue !== null;
        }
    };
    this.errorMessageFn = function (control)
    {
        return ValidationMessage.notempty_error;
    };
}

stringLengthValidation.prototype = new iValidation();
function stringLengthValidation(minLength, maxLength)
{
    if (!maxLength)
    {
        maxLength = minLength;
        minLength = null;
    }
    
    this.MinLength = minLength;
    this.MaxLength = maxLength;
    this.isValid = function (bindedProperty, obj)
    {
        obj = $(obj);
        var currentValue = obj.value();

        if(!currentValue)
            return true;

        return currentValue.length <= maxLength && currentValue.length >= minLength;
    };
    this.errorMessageFn = function (control)
    {
        return ValidationMessage.length_error.format(this, { totalLength: control.value().length });
    };
}

emailValidation.prototype = new iValidation();
function emailValidation()
{
    var reg = new RegExp(/[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/ig);
    
    this.isValid = function (bindedProperty, obj)
    {
        obj = $(obj);
        var currentValue = obj.value();

        if (currentValue)
        {
            reg.lastIndex = 0;
            var result = reg.exec(currentValue);
            
            return result && result.length === 1 && result[0].length === currentValue.length;
        }
        else
        {
            return true;
        }
    };    
    this.errorMessageFn = function (control)
    {
        return ValidationMessage.email_error;
    };
}

regularExpressionValidation.prototype = new iValidation();
function regularExpressionValidation(pattern)
{
    var reg = pattern.constructor === RegExp ? pattern : new RegExp(pattern);
    
    this.pattern = pattern;
    this.isValid = function (bindedProperty, obj)
    {
        obj = $(obj);
        var currentValue = obj.value();

        if (currentValue)
        {
            reg.lastIndex = 0;
            
            var result = reg.exec(currentValue);
            
            return result && result.length > 0 && result[0].length === currentValue.length;
        }
        else
        {
            return true;
        }
    };
}

rangeValidation.prototype = new iValidation();
function rangeValidation(minValue, maxValue)
{
    this.minValue = minValue;
    this.maxValue = maxValue;
    this.isValid = function (bindedProperty, obj)
    {
        obj = $(obj);
        var data = obj.value();
        if (isNullOrEmpty(data)) return true;
        
        var currentValue = parseFloat(data.toString().replace(/,/g, ''));

        return currentValue >= minValue && currentValue <= maxValue;
    };

    this.errorMessageFn = function (control)
    {
        return ValidationMessage.range_error.format(this);
    };
}

greaterThanValidation.prototype = new iValidation();
function greaterThanValidation(comparisonValue)
{
    this.comparisonValue = comparisonValue;
    this.isValid = function (bindedProperty, obj)
    {
        obj = $(obj);
        var data = obj.value();
        if (isNullOrEmpty(data)) return true;
        
        var currentValue = parseFloat(data.toString().replaceAll(','));

        return currentValue > comparisonValue;
    };

    this.errorMessageFn = function (control)
    {
        return ValidationMessage.greaterthan_error.format(this);
    };
}

greaterThanOrEqualValidation.prototype = new iValidation();
function greaterThanOrEqualValidation(comparisonValue)
{
    this.comparisonValue = comparisonValue;
    this.isValid = function (bindedProperty, obj)
    {
        obj = $(obj);
        var data = obj.value();
        if (isNullOrEmpty(data)) return true;
        
        var currentValue = parseFloat(data.toString().replaceAll(','));

        return currentValue >= comparisonValue;
    };

    this.errorMessageFn = function (control)
    {
        return ValidationMessage.greaterthanorequal_error.format(this);
    };
}

lessThanValidation.prototype = new iValidation();
function lessThanValidation(comparisonValue)
{
    this.comparisonValue = comparisonValue;
    this.isValid = function (bindedProperty, obj) {
        obj = $(obj);
        var data = obj.value();
        if (isNullOrEmpty(data)) return true;

        var currentValue = parseFloat(data.toString().replaceAll(','));

        return currentValue < comparisonValue;
    };

    this.errorMessageFn = function (control) {
        return ValidationMessage.lessthan_error.format(this);
    };
}

lessThanOrEqualValidation.prototype = new iValidation();
function lessThanOrEqualValidation(comparisonValue) {
    this.comparisonValue = comparisonValue;
    this.isValid = function (bindedProperty, obj) {
        obj = $(obj);
        var data = obj.value();
        if (isNullOrEmpty(data)) return true;

        var currentValue = parseFloat(data.toString().replaceAll(','));

        return currentValue <= comparisonValue;
    };

    this.errorMessageFn = function (control) {
        return ValidationMessage.lessthanorequal_error.format(this);
    };
}

equalValidation.prototype = new iValidation();
function equalValidation(compareTo)
{
    this.compareTo = compareTo;
    this.isValid = function (bindedProperty, obj)
    {
        obj = $(obj);
        var currentValue = obj.value();

        if (typeof compareTo === 'object')
        {
            return compareTo.value === currentValue;
        }
        else
        {
            return compareTo == currentValue;
        }
    };

    this.errorMessageFn = function (control)
    {
        if (typeof compareTo === 'object')
        {
            return ValidationMessage.equal_error.format(this, { propertyValue: $(compareTo.obj).data('higgs.name') });
        }
        else
        {
            return compareTo;
        }
    };
}

collectionCountValidation.prototype = new iValidation();
function collectionCountValidation(minLength, maxLength)
{
    this.MinLength = minLength;
    this.MaxLength = maxLength;
    this.isValid = function (bindedProperty, obj)
    {
        if ($.isArray(bindedProperty.value))
        {
            var count = bindedProperty.value.length;

            return count >= minLength && (!maxLength || count <= maxLength);
        }
        
        return false;
    };
}
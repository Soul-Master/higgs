/// <reference path="../jquery-1.7.1-vsdoc.js"/>
/// <reference path="../jquery-cookie-1.0.js" />
/// <reference path="Higgs.core.js" />

(function (module, $, undefined)
{
    
function orderByTabIndex(bindingInfo)
{
    var list = [];
    var cIndex = 1;

    for (var propertyName in bindingInfo)
    {
        if (!bindingInfo.hasOwnProperty(propertyName)) continue;
            
        var cTabIndex = parseInt( $( bindingInfo[propertyName].obj ).attr( 'tabindex' ), 10 ) || cIndex++;

        if ( list.length === 0 )
        {
            list.push(bindingInfo[propertyName]);
        }
        else
        {
            for ( var j = 0; j < list.length; j++ )
            {
                var tabIndex = parseInt( $( list[j].obj ).attr( 'tabindex' ), 10) || 0;

                if ( cTabIndex <= tabIndex )
                {
                    list.splice( j, 0, bindingInfo[propertyName] );
                    break;
                }
                else if ( list.length - 1 === j )
                {
                    list.push( bindingInfo[propertyName] );
                    break;
                }
            }
        }
    }

    return list;
}

//#region Common Function & Setting

function validateRules(obj, bindedProperty, fnShowError)
{
    var rules = bindedProperty.rules;

    if (!rules || rules.length === 0) return true;

    var x = $(obj);
    var name = bindedProperty.name || x.data('higgs.name');
    
    if(!name) higgs.error('Unable to find name for "' + (x.attr('id') || x.attr('name')) + '"');
    
    for (var i = 0; i < rules.length; i++)
    {
        if (!rules[i] || rules[i].constructor !== iValidation)
        {
            higgs.error('Validation rule for "' + (x.attr('name') || x.attr('id')) + '" is undefined, null or not support by validation.');
        }
        
        if (!rules[i].isValid(bindedProperty, x))
        {
            var msg;

            if (rules[i].errorMessageFn)
            {
                msg = rules[i].errorMessageFn.call(rules[i], x).format({ name: name, 'PropertyName': name });
            }
            else
            {
                msg = x.data(getTypeName(rules[i].constructor)).format(name);
            }

            fnShowError && fnShowError(obj, msg, rules[i].constructor);
            x.addClass('error')
                .trigger('validationFail', [typeof bindedProperty.validationOutput !== 'undefined' ? bindedProperty.validationOutput : x, msg]);
            bindedProperty.isValid = false;
            
            return false;
        }
    }
    
    x.removeClass('error');
    if(bindedProperty.isValid === false)
    {
        bindedProperty.isValid = true;
        fnShowError && fnShowError(obj);
        x.trigger('validationSuccess', [typeof bindedProperty.validationOutput !== 'undefined' ? bindedProperty.validationOutput : x]);
    }
    
    return true;
}
    
function isRequiredProperty(bindedProperty, obj)
{
    if (!bindedProperty || !bindedProperty.rules) return false;
    if (obj.is(':checkbox')) return false;

    for (var i = 0; i < bindedProperty.rules.length; i++)
    {
        if (bindedProperty.rules[i] instanceof requiredValidation || bindedProperty.rules[i] instanceof notEmptyValidation)
            return true;
    }

    return false;
}

//#endregion

//#region jQuery - Validation Plug-in

var bindedObject = function ( obj, property, fn )
{
    this.obj = obj;
    this.bindedProperty = property;
    this.bindedProperty.obj = obj;
    this.fnShowError = fn;
};

$.fn.bindData = function(bindedProperty, dataName, fnShowError)
{
    var x = $( this );
    
    if ( x.length === 0 ) throw new Error('Unable to find binded control');

    var form = x.parents('form');
    if (dataName === undefined && x[0].id)
    {
        var label = $( 'label[for=' + x[0].id + ']' );
        
        if(label.length === 1)
        {
            dataName = label.text().replace( /(^[ ]*[:\*]?[ ]*)|([ ]*[:\*]?[ ]*$)/gm, '' );
        }
        else if(label.length > 1)
        {
            higgs.error('Duplicate label for control id \"' + x[0].id + '"');
        }
    }
    
    if (!bindedProperty.hasOwnProperty('value'))
    {
        bindedProperty.value = x.value();
    }
    else if (x.is(':input'))
    {
        var currentValue = bindedProperty.value;
        currentValue = (currentValue !== null && currentValue !== undefined) ? bindedProperty.value : ((bindedProperty.defaultValue != null && bindedProperty.defaultValue !== undefined) ? bindedProperty.defaultValue : '');

        x.value(currentValue);
    }

    x.change(function()
    {
        if (bindedProperty.rules && !x.is(':radio'))
        {
            if (!validateRules(this, bindedProperty, fnShowError)) return;
        }

        if (x.is(':input'))
        {
            var oldValue = bindedProperty.value;
            var newValue = x.value();
            bindedProperty.value = newValue;

            if (newValue != oldValue)
            {
                bindedProperty.onChange();
            }
        }
        else
        {
            bindedProperty.onChange();
        }
    });

    if (!x.data('higgs.name')) x.data('higgs.name', dataName);

    // Save binding information for validating on form saving.
    var bindingInfo = form.data('higgs.bindingInfo');
    if (!bindingInfo)
    {
        bindingInfo = { };
        form.data('higgs.bindingInfo', bindingInfo);
    }
    
    bindingInfo[x.attr('name') || x.attr('id')] = new bindedObject(x, bindedProperty, fnShowError);
    setupValidateForm.call( form, bindingInfo );
    
    // set style for required label
    if (isRequiredProperty(bindedProperty, x))
    {
        if (x.is(':radio'))
        {
            $('label[for=' + x.selector.split(' ')[0].substring(1) + ']').addClass('required');
        }
        else
        {
            $('label[for=' + x.attr('id') + ']').addClass('required');
        }
    }

    return x;
};

$.fn.autoBindModel = function(model, validateModel)
{
    var form = $( this );
    var bindingInfo = form.data('higgs.bindingInfo');
    var controls = $(':input, input[type=hidden]', form).not('button,input[type=submit],input[type=button]');
    
    for ( var p in model )
    {
        if(!model.hasOwnProperty(p) || model[p].obj) continue;

        var currentControl = controls.filter( '[name=' + p + ']' );

        if ( currentControl.length === 0 ) continue;
        if( bindingInfo && bindingInfo[currentControl[0].id]) continue;
        
        currentControl.bindData( model[p] );
    }
    
    // Validate Model
    validateModel = validateModel === undefined ? true : validateModel;
    if(validateModel)
    {
        for (var p in model)
        {
            if (!model.hasOwnProperty(p)) continue;

            if (!model[p].obj)
            {
                throw Error('Property name "' + p + '" is not binded to any control.');
            }
        }
    }

    return this;
};
    
function setupValidateForm(bindingInfo)
{
    var form = $( this );
    
    form
        .unbind( 'beforeSubmit.higgs-validation' )
        .bind( 'beforeSubmit.higgs-validation', null,  function (e)
        {
            try
            {
                if(validateRule(bindingInfo))
                {
                    form.trigger( 'validationSuccess' );
                    
                    return true;
                }
            }
            catch(err)
            {
                var errMessage = 'Unexpected error occured while validating ' + (form.attr('id') || '') + ' form';
                
                if(console && console.error)
                {
                    console.error(errMessage);
                }
                else
                {
                    alert(errMessage);
                }
            } 
                
            return false;
        });
}
    
function validateRule(bindingInfo)
{
    var orderedControls = orderByTabIndex(bindingInfo);
            
    for ( var i = 0; i < orderedControls.length; i++ )
    {
        if ( !orderedControls[i].bindedProperty.rules )
        {
            orderedControls[i].obj.blur();
            continue;
        }

        var validationOutputControl = typeof orderedControls[i].bindedProperty.validationOutput !== 'undefined' ? orderedControls[i].bindedProperty.validationOutput : orderedControls[i].bindedProperty.obj;
        var currentControl = orderedControls[i].obj;

        if ( !currentControl.is( ':disabled' ))
        {
            currentControl.change();
            currentControl.blur();
        }

        if ( validationOutputControl.is( '.error' ) )
        {
            for ( var j = i; j < orderedControls.length; j++ )
            {
                orderedControls[j].obj.blur();
            }

            return false;
        }
    }

    return true;
}

$.fn.bindValidationOutput = function(bindedProperty, dataName, fnShowError)
{
    var x = $(this);

    bindedProperty.validationOutput = x;

    // set style for required label
    if (isRequiredProperty(bindedProperty, x))
    {
        $('label[for=' + x.attr('id') + ']').addClass('required');
    }

    return x;
};

higgs.processModel = function(model)
{
    for (var x in model)
    {
        if (!model.hasOwnProperty(x)) continue;

        var property = model[x];

        if ( property.rules )
        {
            for(var i = 0; i < property.rules.length; i++)
            {
                if ( !$.isFunction( property.rules[i] ) ) continue;

                // Automatically create rule from validation type.
                property.rules[i] = new property.rules[i]();
            }
        }
        property._changeEvents = new Array();
        property.onChange = function ( fn )
        {
            if (typeof fn !== 'undefined' && $.isFunction(fn))
            {
                this._changeEvents.push(fn);
            }
            else
            {
                if (this._changeEvents === undefined) return;

                for (var i = 0; i < this._changeEvents.length; i++)
                {
                    this._changeEvents[i](this.value);
                };
            }
        };
    }
};

$.fn.mappingResult = function (result, bindedModel)
{
    var form = $(this);
    var formName = form.attr('id');
    var bindingInfo = form.data('higgs.bindingInfo');
    var replaceValue = function (oldValue)
    {
        var newValue = oldValue[0];
        // fix Google Chrome bug.
        var matches = newValue.match(/[{][^}]*[}]/gm);

        if (!matches) return newValue;

        for (var i = 0; i < matches.length; i++)
        {
            if (!matches[i] || typeof matches[i] !== "string")
                continue;

            var m = matches[i].remove(' ', '{', '}');
            var bindedProperty = bindedModel[m];

            if (bindedProperty)
            {
                for (var j in bindingInfo)
                {
                    if (bindingInfo[j].bindedProperty === bindedProperty)
                    {
                        var realValue = bindingInfo[j].obj.data('higgs.name');

                        if (realValue)
                        {
                            newValue = newValue.replaceAll(matches[i], realValue);
                        }
                        break;
                    }
                }
            }
        }

        return newValue;
    };

    for (var k in result.ErrorList)
    {
        result.ErrorList[k] = replaceValue(result.ErrorList[k]);
    }
};

//#endregion

} (jQuery.higgs.validation = jQuery.higgs.validation || {}, jQuery));
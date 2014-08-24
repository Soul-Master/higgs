/*!
* Higgs Library for jQuery v1.0.0
*
* Copyright 2014 Soul_Master
* Released under the MIT license
*
* Date: @currentTime
*/

module Higgs
{
    export class AjaxResult
    {
        errorList: any;
        isSuccess: boolean;
        redirectTo: string;
        data: any;
    }
    
    export function escapeRegex(text)
    {
        if (!text) return text.toString();

        return text.toString().replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
    };

    export var disableElementSelect = ':input, button, .btn';
}

// #region Object extension methods

interface String 
{
    replaceAll(findString: string, newString?: string, isIgnorCase?: Boolean): string;
    remove(findString: string): string;
    startsWith(value: string, isIgnoreCase?: boolean): boolean;
    endsWith(value: string, isIgnoreCase?: boolean): boolean;
    toCamelCase(): string;
    padLeft(length:number, padChar: string): string;
}

String.prototype.replaceAll = function (findString: string, newString?: string, isIgnorCase?: Boolean)
{
    var temp = this;

    return temp.replace(new RegExp(Higgs.escapeRegex(findString), isIgnorCase ? 'ig' : 'g'), newString || '');
};

String.prototype.remove = function (findString)
{
    var temp = this;
    for (var i = 0; i < arguments.length; i++)
    {
        temp = temp.replaceAll(arguments[i]);
    }

    return temp;
};

String.prototype.startsWith = function (value, isIgnoreCase = false)
{
    if (!isIgnoreCase) return this.indexOf(value) === 0;

    return this.toUpperCase().startsWith(value.toUpperCase());
};

String.prototype.endsWith = function (value, isIgnoreCase = false)
{
    if (!isIgnoreCase) return this.indexOf(value) === this.length - value.length;

    return this.toUpperCase().endsWith(value.toUpperCase());
};

String.prototype.toCamelCase = function()
{
    return this.substring(0, 1).toLowerCase() + this.substring(1);
};

String.prototype.padLeft = function (length:number, padChar: string)
{
    padChar = padChar ? padChar : '0';
    var temp = this + '';

    while (temp.length < length)
    {
        temp = padChar + temp;
    }

    return temp;
};

// #endregion

//#region JQuery static functions

interface JQueryStatic 
{
    newGuid(): string;
    baseUrl(): string;
    getUrl(logicalPath?: string): string;
    getObject(parts: string): any;
    getObject(parts: string, defaultValue: any): any;
    getObject(parts: string, defaultValue: any, context?: Object): any;
    setObject(path: string, value: any, root: Object): any;
    postify(value: Object): string;
    sendData(url: string, data: Object, isNewWindow?: boolean)
    sendData(url: string, data: Object, method?: string, isNewWindow?: boolean)
    sendData(url: string, data: string, isNewWindow?: boolean)
    sendData(url: string, data: string, method?: string, isNewWindow?: boolean)
}

(function($)
{
    var baseUrl: string;

    $.newGuid = function() : string
    {
        // Return rfc4122 version 4 compliant GUID
        // Code From: http://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid-in-javascript

        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) 
        {
            var r = Math.random()*16|0, v = c == 'x' ? r : (r&0x3|0x8);
            return v.toString(16);
        });
    };
    
    $.baseUrl = function(url: string): string
    {
        if (url)
        {
            baseUrl = url;
        }

        return baseUrl;
    };

    $.getUrl = function(logicalPath?: string): string
    {
        if (!logicalPath) return location.href;
        if ((/^[a-z-]+:\/\//).test(logicalPath) || !baseUrl) return logicalPath;

        if (logicalPath.indexOf('~/') < 0) return (location.href.substring(0, location.href.indexOf(location.pathname))) + '/' + (logicalPath.indexOf('/') == 0 ? logicalPath.substring(1) : logicalPath);

        return baseUrl + logicalPath.substring(2);
    }

    $.getObject = function (parts: any, defaultValue: any, context?: Object)
    {
      if (typeof parts === 'string') {
            // Support array index path without JavaScript syntax validation.
            // foo[0].bar

            parts = parts.match(/[0-9a-zA-Z_$]+/g);
        }
      
        context = context || window;
      
        var p;
      
        while (context && parts.length) {
            p = parts.shift();

            if (context[p] === undefined) {
                return defaultValue;
            }

            context = context[p];
        }
      
        return context;
    };
    
    $.setObject = function (name: string, value: any, context: Object)
    {
        var segments = name.split('.');
        var cursor = context || window;
        var segment;

        for (var i = 0; i < segments.length - 1; ++i)
        {
            segment = segments[i];
            cursor = cursor[segment] = cursor[segment] || {};
        }

        return cursor[segments[i]] = value;
    };
    
    $.postify = function (value: Object)
    {
        var result = {};

        var buildResult = function (object, prefix)
        {
            for (var key in object)
            {

                var postKey = isFinite(key)
                    ? (prefix != "" ? prefix : "") + "[" + key + "]"
                    : (prefix != "" ? prefix + "." : "") + key;

                switch (typeof (object[key]))
                {
                    case "number": case "string": case "boolean":
                        result[postKey] = object[key];
                        break;

                    case "object":
                        if (object[key] && object[key].toUTCString)
                            result[postKey] = object[key].toUTCString().replace("UTC", "GMT");
                        else
                        {
                            buildResult(object[key], postKey != "" ? postKey : key);
                        }
                }
            }
        };

        buildResult(value, "");

        return result;
    };
    
    // Original code for http://www.filamentgroup.com/lab/jquery_plugin_for_requesting_ajax_like_file_downloads/
    $.sendData = function (url: string, data: any, method?: string, isNewWindow?: boolean)
    {
        if (arguments.length === 3 && typeof arguments[2] === 'boolean')
        {
            isNewWindow = arguments[2];
            method = undefined;
        }

        if (isNewWindow === undefined) isNewWindow = true;

        if (url.indexOf('~/') === 0) url = $.getUrl(url);
        var form = $('<form />');
        form.attr('id', 'form' + (new Date()).getTime());
        form.attr('action', url);
        form.attr('method', method || 'post');

        if (isNewWindow)
        {
            form.attr('target', '_blank');
        }

        if (data)
        {
            if (typeof data == 'string')
            {
                //split params into form inputs
                $.each(data.split('&'), function ()
                {
                    var pair = this.split('=');
                    $('<input type="hidden" />').attr
                    ({
                        name: pair[0],
                        value: pair[1]
                    }).appendTo(form);
                });
            }
            else
            {
                $.each(data, function (key, value)
                {
                    $('<input />').attr
                    ({
                        type: 'hidden',
                        name: key,
                        value: value + ''
                    }).appendTo(form);
                });
            }
        }

        //send request
        form.appendTo('body').submit();

        if (!isNewWindow)
        {
            form.remove();
        }
    };
})(jQuery);

//#endregion

//#region JQuery functions

interface JQuery
{
    ensureId(): JQuery;
    getEvents(): any;
    disable(isDisabled?: boolean) : JQuery;
    value(): any;
    value(data: any): JQuery;
    submitForm(data?: Object): JQueryXHR;
}

(function(fn)
{
    fn.ensureId = function()
    {
        $(this).each(function()
        {
            var el = $(this);

            if (!el.attr('id')) el.attr('id', $.newGuid());
        });

        return this;
    };

    fn.getEvents = function()
    {
        return jQuery['_data'](this[0], "events" );
    };

    fn.disable = function(isDisabled?: boolean): JQuery
    {
        if (isDisabled === undefined)
        {
            isDisabled = true;
        }

        var controls = $(this);
        controls = !controls.is(Higgs.disableElementSelect) ? controls.find(Higgs.disableElementSelect) : controls;
        controls.prop('disabled', isDisabled);
        controls.toggleClass('disabled', isDisabled);
        controls.trigger(isDisabled ? 'disabled' : 'enabled');
        
        return this;
    };

    // TODO: refactor this.
    fn.value = function(value?: any)
    {
        var x = this;

        if (value === undefined)
        {
            value = null;

            if (x.is(':checkbox'))
            {
                if (x.length === 1)
                {
                    value = getChkValue.call(x);
                } else
                {
                    value = [];

                    for (var i = 0; i < x.length; i++)
                    {
                        var chk = $(x[i]);

                        if (!chk.is(':checked')) continue;

                        value.push(getChkValue.call(chk));
                    }
                }
            } else if (x.is(':radio'))
            {
                if (x.length == 1)
                {
                    x = x.parents('form').find(':radio[name=' + x[0].name + ']');
                }

                x.each(function()
                {
                    if (this.checked)
                    {
                        value = this.value;
                    }
                });
            } else
            {
                var markText = x.data('watermark');
                value = x.val();

                if (value === markText) value = '';
            }

            return value;
        } else
        {
            var compareValue = function(value1, value2)
            {
                if (value1 === undefined || value1 === null)
                {
                    value1 = '';
                } else
                {
                    value1 = value1.toString().toLowerCase();
                }
                if (value2 === undefined || value2 === null)
                {
                    value2 = '';
                } else
                {
                    value2 = value2.toString().toLowerCase();
                }

                return value1 === value2;
            };

            if (x.is(':radio'))
            {
                if (x.length == 1)
                {
                    x = x.parents('form').find(':radio[name=' + x[0].name + ']');
                }

                x.each(function()
                {
                    if (compareValue(this.value, value))
                    {
                        $(this).attr('checked', 'on');
                    } else
                    {
                        $(this).removeAttr('checked');
                    }
                });
            } else if (x.is(':checkbox'))
            {
                if (this[0].value && typeof value !== 'boolean')
                {
                    value = this[0].value === value;
                }

                if (value)
                    x.attr('checked', 'on');
                else
                    x.removeAttr('checked');
            } else if (x.is('select'))
            {
                var selectedValue;

                $('option', x).each(function()
                {
                    var cValue = $(this).val();

                    if (compareValue(cValue, value)) selectedValue = cValue;
                });

                x.val(selectedValue === undefined ? $('option:eq(0)', x).val() : selectedValue);
            } else
            {
                x.val(value);
            }

            return this;
        }

        function getChkValue()
        {
            var value = this.val();

            if (this[0].value !== 'on' && this[0].value)
            {
                return this.is(':checked') ? this[0].value : null;
            }

            return value === 'on' ? this.is(':checked') : value;
        }
    };

    fn.submitForm = function(data?: Object)
    {
        var form = $(this);
        if (!data) data = form.serialize(true);
        
        form.disable();

        return $.ajax
        ({
            type: 'POST',
            dataType: 'json',
            url: $.getUrl(form.attr('action')),
            data: $.postify(data)
        });
    };
})(jQuery.fn);

//#endregion

//#region Model Validation

interface JQuery
{
    serialize(serializeAsObject: boolean): Object;
    serialize(options: Object): Object;
    deserialize(data: Object): JQuery;
    getValidationModel(modelData?: Object): Object;
    invalidateValidateCache(): JQuery;
    validateModel(options?: Higgs.ValidateOptions) : Array<Higgs.ValidationResult>;
    getValidatedModel(): any;
}

module Higgs
{
    export interface IValidationRuleArray extends Array<AbstructValidation>
    {

    }

    export class AbstructValidation
    {
        constructor()
        constructor(param1: any)
        constructor(param1: any, param2: any)
        constructor(param1: any, param2: any, param3: any)
        constructor(param1?: any, param2?: any, param3?: any)
        {
        }

        validate(context: Higgs.ValidateContext<Object>, value: any): boolean
        {
            throw new Error('not implemented method');
        }

        message(context: Higgs.ValidateContext<Object>, value: any): string
        {
            throw new Error('not implemented method');
        }

        orderIndex: number = 0;
    }

    export class ValidationResult
    {
        constructor(path: string, message: any)
        constructor(validationRule: Higgs.AbstructValidation, context: Higgs.ValidateContext<Object>, path: string, value: any)
        constructor(validationRule: any, context: any, path?: string, value?: any)
        {
            if (path)
            {
                validationRule = <Higgs.AbstructValidation>validationRule;
                this.context = <Higgs.ValidateContext<Object>>context;

                this.message = validationRule.message(context, value);
                this.path = path;
                this.context = context;
            }
            else
            {
                this.path = <string>validationRule;
                this.message = <string>context;
                this.context = <Higgs.ValidateContext<Object>>context;
            }
        }

        message: string;
        path: string;
        context: Higgs.ValidateContext<Object>;
    }

    export class SerializeOptions
    {
        ignoreEmptyValue: boolean = true;

        static defaultValue : SerializeOptions = new SerializeOptions();
    }

    export class ValidateOptions
    {
        cacheValidationObject: boolean = true;
        ignoreValidation: boolean = false;
        stopOnFirstInvalid: boolean = true;
        enableFadeInFeedback: boolean = true;
        validateTypes: {};
        validationContainer: JQuery;

        static defaultValue : ValidateOptions = new ValidateOptions();
    }
    
    export class ValidateContext<T>
    {
        constructor(parent: T, container: JQuery)
        {
            this.parent = parent;
            this.validationContainer = container;
        }

        parent: T;
        validationContainer: JQuery;
    }
    
    export class RuleData 
    {
        name: string;
        path: string;
        modelType: Function;
        validation: { new (): any };
    }

    export function isEmpty(value)
    {
        return value === undefined || value != null && value.constructor === String && value === '';
    }
    
    var validationRules = <Array<RuleData>>[];
    
    export module Rules
    {
        export function register(name: string, rule: Function)
        export function register(name: string, rule: Function, path: string)
        export function register(name: string, rule: Function, modelType: Function)
        export function register(name: string, rule: Function, modelType: Function, path: string)
        export function register(name: string, rule: Function, modelType?: any, path?: string)
        {
            if (typeof modelType === 'string')
            {
                path = <string>modelType;
                modelType = undefined;
            }

            var data = new RuleData();
            data.name = name;
            data.path = path;
            data.validation = <{ new (): any }>rule;
            data.modelType = <Function>modelType;

            validationRules.push(data);
        }

        export function toArray() : Array<RuleData>
        {
            return $.extend(true, [], validationRules);
        }

        export function getElementValidation() : Array<RuleData>
        {
            return $.extend(true, [], validationRules.filter(x => !x.path));
        }

        export function getPropertyValidationData() : Array<RuleData>
        {
            return $.extend(true, [], validationRules.filter(x => !!x.path));
        }
    }
}

(function(fn)
{
    var validationModelKey = 'higgs.validationModel';
    var validatedModelKey = 'higgs.validatedModel';

    function validationRuleArray()
    {
        
    }

    validationRuleArray.prototype = Array.prototype;

    Higgs['ValidationRuleArray'] = validationRuleArray;
    fn._serialize = fn.serialize;
    
    fn.serialize = function(options: any): any
    {
        // Use jQuery serializer if there is not parameter.
        if (arguments.length === 0 || options === false) return fn._serialize.apply(this);
        
        var settings = <Higgs.SerializeOptions>(typeof options === 'object' ? $.extend(true, {}, Higgs.SerializeOptions.defaultValue, options) : $.extend(true, {}, Higgs.SerializeOptions.defaultValue));

        if (this.length === 1) return serialize.call(this, settings);

        var result = [];
        $(this).each(function()
        {
            var model = serialize.call(this, settings);

            result.push(model);
        });

        return result;
    };

    function serialize(settings: Higgs.SerializeOptions)
    {
        var result = {};
        var x = $(this);
        var controls, modelType;

        // TODO: support multiple containers
        if (x.is(':input'))
        {
            controls = x;
        }
        else
        {
            controls = $(':input', this);
            modelType = x.data('modelType');

            if (modelType)
            {
                if (typeof modelType === 'string')
                {
                    modelType = $.getObject(modelType);
                }

                if ($.isFunction(modelType)) result = new modelType();
            }
        }

        controls
            .filter(':visible,input[type=hidden]')
            .not(':disabled,button')
            .each(function()
            {
                if (!this.name) return;

                var value = $(this).value();

                if (!settings.ignoreEmptyValue || value !== '') $.setObject(this.name, value, result);
            });
        
        return result;
    }

    fn.deserialize = function (data: Object): any 
    {
        var container = $(this);
        var inputs = $(':input', container).not('button');

        for(var i = 0; i < inputs.length; i++)
        {
            var el = $(inputs[i]);
            var name = el.attr('name');

            if (!name) continue;

            var value = $.getObject(name, undefined, data);

            if (value !== undefined) {
                el.value(value).trigger('deserialize', data).trigger('change');
            }
        }

        return this;
    }

    function orderRules(obj: Object)
    {
        for (var key in obj)
        {
            if(!obj.hasOwnProperty(key)) continue;

            if (typeof obj[key] === 'object')
            {
                if (obj[key] instanceof Higgs['ValidationRuleArray'])
                {
                    (<Higgs.IValidationRuleArray>obj[key]).sort((a, b) => a.orderIndex - b.orderIndex);
                }
                else
                {
                    orderRules(obj[key]);
                }
            }
        }
    }

    fn.getValidationModel = function(modelData?: Object)
    {
        var x = $(this);
        var controls;
        var result = {};

        if (modelData === undefined)
        {
            modelData = x.serialize(true);
        }

        // TODO: support multiple containers
        if (x.is(':input'))
        {
            controls = x;
        }
        else
        {
            controls = $(':input', this);
        }
        controls = controls.filter(':visible');

        var elementValidations = Higgs.Rules.getElementValidation();
        var propertyValidationDatas = Higgs.Rules.getPropertyValidationData();
        
        // Get all element-based rules.
        controls
            .not('button')
            .each((item: number, el: HTMLInputElement) =>
            {
                if (!el.name) return;

                var list = <Higgs.IValidationRuleArray>(new validationRuleArray());
                var elData = $(el).data();

                for (var i = 0; i < elementValidations.length; i++)
                {
                    var rule = elementValidations[i];
                    var ruleFn = rule.validation;
                    var createRuleObjFn = <Function>ruleFn['create'];

                    if(!createRuleObjFn) continue;

                    // create rule instance from DOM
                    var ruleObj = <Higgs.AbstructValidation>createRuleObjFn(el, elData);

                    if (ruleObj)
                    {
                        // Patch with custom error message
                        var customMessage = elData['msg' + rule.name];

                        if (customMessage)
                        {
                            if (typeof customMessage === "string")
                            {
                                ruleObj.message = () => customMessage;
                            }
                            else if (typeof customMessage === "function")
                            {
                                ruleObj.message = customMessage;
                            }
                        }

                        list.push(ruleObj);
                    }
                }

                if (list.length > 0)
                {
                    $.setObject(el.name, list, result);
                }
            });

        // Get property rules
        for (var i = 0; i < propertyValidationDatas.length; i++)
        {
            var ruleData = propertyValidationDatas[i];
            var list = $.getObject(ruleData.path, undefined, result);

            if(ruleData.modelType && !(modelData instanceof ruleData.modelType)) continue;

            if (!list)
            {
                list = <Higgs.IValidationRuleArray>(new validationRuleArray());
                $.setObject(ruleData.path, list, result);
            }

            list.push(new ruleData.validation());
        }

        orderRules(result);

        return result;
    };

    fn.validateModel = function(options?: Higgs.ValidateOptions)
    {
        var container = $(this);
        options = options || $.extend(true, {}, Higgs.ValidateOptions.defaultValue);
        options.validationContainer = container;
        
        if (container.is('form') && (this.noValidate || options.ignoreValidation)) return true;
        
        var obj = container.serialize(true);
        container.data(validatedModelKey, obj);
        
        var validationModel;

        if (options.cacheValidationObject)
        {
            validationModel = container.data(validationModelKey);

            if (!validationModel)
            {
                validationModel = container.getValidationModel(obj);
                container.data(validationModelKey, validationModel);
            }
        }
        else
        {
            validationModel = container.getValidationModel(obj);
        }

        var result = fn.validateModel.test(validationModel, options, obj);

        return result;
    };

    fn.validateModel.test = function(validationModel: Object, options: Higgs.ValidateOptions, obj: Object, path: string)
    {
        var resultList = <Array<Higgs.ValidationResult>>[];
        obj = obj || {};

        for (var name in validationModel)
        {
            if(!validationModel.hasOwnProperty(name)) continue;

            var cPath = (path ? path + '.' : '') + name;

            if (validationModel[name] instanceof Higgs['ValidationRuleArray'])
            {
                var context = new Higgs.ValidateContext<Object>(obj, options.validationContainer);
                var value = obj[name];
                var rules = <Array<Higgs.AbstructValidation>>validationModel[name];

                for (var i = 0; i < rules.length; i++)
                {
                    var result = rules[i].validate(context, value);

                    if (!result)
                    {
                        resultList.push(new Higgs.ValidationResult(rules[i], context, cPath, value));
                        
                        if(options.stopOnFirstInvalid) break;
                    }
                }
            }
            else
            {
                var list = fn.validateModel.test(validationModel[name], obj[name], name, cPath);
                resultList = resultList.concat(list);
            }
        }

        return resultList;
    };

    fn.invalidateValidateCache = function()
    {
        $(this).data(validationModelKey, null);

        return this;
    };

    fn.getValidatedModel = function()
    {
        return $(this).data(validatedModelKey);
    };
})(jQuery.fn);

//#endregion

//#region Validation Rules

module Higgs.Rules
{
    export class FieldDependencyOption
    {
        hasValue: boolean = undefined;
        value: any = undefined;
    };

    export class RequiredValidation extends  Higgs.AbstructValidation
    {
        isRequired: boolean = true;
        fieldDependency: Object = null;
        
        constructor()
        constructor(isRequired: boolean);
        constructor(fieldDependency: Object);
        constructor(fieldDependency: string);
        constructor(option?: any)
        {
            super(option);

            if (option === undefined) return;

            if (typeof option === 'boolean')
            {
                this.isRequired = <boolean>option;
            }
            else if (typeof option === 'string')
            {
                // should be "field1,field2,!field3"
                this.loadDependencyFromString(option);
            }
            else if ($.isArray(option) && (<Array<string>>option).length > 0)
            {
                // Custom field Dependency
                var array = <Array<any>>option;

                if (typeof array[0] === 'string')
                {
                    this.loadDependencyFromStringArray((<Array<string>>option));
                }
                else
                {
                    // Custom
                    this.fieldDependency = option;
                }
            }
        }

        loadDependencyFromString(dependency: string)
        {
            var fields = (<string>dependency).split(',');
            
            this.loadDependencyFromStringArray(fields);
        }

        loadDependencyFromStringArray(fields: Array<string>)
        {
            this.fieldDependency = {};

            for (var i = 0; i < fields.length; i++)
            {
                var fieldOption = new FieldDependencyOption();
                var fieldName = fields[i];

                if (fields[i][0] === '!')
                {
                    // "!field1"
                    fieldOption.hasValue = false;
                    fieldName = fieldName.substring(1);
                }
                else
                {
                    // "field1,field2=5"
                    fieldOption.hasValue = true;

                    var index = fieldName.indexOf('=');
                    if (index > 0)
                    {
                        fieldOption.value = fieldName.substring(index + 1);
                        fieldName = fieldName.substring(0, index);
                    }
                }
                    
                this.fieldDependency[fieldName] = fieldOption;
            }
        }

        validate(context: Higgs.ValidateContext<Object>, value: any)
        {
            if (!this.isRequired) return true;
            if (!this.fieldDependency) return !Higgs.isEmpty(value);

            for (var name in this.fieldDependency)
            {
                if (!this.fieldDependency.hasOwnProperty(name)) continue;

                var fieldOption = <FieldDependencyOption>this.fieldDependency[name];
                
                if (!fieldOption.hasValue)
                {
                    if (!Higgs.isEmpty(context.parent[name])) return true;
                }
                else
                {
                    if (fieldOption.value === undefined)
                    {
                        if (Higgs.isEmpty(context.parent[name])) return true;
                    }
                    else if(fieldOption.value !== context.parent[name])
                    {
                        return true;
                    }
                }
            }
            
            return !Higgs.isEmpty(value);
        }

        message(context: Higgs.ValidateContext<Object>, value: any) : string
        {
            return 'กรุณาระบุข้อมูล';
        }
        
        orderIndex: number = 10;

        static create(el: HTMLInputElement, data: { required: any }) : Higgs.AbstructValidation
        {
            var required = data.required || $(el).attr('required');

            if (!required) return null;
            if (required === 'required') required = true; // Case when set data-required="required" should create same validation as required="required"
            
            return new RequiredValidation(required);
        }
    }

    export class StringLengthValidation extends  Higgs.AbstructValidation
    {
        minLength: number = null;
        maxLength: number = null;

        constructor(maxLength: number)
        constructor(minLength: number, maxLength: number)
        constructor(length1: number, length2?: number)
        {
            super(length1, length2);

            if (length2 === undefined)
            {
                this.maxLength = <number>length1;
            }
            else
            {
                if (length1 !== undefined)
                {
                    this.minLength = <number>length1;
                }
                this.maxLength = <number>length2;
            }
        }

        validate(context: Higgs.ValidateContext<Object>, value: any)
        {
            if (value === undefined) return true;

            var text = <string>value;
            var length = text.length;

            if (this.minLength !== null && length < this.minLength)
            {
                return false;
            }
            if (this.maxLength !== null && length > this.maxLength)
            {
                return false;
            }

            return  true;
        }

        message(context: Higgs.ValidateContext<Object>, value: any) : string
        {
            if(this.minLength === null) return 'ข้อมูลต้องมีความยาวไม่เกิน ' + this.maxLength + ' ตัวอักษร';
            if(this.maxLength === null) return 'ข้อมูลต้องมีความยาวอย่างน้อย ' + this.minLength + ' ตัวอักษร';

            // TODO: Use format
            return 'ข้อมูลต้องมีความยาวระหว่าง ' + this.minLength + '-' + this.maxLength + ' ตัวอักษร';
        }
        
        orderIndex: number = 20;

        static create(el: HTMLInputElement, data: { minLength?: number; maxLength?: number }) : Higgs.AbstructValidation
        {
            var minLength = data.minLength;
            var maxLength = el.maxLength < 50000 ? el.maxLength : 0 || data.maxLength;  // prevent max-length default value.

            if (!minLength && !maxLength) return null;
            
            // Firefox default max & min length value.
            if (maxLength === -1 && !minLength) return null;

            return new StringLengthValidation(minLength, maxLength);
        }
    }
    
    export class PatternValidation extends  Higgs.AbstructValidation
    {
        pattern: RegExp = null;

        constructor(pattern: RegExp)
        constructor(pattern: string)
        constructor(pattern: any)
        {
            super(pattern);

            if (pattern instanceof RegExp)
            {
                this.pattern = pattern;
            }
            else
            {
                this.pattern = new RegExp(pattern);
            }
        }

        validate(context: Higgs.ValidateContext<Object>, value: any)
        {
            if (value === undefined) return true;

            var text = <string>value;
            
            if (this.pattern === null) return true;

            this.pattern.lastIndex = 0;

            return this.pattern.test(text);
        }

        message(context: Higgs.ValidateContext<Object>, value: any) : string
        {
            return 'รูปแบบข้อมูลไม่ถูกต้อง';
        }
        
        orderIndex: number = 50;

        static create(el: HTMLInputElement, data: { pattern?: string }) : Higgs.AbstructValidation
        {
            var pattern = el.pattern || data.pattern;

            if (!pattern) return null;

            return new PatternValidation(pattern);
        }
    }

    export class NumberValidation extends  PatternValidation
    {
        constructor(isFloat?: boolean)
        {
            super(isFloat ? /^\d+(.\d+)?$/ : /^\d+$/);
        }

        message(context: Higgs.ValidateContext<Object>, value: any) : string
        {
            if (this.isInteger)
            {
                return 'ข้อมูลต้องเป็นตัวเลขจำนวนเต็มเท่านั้น';
            }

            return 'ข้อมูลต้องเป็นตัวเลขเท่านั้น';
        }
        
        orderIndex: number = 30;
        isDecimal: boolean;
        isInteger: boolean;

        static create(el: HTMLInputElement, data: { decimal?: boolean; 'integer'?: boolean }) : Higgs.AbstructValidation
        {
            var validation = null;

            if (!!data.decimal)
            {
                validation = new NumberValidation(true);
                validation.isDecimal = true;
            }
            else if(!!data['integer'])
            {
                validation = new NumberValidation(false);
                validation.isInteger = true;
            }

            return validation;
        }
    }

    export class UrlValidation extends  PatternValidation
    {
        constructor()
        {
            super(UrlValidation.getRegex());
        }

        static getRegex()
        {
            // Code from: https://gist.github.com/dperini/729294
            // More Info: http://mathiasbynens.be/demo/url-regex

            var regex = new RegExp(
              "^" +
                // protocol identifier
                "(?:(?:https?|ftp)://)" +
                // user:pass authentication
                "(?:\\S+(?::\\S*)?@)?" +
                "(?:" +
                  // IP address exclusion
                  // private & local networks
                  "(?!(?:10|127)(?:\\.\\d{1,3}){3})" +
                  "(?!(?:169\\.254|192\\.168)(?:\\.\\d{1,3}){2})" +
                  "(?!172\\.(?:1[6-9]|2\\d|3[0-1])(?:\\.\\d{1,3}){2})" +
                  // IP address dotted notation octets
                  // excludes loopback network 0.0.0.0
                  // excludes reserved space >= 224.0.0.0
                  // excludes network & broacast addresses
                  // (first & last IP address of each class)
                  "(?:[1-9]\\d?|1\\d\\d|2[01]\\d|22[0-3])" +
                  "(?:\\.(?:1?\\d{1,2}|2[0-4]\\d|25[0-5])){2}" +
                  "(?:\\.(?:[1-9]\\d?|1\\d\\d|2[0-4]\\d|25[0-4]))" +
                "|" +
                  // host name
                  "(?:(?:[a-z\\u00a1-\\uffff0-9]+-?)*[a-z\\u00a1-\\uffff0-9]+)" +
                  // domain name
                  "(?:\\.(?:[a-z\\u00a1-\\uffff0-9]+-?)*[a-z\\u00a1-\\uffff0-9]+)*" +
                  // TLD identifier
                  "(?:\\.(?:[a-z\\u00a1-\\uffff]{2,}))" +
                ")" +
                // port number
                "(?::\\d{2,5})?" +
                // resource path
                "(?:/[^\\s]*)?" +
              "$", "i"
            );

            return regex;
        }

        message(context: Higgs.ValidateContext<Object>, value: any) : string
        {
            return 'ข้อมูลต้องเป็น Url ที่ถูกต้อง';
        }
        
        orderIndex: number = 30;

        static create(el: HTMLInputElement, data: { url: boolean }) : Higgs.AbstructValidation
        {
            var validation = null;

            if (data.url)
            {
                validation = new UrlValidation();
            }

            return validation;
        }
    }

    export class EmailValidation extends  PatternValidation
    {
        constructor()
        {
            super(EmailValidation.getRegex());
        }

        static getRegex()
        {
            // Code from: http://stackoverflow.com/questions/46155/validate-email-address-in-javascript
            var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

            return regex;
        }

        message(context: Higgs.ValidateContext<Object>, value: any) : string
        {
            return 'ข้อมูลต้องเป็น e-mail ที่ถูกต้อง';
        }
        
        orderIndex: number = 30;

        static create(el: HTMLInputElement, data: { email: boolean }) : Higgs.AbstructValidation
        {
            var validation = null;

            if (data.email)
            {
                validation = new EmailValidation();
            }

            return validation;
        }
    }
    
    export class MoreThanValidation extends Higgs.AbstructValidation
    {
        constructor(moreThan: number)
        {
            super(moreThan);

            this.value = moreThan;
        }

        validate(context: Higgs.ValidateContext<Object>, value: any)
        {
            if (value === undefined) return true;

            return this.value < parseFloat(value + '');
        }

        message(context: Higgs.ValidateContext<Object>, value: any) : string
        {
            return 'ข้อมูลต้องมีค่ามากกว่า ' + this.value;
        }
        
        orderIndex: number = 40;
        value: number;

        static create(el: HTMLInputElement, data: { moreThan: number }) : Higgs.AbstructValidation
        {
            if (data.moreThan === undefined) return null;

            var validation = new MoreThanValidation(data.moreThan);

            return validation;
        }
    }

    export class MinValidation extends Higgs.AbstructValidation
    {
        constructor(min: number)
        {
            super(min);

            this.value = min;
        }

        validate(context: Higgs.ValidateContext<Object>, value: any)
        {
            if (value === undefined) return true;

            return this.value <= parseFloat(value + '');
        }

        message(context: Higgs.ValidateContext<Object>, value: any) : string
        {
            return 'ข้อมูลต้องมีค่ามากกว่าเท่ากับ ' + this.value;
        }

        orderIndex: number = 41;
        value: number;

        static create(el: HTMLInputElement, data: { min: number }) : Higgs.AbstructValidation
        {
            var min = null;

            if (el.min !== undefined && el.min !== '') min = el.min;
            else if (data.min !== undefined) min = data.min;
            if (min === null) return null;

            var validation = new MinValidation(min);

            return validation;
        }
    }
    
    export class LessThanValidation extends Higgs.AbstructValidation
    {
        constructor(lessThan: number)
        {
            super(lessThan);

            this.value = lessThan;
        }

        validate(context: Higgs.ValidateContext<Object>, value: any)
        {
            if (value === undefined) return true;

            return this.value > parseFloat(value + '');
        }

        message(context: Higgs.ValidateContext<Object>, value: any) : string
        {
            return 'ข้อมูลต้องมีค่าน้อยกว่า ' + this.value;
        }
        
        orderIndex: number = 42;
        value: number;

        static create(el: HTMLInputElement, data: { lessThan: number }) : Higgs.AbstructValidation
        {
            if (data.lessThan === undefined) return null;

            var validation = new LessThanValidation(data.lessThan);

            return validation;
        }
    }

    export class MaxValidation extends Higgs.AbstructValidation
    {
        constructor(max: number)
        {
            super(max);

            this.value = max;
        }

        validate(context: Higgs.ValidateContext<Object>, value: any)
        {
            if (value === undefined) return true;

            return this.value >= parseFloat(value + '');
        }

        message(context: Higgs.ValidateContext<Object>, value: any) : string
        {
            return 'ข้อมูลต้องมีค่าน้อยกว่าหรือเท่ากับ ' + this.value;
        }
        
        orderIndex: number = 43;
        value: number;

        static create(el: HTMLInputElement, data: { max: number }) : Higgs.AbstructValidation
        {
            var max = null;

            if (el.max !== undefined && el.max !== '') max = el.max;
            else if (data.max !== undefined) max = data.max;
            if (max === null) return null;

            var validation = new MaxValidation(max);

            return validation;
        }
    }
    
    export class EqualValidation extends  Higgs.AbstructValidation
    {
        equalProperty: string = null;

        constructor(propertyName: string)
        {
            super(propertyName);
            
            this.equalProperty = propertyName;
        }

        validate(context: Higgs.ValidateContext<Object>, value: any)
        {
            if (value === undefined) return true;

            return context.parent[this.equalProperty] === value;
        }

        message(context: Higgs.ValidateContext<Object>, value: any) : string
        {
            return 'ข้อมูลต้องมีค่าเท่ากับ {label:' + this.equalProperty + '}';
        }
        
        orderIndex: number = 60;

        static create(el: HTMLInputElement, data: { equal?: string }) : Higgs.AbstructValidation
        {
            var propertyName = data.equal;

            if (!propertyName) return null;

            return new EqualValidation(propertyName);
        }
    }
    
    export class NotEqualValidation extends  Higgs.AbstructValidation
    {
        notEqualProperty: string = null;

        constructor(propertyName: string)
        {
            super(propertyName);
            
            this.notEqualProperty = propertyName;
        }

        validate(context: Higgs.ValidateContext<Object>, value: any)
        {
            if (value === undefined) return true;

            return context.parent[this.notEqualProperty] !== value;
        }

        message(context: Higgs.ValidateContext<Object>, value: any) : string
        {
            return 'ข้อมูลต้องมีค่าไม่เท่ากับ {label:' + this.notEqualProperty + '}';
        }
        
        orderIndex: number = 60;

        static create(el: HTMLInputElement, data: { notEqual?: string }) : Higgs.AbstructValidation
        {
            var propertyName = data.notEqual;

            if (!propertyName) return null;

            return new NotEqualValidation(propertyName);
        }
    }

    // Auto register all validation
    for (var key in Higgs.Rules)
    {
        if(!Higgs.Rules.hasOwnProperty(key)) continue;

        var fieldName = <string>key;

        if (fieldName.endsWith('Validation')) register(fieldName.substring(0, fieldName.length - 10), Higgs.Rules[fieldName]);
    }
}

//#endregion

//#region Boostrap Validation Display

interface JQuery
{
    redraw(): JQuery;
    validate(options?: Higgs.ValidateOptions) : boolean;
    clearValidationResult();
    displayValidationResult(result: Array<Higgs.ValidationResult>, options?: Higgs.ValidateOptions): boolean;
    disableHtml5Validation(): JQuery;
}

module Higgs
{
    export var validationErrorClass: string = 'glyphicon-exclamation-sign';
}

(function(fn)
{
    var popoverContainerId = 'popover_validationError';
    var popover;
    var popoverContent;
    var focusEvent = 'focus.validationResult.popover';
    var blurEvent = 'blur.validationResult.popover';

    fn.redraw = function() {
      return $(this).each(function(){
        var redraw = this.offsetHeight;
      });
    };

    fn.clearValidationResult = function()
    {
        var container = $(this);

        $('.form-group,.form-group [class^=col-]', container).removeClass('has-error has-feedback');

        // TODO: cache all invalid in data attribute and remove from that
        $('.glyphicon.form-control-feedback', container).remove();
        $(':input', container).unbind('*.validationResult.popover');
    }

    fn.displayValidationResult = function(result: Array<Higgs.ValidationResult>, options?: Higgs.ValidateOptions)
    {
        var container = $(this);
        options = options || $.extend(true, {}, Higgs.ValidateOptions.defaultValue);
        container.clearValidationResult();
        
        var inputs = $('.form-group :input', container);
        inputs.off('.validationResult');
        var isValid = true;

        for (var i = 0; i < result.length; i++)
        {
            var cResult = result[i];
            var input = inputs.filter('[name=' + cResult.path + ']:visible');

            if(input.length === 0) continue;

            isValid = false;
            var inputContainer = input.parents('[class^=col-]:first,.form-group:first').first();
            var feedback = input.nextAll('.form-control-feedback:eq(0)');

            inputContainer.addClass('has-error has-feedback');

            if (feedback.length === 0)
            {
                feedback = $('<span class="glyphicon form-control-feedback"></span>');

                if (input.parent().is('.input-group'))
                {
                    var rightMargin = 0;
                    var nextControls = input.nextAll(':visible');
                    nextControls.each((index:number, el:Element) =>
                    {
                        rightMargin += $(el).outerWidth(true);
                    });

                    if (rightMargin > 0)
                    {
                        feedback.css
                        ({
                            top: 0,
                            right: (rightMargin + 1) + 'px',
                            zIndex: 10
                        });
                    }
                }

                feedback.insertAfter(input);

                if (options.enableFadeInFeedback)
                {
                    feedback
                        .addClass('fade')
                        .redraw()
                        .addClass('in');
                }
            }

            feedback.addClass(Higgs.validationErrorClass);

            input.on(focusEvent, <string>null, <Object>cResult, onFocusInvalidInput);
            if (input.is(':focus')) input.trigger('focus');

            input.on(blurEvent, () =>
            {
                if (!popover || !popover.modalPopover) return;

                popover.modalPopover('hide');
            });
        }

        return isValid;
    };

    function onFocusInvalidInput(e: JQueryEventObject): any
    {
        var input = $(this);
        var result = <Higgs.ValidationResult>e.data;

        if (popover)
        {
            popover.modalPopover('hide');
            popover.remove();
        }
        
        fn.displayValidationResult.createPopover();
        
        $('<p/>')
            .text(getErrorMessage(result))
            .appendTo(popoverContent);

        var popOverTarget = input.data('popoverTarget') || input.parent().is('.input-group') ? input.parent() : input;

        popover
            .modalPopover
            ({
                target: popOverTarget
            })
            .modalPopover('show');
    }

    function getErrorMessage(result: Higgs.ValidationResult): string
    {
        var variableRegex = /\{([^\}:]+)(:([^\}]+))?\}/g;
        
        var match = variableRegex.exec(result.message);
        while (match != null)
        {
            var type = match[1];
            var value = match[3];

            if (type === 'label')
            {
                var labelText = '';

                if (!value)
                {
                    // Current label
                    labelText = $('label[for=' + result.path + ']', result.context.validationContainer).text();
                }
                else
                {
                    labelText = $('label[for=' + value + ']', result.context.validationContainer).text();
                }

                result.message = result.message.replaceAll(match[0], labelText);
            }

            match = variableRegex.exec(result.message);
        }

        return result.message;
    }

    fn.displayValidationResult.createPopover = function()
    {
        popover = $('<div id="dialog" class="popover" />').attr('id', popoverContainerId);
        popover.append($('<div class="arrow" />'));

        popoverContent = $('<div class="popover-content" />').appendTo(popover);

        popover.appendTo(document.body);
    };

    fn.validate = function(options?: Higgs.ValidateOptions): boolean
    {
        var isTotalValid = true;

        $.each(this, function()
        {
            var container = $(this);
            options = options || $.extend(true, {}, Higgs.ValidateOptions.defaultValue);

            var result = container.validateModel(options);

            // Some hidden input is not count as invalid.
            var isValid = container.displayValidationResult(result, options);

            container.off('.higgs.revalidate');
            if (!isValid)
            {
                container.on('blur.higgs.revalidate change.higgs.revalidate', '.has-error :input', options, function(e: JQueryEventObject)
                {
                    if (popover) popover.remove();

                    var validationOptions = <Higgs.ValidateOptions>e.data;

                    validationOptions.enableFadeInFeedback = false;
                    container.validate(validationOptions);
                });
            }

            isTotalValid = isTotalValid && isValid;
        });

        return isTotalValid;
    };

    // Disable HTML5 form validation
    fn.disableHtml5Validation = function()
    {
        $(':input', this).on('invalid', function(e:JQueryEventObject)
        {
            e.stopPropagation();

            return false;
        });

        return this;
    };
})(jQuery.fn);

module Higgs 
{
    export function convertToValidationResult(response: Higgs.AjaxResult) : Array<Higgs.ValidationResult>
    {
        var result = <Array<Higgs.ValidationResult>>[];

        if (!response.isSuccess)
        {
            for (var key in response.errorList)
            {
                if(!response.errorList.hasOwnProperty(key)) continue;
                if(key === 'custom-error') continue;

                var messages = response.errorList[key];

                if(!messages) continue;
                for (var i = 0; i < messages.length; i++)
                {
                    result.push(new Higgs.ValidationResult(key, messages[i]));
                }
            }
        }

        return result;
    }    
}

//#endregion
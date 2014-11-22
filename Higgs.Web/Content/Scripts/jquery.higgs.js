/*!
* Higgs Library for jQuery v1.0.0
*
* Copyright 2014 Soul_Master
* Released under the MIT license
*
* Date: @currentTime
*/
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var Higgs;
(function (Higgs) {
    var AjaxResult = (function () {
        function AjaxResult() {
        }
        return AjaxResult;
    })();
    Higgs.AjaxResult = AjaxResult;

    function escapeRegex(text) {
        if (!text)
            return text.toString();

        return text.toString().replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
    }
    Higgs.escapeRegex = escapeRegex;
    ;

    Higgs.disableElementSelect = ':input, button, .btn';

    Higgs.locales = new Array();
    Higgs.locale;
    function setLanguage(code) {
        var loc = null;

        for (var i = 0; i < Higgs.locales.length; i++) {
            if (Higgs.locales[i].name.toUpperCase() === code.toUpperCase()) {
                loc = Higgs.locales[i];
            }
        }

        if (loc) {
            Higgs.locale = loc;
            return true;
        }

        return false;
    }
    Higgs.setLanguage = setLanguage;
    ;
})(Higgs || (Higgs = {}));

(function (locales) {
    var f = function (message) {
        return function () {
            return message.format(this);
        };
    };

    locales.push({
        name: 'en',
        rules: {
            requiredValidation: f('Please specify'),
            stringLengthValidation: f('Data must be between {minLength}-{maxLength} characters in length.'),
            stringLengthValidation_NullMinLength: f('Data length greater than {maxLength} characters'),
            stringLengthValidation_NullMaxLength: f('Data length less than {minLength} characters'),
            patternValidation: f('Data is not in the correct format'),
            numberValidation_Integer: f('Data is not a valid integer'),
            numberValidation_Decimal: f('Data is not a valid number'),
            urlValidation: f('Data is not a valid URL'),
            emailValidation: f('Data is not a valid email address'),
            moreThanValidation: f('Data should has value more than {value}'),
            minValidation: f('Data should has value at least {value}'),
            lessThanValidation: f('Data should has value less than {value}'),
            maxValidation: f('Data should has value less than or equal {value}'),
            equalValidation: f('Data should has value equal to {label:{equalProperty}}'),
            notEqualValidation: f('Data should has value not equal to {label:{notEqualProperty}}')
        }
    });
    locales.push({
        name: 'th',
        rules: {
            requiredValidation: f('กรุณาระบุข้อมูล'),
            stringLengthValidation: f('ข้อมูลต้องมีความยาวระหว่าง {minLength}-{maxLength} ตัวอักษร'),
            stringLengthValidation_NullMinLength: f('ข้อมูลต้องมีความยาวไม่เกิน {maxLength} ตัวอักษร'),
            stringLengthValidation_NullMaxLength: f('ข้อมูลต้องมีความยาวอย่างน้อย {minLength} ตัวอักษร'),
            patternValidation: f('รูปแบบข้อมูลไม่ถูกต้อง'),
            numberValidation_Integer: f('ข้อมูลต้องเป็นตัวเลขจำนวนเต็มเท่านั้น'),
            numberValidation_Decimal: f('ข้อมูลต้องเป็นตัวเลขเท่านั้น'),
            urlValidation: f('ข้อมูลต้องเป็น Url ที่ถูกต้อง'),
            emailValidation: f('ข้อมูลต้องเป็น e-mail ที่ถูกต้อง'),
            moreThanValidation: f('ข้อมูลต้องมีค่ามากกว่า {value}'),
            minValidation: f('ข้อมูลต้องมีมากกว่าเท่ากับ {value}'),
            lessThanValidation: f('ข้อมูลต้องมีค่าน้อยกว่า {value}'),
            maxValidation: f('ข้อมูลต้องมีค่าน้อยกว่าหรือเท่ากับ {value}'),
            equalValidation: f('ข้อมูลต้องมีค่าเท่ากับ {label:{equalProperty}}'),
            notEqualValidation: f('ข้อมูลต้องมีค่าไม่เท่ากับ {label:{notEqualProperty}}')
        }
    });
})(Higgs.locales);

Higgs.locale = Higgs.locales[0];


// TODO: Make sure this function do same process as StringHelper.Format
String.prototype.format = function () {
    var data = [];
    for (var _i = 0; _i < (arguments.length - 0); _i++) {
        data[_i] = arguments[_i + 0];
    }
    var temp = this.toString();

    for (var i = 0; i < data.length; i++) {
        var par = data[i];

        if (typeof par === 'object') {
            if (!par)
                continue;

            for (var j in par) {
                if (!par.hasOwnProperty(j))
                    continue;
                if (typeof par[j] === 'object' || par[j] === null || par[j] === undefined || $.isFunction(par[j]))
                    continue;

                temp = temp.replaceAll('{' + j + '}', par[j], true);
            }
        } else {
            temp = temp.replaceAll('{' + i + '}', arguments[i], true);
        }
    }

    return temp;
};

String.prototype.replaceAll = function (findString, newString, isIgnorCase) {
    var temp = this;

    return temp.replace(new RegExp(Higgs.escapeRegex(findString), isIgnorCase ? 'ig' : 'g'), newString || '');
};

String.prototype.remove = function (findString) {
    var temp = this;
    for (var i = 0; i < arguments.length; i++) {
        temp = temp.replaceAll(arguments[i]);
    }

    return temp;
};

String.prototype.startsWith = function (value, isIgnoreCase) {
    if (typeof isIgnoreCase === "undefined") { isIgnoreCase = false; }
    if (!isIgnoreCase)
        return this.indexOf(value) === 0;

    return this.toUpperCase().startsWith(value.toUpperCase());
};

String.prototype.endsWith = function (value, isIgnoreCase) {
    if (typeof isIgnoreCase === "undefined") { isIgnoreCase = false; }
    if (!isIgnoreCase)
        return this.indexOf(value) === this.length - value.length;

    return this.toUpperCase().endsWith(value.toUpperCase());
};

String.prototype.toCamelCase = function () {
    return this.substring(0, 1).toLowerCase() + this.substring(1);
};

String.prototype.padLeft = function (length, padChar) {
    padChar = padChar ? padChar : '0';
    var temp = this + '';

    while (temp.length < length) {
        temp = padChar + temp;
    }

    return temp;
};


(function ($) {
    var baseUrl;

    $.newGuid = function () {
        // Return rfc4122 version 4 compliant GUID
        // Code From: http://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid-in-javascript
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };

    $.baseUrl = function (url) {
        if (url) {
            baseUrl = url;
        }

        return baseUrl;
    };

    $.getUrl = function (logicalPath) {
        if (!logicalPath)
            return location.href;
        if ((/^[a-z-]+:\/\//).test(logicalPath) || !baseUrl)
            return logicalPath;

        if (logicalPath.indexOf('~/') < 0)
            return (location.href.substring(0, location.href.indexOf(location.pathname))) + '/' + (logicalPath.indexOf('/') == 0 ? logicalPath.substring(1) : logicalPath);

        return baseUrl + logicalPath.substring(2);
    };

    $.getObject = function (parts, defaultValue, context) {
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

    $.setObject = function (name, value, context) {
        var segments = name.split('.');
        var cursor = context || window;
        var segment;

        for (var i = 0; i < segments.length - 1; ++i) {
            segment = segments[i];
            cursor = cursor[segment] = cursor[segment] || {};
        }

        return cursor[segments[i]] = value;
    };

    $.postify = function (value) {
        var result = {};

        var buildResult = function (object, prefix) {
            for (var key in object) {
                var postKey = isFinite(key) ? (prefix != "" ? prefix : "") + "[" + key + "]" : (prefix != "" ? prefix + "." : "") + key;

                switch (typeof (object[key])) {
                    case "number":
                    case "string":
                    case "boolean":
                        result[postKey] = object[key];
                        break;

                    case "object":
                        if (object[key] && object[key].toUTCString)
                            result[postKey] = object[key].toUTCString().replace("UTC", "GMT");
                        else {
                            buildResult(object[key], postKey != "" ? postKey : key);
                        }
                }
            }
        };

        buildResult(value, "");

        return result;
    };

    // Original code for http://www.filamentgroup.com/lab/jquery_plugin_for_requesting_ajax_like_file_downloads/
    $.sendData = function (url, data, method, isNewWindow) {
        if (arguments.length === 3 && typeof arguments[2] === 'boolean') {
            isNewWindow = arguments[2];
            method = undefined;
        }

        if (isNewWindow === undefined)
            isNewWindow = true;

        if (url.indexOf('~/') === 0)
            url = $.getUrl(url);
        var form = $('<form />');
        form.attr('id', 'form' + (new Date()).getTime());
        form.attr('action', url);
        form.attr('method', method || 'post');

        if (isNewWindow) {
            form.attr('target', '_blank');
        }

        if (data) {
            if (typeof data == 'string') {
                //split params into form inputs
                $.each(data.split('&'), function () {
                    var pair = this.split('=');
                    $('<input type="hidden" />').attr({
                        name: pair[0],
                        value: pair[1]
                    }).appendTo(form);
                });
            } else {
                $.each(data, function (key, value) {
                    $('<input />').attr({
                        type: 'hidden',
                        name: key,
                        value: value + ''
                    }).appendTo(form);
                });
            }
        }

        //send request
        form.appendTo('body').submit();

        if (!isNewWindow) {
            form.remove();
        }
    };

    $.any = function (arr, filterFn) {
        if (!arr || !$.isArray(arr) || !arr.length)
            return false;

        for (var i = 0; i < arr.length; i++) {
            if (filterFn(arr[i]))
                return true;
        }

        return false;
    };
})(jQuery);


(function (fn) {
    fn.ensureId = function () {
        $(this).each(function () {
            var el = $(this);

            if (!el.attr('id'))
                el.attr('id', $.newGuid());
        });

        return this;
    };

    fn.getEvents = function () {
        return jQuery['_data'](this[0], "events");
    };

    fn.disable = function (isDisabled) {
        if (isDisabled === undefined) {
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
    fn.value = function (value) {
        var x = this;

        if (value === undefined) {
            value = null;

            if (x.is(':checkbox')) {
                if (x.length === 1) {
                    value = getChkValue.call(x);
                } else {
                    value = [];

                    for (var i = 0; i < x.length; i++) {
                        var chk = $(x[i]);

                        if (!chk.is(':checked'))
                            continue;

                        value.push(getChkValue.call(chk));
                    }
                }
            } else if (x.is(':radio')) {
                if (x.length == 1) {
                    x = x.parents('form').find(':radio[name=' + x[0].name + ']');
                }

                x.each(function () {
                    if (this.checked) {
                        value = this.value;
                    }
                });
            } else {
                value = x.val();
            }

            return value;
        } else {
            var compareValue = function (value1, value2) {
                if (value1 === undefined || value1 === null) {
                    value1 = '';
                } else {
                    value1 = value1.toString().toLowerCase();
                }
                if (value2 === undefined || value2 === null) {
                    value2 = '';
                } else {
                    value2 = value2.toString().toLowerCase();
                }

                return value1 === value2;
            };

            if (x.is(':radio')) {
                if (x.length == 1) {
                    x = x.parents('form').find(':radio[name=' + x[0].name + ']');
                }

                x.each(function () {
                    if (compareValue(this.value, value)) {
                        $(this).attr('checked', 'on');
                    } else {
                        $(this).removeAttr('checked');
                    }
                });
            } else if (x.is(':checkbox')) {
                if (this[0].value && typeof value !== 'boolean') {
                    value = this[0].value === value;
                }

                if (value)
                    x.attr('checked', 'on');
                else
                    x.removeAttr('checked');
            } else if (x.is('select')) {
                var selectedValue;

                $('option', x).each(function () {
                    var cValue = $(this).val();

                    if (compareValue(cValue, value))
                        selectedValue = cValue;
                });

                x.val(selectedValue === undefined ? $('option:eq(0)', x).val() : selectedValue);
            } else {
                x.val(value);
            }

            return this;
        }

        function getChkValue() {
            var value = this.val();

            if (this[0].value !== 'on' && this[0].value) {
                return this.is(':checked') ? this[0].value : null;
            }

            return value === 'on' ? this.is(':checked') : value;
        }
    };

    fn.submitForm = function (data) {
        var form = $(this);
        if (!data)
            data = form.serialize(true);

        form.disable();

        return $.ajax({
            type: 'POST',
            dataType: 'json',
            url: $.getUrl(form.attr('action')),
            data: $.postify(data)
        });
    };
})(jQuery.fn);


var Higgs;
(function (Higgs) {
    var AbstructValidation = (function () {
        function AbstructValidation(param1, param2, param3) {
            this.orderIndex = 0;
        }
        AbstructValidation.prototype.validate = function (context, value) {
            throw new Error('not implemented method');
        };

        AbstructValidation.prototype.message = function (context, value) {
            throw new Error('not implemented method');
        };
        return AbstructValidation;
    })();
    Higgs.AbstructValidation = AbstructValidation;

    var ValidationResult = (function () {
        function ValidationResult(validationRule, context, path, value) {
            if (path) {
                validationRule = validationRule;
                this.context = context;

                this.message = validationRule.message(context, value);
                this.path = path;
                this.context = context;
            } else {
                this.path = validationRule;
                this.message = context;
                this.context = context;
            }
        }
        return ValidationResult;
    })();
    Higgs.ValidationResult = ValidationResult;

    var SerializeOptions = (function () {
        function SerializeOptions() {
            this.ignoreEmptyValue = true;
        }
        SerializeOptions.defaultValue = new SerializeOptions();
        return SerializeOptions;
    })();
    Higgs.SerializeOptions = SerializeOptions;

    var ValidateOptions = (function () {
        function ValidateOptions() {
            this.cacheValidationObject = true;
            this.ignoreValidation = false;
            this.stopOnFirstInvalid = true;
            this.enableFadeInFeedback = true;
        }
        ValidateOptions.defaultValue = new ValidateOptions();
        return ValidateOptions;
    })();
    Higgs.ValidateOptions = ValidateOptions;

    var ValidateContext = (function () {
        function ValidateContext(parent, container) {
            this.parent = parent;
            this.validationContainer = container;
        }
        return ValidateContext;
    })();
    Higgs.ValidateContext = ValidateContext;

    var RuleData = (function () {
        function RuleData() {
        }
        return RuleData;
    })();
    Higgs.RuleData = RuleData;

    function isEmpty(value) {
        return value === undefined || value != null && value.constructor === String && value === '';
    }
    Higgs.isEmpty = isEmpty;

    var validationRules = [];

    (function (Rules) {
        function register(name, rule, modelType, path) {
            if (typeof modelType === 'string') {
                path = modelType;
                modelType = undefined;
            }

            var data = new RuleData();
            data.name = name;
            data.path = path;
            data.validation = rule;
            data.modelType = modelType;

            validationRules.push(data);
        }
        Rules.register = register;

        function toArray() {
            return $.extend(true, [], validationRules);
        }
        Rules.toArray = toArray;

        function getElementValidation() {
            return $.extend(true, [], validationRules.filter(function (x) {
                return !x.path;
            }));
        }
        Rules.getElementValidation = getElementValidation;

        function getPropertyValidationData() {
            return $.extend(true, [], validationRules.filter(function (x) {
                return !!x.path;
            }));
        }
        Rules.getPropertyValidationData = getPropertyValidationData;
    })(Higgs.Rules || (Higgs.Rules = {}));
    var Rules = Higgs.Rules;
})(Higgs || (Higgs = {}));

(function (fn) {
    var validationModelKey = 'higgs.validationModel';
    var validatedModelKey = 'higgs.validatedModel';

    function validationRuleArray() {
    }

    validationRuleArray.prototype = Array.prototype;

    Higgs['ValidationRuleArray'] = validationRuleArray;
    fn._serialize = fn.serialize;

    fn.serialize = function (options) {
        // Use jQuery serializer if there is not parameter.
        if (arguments.length === 0 || options === false)
            return fn._serialize.apply(this);

        var settings = (typeof options === 'object' ? $.extend(true, {}, Higgs.SerializeOptions.defaultValue, options) : $.extend(true, {}, Higgs.SerializeOptions.defaultValue));

        if (this.length === 1)
            return serialize.call(this, settings);

        var result = [];
        $(this).each(function () {
            var model = serialize.call(this, settings);

            result.push(model);
        });

        return result;
    };

    function serialize(settings) {
        var result = {};
        var x = $(this);
        var controls, modelType;

        // TODO: support multiple containers
        if (x.is(':input')) {
            controls = x;
        } else {
            controls = $(':input', this);
            modelType = x.data('modelType');

            if (modelType) {
                if (typeof modelType === 'string') {
                    modelType = $.getObject(modelType);
                }

                if ($.isFunction(modelType))
                    result = new modelType();
            }
        }

        controls.filter(':visible,input[type=hidden]').not(':disabled,button').each(function () {
            if (!this.name)
                return;

            var el = $(this);
            var value = el.value();

            if (settings.ignoreEmptyValue && (value === '' || value === null))
                return;

            var cValue = $.getObject(this.name, undefined, result);

            if (el.is(':radio')) {
                if (value) {
                    $.setObject(this.name, value, result);
                }
            } else if (cValue !== undefined) {
                if ($.isArray(cValue)) {
                    var cArray = cValue;
                    cArray.push(value);
                } else {
                    $.setObject(this.name, [cValue, value], result);
                }
            } else {
                $.setObject(this.name, value, result);
            }
        });

        return result;
    }

    fn.deserialize = function (data) {
        var container = $(this);
        var inputs = $(':input', container).not('button');

        for (var i = 0; i < inputs.length; i++) {
            var el = $(inputs[i]);
            var name = el.attr('name');

            if (!name)
                continue;

            var value = $.getObject(name, undefined, data);

            if (value !== undefined) {
                el.value(value).trigger('deserialize', data).trigger('change');
            }
        }

        return this;
    };

    function orderRules(obj) {
        for (var key in obj) {
            if (!obj.hasOwnProperty(key))
                continue;

            if (typeof obj[key] === 'object') {
                if (obj[key] instanceof Higgs['ValidationRuleArray']) {
                    obj[key].sort(function (a, b) {
                        return a.orderIndex - b.orderIndex;
                    });
                } else {
                    orderRules(obj[key]);
                }
            }
        }
    }

    fn.getValidationModel = function (modelData) {
        var x = $(this);
        var controls;
        var result = {};

        if (modelData === undefined) {
            modelData = x.serialize(true);
        }

        // TODO: support multiple containers
        if (x.is(':input')) {
            controls = x;
        } else {
            controls = $(':input', this);
        }
        controls = controls.filter(':visible');

        var elementValidations = Higgs.Rules.getElementValidation();
        var propertyValidationDatas = Higgs.Rules.getPropertyValidationData();

        // Get all element-based rules.
        controls.not('button').each(function (item, el) {
            if (!el.name)
                return;

            var list = (new validationRuleArray());
            var elData = $(el).data();

            for (var i = 0; i < elementValidations.length; i++) {
                var rule = elementValidations[i];
                var ruleFn = rule.validation;
                var createRuleObjFn = ruleFn['create'];

                if (!createRuleObjFn)
                    continue;

                // create rule instance from DOM
                var ruleObj = createRuleObjFn(el, elData);

                if (ruleObj) {
                    // Patch with custom error message
                    var customMessage = elData['msg' + rule.name];

                    if (customMessage) {
                        if (typeof customMessage === "string") {
                            ruleObj.message = function () {
                                return customMessage;
                            };
                        } else if (typeof customMessage === "function") {
                            ruleObj.message = customMessage;
                        }
                    }

                    list.push(ruleObj);
                }
            }

            if (list.length > 0) {
                $.setObject(el.name, list, result);
            }
        });

        for (var i = 0; i < propertyValidationDatas.length; i++) {
            var ruleData = propertyValidationDatas[i];
            var list = $.getObject(ruleData.path, undefined, result);

            if (ruleData.modelType && !(modelData instanceof ruleData.modelType))
                continue;

            if (!list) {
                list = (new validationRuleArray());
                $.setObject(ruleData.path, list, result);
            }

            list.push(new ruleData.validation());
        }

        orderRules(result);

        return result;
    };

    fn.validateModel = function (options) {
        var container = $(this);
        options = options || $.extend(true, {}, Higgs.ValidateOptions.defaultValue);
        options.validationContainer = container;

        if (container.is('form') && (this.noValidate || options.ignoreValidation))
            return true;

        var obj = container.serialize(true);
        container.data(validatedModelKey, obj);

        var validationModel;

        if (options.cacheValidationObject) {
            validationModel = container.data(validationModelKey);

            if (!validationModel) {
                validationModel = container.getValidationModel(obj);
                container.data(validationModelKey, validationModel);
            }
        } else {
            validationModel = container.getValidationModel(obj);
        }

        var result = fn.validateModel.test(validationModel, options, obj);

        return result;
    };

    fn.validateModel.test = function (validationModel, options, obj, path) {
        var resultList = [];
        obj = obj || {};

        for (var name in validationModel) {
            if (!validationModel.hasOwnProperty(name))
                continue;

            var cPath = (path ? path + '.' : '') + name;

            if (validationModel[name] instanceof Higgs['ValidationRuleArray']) {
                var context = new Higgs.ValidateContext(obj, options.validationContainer);
                var value = obj[name];
                var rules = validationModel[name];

                for (var i = 0; i < rules.length; i++) {
                    var result = rules[i].validate(context, value);

                    if (!result) {
                        resultList.push(new Higgs.ValidationResult(rules[i], context, cPath, value));

                        if (options.stopOnFirstInvalid)
                            break;
                    }
                }
            } else {
                var list = fn.validateModel.test(validationModel[name], obj[name], name, cPath);
                resultList = resultList.concat(list);
            }
        }

        return resultList;
    };

    fn.invalidateValidateCache = function () {
        $(this).data(validationModelKey, null);

        return this;
    };

    fn.getValidatedModel = function () {
        return $(this).data(validatedModelKey);
    };
})(jQuery.fn);

var Higgs;
(function (Higgs) {
    //#endregion
    //#region Validation Rules
    (function (Rules) {
        var FieldDependencyOption = (function () {
            function FieldDependencyOption() {
                this.hasValue = undefined;
                this.value = undefined;
            }
            return FieldDependencyOption;
        })();
        Rules.FieldDependencyOption = FieldDependencyOption;
        ;

        var RequiredValidation = (function (_super) {
            __extends(RequiredValidation, _super);
            function RequiredValidation(option) {
                _super.call(this, option);
                this.isRequired = true;
                this.fieldDependency = null;
                this.orderIndex = 10;

                if (option === undefined)
                    return;

                if (typeof option === 'boolean') {
                    this.isRequired = option;
                } else if (typeof option === 'string') {
                    // should be "field1,field2,!field3"
                    this.loadDependencyFromString(option);
                } else if ($.isArray(option) && option.length > 0) {
                    // Custom field Dependency
                    var array = option;

                    if (typeof array[0] === 'string') {
                        this.loadDependencyFromStringArray(option);
                    } else {
                        // Custom
                        this.fieldDependency = option;
                    }
                }
            }
            RequiredValidation.prototype.loadDependencyFromString = function (dependency) {
                var fields = dependency.split(',');

                this.loadDependencyFromStringArray(fields);
            };

            RequiredValidation.prototype.loadDependencyFromStringArray = function (fields) {
                this.fieldDependency = {};

                for (var i = 0; i < fields.length; i++) {
                    var fieldOption = new FieldDependencyOption();
                    var fieldName = fields[i];

                    if (fields[i][0] === '!') {
                        // "!field1"
                        fieldOption.hasValue = false;
                        fieldName = fieldName.substring(1);
                    } else {
                        // "field1,field2=5"
                        fieldOption.hasValue = true;

                        var index = fieldName.indexOf('=');
                        if (index > 0) {
                            fieldOption.value = fieldName.substring(index + 1);
                            fieldName = fieldName.substring(0, index);
                        }
                    }

                    this.fieldDependency[fieldName] = fieldOption;
                }
            };

            RequiredValidation.prototype.validate = function (context, value) {
                if (!this.isRequired)
                    return true;
                if (!this.fieldDependency)
                    return !Higgs.isEmpty(value);

                for (var name in this.fieldDependency) {
                    if (!this.fieldDependency.hasOwnProperty(name))
                        continue;

                    var fieldOption = this.fieldDependency[name];

                    if (!fieldOption.hasValue) {
                        if (!Higgs.isEmpty(context.parent[name]))
                            return true;
                    } else {
                        if (fieldOption.value === undefined) {
                            if (context.parent[name] === false)
                                return true;
                            if (Higgs.isEmpty(context.parent[name]))
                                return true;
                        } else if (fieldOption.value !== context.parent[name]) {
                            return true;
                        }
                    }
                }

                return !Higgs.isEmpty(value);
            };

            RequiredValidation.prototype.message = function (context, value) {
                return Higgs.locale.rules.requiredValidation.apply(this, arguments);
            };

            RequiredValidation.create = function (el, data) {
                var required = data.required || $(el).attr('required');

                if (!required)
                    return null;
                if (required === 'required')
                    required = true; // Case when set data-required="required" should create same validation as required="required"

                return new RequiredValidation(required);
            };
            return RequiredValidation;
        })(Higgs.AbstructValidation);
        Rules.RequiredValidation = RequiredValidation;

        var StringLengthValidation = (function (_super) {
            __extends(StringLengthValidation, _super);
            function StringLengthValidation(length1, length2) {
                _super.call(this, length1, length2);
                this.minLength = null;
                this.maxLength = null;
                this.orderIndex = 20;

                if (length2 === undefined) {
                    this.maxLength = length1;
                } else {
                    if (length1 !== undefined) {
                        this.minLength = length1;
                    }
                    this.maxLength = length2;
                }
            }
            StringLengthValidation.prototype.validate = function (context, value) {
                if (value === undefined)
                    return true;

                var text = value;
                var length = text.length;

                if (this.minLength !== null && length < this.minLength) {
                    return false;
                }
                if (this.maxLength !== null && length > this.maxLength) {
                    return false;
                }

                return true;
            };

            StringLengthValidation.prototype.message = function (context, value) {
                if (this.minLength === null)
                    return Higgs.locale.rules.stringLengthValidation_NullMinLength.apply(this, arguments);
                if (this.maxLength === null)
                    return Higgs.locale.rules.stringLengthValidation_NullMaxLength.apply(this, arguments);

                return Higgs.locale.rules.stringLengthValidation.apply(this, arguments);
            };

            StringLengthValidation.create = function (el, data) {
                var minLength = data.minLength;
                var maxLength = el.maxLength < 50000 ? el.maxLength : 0 || data.maxLength;

                if (!minLength && !maxLength)
                    return null;

                // Firefox default max & min length value.
                if (maxLength === -1 && !minLength)
                    return null;

                return new StringLengthValidation(minLength, maxLength);
            };
            return StringLengthValidation;
        })(Higgs.AbstructValidation);
        Rules.StringLengthValidation = StringLengthValidation;

        var PatternValidation = (function (_super) {
            __extends(PatternValidation, _super);
            function PatternValidation(pattern) {
                _super.call(this, pattern);
                this.pattern = null;
                this.orderIndex = 50;

                if (pattern instanceof RegExp) {
                    this.pattern = pattern;
                } else {
                    this.pattern = new RegExp(pattern);
                }
            }
            PatternValidation.prototype.validate = function (context, value) {
                if (value === undefined)
                    return true;

                var text = value;

                if (this.pattern === null)
                    return true;

                this.pattern.lastIndex = 0;

                return this.pattern.test(text);
            };

            PatternValidation.prototype.message = function (context, value) {
                return Higgs.locale.rules.patternValidation.apply(this, arguments);
            };

            PatternValidation.create = function (el, data) {
                var pattern = el.pattern || data.pattern;

                if (!pattern)
                    return null;

                return new PatternValidation(pattern);
            };
            return PatternValidation;
        })(Higgs.AbstructValidation);
        Rules.PatternValidation = PatternValidation;

        var NumberValidation = (function (_super) {
            __extends(NumberValidation, _super);
            function NumberValidation(isFloat) {
                _super.call(this, isFloat ? /^\d+(.\d+)?$/ : /^\d+$/);
                this.orderIndex = 30;
            }
            NumberValidation.prototype.message = function (context, value) {
                if (this.isInteger) {
                    return Higgs.locale.rules.numberValidation_Integer.apply(this, arguments);
                }

                return Higgs.locale.rules.numberValidation_Decimal.apply(this, arguments);
            };

            NumberValidation.create = function (el, data) {
                var validation = null;

                if (!!data.decimal) {
                    validation = new NumberValidation(true);
                    validation.isDecimal = true;
                } else if (!!data['integer']) {
                    validation = new NumberValidation(false);
                    validation.isInteger = true;
                }

                return validation;
            };
            return NumberValidation;
        })(PatternValidation);
        Rules.NumberValidation = NumberValidation;

        var UrlValidation = (function (_super) {
            __extends(UrlValidation, _super);
            function UrlValidation() {
                _super.call(this, UrlValidation.getRegex());
                this.orderIndex = 30;
            }
            UrlValidation.getRegex = function () {
                // Code from: https://gist.github.com/dperini/729294
                // More Info: http://mathiasbynens.be/demo/url-regex
                var regex = new RegExp("^" + "(?:(?:https?|ftp)://)" + "(?:\\S+(?::\\S*)?@)?" + "(?:" + "(?!(?:10|127)(?:\\.\\d{1,3}){3})" + "(?!(?:169\\.254|192\\.168)(?:\\.\\d{1,3}){2})" + "(?!172\\.(?:1[6-9]|2\\d|3[0-1])(?:\\.\\d{1,3}){2})" + "(?:[1-9]\\d?|1\\d\\d|2[01]\\d|22[0-3])" + "(?:\\.(?:1?\\d{1,2}|2[0-4]\\d|25[0-5])){2}" + "(?:\\.(?:[1-9]\\d?|1\\d\\d|2[0-4]\\d|25[0-4]))" + "|" + "(?:(?:[a-z\\u00a1-\\uffff0-9]+-?)*[a-z\\u00a1-\\uffff0-9]+)" + "(?:\\.(?:[a-z\\u00a1-\\uffff0-9]+-?)*[a-z\\u00a1-\\uffff0-9]+)*" + "(?:\\.(?:[a-z\\u00a1-\\uffff]{2,}))" + ")" + "(?::\\d{2,5})?" + "(?:/[^\\s]*)?" + "$", "i");

                return regex;
            };

            UrlValidation.prototype.message = function (context, value) {
                return Higgs.locale.rules.urlValidation.apply(this, arguments);
            };

            UrlValidation.create = function (el, data) {
                var validation = null;

                if (data.url) {
                    validation = new UrlValidation();
                }

                return validation;
            };
            return UrlValidation;
        })(PatternValidation);
        Rules.UrlValidation = UrlValidation;

        var EmailValidation = (function (_super) {
            __extends(EmailValidation, _super);
            function EmailValidation() {
                _super.call(this, EmailValidation.getRegex());
                this.orderIndex = 30;
            }
            EmailValidation.getRegex = function () {
                // Code from: http://stackoverflow.com/questions/46155/validate-email-address-in-javascript
                var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

                return regex;
            };

            EmailValidation.prototype.message = function (context, value) {
                return Higgs.locale.rules.emailValidation.apply(this, arguments);
            };

            EmailValidation.create = function (el, data) {
                var validation = null;

                if (data.email) {
                    validation = new EmailValidation();
                }

                return validation;
            };
            return EmailValidation;
        })(PatternValidation);
        Rules.EmailValidation = EmailValidation;

        var MoreThanValidation = (function (_super) {
            __extends(MoreThanValidation, _super);
            function MoreThanValidation(moreThan) {
                _super.call(this, moreThan);
                this.orderIndex = 40;

                this.value = moreThan;
            }
            MoreThanValidation.prototype.validate = function (context, value) {
                if (value === undefined)
                    return true;

                return this.value < parseFloat(value + '');
            };

            MoreThanValidation.prototype.message = function (context, value) {
                return Higgs.locale.rules.moreThanValidation.apply(this, arguments);
            };

            MoreThanValidation.create = function (el, data) {
                if (data.moreThan === undefined)
                    return null;

                var validation = new MoreThanValidation(data.moreThan);

                return validation;
            };
            return MoreThanValidation;
        })(Higgs.AbstructValidation);
        Rules.MoreThanValidation = MoreThanValidation;

        var MinValidation = (function (_super) {
            __extends(MinValidation, _super);
            function MinValidation(min) {
                _super.call(this, min);
                this.orderIndex = 41;

                this.value = min;
            }
            MinValidation.prototype.validate = function (context, value) {
                if (value === undefined)
                    return true;

                return this.value <= parseFloat(value + '');
            };

            MinValidation.prototype.message = function (context, value) {
                return Higgs.locale.rules.minValidation.apply(this, arguments);
            };

            MinValidation.create = function (el, data) {
                var min = null;

                if (el.min !== undefined && el.min !== '')
                    min = el.min;
                else if (data.min !== undefined)
                    min = data.min;
                if (min === null)
                    return null;

                var validation = new MinValidation(min);

                return validation;
            };
            return MinValidation;
        })(Higgs.AbstructValidation);
        Rules.MinValidation = MinValidation;

        var LessThanValidation = (function (_super) {
            __extends(LessThanValidation, _super);
            function LessThanValidation(lessThan) {
                _super.call(this, lessThan);
                this.orderIndex = 42;

                this.value = lessThan;
            }
            LessThanValidation.prototype.validate = function (context, value) {
                if (value === undefined)
                    return true;

                return this.value > parseFloat(value + '');
            };

            LessThanValidation.prototype.message = function (context, value) {
                return Higgs.locale.rules.lessThanValidation.apply(this, arguments);
            };

            LessThanValidation.create = function (el, data) {
                if (data.lessThan === undefined)
                    return null;

                var validation = new LessThanValidation(data.lessThan);

                return validation;
            };
            return LessThanValidation;
        })(Higgs.AbstructValidation);
        Rules.LessThanValidation = LessThanValidation;

        var MaxValidation = (function (_super) {
            __extends(MaxValidation, _super);
            function MaxValidation(max) {
                _super.call(this, max);
                this.orderIndex = 43;

                this.value = max;
            }
            MaxValidation.prototype.validate = function (context, value) {
                if (value === undefined)
                    return true;

                return this.value >= parseFloat(value + '');
            };

            MaxValidation.prototype.message = function (context, value) {
                return Higgs.locale.rules.maxValidation.apply(this, arguments);
            };

            MaxValidation.create = function (el, data) {
                var max = null;

                if (el.max !== undefined && el.max !== '')
                    max = el.max;
                else if (data.max !== undefined)
                    max = data.max;
                if (max === null)
                    return null;

                var validation = new MaxValidation(max);

                return validation;
            };
            return MaxValidation;
        })(Higgs.AbstructValidation);
        Rules.MaxValidation = MaxValidation;

        var EqualValidation = (function (_super) {
            __extends(EqualValidation, _super);
            function EqualValidation(propertyName) {
                _super.call(this, propertyName);
                this.equalProperty = null;
                this.orderIndex = 60;

                this.equalProperty = propertyName;
            }
            EqualValidation.prototype.validate = function (context, value) {
                if (value === undefined)
                    return true;

                return context.parent[this.equalProperty] === value;
            };

            EqualValidation.prototype.message = function (context, value) {
                return Higgs.locale.rules.equalValidation.apply(this, arguments);
            };

            EqualValidation.create = function (el, data) {
                var propertyName = data.equal;

                if (!propertyName)
                    return null;

                return new EqualValidation(propertyName);
            };
            return EqualValidation;
        })(Higgs.AbstructValidation);
        Rules.EqualValidation = EqualValidation;

        var NotEqualValidation = (function (_super) {
            __extends(NotEqualValidation, _super);
            function NotEqualValidation(propertyName) {
                _super.call(this, propertyName);
                this.notEqualProperty = null;
                this.orderIndex = 60;

                this.notEqualProperty = propertyName;
            }
            NotEqualValidation.prototype.validate = function (context, value) {
                if (value === undefined)
                    return true;

                return context.parent[this.notEqualProperty] !== value;
            };

            NotEqualValidation.prototype.message = function (context, value) {
                return Higgs.locale.rules.notEqualValidation.apply(this, arguments);
            };

            NotEqualValidation.create = function (el, data) {
                var propertyName = data.notEqual;

                if (!propertyName)
                    return null;

                return new NotEqualValidation(propertyName);
            };
            return NotEqualValidation;
        })(Higgs.AbstructValidation);
        Rules.NotEqualValidation = NotEqualValidation;

        for (var key in Higgs.Rules) {
            if (!Higgs.Rules.hasOwnProperty(key))
                continue;

            var fieldName = key;

            if (fieldName.endsWith('Validation'))
                Rules.register(fieldName.substring(0, fieldName.length - 10), Higgs.Rules[fieldName]);
        }
    })(Higgs.Rules || (Higgs.Rules = {}));
    var Rules = Higgs.Rules;
})(Higgs || (Higgs = {}));


var Higgs;
(function (Higgs) {
    Higgs.validationErrorClass = 'glyphicon-exclamation-sign';
})(Higgs || (Higgs = {}));

(function (fn) {
    var popoverContainerId = 'popover_validationError';
    var popover;
    var popoverContent;
    var focusEvent = 'focus.validationResult.popover';
    var blurEvent = 'blur.validationResult.popover';

    fn.redraw = function () {
        return $(this).each(function () {
            var redraw = this.offsetHeight;

            return redraw;
        });
    };

    fn.clearValidationResult = function () {
        var container = $(this);

        $('.form-group,.form-group [class^=col-]', container).removeClass('has-error has-feedback');

        // TODO: cache all invalid in data attribute and remove from that
        $('.glyphicon.form-control-feedback', container).remove();
        $(':input', container).unbind('*.validationResult.popover');
    };

    fn.displayValidationResult = function (result, options) {
        var container = $(this);
        options = options || $.extend(true, {}, Higgs.ValidateOptions.defaultValue);
        container.clearValidationResult();

        var inputs = $('.form-group :input', container);
        inputs.off('.validationResult');
        var isValid = true;

        for (var i = 0; i < result.length; i++) {
            var cResult = result[i];
            var input = inputs.filter('[name=' + cResult.path + ']:visible');

            if (input.length === 0)
                continue;

            isValid = false;
            var inputContainer = input.parents('[class^=col-]:first,.form-group:first').first();
            var feedback = input.nextAll('.form-control-feedback:eq(0)');

            inputContainer.addClass('has-error has-feedback');

            if (feedback.length === 0) {
                feedback = $('<span class="glyphicon form-control-feedback"></span>');

                if (input.parent().is('.input-group')) {
                    var rightMargin = 0;
                    var nextControls = input.nextAll(':visible');
                    nextControls.each(function (index, el) {
                        rightMargin += $(el).outerWidth(true);
                    });

                    if (rightMargin > 0) {
                        feedback.css({
                            top: 0,
                            right: (rightMargin + 1) + 'px',
                            zIndex: 10
                        });
                    }
                }

                feedback.insertAfter(input);
                feedback.on('click', input, function (e) {
                    var control = e.data;
                    control.focus();
                });

                if (options.enableFadeInFeedback) {
                    feedback.addClass('fade').redraw().addClass('in');
                }
            }

            feedback.addClass(Higgs.validationErrorClass);

            input.on(focusEvent, null, cResult, onFocusInvalidInput);
            if (input.is(':focus'))
                input.trigger('focus');

            var hidePopupFn = function () {
                if (!popover || !popover.modalPopover)
                    return;

                popover.modalPopover('hide');

                // TODO: find out the better way to hide modal popover
                popover.remove();
            };
            input.on(blurEvent, hidePopupFn);
        }

        return isValid;
    };

    function onFocusInvalidInput(e) {
        var input = $(this);
        var result = e.data;

        if (popover) {
            popover.modalPopover('hide');
            popover.remove();
        }

        fn.displayValidationResult.createPopover();

        $('<p/>').text(getErrorMessage(result)).appendTo(popoverContent);

        var popOverTarget = input.data('popoverTarget') || input.parent().is('.input-group') ? input.parent() : input;

        popover.modalPopover({
            target: popOverTarget
        }).modalPopover('show');
    }

    function getErrorMessage(result) {
        var variableRegex = /\{([^\}:]+)(:([^\}]+))?\}/g;

        var match = variableRegex.exec(result.message);
        while (match != null) {
            var type = match[1];
            var value = match[3];

            if (type === 'label') {
                var labelText = '';

                if (!value) {
                    // Current label
                    labelText = $('label[for=' + result.path + ']', result.context.validationContainer).text();
                } else {
                    labelText = $('label[for=' + value + ']', result.context.validationContainer).text();
                }

                result.message = result.message.replaceAll(match[0], labelText);
            }

            match = variableRegex.exec(result.message);
        }

        return result.message;
    }

    fn.displayValidationResult.createPopover = function () {
        popover = $('<div id="dialog" class="popover" />').attr('id', popoverContainerId);
        popover.append($('<div class="arrow" />'));

        popoverContent = $('<div class="popover-content" />').appendTo(popover);

        popover.appendTo(document.body);
    };

    fn.validate = function (options) {
        var isTotalValid = true;

        $.each(this, function () {
            var container = $(this);
            options = options || $.extend(true, {}, Higgs.ValidateOptions.defaultValue);

            var result = container.validateModel(options);

            // Some hidden input is not count as invalid.
            var isValid = container.displayValidationResult(result, options);

            container.off('.higgs.revalidate');
            if (!isValid) {
                container.on('blur.higgs.revalidate change.higgs.revalidate', '.has-error :input', options, function (e) {
                    if (popover)
                        popover.remove();

                    var validationOptions = e.data;

                    validationOptions.enableFadeInFeedback = false;
                    container.validate(validationOptions);
                });
            }

            isTotalValid = isTotalValid && isValid;
        });

        return isTotalValid;
    };

    // Disable HTML5 form validation
    fn.disableHtml5Validation = function () {
        $(':input', this).on('invalid', function (e) {
            e.stopPropagation();

            return false;
        });

        return this;
    };
})(jQuery.fn);

var Higgs;
(function (Higgs) {
    function convertToValidationResult(response) {
        var result = [];

        if (!response.isSuccess) {
            for (var key in response.errorList) {
                if (!response.errorList.hasOwnProperty(key))
                    continue;
                if (key === 'custom-error')
                    continue;

                var messages = response.errorList[key];

                if (!messages)
                    continue;
                for (var i = 0; i < messages.length; i++) {
                    result.push(new Higgs.ValidationResult(key, messages[i]));
                }
            }
        }

        return result;
    }
    Higgs.convertToValidationResult = convertToValidationResult;
})(Higgs || (Higgs = {}));
//#endregion
//# sourceMappingURL=jquery.higgs.js.map

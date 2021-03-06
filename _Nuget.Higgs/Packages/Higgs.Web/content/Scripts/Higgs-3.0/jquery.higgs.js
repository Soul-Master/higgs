/*!
* Higgs Library for jQuery v1.0.0
*
* Copyright 2014 Soul_Master
* Released under the MIT license
*
* Date: @currentTime
*/
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Higgs;
(function (Higgs) {
    var AjaxResult = /** @class */ (function () {
        function AjaxResult() {
        }
        return AjaxResult;
    }());
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
        return function () { return message.format(this); };
    };
    locales.push({
        name: 'EN',
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
            moreThanValidation: f('Data should have value more than {value}'),
            minValidation: f('Data should have value at least {value}'),
            lessThanValidation: f('Data should have value less than {value}'),
            maxValidation: f('Data should have value less than or equal {value}'),
            equalValidation: f('Data should have value equal to {label:{equalProperty}}'),
            notEqualValidation: f('Data should have value not equal to {label:{notEqualProperty}}')
        }
    });
})(Higgs.locales);
if (!window['locale'] || window['locale'].toUpperCase() === 'EN')
    Higgs.locale = Higgs.locales[Higgs.locales.length - 1];
String.prototype.format = function (data) {
    var temp = this.toString();
    var args = !Array.isArray(data) ? [data] : data;
    for (var i = 0; i < args.length; i++) {
        var par = args[i];
        if (typeof par === 'object') {
            if (!par)
                continue;
            for (var j in par) {
                if (!par.hasOwnProperty(j))
                    continue;
                if (typeof par[j] === 'object' || par[j] === null || par[j] === undefined || $.isFunction(par[j]))
                    continue;
                temp = temp.replaceAll('{' + j + '}', par[j] + '', true);
            }
        }
        else {
            temp = temp.replaceAll('{' + i + '}', arguments[i] + '', true);
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
    if (isIgnoreCase === void 0) { isIgnoreCase = false; }
    if (!isIgnoreCase)
        return this.indexOf(value) === 0;
    return this.toUpperCase().startsWith(value.toUpperCase());
};
String.prototype.endsWith = function (value, isIgnoreCase) {
    if (isIgnoreCase === void 0) { isIgnoreCase = false; }
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
        if (!url) {
            return baseUrl;
        }
        baseUrl = url;
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
        if (!parts)
            return defaultValue;
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
            var isArray = $.isArray(object);
            for (var key in object) {
                var postKey = '';
                if (isArray || typeof key === 'number') {
                    postKey = (prefix != '' ? prefix : '') + '[' + key + ']';
                }
                else {
                    postKey = (prefix != '' ? prefix + '.' : '') + key;
                }
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
            }
            else {
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
        var isControl = controls.is(Higgs.disableElementSelect);
        if (!isControl) {
            var container = controls;
            container.toggleClass('disabled', isDisabled);
            controls = controls.find(Higgs.disableElementSelect);
        }
        controls.prop('disabled', isDisabled);
        controls.toggleClass('disabled', isDisabled);
        controls.trigger(isDisabled ? 'disabled' : 'enabled');
        return this;
    };
    fn.readonly = function () {
        var controls = $(this);
        controls.each(function (i, el) {
            var $el = $(el);
            if ($el.is(':radio')) {
                $el.disable();
            }
            else {
                $el.attr('readonly', 'readonly')
                    .trigger('readonly');
            }
        });
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
                }
                else {
                    value = [];
                    for (var i = 0; i < x.length; i++) {
                        var chk = $(x[i]);
                        if (!chk.is(':checked'))
                            continue;
                        value.push(getChkValue.call(chk));
                    }
                }
            }
            else if (x.is(':radio')) {
                if (x.length == 1) {
                    x = x.parents('form').find(':radio[name="' + x[0].name + '"]');
                }
                x.each(function () {
                    if (this.checked) {
                        value = this.value;
                    }
                });
            }
            else {
                value = x.val();
            }
            return value;
        }
        var compareValue = function (value1, value2) {
            if (value1 === undefined || value1 === null) {
                value1 = '';
            }
            else {
                value1 = value1.toString().toLowerCase();
            }
            if (value2 === undefined || value2 === null) {
                value2 = '';
            }
            else {
                value2 = value2.toString().toLowerCase();
            }
            if (typeof value2 === 'number') {
                value1 = parseFloat(value1);
            }
            return value1 === value2;
        };
        if (x.is(':radio')) {
            if (x.length == 1) {
                x = x.parents('form').find(':radio[name="' + x[0].name + '"]');
            }
            x.each(function () {
                if (compareValue(this.value, value))
                    $(this).prop('checked', true);
            });
        }
        else if (x.is(':checkbox')) {
            if (this[0].value && typeof value !== 'boolean') {
                value = this[0].value === value;
            }
            this.prop('checked', value);
        }
        else if (x.is('select')) {
            var selectedValue;
            $('option', x).each(function () {
                var cValue = $(this).val();
                if (compareValue(cValue, value))
                    selectedValue = cValue;
            });
            x.val(selectedValue === undefined ? $('option:eq(0)', x).val() : selectedValue);
        }
        else {
            x.val(value);
        }
        return this;
    };
    function getChkValue() {
        var value = this.val();
        if (this[0].value !== 'on' && this[0].value) {
            return this.is(':checked') ? this[0].value : null;
        }
        return value === 'on' ? this.is(':checked') : value;
    }
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
(function (Higgs) {
    var AbstructValidation = /** @class */ (function () {
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
    }());
    Higgs.AbstructValidation = AbstructValidation;
    var ValidationResult = /** @class */ (function () {
        function ValidationResult(validationRule, context, path, value) {
            if (path) {
                validationRule = validationRule;
                this.context = context;
                this.message = validationRule.message(context, value);
                this.path = path;
                this.context = context;
            }
            else {
                this.path = validationRule;
                this.message = context;
                this.context = context;
            }
        }
        return ValidationResult;
    }());
    Higgs.ValidationResult = ValidationResult;
    var SerializeOptions = /** @class */ (function () {
        function SerializeOptions() {
            this.ignoreEmptyValue = true;
        }
        SerializeOptions.defaultValue = new SerializeOptions();
        return SerializeOptions;
    }());
    Higgs.SerializeOptions = SerializeOptions;
    var ValidateOptions = /** @class */ (function () {
        function ValidateOptions() {
            // For dynamic form, don't cache
            this.cacheValidationObject = false;
            this.ignoreValidation = false;
            this.stopOnFirstInvalid = true;
            this.enableFadeInFeedback = true;
        }
        ValidateOptions.defaultValue = new ValidateOptions();
        return ValidateOptions;
    }());
    Higgs.ValidateOptions = ValidateOptions;
    var ValidateContext = /** @class */ (function () {
        function ValidateContext(parent, container, elementName) {
            this.parent = parent;
            this.validationContainer = container;
            this.elementName = elementName;
        }
        return ValidateContext;
    }());
    Higgs.ValidateContext = ValidateContext;
    var RuleData = /** @class */ (function () {
        function RuleData() {
        }
        return RuleData;
    }());
    Higgs.RuleData = RuleData;
    function isEmpty(value) {
        return value === undefined || value != null && value.constructor === String && value === '';
    }
    Higgs.isEmpty = isEmpty;
    var validationRules = [];
    var Rules;
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
        function getElementValidation(allowedRules) {
            return $.extend(true, [], validationRules.filter(function (x) { return !x.path && (!allowedRules || $.inArray(x.validation, allowedRules) >= 0); }));
        }
        Rules.getElementValidation = getElementValidation;
        function getPropertyValidationData(allowedRules) {
            return $.extend(true, [], validationRules.filter(function (x) { return !!x.path && (!allowedRules || $.inArray(x.validation, allowedRules) >= 0); }));
        }
        Rules.getPropertyValidationData = getPropertyValidationData;
    })(Rules = Higgs.Rules || (Higgs.Rules = {}));
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
        var result = {};
        this.each(function () {
            var model = serialize.call(this, settings);
            $.extend(result, model);
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
        }
        else {
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
        controls
            .filter(':visible,input[type=hidden]')
            .not(':disabled,button')
            .each(function () {
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
            }
            else if (cValue !== undefined) {
                if ($.isArray(cValue)) {
                    var cArray = cValue;
                    cArray.push(value);
                }
                else {
                    $.setObject(this.name, [cValue, value], result);
                }
            }
            else {
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
                    obj[key].sort(function (a, b) { return a.orderIndex - b.orderIndex; });
                }
                else {
                    orderRules(obj[key]);
                }
            }
        }
    }
    fn.getValidationModel = function (modelData, options) {
        var x = $(this);
        var controls;
        var result = {};
        // TODO: support multiple containers
        controls = x.is(':input') ? x : $(':input', this);
        controls = controls.filter(':visible');
        var elementValidations = Higgs.Rules.getElementValidation(options.allowedRules);
        var propertyValidationDatas = Higgs.Rules.getPropertyValidationData(options.allowedRules);
        // Get all element-based rules.
        controls
            .not('button')
            .each(function (item, el) {
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
                            ruleObj.message = function () { return customMessage; };
                        }
                        else if (typeof customMessage === "function") {
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
        // Get property rules
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
    fn.validateModel = function (options, formData) {
        var container = $(this);
        options = options || $.extend(true, {}, Higgs.ValidateOptions.defaultValue);
        options.validationContainer = container;
        if (container.is('form') && (options.ignoreValidation))
            return true;
        if (options.validationModel)
            options.cacheValidationObject = false;
        formData = formData || container.serialize(true);
        //container.data(validatedModelKey, obj);
        var validationModel;
        if (options.cacheValidationObject && !options.validationModel) {
            validationModel = container.data(validationModelKey);
            if (!validationModel) {
                validationModel = container.getValidationModel(formData, options);
                container.data(validationModelKey, validationModel);
            }
        }
        else {
            validationModel = container.getValidationModel(formData, options);
        }
        if (options.validationModel) {
            for (var key in options.validationModel) {
                if (options.validationModel[key].hasOwnProperty(key))
                    continue;
                var validationRules = options.validationModel[key];
                if (!validationRules || !validationRules.length)
                    continue;
                var cRules = validationModel[key];
                if (!cRules) {
                    cRules = validationModel[key] = (new validationRuleArray());
                }
                for (var i = 0; i < validationRules.length; i++) {
                    var r = validationRules[i];
                    var lastIndex = 0;
                    for (var j = 0; j < cRules.length; j++) {
                        if (r.orderIndex < cRules[j].orderIndex) {
                            break;
                        }
                        else {
                            lastIndex = j;
                        }
                    }
                    cRules.splice(lastIndex, 0, r);
                }
            }
        }
        var result = fn.validateModel.test(validationModel, options, formData);
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
                var context = new Higgs.ValidateContext(obj, options.validationContainer, name);
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
            }
            else {
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
})(jQuery.fn);
//#endregion
//#region Validation Rules
(function (Higgs) {
    var Rules;
    (function (Rules) {
        var FieldDependencyOption = /** @class */ (function () {
            function FieldDependencyOption() {
                this.hasValue = undefined;
                this.value = undefined;
            }
            return FieldDependencyOption;
        }());
        Rules.FieldDependencyOption = FieldDependencyOption;
        ;
        var RequiredValidation = /** @class */ (function (_super) {
            __extends(RequiredValidation, _super);
            function RequiredValidation(option) {
                var _this = _super.call(this, option) || this;
                _this.isRequired = true;
                _this.fieldDependency = null;
                _this.orderIndex = 10;
                if (option === undefined)
                    return _this;
                if (typeof option === 'boolean') {
                    _this.isRequired = option;
                }
                else if (typeof option === 'string') {
                    // should be "field1,field2,!field3"
                    _this.loadDependencyFromString(option);
                }
                else if ($.isArray(option) && option.length > 0) {
                    // Custom field Dependency
                    var array = option;
                    if (typeof array[0] === 'string') {
                        _this.loadDependencyFromStringArray(option);
                    }
                    else {
                        // Custom
                        _this.fieldDependency = option;
                    }
                }
                return _this;
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
                    }
                    else {
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
                    }
                    else {
                        if (fieldOption.value === undefined) {
                            if (context.parent[name] === false || context.parent[name] === undefined)
                                return true;
                            if (Higgs.isEmpty(context.parent[name]))
                                return true;
                        }
                        else if (fieldOption.value !== context.parent[name]) {
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
                // Beware: `data.required` doesn't work for dynamic element
                var required = $(el).attr('required') || $(el).attr('data-required');
                if (!required)
                    return null;
                if (required === 'required')
                    required = true; // Case when set data-required="required" should create same validation as required="required"
                return new RequiredValidation(required);
            };
            return RequiredValidation;
        }(Higgs.AbstructValidation));
        Rules.RequiredValidation = RequiredValidation;
        var StringLengthValidation = /** @class */ (function (_super) {
            __extends(StringLengthValidation, _super);
            function StringLengthValidation(length1, length2) {
                var _this = _super.call(this, length1, length2) || this;
                _this.minLength = null;
                _this.maxLength = null;
                _this.orderIndex = 20;
                if (length2 === undefined) {
                    _this.maxLength = length1;
                }
                else {
                    if (length1 !== undefined) {
                        _this.minLength = length1;
                    }
                    _this.maxLength = length2;
                }
                return _this;
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
                var maxLength = el.maxLength < 50000 ? el.maxLength : 0 || data.maxLength; // prevent max-length default value.
                if (!minLength && !maxLength)
                    return null;
                // Firefox default max & min length value.
                if (maxLength === -1 && !minLength)
                    return null;
                return new StringLengthValidation(minLength, maxLength);
            };
            return StringLengthValidation;
        }(Higgs.AbstructValidation));
        Rules.StringLengthValidation = StringLengthValidation;
        var PatternValidation = /** @class */ (function (_super) {
            __extends(PatternValidation, _super);
            function PatternValidation(pattern) {
                var _this = _super.call(this, pattern) || this;
                _this.pattern = null;
                _this.orderIndex = 50;
                if (pattern instanceof RegExp) {
                    _this.pattern = pattern;
                }
                else {
                    _this.pattern = new RegExp(pattern);
                }
                return _this;
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
        }(Higgs.AbstructValidation));
        Rules.PatternValidation = PatternValidation;
        var NumberValidation = /** @class */ (function (_super) {
            __extends(NumberValidation, _super);
            function NumberValidation(isFloat) {
                var _this = _super.call(this, isFloat ? /^-?\d{1,3}(,?\d{3})*(\.\d+)?$/g : /^-?\d{1,3}(,?\d{3})*$/g) || this;
                _this.orderIndex = 30;
                return _this;
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
                }
                else if (!!data['integer']) {
                    validation = new NumberValidation(false);
                    validation.isInteger = true;
                }
                return validation;
            };
            return NumberValidation;
        }(PatternValidation));
        Rules.NumberValidation = NumberValidation;
        var UrlValidation = /** @class */ (function (_super) {
            __extends(UrlValidation, _super);
            function UrlValidation() {
                var _this = _super.call(this, UrlValidation.getRegex()) || this;
                _this.orderIndex = 30;
                return _this;
            }
            UrlValidation.getRegex = function () {
                // Code from: https://gist.github.com/dperini/729294
                // More Info: http://mathiasbynens.be/demo/url-regex
                var regex = new RegExp("^" +
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
                    "$", "i");
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
        }(PatternValidation));
        Rules.UrlValidation = UrlValidation;
        var EmailValidation = /** @class */ (function (_super) {
            __extends(EmailValidation, _super);
            function EmailValidation() {
                var _this = _super.call(this, EmailValidation.getRegex()) || this;
                _this.orderIndex = 30;
                return _this;
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
        }(PatternValidation));
        Rules.EmailValidation = EmailValidation;
        var MoreThanValidation = /** @class */ (function (_super) {
            __extends(MoreThanValidation, _super);
            function MoreThanValidation(moreThan) {
                var _this = _super.call(this, moreThan) || this;
                _this.orderIndex = 40;
                _this.value = moreThan;
                return _this;
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
        }(Higgs.AbstructValidation));
        Rules.MoreThanValidation = MoreThanValidation;
        var MinValidation = /** @class */ (function (_super) {
            __extends(MinValidation, _super);
            function MinValidation(min) {
                var _this = _super.call(this, min) || this;
                _this.orderIndex = 41;
                _this.value = min;
                return _this;
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
        }(Higgs.AbstructValidation));
        Rules.MinValidation = MinValidation;
        var LessThanValidation = /** @class */ (function (_super) {
            __extends(LessThanValidation, _super);
            function LessThanValidation(lessThan) {
                var _this = _super.call(this, lessThan) || this;
                _this.orderIndex = 42;
                _this.value = lessThan;
                return _this;
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
        }(Higgs.AbstructValidation));
        Rules.LessThanValidation = LessThanValidation;
        var MaxValidation = /** @class */ (function (_super) {
            __extends(MaxValidation, _super);
            function MaxValidation(max) {
                var _this = _super.call(this, max) || this;
                _this.orderIndex = 43;
                _this.value = max;
                return _this;
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
        }(Higgs.AbstructValidation));
        Rules.MaxValidation = MaxValidation;
        var EqualValidation = /** @class */ (function (_super) {
            __extends(EqualValidation, _super);
            function EqualValidation(propertyName) {
                var _this = _super.call(this, propertyName) || this;
                _this.equalProperty = null;
                _this.orderIndex = 60;
                _this.equalProperty = propertyName;
                return _this;
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
        }(Higgs.AbstructValidation));
        Rules.EqualValidation = EqualValidation;
        var NotEqualValidation = /** @class */ (function (_super) {
            __extends(NotEqualValidation, _super);
            function NotEqualValidation(propertyName) {
                var _this = _super.call(this, propertyName) || this;
                _this.notEqualProperty = null;
                _this.orderIndex = 60;
                _this.notEqualProperty = propertyName;
                return _this;
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
        }(Higgs.AbstructValidation));
        Rules.NotEqualValidation = NotEqualValidation;
        // Auto register all validation
        for (var key in Higgs.Rules) {
            if (!Higgs.Rules.hasOwnProperty(key))
                continue;
            var fieldName = key;
            if (fieldName.endsWith('Validation'))
                Rules.register(fieldName.substring(0, fieldName.length - 10), Higgs.Rules[fieldName]);
        }
    })(Rules = Higgs.Rules || (Higgs.Rules = {}));
})(Higgs || (Higgs = {}));
(function (Higgs) {
    Higgs.validationErrorClass = 'glyphicon-exclamation-sign';
})(Higgs || (Higgs = {}));
(function (fn) {
    var popoverContainerId = 'popover_validationError';
    var popover;
    var popoverContent;
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
        options = $.extend(true, {}, options || {}, Higgs.ValidateOptions.defaultValue);
        container.clearValidationResult();
        var inputs = $('.form-group :input', container);
        inputs.off('.validationResult');
        var isValid = true;
        for (var i = 0; i < result.length; i++) {
            var cResult = result[i];
            var input = inputs.filter('[name="' + cResult.path + '"]:visible');
            if (input.length === 0)
                continue;
            isValid = false;
            var inputContainer = input.parents('[class^=col-]:first,.form-group:first').first();
            var feedback = input.nextAll('.form-control-feedback:eq(0)');
            var label = $('label[for="' + cResult.path + '"]:eq(0)', container);
            inputContainer.addClass('has-error has-feedback');
            if (!feedback.length) {
                feedback = $('<span class="glyphicon form-control-feedback"></span>');
                feedback.css('top', label.length ? label.outerHeight(true) : 0);
                var inputParent = input.parent();
                if (inputParent.is('.input-group')) {
                    var inputGroup = inputParent;
                    var rightMargin = 0;
                    var nextControls = input.nextAll(':visible');
                    nextControls.each(function (index, el) {
                        rightMargin += $(el).outerWidth(true);
                    });
                    feedback.insertAfter(inputGroup);
                    if (rightMargin > 0) {
                        feedback.css({
                            right: (rightMargin + parseInt(feedback.css('right'), 10)) + 'px'
                        });
                    }
                }
                else {
                    feedback.insertAfter(input);
                }
                feedback.on('click', input, function (e) {
                    var control = e.data;
                    control.focus();
                });
                if (options.enableFadeInFeedback) {
                    feedback
                        .addClass('fade')
                        .redraw()
                        .addClass('in');
                }
            }
            feedback.addClass(Higgs.validationErrorClass);
            // Convert to plug-in module
            input.off('*.validationResult.popover');
            if (input.is('.select2')) {
                input.on('select2:opening.validationResult.popover', null, cResult, onFocusInvalidInput);
            }
            else {
                input.on('focus.validationResult.popover', null, cResult, onFocusInvalidInput);
            }
            if (input.is(':focus'))
                input.trigger('focus');
        }
        return isValid;
    };
    function onFocusInvalidInput(e) {
        var input = $(this);
        var result = e.data;
        if (input.is(':disabled'))
            return;
        if (popover) {
            popover.modalPopover('hide');
            popover.remove();
        }
        fn.displayValidationResult.createPopover();
        $('<p/>')
            .html(getErrorMessage(result))
            .appendTo(popoverContent);
        var popOverTarget = input.data('popoverTarget') || input.parent().is('.input-group') ? input.parent() : input;
        // TODO: auto display in other placement if popover is overflow.
        popover
            .data('target', this)
            .modalPopover({
            target: popOverTarget
        })
            .modalPopover('show');
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
                }
                else {
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
    fn.validate = function (options, customValidationResult) {
        var isTotalValid = true;
        $.each(this, function () {
            var container = $(this);
            options = options || $.extend(true, {}, Higgs.ValidateOptions.defaultValue);
            var result = container.validateModel(options);
            if (customValidationResult)
                result = $.merge(result, customValidationResult);
            // Some hidden input is not counted as invalid.
            var isValid = container.displayValidationResult(result, options);
            container.off('.higgs.revalidate');
            if (!isValid)
                setupValidationBlur(container, options);
            isTotalValid = isTotalValid && isValid;
        });
        return isTotalValid;
    };
    function setupValidationBlur(container, options) {
        container.on('blur.higgs.revalidate change.higgs.revalidate', '.has-error :input', options, function (e) {
            if (popover) {
                var cTarget = popover.data('target');
                if (cTarget === e.target) {
                    popover.remove();
                }
                else {
                    // TODO: Investigate why select2-focusser trigger blur event instead of normal input.
                    var $target = $(e.target);
                    if ($target.is('.select2-focusser') && $target.parents('.select2-container').next()[0] === cTarget) {
                        popover.remove();
                    }
                }
            }
            var validationOptions = e.data;
            validationOptions.enableFadeInFeedback = false;
            container.validate(validationOptions);
        });
    }
    // Disable HTML5 form validation
    fn.disableHtml5Validation = function () {
        $(':input', this).on('invalid', function (e) {
            e.stopPropagation();
            return false;
        });
        return this;
    };
})(jQuery.fn);
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
/// <reference path="../jquery-1.9.1.js" />
/// <reference path="../jquery.cookie-1.0.js" />

//#region Global Variables

$defaultSpeed = window.$defaultSpeed = 250;
$speed = window.$speed = $.cookie && $.cookie('$speed') || $defaultSpeed;
$theme = $.cookie && $.cookie('$theme') || "default";
css = window.css =
{
    hover: 'hover',
    focus: 'focus',
    must_focus: 'focus_important',
    click: 'click',
    mouseDown: 'mousedown',
    mouseUp: 'mouseup'
};
emptyGuid = '00000000-0000-0000-0000-000000000000';

// Polyfill for old IE
$(function ()
{
    if ($.browser.msie && $.browser.version <= 7)
    {
        $(document)
            .on('focus', ':input', undefined, function (event, input, errorMsg)
            {
                $(this).addClass('focus');
            })
            .on('blur', ':input', undefined, function (event, input, errorMsg)
            {
                $(this).removeClass('focus');
            });
    }
});

//#endregion

//#region Common Function

function openUrl(url)
{
    location.href = getUrl(url);
}

function setSpeed(speed)
{
    ///	<summary>
    ///		 Keep animation speed in browser cookie.
    ///	</summary>
    ///	<param name="speed" type="Number">
    ///		Minimum time is used as delay between each animation frame.
    ///	</param>

    speed = speed > 1 ? speed : 1;
    $.cookie('$speed', speed, { path: '/', expires: 3650 });
    $speed = speed;
}

if (!$speed) setSpeed($defaultSpeed);

function getUrl(logicalPath)
{
    ///	<summary>
    ///		 Retrieve URL based on given path and current application URL.
    ///	</summary>
    ///	<param name="logicalPath" type="String">
    ///		Path refers to exact path based on application URL. 
    ///    For example, ~/images/some-file.jpg
    ///	</param>
    ///	<returns type="String" />

    if ((/^[a-z-]+:\/\//).test(logicalPath) || !applicationUrl) return logicalPath;

    if (logicalPath.indexOf('~/') < 0) return (location.origin || location.href.substring(0, location.href.indexOf(location.pathname))) + '/' + (logicalPath.indexOf('/') == 0 ? logicalPath.substring(1) : logicalPath);
    if (logicalPath.indexOf('~/App_Themes/') > 0)
    {
        logicalPath = logicalPath.replace('~/App_Themes/', '~/App_Themes/' + $theme + '/');
    }

    return applicationUrl + logicalPath.substring(2);
}

function GotoUrl(url)
{
    location.href = getUrl(url);
}

function noCacheUrl(url)
{
    var tick = (new Date()).getTime();
    var noCache = '__requestid__=' + tick;

    var index = url.indexOf('?');
    if (index > 0)
    {
        return url + '&' + noCache;
    }
    else
    {
        return url + '?' + noCache;
    }
}

// Use to serialize data from .NET JSON Date
function DeserializeJSONDate(data)
{
    return eval('new' + data.replace(/\//g, ' '));
}

function isNullOrEmpty(data)
{
    return data === undefined || data === null || data === '';
}

function preLoad(src)
{
    var img = new Image();
    img.src = getUrl(src);
}

// Original code at http://www.filamentgroup.com/lab/jquery_plugin_for_requesting_ajax_like_file_downloads/
$.sendData = function (url, data, method)
{
    //url and data options required
    if (url)
    {
        if (url.indexOf('~/') === 0) url = getUrl(url);
        var form = $('<form action="' + url + '" method="' + (method || 'post') + '"></form>');

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
        form.remove();
    };
};

//#endregion

//#region String Function

// TODO: Make sure this function do same process as StringHelper.Format
String.prototype.format = function (params)
{
    ///	<summary>
    ///		 Replace variable in current string with given parameter[s].
    ///	</summary>
    ///	<param name="params" type="Array">
    ///		Group of parameter that will be replace in current string. 
    ///    They can be both simple string or Object.
    ///	</param>
    var temp = this.toString();

    for (var i = 0; i < arguments.length; i++)
    {
        var par = arguments[i];

        if (typeof par === 'object')
        {
            if (!par) continue;

            for (var j in par)
            {
                if (!par.hasOwnProperty(j)) continue;
                if (typeof par[j] === 'object' || par[j] === null || par[j] === undefined || $.isFunction(par[j])) continue;

                temp = temp.replaceAll('{' + j + '}', par[j], true);
            }
        }
        else
        {
            temp = temp.replaceAll('{' + i + '}', arguments[i], true);
        }
    }

    return temp;
};

String.prototype.startsWith = function (value)
{
    return new RegExp('^' + RegExp.escape(value)).test(this);
};

String.prototype.endsWith = function (value)
{
    return new RegExp(RegExp.escape(value) + '$').test(this);
};

String.prototype.padLeft = function (length, padChar)
{
    padChar = padChar ? padChar : '0';
    var temp = this + '';

    while (temp.length < length)
    {
        temp = padChar + temp;
    }

    return temp;
};

String.prototype.replaceAll = function (findString, newString, isIgnorCase)
{
    var temp = this;

    return temp.replace(new RegExp(RegExp.escape(findString), isIgnorCase ? 'ig' : 'g'), newString);
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

String.prototype.fill = function (model)
{
    var msg = this;
    var pattern = /\$\{([a-z_\$]+)\}/ig;
    var match;

    while ((match = pattern.exec(msg)) !== null)
    {
        var variableName = match[1];
        var variableValue = model[variableName];

        if (variableValue !== undefined)
        {
            msg = msg.replaceAll('${' + variableName + '}', variableValue || '', false);
        }
    }

    return msg;
};

if (!String.prototype.trim)
{
    String.prototype.trim = function () { return this.replace(/^\s\s*/, '').replace(/\s\s*$/, ''); };
}

if (!String.prototype.ltrim)
{
    String.prototype.ltrim = function () { return this.replace(/^\s+/, ''); };
}

if (!String.prototype.rtrim)
{
    String.prototype.rtrim = function () { return this.replace(/\s+$/, ''); };
}

RegExp.escape = function (text)
{
    if (!text) return text.toString();

    return text.toString().replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
};

//#endregion

//#region Array Helpers

Array.prototype.indexOf = Array.prototype.indexOf || function (elt)
{
    var len = this.length;

    var from = Number(arguments[1]) || 0;
    from = (from < 0) ? Math.ceil(from) : Math.floor(from);

    if (from < 0) from += len;

    for (; from < len; from++)
    {
        if (from in this && this[from] === elt) return from;
    }

    return -1;
};

Array.prototype.remove = function (item)
{
    var toBeRemoved = arguments;

    return $.grep(this, function (value)
    {
        for (var i = 0; i < toBeRemoved.length; i++)
        {
            if (toBeRemoved[i] === value)
                return false;
        }

        return true;
    });
};

Array.prototype.any = function (fn)
{
    for (var i = 0; i < this.length; i++)
    {
        if (fn.call(this[i])) return true;
    }

    return false;
};

Array.prototype.all = function (fn)
{
    for (var i = 0; i < this.length; i++)
    {
        if (!fn.call(this[i])) return false;
    }

    return true;
};

// IE 7-8 Polyfill
if (!Array.prototype.map)
{
    Array.prototype.map = function (callback, thisArg)
    {

        var T, A, k;

        if (this == null)
        {
            throw new TypeError(" this is null or not defined");
        }

        // 1. Let O be the result of calling ToObject passing the |this| value as the argument.
        var O = Object(this);

        // 2. Let lenValue be the result of calling the Get internal method of O with the argument "length".
        // 3. Let len be ToUint32(lenValue).
        var len = O.length >>> 0;

        // 4. If IsCallable(callback) is false, throw a TypeError exception.
        // See: http://es5.github.com/#x9.11
        if ({}.toString.call(callback) != "[object Function]")
        {
            throw new TypeError(callback + " is not a function");
        }

        // 5. If thisArg was supplied, let T be thisArg; else let T be undefined.
        if (thisArg)
        {
            T = thisArg;
        }

        // 6. Let A be a new array created as if by the expression new Array(len) where Array is
        // the standard built-in constructor with that name and len is the value of len.
        A = new Array(len);

        // 7. Let k be 0
        k = 0;

        // 8. Repeat, while k < len
        while (k < len)
        {

            var kValue, mappedValue;

            // a. Let Pk be ToString(k).
            //   This is implicit for LHS operands of the in operator
            // b. Let kPresent be the result of calling the HasProperty internal method of O with argument Pk.
            //   This step can be combined with c
            // c. If kPresent is true, then
            if (k in O)
            {

                // i. Let kValue be the result of calling the Get internal method of O with argument Pk.
                kValue = O[k];

                // ii. Let mappedValue be the result of calling the Call internal method of callback
                // with T as the this value and argument list containing kValue, k, and O.
                mappedValue = callback.call(T, kValue, k, O);

                // iii. Call the DefineOwnProperty internal method of A with arguments
                // Pk, Property Descriptor {Value: mappedValue, : true, Enumerable: true, Configurable: true},
                // and false.

                // In browsers that support Object.defineProperty, use the following:
                // Object.defineProperty(A, Pk, { value: mappedValue, writable: true, enumerable: true, configurable: true });

                // For best browser support, use the following:
                A[k] = mappedValue;
            }
            // d. Increase k by 1.
            k++;
        }

        // 9. return A
        return A;
    };
}

if (!Array.prototype.forEach)
{
    Array.prototype.forEach = function (fn, scope)
    {
        var i, len;
        for (i = 0, len = this.length; i < len; ++i)
        {
            if (i in this)
            {
                fn.call(scope, this[i], i, this);
            }
        }
    };
}

//#endregion

//#region Function Helpers

// PolyFill for bind function
if (!Function.prototype.bind)
{
    Function.prototype.bind = function (context)
    {
        ///	<summary>
        ///	    Create Function pointer with custom context.
        ///	</summary>
        ///	<returns type="Function" />

        var method = this;
        var args = arguments;

        return function ()
        {
            return method.apply(context, args);
        };
    };
}

Function.prototype.bindArg = function ()
{
    ///	<summary>
    ///	    Create Function pointer with custom context.
    ///	</summary>
    ///	<returns type="Function" />

    var method = this;
    var args = arguments;

    return function ()
    {
        return method.apply(this, args);
    };
};

//#endregion

//#region Reflection Function

function getTypeName(obj)
{
    if (typeof obj !== "function") return null;
    
    var regex = /function[\s]*?([\S]+)[\s]*?\(/gm;
    var context = obj.toString();
    var result = regex.exec(context);

    return result[1];
}

//#endregion

//#region Enum Function

Number.prototype.ToEnum = function (type)
{
    var value = this;
    var enumValue = null;

    for (var x in type)
    {
        if (type[x] === value) enumValue = x;
    }

    return enumValue;
};
//#endregion

//#region jQuery - override function

var oldAjax = $.ajax;
$.ajax = function ()
{
    if (typeof arguments[0].url === 'function')
    {
        arguments[0].url = arguments[0].url();
    }
    
    return oldAjax.apply(this, arguments);
};

$.req = function (url, data, callback)
{
    if (callback === undefined && data !== undefined)
    {
        callback = data;
    }

    $.ajax
    ({
        type: 'POST',
        dataType: 'json',
        url: url,
        data: data,
        success: callback
    });
};

//#endregion

//#region jQuery - Common Plug-in

higgs =
{
    error: function (msg)
    {
        if (console && console.error) console.error(msg);

        throw new Error(msg);
    }
};

// TODO: Move to higgs
$.higgs = {};
$.higgs.controls = {};
$.extend($.easing,
{
    easeInCubic: function (x, t, b, c, d)
    {
        return c * (t /= d) * t * t + b;
    },
    easeOutCubic: function (x, t, b, c, d)
    {
        return c * ((t = t / d - 1) * t * t + 1) + b;
    },
    easeInOutCubic: function (x, t, b, c, d)
    {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t + 2) + b;
    }
});

$.fn.exists = function ()
{
    return $(this).length > 0;
};

$.isGuid = function (text)
{
    var guidPattern = /^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$/;

    return guidPattern.test(text);
};

$.fn.documentOffset = function ()
{
    ///	<summary>
    ///		Exactly find current location of current from top-left of screen in pixel.
    ///	</summary>
    ///	<returns type="Number" />

    var curleft = this[0].offsetLeft || 0;
    var curtop = this[0].offsetTop || 0;
    var temp = this[0];

    while (temp = temp.offsetParent)
    {
        curleft += temp.offsetLeft;
        curtop += temp.offsetTop;
    }

    return { left: curleft, top: curtop };
};

$.fn.delayFireEvent = function (eventName, time, fn)
{
    ///	<summary>
    ///		 Create some delay before fire function after original jQuery event handler fire it.
    ///	</summary>
    ///	<param name="eventName" type="String">
    ///		Name of binded event that will fire function after specify time.
    ///	</param>
    ///	<param name="time" type="Number">
    ///		Delay before fire function in millisecond.
    ///	</param>
    ///	<param name="fn" type="Function">
    ///		The function will be fired by binded event.
    ///	</param>

    var timer;
    var delayImpl = function (eventObj)
    {
        if (timer !== null) clearTimeout(timer);

        var newFn = function ()
        {
            fn(eventObj);
        };

        timer = setTimeout(newFn, time);
    };

    return this.each(function ()
    {
        $(this).bind(eventName, function (eventObj)
        {
            delayImpl(eventObj);
        });
    });
};

$.fn.ensureHasId = function (pattern)
{
    if (!$(this).attr('id'))
    {
        if (!pattern) pattern = '{0}-{1}';

        return $(this).each(function ()
        {
            var i = 0;
            var temp;

            do
            {
                i++;
                temp = pattern.format(this.tagName, i);
            } while ($('#' + temp).size() > 0)

            $(this).attr('id', pattern.format(this.tagName, i));
        });
    }

    return $(this);
};

$.fn.disableSelection = function ()
{
    var x = $(this);

    x.each(function ()
    {
        this.onselectstart = function ()
        {
            return false;
        };
        this.unselectable = "on";

        if (this.style)
        {
            this.style.MozUserSelect = "none";
            this.style.cursor = "default";
        }
    });

    return x;
};

$.fn.selectText = function (selectionStart, selectionEnd)
{
    var x = this[0];

    if (!selectionEnd)
    {
        selectionEnd = $(this).value().length;
    }

    if (x.setSelectionRange)
    {
        x.focus();
        x.setSelectionRange(selectionStart, selectionEnd);
    }
    else if (x.createTextRange)
    {
        var range = x.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    }
};

//#endregion

//#region jQuery - Form Plug-in

$.each('prepareSubmit beforeSubmit afterSubmit'.split(' '), function (i, o)
{
    $.fn[o] = function (fn)
    {
        var form = $(this);

        if (form.size() === 0) return form;

        if (fn)
        {
            form.bind(o, null, fn);
        }
        else
        {
            form = form.is('form') ? form : form.parents('form');
            form.trigger(o);
        }

        return this;
    };
});

$.fn.submitTask = function (fn)
{
    var x = $(this);
    var submitTasks = x.data('higgs.submitTask');

    if (!submitTasks) submitTasks = [];

    if ($.isFunction(fn))
    {
        submitTasks.push(fn);
        x.data('higgs.submitTask', submitTasks);
    }

    return x;
};

$.fn.processTask = function (callback)
{
    var x = $(this);
    var submitTasks = x.data('higgs.submitTask');

    if (!$.isArray(submitTasks) || submitTasks.length === 0)
    {
        callback();
    }
    else
    {
        var processFn = function (i)
        {
            if (i === submitTasks.length)
            {
                callback();
                return;
            }

            submitTasks[i].call(this, function ()
            {
                processFn(i + 1);
            });
        };

        processFn(0);
    }

    return this;
};

$.fn._jQuerySubmit = $.fn.submit;
$.fn.submit = function (fn)
{
    var x = $(this);
    var form = x.is('form') ? x : x.parents('form');

    if (form.length === 0) return this;
    if ($.isFunction(fn))
    {
        return form._jQuerySubmit(fn);
    }

    form.prepareSubmit();

    var parent = form.parent();
    var submitTaskCallback = function ()
    {
        form._jQuerySubmit()
            .afterSubmit();
    };
    var beforeSubmitCallback = function ()
    {
        form.processTask(submitTaskCallback);
    };

    parent.on('beforeSubmit', beforeSubmitCallback);
    form.beforeSubmit();
    parent.off('beforeSubmit', beforeSubmitCallback);

    return false;
};

$.fn.reset = function ()
{
    var elements = this.is(':input') ? this : $(':input', this);

    elements.each(function ()
    {
        $(this).value('').blur();
    });

    return this;
};

$.fn.readOnly = function (value, clearValue, defaultValue)
{
    var x = $(this);
    value = typeof value !== 'undefined' ? value : true;

    if (x.is(':input'))
    {
        if (x.is('select,button,:checkbox'))
        {
            x.attr('disabled', value);
        }
        else
        {
            x
                .attr('readonly', value)
                .addClass('readOnly');
        }
    }
    else
    {
        x.find(':input').not('button,select,:checkbox')
            .attr('readonly', value)
            .addClass('readOnly');

        x.find(':checkbox,button,select').attr('disabled', value);
    }

    if (clearValue)
    {
        if (value)
        {
            x.value(typeof defaultValue !== 'undefined' ? defaultValue : null);
        }
        else
        {
            x.focus();
        }
    }

    return this;
};

$.fn.value = function (value)
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
            }
            else
            {
                value = [];

                for (var i = 0; i < x.length; i++)
                {
                    var chk = $(x[i]);

                    if (!chk.is(':checked')) continue;

                    value.push(getChkValue.call(chk));
                }
            }
        }
        else if (x.is(':radio'))
        {
            if (x.length == 1)
            {
                x = x.parents('form').find(':radio[name=' + x[0].name + ']');
            }

            x.each(function ()
            {
                if (this.checked)
                {
                    value = this.value;
                }
            });
        }
        else
        {
            var markText = x.data('watermark');
            value = x.val();

            if (value === markText) value = '';
        }

        return value;
    }
    else
    {
        var compareValue = function (value1, value2)
        {
            if (value1 === undefined || value1 === null)
            {
                value1 = '';
            }
            else
            {
                value1 = value1.toString().toLowerCase();
            }
            if (value2 === undefined || value2 === null)
            {
                value2 = '';
            }
            else
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

            x.each(function ()
            {
                if (compareValue(this.value, value))
                {
                    $(this).attr('checked', 'on');
                }
                else
                {
                    $(this).removeAttr('checked');
                }
            });
        }
        else if (x.is(':checkbox'))
        {
            if (this[0].value && typeof value !== 'boolean')
            {
                value = this[0].value === value;
            }

            if (value)
                x.attr('checked', 'on');
            else
                x.removeAttr('checked');
        }
        else if (x.is('select'))
        {
            var selectedValue;

            $('option', x).each(function ()
            {
                var cValue = $(this).val();

                if (compareValue(cValue, value)) selectedValue = cValue;
            });

            x.val(selectedValue === undefined ? $('option:eq(0)', x).val() : selectedValue);
        }
        else
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

$.fn.disable = function (isDisabled)
{
    if (isDisabled === undefined)
    {
        isDisabled = true;
    }

    var controls = $(this);
    controls = !controls.is(':input, .button') ? controls.find(':input, .button') : controls;

    if (isDisabled)
    {
        controls.attr('disabled', true);
    }
    else
    {
        controls.removeAttr('disabled');
    }

    return this;
};

//#endregion

//#region jQuery - Ajax Plug-in

window.AutoProcessResult = function (result, ignoreUnhandleResult, alertFn)
{
    alertFn = alertFn || window.alert;

    if (result === null && !ignoreUnhandleResult)
    {
        throw Error('Result must not be null');
    }

    if (result.RedirectTo)
    {
        GotoUrl(result.RedirectTo);
        return true;
    }
    else if (result.RedirectTo === '')
    {
        location.reload();
        return true;
    }

    if (typeof result.ErrorList !== 'undefined')
    {
        var temp = '';
        for (var x in result.ErrorList)
        {
            temp += result.ErrorList[x] + '\n\n';
        }

        if (temp)
        {
            alertFn(temp);
            return true;
        }
    }

    if (!ignoreUnhandleResult)
    {
        throw Error('Cannot handler given result');
    }
    else
    {
        return false;
    }
};

$.fn.serializeModel = function ()
{
    var model = {};
    var x = $(this);
    var controls;

    if (x.is(':input'))
    {
        controls = x.filter(':input');
    }
    else
    {
        var form = this;
        controls = $(':input', form);
    }

    controls
        .not(':visible :disabled button')
        .each(function ()
        {
            if (!this.name) return;

            model[this.name] = $(this).value();
        });

    return model;
};

$.fn.sendData = function (data, callback)
{
    var x = $(this);
    var form = x.is('form') ? x : x.parents('form');
    if (form.size() === 0) return;

    // Extract only value from current model
    var model = {};
    for (var i in data)
    {
        model[i] = data[i].value;
    }

    var processor = x.data('higgs.modelProcessor');
    if (processor && $.isFunction(processor))
    {
        model = processor.call(this, model);
    }

    var callback_wrapper = function (result)
    {
        form.mappingResult(result, data);
        form.data('isSubmitting', false);

        callback(result);
    };

    form.data('isSubmitting', true);
    $.req(form.attr('action'), $.postify(model), callback_wrapper);
};

$.fn.ajaxForm = function (model, successFn, failFn)
{
    var x = $(this);

    x.data('higgs.ajaxForm', true);

    x.submit(function ()
    {
        x.sendData(model, function (response)
        {
            if (response.IsComplete)
            {
                if (successFn) successFn(response, model);
                else AutoProcessResult(response, true);

                x.ajaxFormSuccess(response, model);
            }
            else
            {
                if (failFn) failFn(response, model);

                x.ajaxFormFail(response, model);
            }

            x.ajaxFormDone();
        });

        x.trigger('ajaxFormSending');

        return false;
    });

    x.find('button[type=submit],.button.submit').each(function ()
    {
        $(this).click(x.submit);
    });

    return x;
};

$.each('ajaxFormSending ajaxFormSuccess ajaxFormFail ajaxFormDone'.split(' '), function (i, o)
{
    $.fn[o] = function (fn)
    {
        var form = $(this);

        if (form.size() === 0) return form;

        if ($.isFunction(fn))
        {
            form.bind(o, null, fn);
            return form;
        }
        else
        {
            form = form.is('form') ? form : form.parents('form');
            form.trigger(o, arguments);
        }
    };
});

$.postify = function (value)
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

// TODO: Move to same place.
// Default behaviour
$(document)
    .afterSubmit(function (e)
    {
        $(e.target).readOnly();
    })
    .ajaxFormDone(function (e)
    {
        $(e.target).readOnly(false);
    });

//#endregion

//#region jQuery - Simple Control

$.fn.waterMark = function (markText, displayControl)
{
    var x = this;

    if (x.length > 1)
    {
        return x.each(function ()
        {
            $(this).waterMark(markText, displayControl);
        });
    }

    if (!markText) markText = x.data('watermark');
    if (!markText && !displayControl) return $('#' + x.attr('id') + '_waterMark');

    var y = (displayControl) ? $(displayControl) : undefined;
    var form = x.parents('form');
    var onBlur = function ()
    {
        if (x.is(':focus')) return;

        var value = x.value();

        if (!value || value === markText)
        {
            if (y)
            {
                y
                 .addClass('watermark')
                 .val(markText)
                 .removeClass('hidden');
                x.addClass('hidden');
            }
            else
            {
                x
                 .addClass('watermark')
                 .val(markText);
            }
        }
        else
        {
            x.removeClass('hidden watermark');

            if (y)
            {
                y.addClass('hidden');
            }
        }
    };

    x.on('blur', onBlur);
    x.on('change', onBlur);
    x.on('focus', function()
    {
        if (x.val() === markText)
        {
            if (y)
            {
                y.removeClass('watermark')
                    .val('');
            } else
            {
                x.removeClass('watermark')
                    .val('');
            }
        }
    });

    if (x.is(':password'))
    {
        x.bind('validationSuccess', null, function (event, input, errorMsg)
        {
            y.removeClass('error');
        })
        .bind('validationFail', null, function (event, input, errorMsg)
        {
            y.addClass('error');
        });
    }

    if (y)
    {
        y.focus(function ()
        {
            y.addClass('hidden');
            x.removeClass('hidden');
            x.focus();
        });

        if (x.attr('tabindex'))
        {
            y.attr('tabindex', x.attr('tabindex'));
        }
    }
    onBlur();
    form
        .on('prepareSubmit', function ()
        {
            if (x.val() === markText) x.val('');
        })
        .on('validationFail', function ()
        {
            x.blur();
        });

    return x;
};

$.fn.showText = function (text, cssClass)
{
    var x = $(this);
    var span1, span2;
    var reset = function (span)
    {
        span.css
        ({
            position: 'absolute',
            top: 0,
            left: 0,
            opacity: 0
        });
    };
    var display = function (showSpan, hideSpan)
    {
        showSpan.attr('class', '');

        if (cssClass) showSpan.addClass(cssClass);

        hideSpan
            .stop()
            .removeClass('isShowed');
        showSpan
            .stop()
            .addClass('isShowed')
            .css('opacity', 0)
            .html(text);

        hideSpan.animate({ opacity: 0 }, $speed);
        showSpan.fadeTo({ duration: $speed }, 1);

        var cHeight = x.outerHeight();
        x.height(cHeight);

        reset(showSpan);
        var newHeight = showSpan.outerHeight();
        if (newHeight !== cHeight)
        {
            x.css({ overflow: 'hidden' })
                .stop()
                .animate({ height: newHeight }, { duration: $speed });
        }
    };

    x.css('position', 'relative');
    span1 = x.children('span:eq(0)');
    span2 = x.children('span:eq(1)');

    if (span1.length === 0)
    {
        span1 = $('<span></span>');
        reset(span1);
        span1.appendTo(x);
    }
    if (span2.length === 0)
    {
        span2 = $('<span></span>').addClass('isShowed');
        reset(span2);
        span2.appendTo(x);
    }

    var isShowSpan1 = !span1.is('.isShowed');

    if (text === undefined)
    {
        return (isShowSpan1 ? span1 : span2).text();
    }

    display(isShowSpan1 ? span1 : span2, !isShowSpan1 ? span1 : span2);

    return x;
};

$.fn.allowChars = function (chars)
{
    var allowKeys = {};
    //chars = chars.toUpperCase();

    for (var i = 0; i < chars.length; i++)
    {
        allowKeys[chars.charCodeAt(i)] = true;
    }

    this.keypress(function (e)
    {
        if (!allowKeys[e.keyCode]) return false;
    });

    return this;
};

//#endregion
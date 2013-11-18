/// <reference path="../jquery-1.7.1-vsdoc.js" />
/// <reference path="higgs.core.js" />

(function (w, $, undefined)
{
    // Short-hand for logging and resetting log via new keyword.
    var log = function (title, level, tag, dataObj)
    {
        ///	<summary>
        ///    Store current log message to internal storage.
        ///	</summary>
        ///	<param name="title" type="String">
        ///		Log message.
        ///	</param>
        ///	<param name="level" type="Number" optional="true">
        ///		Level of log message that will be show or not show depend on this value.
        ///	</param>
        ///	<param name="tag" type="String" optional="true">
        ///		tag of log message with comma-separated string.
        ///	</param>
        ///	<param name="dataObj" type="Object" optional="true">
        ///		Data object of this log message that can be used for diagnostic or track.
        ///	</param>

        if (this instanceof log)
        {
            return log.reset.apply(log, arguments);
        }

        return log.write.apply(log, arguments);
    };

    //#region Static field.
    
    log.level =
    {
        Info: 0,
        Warning: 50,
        Error: 100
    };
    log.consolePrinter = function (logModel)
    {
        if (typeof console === 'undefined') return;

        if (logModel.level < log.level.Warning && console.log !== undefined)
        {
            console.log(logModel.title);
        }
        else if (logModel.level < log.level.Error && console.warn !== undefined)
        {
            console.warn(logModel.title);
        }
        else if (console.error !== undefined)
        {
            console.error(logModel.title);
        }
    };
    
    //#endregion

    //#region Default options.
    log.fn = log.prototype;
    log.fn.options =
    {
        enable: true,
        defaultLevel: log.level.Info,
        maxItem: 50
    };
    //#endregion

    //#region Public method
    log.reset = function (options)
    {
        ///	<summary>
        ///    Reset state of log object to default value.
        ///	</summary>
        ///	<param name="options" type="Object" optional="true">
        ///		options value for using in new log.
        ///	</param>

        $.extend(log, log.fn.options, options || {});
        this.logList = [];
        this.bindingList = [new bindingModel(-1, log.consolePrinter)];

        return this;
    };

    log.write = function (title, level, tag, dataObj)
    {
        ///	<summary>
        ///    Store current log message to internal storage.
        ///	</summary>
        ///	<param name="title" type="String">
        ///		Log message.
        ///	</param>
        ///	<param name="level" type="Number" optional="true">
        ///		Level of log message that will be show or not show depend on this value.
        ///	</param>
        ///	<param name="tag" type="String" optional="true">
        ///		tag of log message with comma-separated string.
        ///	</param>
        ///	<param name="dataObj" type="Object" optional="true">
        ///		Data object of this log message that can be used for diagnostic or track.
        ///	</param>

        if (!this.enable) return this;

        var logModel = new logInfo(level !== undefined ? level : log.fn.options.defaultLevel, title, tag, dataObj);

        if (this.logList.length + 1 > this.maxItem) this.logList = this.logList.splice(this.logList.length - this.maxItem + 1);

        this.logList.push(logModel);
        this.display(logModel);
    };

    log.display = function (logModel)
    {
        ///	<summary>
        ///    Display current log data by calling all matched binding function.
        ///	</summary>
        ///	<param name="logModel" type="logInfo">
        ///		Log message.
        ///	</param>

        if (!this.bindingList.length) return this;

        $.each(this.bindingList, function (i, binder)
        {
            if (binder.level === -1 || logModel.level >= binder.level) binder.fn.call(this, logModel);
        });
    };

    log.print = function (level, printFn)
    {
        ///	<summary>
        ///    Print all log to console.
        ///	</summary>
        ///	<param name="level" type="Number" optional="true">
        ///		Maximum level of log to be printed. Default value is 0.
        ///	</param>
        ///	<param name="printFn" type="Function" optional="true">
        ///		Custom print function.
        ///	</param>

        level = level ? level : 0;
        printFn = printFn ? printFn : function (logObj)
        {
            var logPattern = "{hour}:{minute}:{second}.{millisec} - ";
            var printMsg = logPattern.format
            ({
                hour: logObj.time.getHours().toString().padLeft(2),
                minute: logObj.time.getMinutes().toString().padLeft(2),
                second: logObj.time.getSeconds().toString().padLeft(2),
                millisec: logObj.time.getMilliseconds().toString().padLeft(3)
            });

            if (logObj.tag)
            {
                var tagMsg = '';
                
                for (var i = 0; i < logObj.tag.length; i++)
                {
                    tagMsg += '[' + logObj.tag[i] + ']';
                }

                printMsg += tagMsg + ' ';
            }

            log.consolePrinter($.extend({}, logObj, { title: printMsg + logObj.title }));
        };

        for (var j = this.logList.length - 1; j >= 0; j--)
        {
            if (this.logList[j].level >= level)
            {
                printFn.call(this, this.logList[j]);
            }
        }
    };

    log.bind = function (fn, logLevel)
    {
        ///	<summary>
        ///		 Handler event when log manager save log message.
        ///	</summary>
        ///	<param name="fn" type="Function">
        ///		Function to be fired when log manager save any message.
        ///	</param>
        ///	<param name="logLevel" type="int">
        ///		Minimum level of log message require for firing this handler.
        ///	</param>

        this.bindingList.push(new bindingModel(logLevel !== undefined ? logLevel : this.defaultLogLevel, fn));
    };

    log.unbind = function (fn)
    {
        ///	<summary>
        ///		 Unhandler/clear event when log manager save log message.
        ///	</summary>
        ///	<param name="fn" type="Function" optional="true">
        ///		Function to be fired when log manager save any message.
        ///	</param>

        if (fn)
        {
            for(var i = 0;i < this.bindingList.length; i++)
            {
                if(this.bindingList[i].fn !== fn) continue;

                this.bindingList.remove(this.bindingList[i]);
                return true;
            }
            
            return false;
        }
        else
        {
            this.bindingList = [];
            return true;
        }
    };

    log.isExist = function (minLevel, time, title)
    {
        ///	<summary>
        ///		 Check the current log data against condition and return true if at least one of these data matches the condition.
        ///	</summary>
        ///	<param name="minLevel" type="Number">
        ///		Level of log to be matched.
        ///	</param>
        ///	<param name="time" type="Date">
        ///		Only log that occurs later than specified time to be matched.
        ///	</param>
        ///	<param name="title" type="String">
        ///		Keyword to be search.
        ///	</param>

        var level = minLevel ? minLevel : 0;
        var startTime = time ? (typeof time === 'number' ? new Date(time) : time) : new Date(0);
        var keywords = title ? title.split(' ') : new Array();

        for (var i = 0; i < this.logList.length; i++)
        {
            if (this.logList[i].level >= level && this.logList[i].time >= startTime)
            {
                for (var j = 0; j < keywords.length; j++)
                {
                    if (this.logList[i].title.indexOf(keywords[j]) >= 0)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    };
    //#endregion

    //#region Private model.
    function bindingModel(level, fn)
    {
        this.level = level;
        this.fn = fn;
    };

    function logInfo(level, title, tag, dataObj)
    {
        this.level = level;
        this.title = title;
        this.tag = tag.split(',');
        this.time = new Date();
        this.dataObj = dataObj;
    };
    //#endregion
    
    // log all AJAX request.
    $(function ()
    {
        $(document).ajaxSuccess(function (obj, xhr, response)
        {
            var domain = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');

            window.log && log.write
            (
                response.url.indexOf(domain) >= 0 ? response.url : domain + response.url,
                log.Info,
                response.type.toUpperCase() + '/' + (response.dataType ? response.dataType.toUpperCase() : 'TEXT')
            );
        });
    });

    // Create new logger object and map to global scope.
    log.reset();
    w.log = log;
})(window, jQuery);
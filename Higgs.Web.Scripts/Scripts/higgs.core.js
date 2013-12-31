/// <reference path="typings/jquery/jquery.d.ts" />

var Higgs;
(function (Higgs) {
    function isEmptyGuid(msg) {
        return (/^[0]{8}-?[0]{4}-?[0]{4}-?[0]{4}-?[0]{12}$/i).test(msg);
    }
    Higgs.isEmptyGuid = isEmptyGuid;

    /** Retrieve URL based on given path and current application URL.
    @param logicalPath   Path refers to exact path based on application URL.
    For example, ~/images/some-file.jpg
    @param isNoCache   Append timestamp to url to force web browser don't cache it.
    */
    function getUrl(logicalPath, isNoCache) {
        if (typeof isNoCache === "undefined") { isNoCache = false; }
        var url = logicalPath;

        if (!(/^[a-z-]+:\/\//).test(logicalPath) && window.applicationUrl) {
            if (logicalPath.indexOf('~/') < 0) {
                url = (location.origin || location.href.substring(0, location.href.indexOf(location.pathname))) + '/' + (logicalPath.indexOf('/') == 0 ? logicalPath.substring(1) : logicalPath);
            } else {
                url = window.applicationUrl + logicalPath.substring(2);
            }
        }

        var tick = (new Date()).getTime();
        var noCache = '__requestid__=' + tick;

        var index = url.indexOf('?');
        if (index > 0) {
            return url + '&' + noCache;
        } else {
            return url + '?' + noCache;
        }
    }
    Higgs.getUrl = getUrl;

    // TODO: To be removed
    function openUrl(url) {
        location.href = getUrl(url);
    }
    Higgs.openUrl = openUrl;
})(Higgs || (Higgs = {}));

(function ($) {
    // Original code at http://www.filamentgroup.com/lab/jquery_plugin_for_requesting_ajax_like_file_downloads/
    $.sendData = function (url, data, method, isNewWindow) {
        if (isNewWindow === undefined)
            isNewWindow = true;

        if (url.indexOf('~/') === 0)
            url = Higgs.getUrl(url);
        var form = $('<form />');
        form.attr('id', 'form' + (new Date()).getTime());
        form.attr('action', url);
        form.attr('method', method || 'post');

        if (isNewWindow)
            form.attr('target', '_blank');

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

        if (!isNewWindow)
            form.remove();
    };
})(jQuery);

/// <reference path="../jquery-1.4.4.js" />
/// <reference path="higgs.core.js" />

$.higgs.notifications =
{
    // 1: WebKit-based
    // 2: IE
    // 3: Other
    _type: typeof webkitNotifications !== 'undefined' ? 1 : (typeof createPopup !== 'undefined' ? 2 : 3),
    permissionStatus: { Allowed: 0, NotAllowed: 1, Denied: 2 },
    checkPermission: function ()
    {
        if ($.higgs.notifications._type === 1)
        {
            return webkitNotifications.checkPermission();
        }

        return $.higgs.notifications.permissionStatus.Allowed;
    },
    requestPermission: function ()
    {
        var status = $.higgs.notifications.checkPermission();

        // trigger this function by click event only!
        if (status === 1)
        {
            if ($.higgs.notifications._type === 1)
            {
                webkitNotifications.requestPermission();
            }
        }
    },
    show: function (url)
    {
        var permission = $.higgs.notifications.checkPermission();

        if ($.higgs.notifications._type === 1 && permission === $.higgs.notifications.permissionStatus.Allowed)
        {
            webkitNotifications.createHTMLNotification(url.indexOf('?') >= 0 ? url + '&noHeader=1' : url + '?noHeader=1').show();
        }
        else
        {
            $.get(url, null, function (html)
            {
                $.higgs.notifications._createInbrowserNotification(html);
            }, 'text');
        }
    },
    _createPopup: function (html)
    {
        var linkReg = /\<link[ ][\s\S]*?href=["']([\S]*?)["'][\s\S]*?\/>/g;
        var bodyReg = /<body>([\s\S]*?)<\/body>/g;
        var match;
        var bodyObj = $(html);
        var screenWidth = window.screen.width;
        var screenHeight = window.screen.height;
        var toastVerticalMargin = 28;
        var toastHorizontalMargin = 16;
        popupObj = window.createPopup();

        while ((match = linkReg.exec(html)) != null)
        {
            popupObj.document.createStyleSheet(match[1]);
        }

        match = bodyReg.exec(html);
        popupObj.document.body.innerHTML = match[1];

        var popupwidth = 300;
        var popupheight = 120;

        $(popupObj.document).click(function ()
        {
            popupObj.hide();
        });

        popupObj.show(screenWidth - toastHorizontalMargin - popupwidth, screenHeight - toastVerticalMargin - popupheight, popupwidth, popupheight);
    },
    _createModelessDialog: function (url)
    {
        var screenWidth = window.screen.width;
        var screenHeight = window.screen.height;
        var toastVerticalMargin = 28;
        var toastHorizontalMargin = 16;
        var popupwidth = 300;
        var popupheight = 120;

        showModelessDialog(url, 'Notification', "status:0;scroll:0;unadorned:1;dialogWidth:300px;dialogHeight:120px;dialogLeft:{0};dialogTop:{1};".format(screenWidth - toastHorizontalMargin - popupwidth, screenHeight - toastVerticalMargin - popupheight));
    },
    _createInbrowserNotification: function (html)
    {
        var bodyReg = /<body>([\s\S]*?)<\/body>/g;
        match = bodyReg.exec(html);

        $('#higgs-notification').remove();

        var notify = $('<div />')
                            .attr('id', 'higgs-notification')
                            .css
                            ({
                                position: 'fixed',
                                bottom: '0',
                                right: '5px',
                                width: '300px',
                                height: 'auto',
                                opacity: 0.4
                            })
                            .html(match[1])
                            .click(function ()
                            {
                                $(this)
                                    .fadeOut($.fx.speeds._default, 'linear', function ()
                                    {
                                        $(this).remove();
                                    });
                            })
                            .appendTo($('body'));

        notify.animate({ bottom: '27px', opacity: 1 }, $.fx.speeds._default, 'linear', function ()
        {
            if ($.browser.msie && $.browser.version < 9) this.style.removeAttribute('filter');
        });
    }
};
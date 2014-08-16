/// <reference path="../../jquery-1.7.1-vsdoc.js" />
/// <reference path="../higgs.core.js" />
/// <reference path="../../jquery.ui.core.js" />
/// <reference path="../../jquery.ui.widget.js" />
/// <reference path="../../jquery.ui.mouse.js" />
/// <reference path="../../jquery.ui.draggable.js" />

/*!
* Higgs Dialog v1.0, a part of Higgs RIA Framework.
*
* Copyright@2011 by Soul_Master
* License: Microsoft Public License
* https://github.com/Soul-Master/higgs/
*/
(function ($, undef)
{
    // A simple way to check for HTML strings or ID strings
    // (both of which we optimize for)
    var quickExpr = /^(?:[^<]*(<[\w\W]+>)[^>]*$|#([\w\-]+)$)/;
    var dialog = window.dialog = function (content, title, isModal, options)
    {
        /// <summary>
        ///     Create flexible dialog for displaying content in current window. This dialog will be asynchronous shown in 50ms later.
        ///     <para> - $(content, header, isModal, options) </para>
        ///     <para> - $(options) </para>
        /// </summary>
        /// <param name="content" type="String">
        ///     Content to be displayed
        ///     <para>1. Html string</para>
        ///     <para>2. Dom element</para>
        ///     <para>3. Url</para>
        /// </param>
        /// <param name="title" type="String">
        ///     Text to be displayed at the header of dialog
        /// </param>
        /// <param name="isModal" type="Boolean">
        ///     Flag to specific this is modal dialog or not. A modal window is a child window that requires users to interact with it before they can return to operating the parent application
        /// </param>
        /// <param name="options" type="Object">
        ///     Object contains all dialog settings
        /// </param>
        /// <returns type="dialog" />
        
        // Undefined title
        if(typeof title === 'boolean')
        {
            options = isModal;
            isModal = title;
            title = undefined;
        }

        if (typeof $speed === 'undefined') window.$speed = 400;
        if (content instanceof $ || typeof content === 'string')
        {
            options = options || {};
            options.content = content;
            options.title = title;
            options.isModal = isModal;
        }
        else
        {
            options = arguments[0];
        }

        return new dialog.fn.init(options);
    };

    dialog.fn = dialog.prototype =
    {
        options:
        {
            httpMethod: 'GET',
            container: null,
            title: null,
            content: null,
            width: null,
            height: null,
            url: null,
            urlData: null,
            delay: 50,
            displayContentDelay: 50,
            isModal: false,
            showCloseButton: true,
            draggable: true,
            isCloseOnAjaxSuccess: true,
            splashImgurl: getUrl('~/Content/Higgs/higgs.dialog.progressbar.gif'),
            template: '<div class="higgs-dialog">' +
            '<div class="border top l"></div>' +
            '<div class="border top m"></div>' +
            '<div class="border top r"></div>' +
            '<div class="border middle l"><div></div></div>' +
            '<div class="border middle r"><div></div></div>' +
            '<div class="border bottom l"></div>' +
            '<div class="border bottom m"></div>' +
            '<div class="border bottom r"></div>' +
            '<div class="saving-indicator"><div><span></span></div></div>' +
            '<div class="header">' +
            '<h4></h4>' +
            '<div class="button close"><span></span></div>' +
            '</div>' +
            '<div class="splash-screen"><img alt="Loading Indicator" /></div>' +
            '<div class="content"></div>' +
            '</div>'
        },
        obj: null,
        header: null,
        content: null,
        bgDialog: null,
        close: null,
        onDisplayedCallback: null,
        onCloseCallback: null,
        init: function (options)
        {
            ///	<summary>
            ///		Intenal function to create dialog object.
            ///	</summary>
            var x = this;

            x.options = $.extend({}, x.options, options);
            x.obj = $(x.options.template);
            x.header = x.obj.find('.header');
            x.content = x.obj.find('.content');
            x.close = function (data)
            {
                var result = true;
                data = typeof data !== $.Event ? data : undefined;
                
                if (x.onCloseCallback)
                {
                    result = x.onCloseCallback.call(x, data);
                    
                    // To prevent closing dialog by returning false value.
                    if(result === false) return false;
                }
                
                x.obj.fadeOut($speed, function ()
                {
                    $(this).remove();
                });

                return result;
            };

            // Prepare content
            if (x.options.content instanceof $)
            {
                // jQuery object
                x.options.content = x.options.content
                                                .clone()
                                                .css('display', 'block');
                
                x.options.content.removeClass('hidden');
            }
            else if (typeof x.options.content === "string")
            {
                var match = quickExpr.exec(x.options.content);

                if (match && (match[1]))
                {
                    // Html string
                    x.options.content = $(x.options.content);
                }
            }

            if (x.options.delay >= 0)
            {
                setTimeout(function ()
                {
                    x.show();
                }, x._delay);
            }
            else
            {
                x.show();
            }

            return this;
        },
        show: function ()
        {
            ///	<summary>
            ///		Intenal function to create dialog object.
            ///	</summary>
            
            var x = this;

            if (x.options.isModal) x.createDialogBg();

            x.prepareDialog();
            if (x.options.content instanceof $)
            {
                x.obj
                    .find('.splash-screen')
                        .remove().end()
                    .css('opacity', 0.01)
                    .appendTo(x.options.container || $('body'));
                
                if ( x.options.draggable && $.ui && $.ui.draggable ) x.obj.draggable( { handle: '.header' } );

                x.displayContent(x.options.content);
            }
            else
            {
                x.displaySplashScreen(function ()
                {
                    x.loadContent();
                });
            }
        },
        createDialogBg: function ()
        {
            var x = this;

            if (!x.options.isModal)
            {
                x.obj.css('zIndex', '5000');
            }
            else
            {
                var lastDialog = $('.higgs-dialog-bg').last().next('.higgs-dialog');
                var bgZindex = lastDialog.length > 0 ? parseInt(lastDialog.css('zIndex'), 10) + 1 : undefined;
                                
                if (bgZindex)
                {
                    x.obj.css('zIndex', bgZindex);
                }
                
                x.bgDialog = $('<div/>')
                    .addClass('higgs-dialog-bg')
                    .css
                    ({
                        display: 'none',
                        zIndex: bgZindex,
                        position: !x.options.container ? 'fixed' : 'absolute'
                    })
                    .appendTo(x.options.container || $('body'))
                    .fadeTo($speed, 0.25);

                var escHandler = function (e) {
                    if (e.which == 27) x.close();
                };
                
                if (x.options.showCloseButton)
                {
                    $(document).bind('keyup', escHandler);
                }

                var oldCloseDialogFn = x.close;
                x.close = function (data)
                {
                    x.obj.stop(true, false);
                    x.bgDialog.stop(true, false);
                    
                    if(oldCloseDialogFn(data) === false) return;
                    if (x.options.showCloseButton) $(document).unbind('keyup', escHandler);

                    x.bgDialog.fadeOut($speed, function ()
                    {
                        x.bgDialog.remove();
                    });
                };
                    
                // Is custom container
                if (x.options.container) x.options.container.css({ overflow: 'hidden' });
            }
        },
        prepareDialog: function ()
        {
            var x = this;

            var h4 = $('h4', x.header);

            if (x.options.title) h4.html(x.options.title);
            else h4.remove();

            // Is custom container
            if (x.options.container) x.obj.css({ position: 'absolute' });
            
            if (x.options.showCloseButton)
            {
                x.header.find('.button')
                    .mouseenter(function ()
                    {
                        $(this)
                            .removeClass('active')
                            .addClass('hover');
                    })
                    .mouseleave(function ()
                    {
                        $(this).removeClass('active hover');
                    })
                    .mousedown(function ()
                    {
                        $(this)
                            .removeClass('hover')
                            .addClass('active');

                        return false;
                    })
                    .mouseup(function ()
                    {
                        $(this)
                            .removeClass('active')
                            .addClass('hover');
                    });

                x.header.find( '.close.button' ).click( function ()
                {
                    x.close();
                });
            }
            else
            {
                x.header.find('.close.button').remove();
            }
        },
        displaySplashScreen: function (callbackFn)
        {
            var x = this;

            x.obj
                .find('.splash-screen img')
                    .attr('src', x.options.splashImgurl).end()
                .css('opacity', 0.01)
                .appendTo(x.options.container || $('body'))
                .fadeTo($speed * 2, 1, 'linear', callbackFn);
            
            if ( $.ui && $.ui.draggable ) x.obj.draggable( { handle: '.header' } );

            x.alignDialog();
        },
        alignDialog: function ()
        {
            var x = this;

            x.obj.css
            ({
                marginTop: -x.obj.height() / 2,
                marginLeft: -x.obj.width() / 2
            });
        },
        loadContent: function ()
        {
            var x = this;

            $.ajax
            ({
                url: noCacheUrl(getUrl(x.options.content)),
                data: x.options.urlData,
                dataType: 'html',
                method: x.options.httpMethod,
                success: function (html)
                {
                    x.displayContent(html);
                },
                error: function (xhr, status, ex)
                {
                    if (xhr.status == 401 || xhr.status == 403)
                    {
                        alert('Unable to access this page');
                        x.close();
                    }
                    if (xhr.status == 500)
                    {
                        alert(xhr.statusText);
                        x.close();
                    }
                    else
                    {
                        this.success(xhr.responseText);
                    }
                }
            });
        },
        displayContent: function (content)
        {
            var x = this;
            var isAjax = !(x.options.content instanceof $);

            x.content
                .css({ clear: 'both', float: 'left', width: 'auto', height: 'auto', marginTop: '1000px' })
                .html(content)
                .find( 'button.close, .button.close' ).click( function ()
                {
                    x.close();
                });
                
            setTimeout(function() 
            {
                if (isAjax)
                {
                    x.content.children()
                        .css('opacity', 0.01).end();
                }

                var newWidth = x.options.width ? ((x.options.width < 50) ? x.options.width + x.content.width() : x.options.width) : x.content.width();
                var newHeight = x.options.height ? x.options.height : parseInt($('.dialog.ui-draggable').css('paddingTop'), 10) + parseInt($('.dialog.ui-draggable').css('paddingBottom')) + x.header.height() + x.content.height();

                x.obj.find('.splash-screen').remove();

                x.content
                    .css({ display: 'block', clear: 'none', float: 'none', marginTop: '0' });

                if (x.options.height)
                {
                    x.content
                        .css('height', newHeight + 'px')
                        .css('overflow', 'auto');
                }
                if (x.options.width)
                {
                    x.content.css
                    ({
                        width: newWidth,
                        overflow: 'auto'
                    });

                    // Fix IE7 rendering bug by fixing width of dialog header.
                    if ($.browser.msie && $.browser.version < 8) x.header.width(x.obj.outerWidth());
                }
                
                x.obj.css({ width: 'auto', height: 'auto', overflow: 'hidden' });
                x.alignDialog();
                x.obj.addClass('loaded');

                if (isAjax)
                {
                    var children = x.content.children();
                    var childCount = children.length;
                    var loadedChild = 0;

                    children.fadeTo($speed, 1, 'linear', function ()
                    {
                        loadedChild++;

                        if(loadedChild < childCount) return;

                        x.onDisplayedCallback && x.onDisplayedCallback.call(x);
                        x.setupAjaxForm.call(x);
                    });
                }
                else
                {
                    x.obj.fadeTo($speed, 1, 'linear', function ()
                    {
                        x.onDisplayedCallback && x.onDisplayedCallback.call(x);
                        x.setupAjaxForm.call(x);
                    });
                }
            }, x.options.displayContentDelay);
        },
        setupAjaxForm: function()
        {
            var x = this;
            var form = x.content.find('form');
            
            if(form.size() === 0 || !form.data('higgs.ajaxForm')) return;
            
            form
                .afterSubmit(function(e) 
                {
                    x.displayLoading();
                })
                .ajaxFormDone(function() 
                {
                    x.stopLoading(false);
                });
            
            
            if (!this.options.isCloseOnAjaxSuccess) return;
            
            form.ajaxFormSuccess(function(e, response, model) 
            {
                x.close(model);
            });
        },
        displayLoading: function (timeout, callback)
        {
            var x = this;
            var loadingContainer = x.obj.find('.saving-indicator');
            var loadingIndicator = loadingContainer.find('span');
            var duration = 4000;

            if (timeout)
            {
                duration = timeout % duration;
                timeout -= duration;
            }
            
            loadingContainer.css('display', 'block');
            loadingIndicator
                .css({ display: 'block', marginLeft: '0%' })
                .animate({ marginLeft: '100%' }, duration, 'linear', function ()
                {
                    if (timeout === undef || timeout > 0) x.displayLoading.call(x, timeout, callback);

                    callback && callback.call(x);
                } );

            return x;
        },
        stopLoading: function ()
        {
            var x = this;
            var loadingContainer = x.obj.find('.saving-indicator');
            var loadingIndicator = loadingContainer.find('span');

            loadingIndicator.stop();
            loadingContainer.css( { display: 'none' } );

            return x;
        },
        /* Extension method */
        width: function (size)
        {
            this.options.width = size;
            return this;
        },
        height: function (size)
        {
            this.options.height = size;
            return this;
        },
        onClose: function (callbackFn)
        {
            this.onCloseCallback = callbackFn;
            return this;
        },
        onDisplayed: function (callbackFn)
        {
            this.onDisplayedCallback = callbackFn;
            return this;
        }
    };

    // TODO: remove it.
    window.openDialog = function(url, title)
    {
        return dialog(url, title, true).width(400);
    };
    
    dialog.fn.init.prototype = dialog.fn;
})(jQuery);

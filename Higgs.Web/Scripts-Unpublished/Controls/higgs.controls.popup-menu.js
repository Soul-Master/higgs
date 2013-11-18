/// <reference path="../../jquery-1.7.1-vsdoc.js" />
/// <reference path="../higgs.core.js" />
/// <reference path="../higgs.animation.js" />

(function ()
{
    var window = this,
         popupMenu = window.popupMenu = function (id, minWidth)
         {
             ///	<summary>
             ///		These class represent pop-up menus that can be used to display dropdown menu for specify control or context.
             ///	</summary>
             ///	<param name="id" type="String">
             ///		1: DOMElementID - Use to identify and create pop-up menu element in current document.
             ///	</param>
             ///	<param name="minWidth" type="Number" optional="true">
             ///		[Optional] Minimum width of current pop-up menu must greater than or equal 100 in pixels.
             ///	</param>
             ///	<returns type="popupMenu" />

             return new popupMenu.fn.init(id, minWidth);
         };

    popupMenu.fn = popupMenu.prototype =
    {
        id: '',
        minWidth: 0,
        _obj: null,
        isOpen: false,
        openedBy: null,
        init: function (id, minWidth)
        {
            ///	<summary>
            ///		Initialize pop-up menu object and clear all data in current object.
            ///	</summary>
            ///	<param name="id" type="String">
            ///		1: DOMElementID - Use to identify and create pop-up menu element in current document.
            ///	</param>
            ///	<param name="minWidth" type="Number" optional="true">
            ///		[Optional] Minimum width of current pop-up menu must greater than or equal 100 in pixels.
            ///	</param>
            ///	<returns type="popupMenu" />

            this.id = id;
            this.minWidth = minWidth || 0;
            this.items = new Array();

            return this;
        },
        items: new Array(),
        template: "<div id='{0}' style='display:none;' class='popup-menu shadow'>" +
                            "<div class='content'>" +
                                "<div class='bg hover'><div class='l'></div><div class='c'></div><div class='r'></div></div>" +
                            "</div>" +
                            "<div class='bg background'>" +
                            "  <div class='l'></div>" +
                            "</div>" +
                        "</div>",
        addMenu: function (menuItem)
        {
            ///	<summary>
            ///		Add popup menu item to current popup menu.
            ///	</summary>
            ///	<param name="menuItem" type="popupMenuItem">
            ///		Menu item to be added to current popup menu.
            ///	</param>

            menuItem.parentMenu = this;
            this.items.push(menuItem);

            if (this._obj != null) this._obj.remove();

            return this;
        },
        create: function ()
        {
            ///	<summary>
            ///		Immediately create pop-up menu element inside current document.
            ///	</summary>
            ///	<returns type="popupMenu" />

            var html = this.template.format(this.id);
            this._obj = $(html);

            if (this.minWidth)
                this._obj.css('minWidth', this.minWidth);

            var menuContent = this._obj.find('.content');

            for (var i in this.items)
            {
                if (typeof this.items[i] === "object")
                    this.items[i].render().appendTo(menuContent);
            }
            this._obj.appendTo('body');

            this.bindEvent();

            return this;
        },
        bindEvent: function ()
        {
            var x = $(this._obj);
            var bgHover = x.find('.content .hover');

            x.find('.content .item')
                .mouseenter(function ()
                {
                    clearTimeout(this.mouseLeaveTimeout);
                    this.mouseLeaveTimeout = null;

                    var itemPosition = $(this).position();

                    if (!$(this).parent().is('.hover'))
                    {
                        $(this).parent().addClass('hover');

                        bgHover
                            .stop(true, false)
                            .fadeTo(0, 0, function ()
                            {
                                bgHover
                                    .css('top', itemPosition.top + 1)
                                    .fadeTo($.fx.speeds._default, 1);
                            });
                    }
                    else
                    {
                        bgHover.css({ top: itemPosition.top + 1 });
                    }
                });

            $(this._obj).mouseleave(function ()
            {
                bgHover
                    .stop(true, false)
                    .fadeOut($.fx.speeds._default, function ()
                    {
                        x.find('.content').removeClass('hover');
                    });
            });
        },
        show: function (top, left, animationSpeed)
        {
            ///	<summary>
            ///		Show current menu by fading in object if it exists in current document.
            ///     Otherwise, it will append object into current document before displaying it.
            ///	</summary>
            ///	<param name="top" type="Number" optional="true">
            ///		[Optional] Starting position of menu in Y-ordinate from top side of the screen, this Default value of this parameter is 0 (pixel).
            ///	</param>
            ///	<param name="left" type="Number" optional="true">
            ///		[Optional] Starting position of menu in X-ordinate from left side of the screen, this Default value of this parameter is 0 (pixel).
            ///	</param>
            ///	<param name="animationSpeed" type="Number" optional="true">
            ///		[Optional] A positive number of millisecond represents how fast animation should be.
            ///	</param>
            ///	<returns type="popupMenu" />
            if (!this._obj) this.create();

            $(this._obj)
                .css(
                {
                    top: top || 0,
                    left: left || 0
                })
                .stop()
                .fadeIn($.fx.speeds._default)
                .find('.content .hover')
                    .css('display', 'none');
                    
            var x = this;
            setTimeout(function ()
            {
                $(document).bind('click.' + x.id, $.fx.speeds._default * 10, function (e)
                {
                    x.close();
                });

                $(window).bind('resize.' + x.id, $.fx.speeds._default * 10, function (e)
                {
                    x.close();
                });
            });

            this.isOpen = true;
            return this;
        },
        closeFn: new Array(),
        close: function (fn)
        {
            ///	<summary>
            ///		Close this pop-up menu or define function that will be fired when this pop-up menu is closed.
            ///	</summary>
            ///	<param name="fn" type="Function">
            ///		[Optional] Function will be fired when menu is closed.
            ///	</param>
            ///	<returns type="popupMenu" />

            if (typeof fn === 'function')
            {
                this.closeFn.push(fn);
            }
            else
            {
                var temp;
                $(this._obj)
                    .fadeOut($.fx.speeds._default);

                $(document).unbind('click.' + this.id);
                $(window).unbind('resize.' + this.id);

                while (temp = this.closeFn.pop())
                {
                    temp();
                }
            }
            
            this.isOpen = false;
            return this;
        }
    };

    popupMenu.fn.init.prototype = popupMenu.fn;
})();

(function ()
{
    var window = this,
         popupMenuItem = window.popupMenuItem = function (id, label, iconUrl, option, isSelected)
         {
             ///	<summary>
             ///		These class represent pop-up menus item will be be displayed as item in pop-up Menu.
             ///	</summary>
             ///	<param name="id" type="String" optional="true">
             ///		[Optional] 1: DOMElementID - Use to identify and create pop-up menu item element in pop-up menu.
             ///	</param>
             ///	<param name="label" type="String">
             ///		If you do not specify label, this will be generated as menu separation.
             ///	</param>
             ///	<param name="iconUrl" type="String" optional="true">
             ///		[Optional] Absolute or relative path to menu icon. This maximum size of icon is 20x20.
             ///       For example, http://www.some-site.com/images/menu-icon.png
             ///	</param>
             ///	<param name="option" type="String">
             ///		[Optional]
             ///       1: Group Name [String] - Name of menu item that can be selected only one item in its group.
             ///       2: Selection Status [Boolean] - Status of selection of current menu item, this value can be both true and false.
             ///	</param>
             ///	<param name="isSelected" type="Boolean">
             ///		[Optional] Status of menu item in current group, this value can be both true and false.
             ///	</param>
             ///	<returns type="popupMenuItem" />
             
             return new popupMenuItem.fn.init(id, label, iconUrl, option, isSelected);
         };

    popupMenuItem.fn = popupMenuItem.prototype =
    {
        id: '',
        parentMenu: null,
        label: '',
        data: null,
        isCheckBox: false,
        groupName: '',
        iconUrl: '',    
        isSelected: false,
        customHTML: '',
        clickEvents: new Array(),
        init: function (id, label, iconUrl, option, isSelected)
        {
             ///	<summary>
             ///		Initialize pop-up menu object and clear all data in current object.
             ///	</summary>
             ///	<param name="id" type="String" optional="true">
             ///		[Optional] 1: DOMElementID - Use to identify and create pop-up menu item element in pop-up menu.
             ///	</param>
             ///	<param name="label" type="String">
             ///		[Optional] Label of current menu item that can be both text and HTML text.
             ///       If you do not specify label, this will be generated as menu separation.
             ///	</param>
             ///	<param name="iconUrl" type="String" optional="true">
             ///		[Optional] Absolute or relative path to menu icon. This maximum size of icon is 20x20.
             ///       For example, http://www.some-site.com/images/menu-icon.png
             ///	</param>
             ///	<param name="option" type="String">
             ///		[Optional]
             ///       1: Group Name [String] - Name of menu item that can be selected only one item in its group.
             ///       2: Selection Status [Boolean] - Status of selection of current menu item, this value can be both true and false.
             ///	</param>
             ///	<param name="isSelected" type="Boolean">
             ///		[Optional] Status of menu item in current group, this value can be both true and false.
             ///	</param>
             ///	<returns type="popupMenuItem" />
             
            this.id = (!label && option && isSelected) ? '' : id;
            this.label = label || this.label;
            this.iconUrl = iconUrl || this.iconUrl;    
            this.isSelected = isSelected || this.isSelected;
            this.customHTML = (!label && option && isSelected) ? id : this.customHTML;
            this.clickEvents = new Array();

            if (typeof option === "boolean")
            {
                this.isCheckBox = true;
                this.isSelected = option;
            }
            else if(typeof option === "string")
            {
                this.groupName = option;
            }
            
            return this;
        },
        click: function(fn)
        {
             ///	<summary>
             ///		Bind click event of current menu item.
             ///	</summary>
             ///	<param name="fn" type="Function">
             ///		Function that will be fired when menu item is clicked.
             ///	</param>
             ///	<returns type="popupMenuItem" />
             this.clickEvents.push(fn);
             return this;
        },
        render: function ()
        {
            ///	<summary>
            ///		Render HTML element for this current menu item. 
            ///	</summary>
            ///	<returns type="jQuery" />
            
            if (this.customHTML)
            {
                return $(this.customHTML);
            }
            else
            {
                var item = $('<div/>');
                
                if(this.label)
                {
                    item.addClass('item');

                    if (this.groupName && this.isSelected)
                    {
                        // render group
                    }
                    else
                    {
                        var iconElement = $('<div/>').addClass('icon');

                        if (this.isCheckBox || this.groupName)
                        {
                            if (!this.iconUrl)
                            {
                                $('<div/>').appendTo(iconElement);
                            }

                            iconElement.addClass('selected')
                                .css('display', this.isSelected ? 'block' : 'none');
                        }
                        if (this.iconUrl)
                        {
                            $('<img src="{0}" alt="{1}" />'.format(getUrl(this.iconUrl), this.label + " menu icon"))
                                .appendTo(iconElement);
                        }

                        iconElement.appendTo(item);

                        if (this.label)
                        {
                            $('<span>' + this.label + '</span>').appendTo(item);
                        }
                    }
                }
                else
                {
                    item.addClass('sep').append($('<span />'));
                }

                var x = this;

                item.click(function()
                {
                    if(x.isCheckBox)
                    {
                        x.isSelected = !x.isSelected;
                        $(this).find(".icon").css('display', x.isSelected ? 'block' : 'none');
                    }
                        
                    for(var i in x.clickEvents)
                    {
                        x.clickEvents[i](x.parentMenu.openedBy, x.label, x.data);
                    }
                });
            }
            
            return item;
        }
    };
    
    popupMenuItem.fn.init.prototype = popupMenuItem.fn;
})();

jQuery.fn.bindMenu = function (menuObj, menuButtonId)
{
    ///	<summary>
    ///		Bind menu with current jQuery object or specified button.
    ///	</summary>
    ///	<returns type="Element" />
    ///	<param name="menuObj" type="popupMenu">
    ///		Menu object to be displayed.
    ///	</param>
    ///	<param name="menuButtonId" type="String" optional="true">
    ///		Specified button id to be binded.
    ///	</param>
    
    var bindedControl = $(this);
    var menuButton = menuButtonId ? $(menuButtonId) : bindedControl;

    if (!bindedControl.is(':input'))
    {
        menuButton = bindedControl;
        bindedControl = $('#undefinedControlId');
    }

    bindedControl
        .attr('autocomplete', 'off')
        .mouseenter(function ()
        {
            menuButton.addClass(cssClass.hover);
        })
        .focus(function ()
        {
            bindedControl.addClass(cssClass.focus + ' ' + cssClass.must_focus);

            menuButton
                .removeClass(cssClass.hover)
                .addClass(cssClass.focus);
        })
        .mouseleave(function ()
        {
            if (!bindedControl.hasClass(cssClass.focus))
                menuButton.removeClass(cssClass.hover + ' ' + cssClass.focus);
        })
        .blur(function ()
        {
            menuButton.removeClass(cssClass.focus + ' ' + cssClass.hover);
            bindedControl.removeClass(cssClass.focus + ' ' + cssClass.hover + ' ' + cssClass.must_focus);

            if (menuButton.hasClass(cssClass.mouseDown))
            {
                bindedControl.addClass(cssClass.focus);
            }
        });

    menuButton
        .mouseenter(function ()
        {
            bindedControl.addClass(cssClass.hover);
            menuButton.addClass(cssClass.hover);
        })
        .mousedown(function ()
        {
            bindedControl
                .removeClass(cssClass.hover)
                .addClass(cssClass.focus);
            menuButton
                .removeClass(cssClass.hover)
                .addClass(cssClass.mouseDown);
        })
        .mouseleave(function ()
        {
            if (!bindedControl.hasClass(cssClass.focus))
            {
                bindedControl
                    .removeClass(cssClass.hover)
                    .removeClass(cssClass.focus);
                menuButton
                    .removeClass(cssClass.hover)
                    .removeClass(cssClass.focus);
            }
        })
        .click(function ()
        {
            if(menuObj.isOpen)
            {
                menuObj.close();
                return;
            }
            
            var offset = bindedControl.size() > 0 ? bindedControl.documentOffset() : menuButton.documentOffset();

            $(menuObj._obj).css(
            {
                postion: 'absolute',
                top: '50%',
                left: '50%'
            });

            menuObj.openedBy = bindedControl.size() > 0 ? bindedControl : menuButton;

            menuObj
                .close(function ()
                {
                    if (!bindedControl.hasClass(cssClass.must_focus))
                    {
                        bindedControl.removeClass(cssClass.focus + ' ' + cssClass.hover);
                        menuButton.removeClass(cssClass.focus + ' ' + cssClass.hover + ' ' + cssClass.mouseDown);
                    }
                    else
                    {
                        menuButton.removeClass(cssClass.mouseDown);
                        bindedControl.removeClass(cssClass.must_focus);
                    }

                    menuObj.openedBy = null;
                });

            if (!menuObj._obj) menuObj.create();

            var control = bindedControl;
            if (!control || control.size() === 0)
            {
                control = $(this);
            }

            var displayPosLeft = offset.left;
            var displayPosTop = offset.top + control.outerHeight() + 1;

            if (displayPosLeft + menuObj._obj.width() >= document.width)
            {
                displayPosLeft = displayPosLeft + (control.outerWidth() - menuObj._obj.width());
            }

            menuObj.show(displayPosTop, displayPosLeft);
        });

    return $(this);
};
     
function setValueByMenuLabel(bindedControl, label, data)
{
    bindedControl.val(label);
}

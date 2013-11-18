/// <reference path="../VsDoc/jquery-1.4.4-vsdoc.js"/>
/// <reference path="core.js" />

function PlayHoverEffect(target, bgHoverObj, isEnableAnimation, cssClass)
{
    cssClass = cssClass ? cssClass : 'hover';

    target.addClass(cssClass);
    
    if (isEnableAnimation)
    {
        bgHoverObj.stop();

        if (bgHoverObj.css('opacity', 1))   bgHoverObj.css('opacity', 0);

        bgHoverObj.animate({ opacity: 1 }, $.fx.speeds._default);
    }
}

function PlayBlurEffect(target, bgBlurObj, isEnableAnimation, cssBlurClass)
{
    cssBlurClass = cssBlurClass ? cssBlurClass : 'hover';

    if (isEnableAnimation)
    {
        bgBlurObj
            .stop()
            .animate({ opacity: 0 }, $.fx.speeds._default, 'linear', function ()
            {
                    target.removeClass(cssBlurClass);
            });
    }
    else
    {
        target.removeClass(cssBlurClass);
    }
}

function PlaySwapEffect(target, bgOldObj, bgNewObj, cssOldClass, cssNewClass, isEnableAnimation)
{
    target.addClass(cssNewClass);

    if (isEnableAnimation)
    {
        bgNewObj
            .stop()
            .css('opacity', 0);

        bgNewObj.animate({ opacity: 1 }, $.fx.speeds._default);

        // Use animate method instead of fadeOut for avoiding jQuery isHidden bug.
        bgOldObj
            .stop()
            .animate({ opacity: 0 }, $.fx.speeds._default, 'linear', function () { target.removeClass(cssOldClass); });
    }
    else
    {
        target.removeClass(cssOldClass);
    }
}
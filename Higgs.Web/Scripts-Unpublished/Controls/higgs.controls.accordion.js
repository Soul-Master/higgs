/// <reference path="../../jquery-1.7.1-vsdoc.js" />
/// <reference path="../higgs.core.js" />
/// <reference path="../higgs.animation.js" />

(function ()
{
    var window = this,
         accordion = window.accordion = function (containerId)
         {
             return new accordion.fn.init(containerId);
         };

    accordion.fn = accordion.prototype =
    {
        container: $(document),
        init: function (containerId)
        {
            var x = this;

            this.container = $('#' + containerId);
            this.container.find('.accordion-header a').click(function ()
            {
                x.select($(this).attr('href').substring(1));

                return false;
            });

            return this;
        },
        select: function (contentId)
        {
            this.container
                .find('.accordion-header')
                    .removeClass('active').end()
                .find('.accordion-header a[href=#divAppointmentSidebar]')
                    .addClass('active').end()
                .find('.accordion-content.active')
                    .fadeOut($.fx.speeds._default * 2, function ()
                    {
                        $(this).removeClass('active');
                    }).end()
                .find('#' + contentId)
                    .fadeIn($.fx.speeds._default * 2);
        }
    };

    accordion.fn.init.prototype = accordion.fn;
})();

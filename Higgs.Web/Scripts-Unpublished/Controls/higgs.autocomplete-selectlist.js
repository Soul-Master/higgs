/// <reference path="../../jquery-1.7.1-vsdoc.js" />
/// <reference path="../higgs.core.js" />

(function (higgs, $, undef)
{
    $.fn.autocompleteSelectList = function (url, hiddenControl)
    {
        var x = $(this);
        var output = hiddenControl ? $(hiddenControl) : x;
        var validItem = [];
        var initLabel = x.value();
        var initValue = output.value();
        var isValid = function ()
        {
            var value = output.value(),
            label = x.value();

            for (var i = 0; i < validItem.length; i++)
            {
                if (validItem[i].value === value && validItem[i].label === label) return true;
            }

            return false;
        };

        x.autocomplete
        ({
            source: typeof url !== 'function' ? getUrl(url) : url,
            minLength: 3,
            previewSelectionChange: true,
            getPreviewText: function (item)
            {
                return item.label.split(',')[0];
            },
            getSelectedText: function (item)
            {
                return item.label.split(',')[0];
            },
            select: function (e, ui)
            {
                x.value(ui.item.label);
                output.value(ui.item.value).change();

                validItem.push({ value: ui.item.value, label: ui.item.label.split(',')[0] });

                setTimeout(x.blur, 100);
            }
        })
        .focus(function ()
        {
            if (x.is('[readonly=true]')) return;

            x.removeClass('validItem', 'invalidItem');

            setTimeout(function ()
            {
                x.selectText(0);
            }, 100);
        })
        .blur(function ()
        {
            if (isValid())
            {
                x.removeClass('invalidItem');
                x.addClass('validItem');
            }
            else
            {
                output.value('').change();
                x.removeClass('validItem');

                if (x.value())
                {
                    x.addClass('invalidItem');
                }
            }
        })
        .bind('autocompleteSelectList-addItem', null, function (e, value, label)
        {
            validItem.push({ value: value, label: label });
        });

        if (initLabel !== undef && initValue !== undef && initValue !== emptyGuid)
        {
            validItem.push({ label: initLabel, value: initValue });

            x.data('autocompleteSelectlist.selectitem', {}).blur();
        }

        return x;
    };

} (window.higgs = window.higgs || {}, jQuery));

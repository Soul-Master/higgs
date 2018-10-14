 (function (locales)
{
    var f = function (message: string)
    {
        return function () { return message.format(this); };
    };

    locales.push
    ({
        name: 'TH',
        rules:
        {
            requiredValidation: f('กรุณาระบุข้อมูล'),
            stringLengthValidation: f('ข้อมูลต้องมีความยาวระหว่าง {minLength}-{maxLength} ตัวอักษร'),
            stringLengthValidation_NullMinLength: f('ข้อมูลต้องมีความยาวไม่เกิน {maxLength} ตัวอักษร'),
            stringLengthValidation_NullMaxLength: f('ข้อมูลต้องมีความยาวอย่างน้อย {minLength} ตัวอักษร'),
            patternValidation: f('รูปแบบข้อมูลไม่ถูกต้อง'),
            numberValidation_Integer: f('ข้อมูลต้องเป็นตัวเลขจำนวนเต็มเท่านั้น'),
            numberValidation_Decimal: f('ข้อมูลต้องเป็นตัวเลขเท่านั้น'),
            urlValidation: f('ข้อมูลต้องเป็น Url ที่ถูกต้อง'),
            emailValidation: f('ข้อมูลต้องเป็น e-mail ที่ถูกต้อง'),
            moreThanValidation: f('ข้อมูลต้องมีค่ามากกว่า {value}'),
            minValidation: f('ข้อมูลต้องมีมากกว่าเท่ากับ {value}'),
            lessThanValidation: f('ข้อมูลต้องมีค่าน้อยกว่า {value}'),
            maxValidation: f('ข้อมูลต้องมีค่าน้อยกว่าหรือเท่ากับ {value}'),
            equalValidation: f('ข้อมูลต้องมีค่าเท่ากับ {label:{equalProperty}}'),
            notEqualValidation: f('ข้อมูลต้องมีค่าไม่เท่ากับ {label:{notEqualProperty}}')
        }
    });
})(Higgs.locales);

if(!window['locale'] || (<string>window['locale']).toUpperCase() === 'th') Higgs.locale = Higgs.locales[Higgs.locales.length - 1];
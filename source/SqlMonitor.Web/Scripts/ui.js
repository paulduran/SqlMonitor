function applyUIStyles(parent) {
    parent.find('input').placeholder();
    parent.find('button').button(); // JQuery Date pickers
    parent.find(".datepicker").datepicker({
        showOn: 'button',
        buttonImage: basePath + 'Content/images/calendar.gif',
        buttonImageOnly: true,
        dateFormat: 'd/mm/yy'
    });
}

applyUIStyles($(document));
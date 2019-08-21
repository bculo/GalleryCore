var my = {}

my.viewname = {
    init: function (settings) {
        my.uploadUrl = settings.uploadUrl;
        my.nameInputId = settings.nameInputId;
        my.categoryImageInputId = settings.categoryImageInputId;
        my.redirectUrl = settings.redirectUrl;

        //Selectors part
        my.categoryImageDivSelector = "#".concat(settings.categoryImageDivId);
        my.formSelector = "#".concat(settings.formId);
        my.categoryImageSelector = "#".concat(settings.categoryImageInputId);
        my.nameInputSelector = "#".concat(settings.nameInputId);
    }
}
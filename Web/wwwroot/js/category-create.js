var my = {};

my.viewname = {
    init: function (settings) {
        my.uploadUrl = settings.uploadUrl;
        my.nameInputId = settings.nameInputId;
        my.categoryImageInputId = settings.categoryImageInputId;

        //Selectors part
        my.formSelector = "#".concat(settings.formId);
        my.categoryImageSelector = "#".concat(settings.categoryImageInputId);
        my.nameInputSelector = "#".concat(settings.nameInputId);
    }
}

var upload; 

$(document).ready(function () {
    upload = new FileUpload(uploadResult, my.uploadUrl);
    let token = $('input[name="__RequestVerificationToken"]').val();
    upload.addItem("__RequestVerificationToken", token);

    $("#submitForm").click(submitFile);
});

function submitFile(e) {
    e.preventDefault();

    var validator = $(my.formSelector).validate();
    if (!validator.form()) {
        return;
    }

    let file = $(my.categoryImageSelector).prop('files')[0];
    let name = $(my.nameInputSelector).val();

    upload.addItem(my.nameInputId, name);
    upload.addItem(my.categoryImageInputId, file);

    upload.uploadForm();
}

function uploadResult(result) {
    if (!result.status) {
        upload.removeItem(my.nameInputId);
        upload.removeItem(my.categoryImageInputId);

        //TODO
        //Display error
    }
    else {
        //TODO
        //Show success message and redirect
    }
}
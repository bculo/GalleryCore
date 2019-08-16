var my = {};

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

var upload;
var dragAndDrop;

$(document).ready(function () {
    upload = new FileUpload(uploadResult, my.uploadUrl);
    prepareForDragAndDrop();

    let token = $('input[name="__RequestVerificationToken"]').val();
    upload.addItem("__RequestVerificationToken", token);

    $("#submitForm").click(submitFile);
});

function prepareForDragAndDrop() {
    dragAndDrop = new DragAndDrop(my.categoryImageDivSelector, ['jpg', 'jpeg', 'png']);
    $(my.categoryImageDivSelector).remove();
}

function submitFile(e) {
    e.preventDefault();

    var validator = $(my.formSelector).validate();
    if (!validator.form()) {
        console.log("validation failed");
        return;
    }

    //let file = $(my.categoryImageSelector).prop('files')[0];
    let file = dragAndDrop.getFile().file;
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
        let form = $(my.formSelector);
        $("<p> Cattegory succesfuly added </p>").insertAfter(form);
        form.remove();

        setTimeout(function () {
            window.location.replace(my.redirectUrl);
        }, 2000);
    }
}
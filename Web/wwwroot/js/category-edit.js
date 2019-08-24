var my = {}

my.viewname = {
    init: function (settings) {
        my.uploadUrl = settings.uploadUrl;
        my.nameInputId = settings.nameInputId;
        my.categoryImageInputId = settings.categoryImageInputId;
        my.redirectUrl = settings.redirectUrl;
        my.Id = settings.IdOfId;

        //Selectors part
        my.IdSelector = "#".concat(settings.IdOfId);
        my.submitFormButtonSelector = "#".concat(settings.submitFormButtonId);
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

    let categoryId = $(my.IdSelector).val();
    upload.addItem(my.Id, categoryId);

    $(my.submitFormButtonSelector).click(submitFile);
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

    let fileInformation = dragAndDrop.getFile();

    if (!fileInformation) {
        console.log("Upload file first");
        return;
    }

    let file = fileInformation.file;
    let name = $(my.nameInputSelector).val();

    upload.addItem(my.nameInputId, name);
    upload.addItem(my.categoryImageInputId, file);

    upload.uploadForm();
}


function uploadResult(result) {
    if (!result.status) {
        let form = $(my.formSelector);
        upload.removeItem(my.nameInputId);
        upload.removeItem(my.categoryImageInputId);

        $("<p> Problem occurred on category update </p>").insertBefore(form);
    }
    else {
        let form = $(my.formSelector);
        $("<p> Cattegory succesfuly updated </p>").insertAfter(form);
        form.remove();

        setTimeout(function () {
            window.location.replace(my.redirectUrl);
        }, 2000);
    }
}

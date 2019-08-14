var my = {};

my.viewname = {
    init: function (settings) {
        my.uploadUrl = settings.uploadUrl;
        my.nameInputId = settings.nameInputId;
        my.nameInputSelector = "#".concat(settings.nameInputId);
        my.categoryImageInputId = settings.categoryImageInputId;
        my.categoryImageSelector = "#".concat(settings.categoryImageInputId);
    }
}

var upload = new FileUpload();

window.onload = function () {
    upload.setUploadUrl(my.uploadUrl);

    let token = $('input[name="__RequestVerificationToken"]').val();
    upload.addItem("__RequestVerificationToken", token);

    $("#submitForm").click(submitFile);
}

function submitFile(e) {
    e.preventDefault();

    let file = $(my.categoryImageSelector).prop('files')[0];
    let name = $(my.nameInputSelector).val();

    if (!name || !file) {
        console.log("Something aint riht");
        return;
    }

    upload.addItem(my.nameInputId, name);
    upload.addItem(my.categoryImageInputId, file);
    upload.uploadForm();
}
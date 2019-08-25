var my = {}

my.viewname = {
    init: function(settings) {
        my.uploadUrl = settings.uploadUrl;
        my.categoryInputId = settings.categoryInputId;
        my.redirectUrl = settings.redirectUrl;
        my.descriptionInputId = settings.descriptionInputId;
        my.tagId = settings.tagsDivId;
        my.imageId = settings.imageUploadDivId;

        my.submitFormButtonSelector = "#".concat(settings.submitFormButtonId);
        my.imageUploadDivId = "#".concat(settings.imageUploadDivId);
        my.formSelector = "#".concat(settings.formId);
        my.categoryInputSelector = "#".concat(settings.categoryInputId);
        my.tagsDivSelector = "#".concat(settings.tagsDivId);
        my.descriptionInputSelector = "#".concat(settings.descriptionInputId);

        //Tag selector
        my.tagInputSelector = "#".concat("single-tag");
        my.addTagButtonSelector = "#".concat("add-tag");
        my.tagContainerDivSelector = ".".concat("tag-container");
        my.tagContainerItemSelector = ".".concat("tag-item");
        my.tagTextSelector = ".".concat("tag-text");
    }
}

var upload;
var dragAndDrop;

$(document).ready(function () {
    upload = new FileUpload(uploadResult, my.uploadUrl);
    prepareForDragAndDrop();

    let token = $('input[name="__RequestVerificationToken"]').val();
    upload.addItem("__RequestVerificationToken", token);

    let categoryId = $(my.categoryInputSelector).val();
    upload.addItem(my.categoryInputId, categoryId);

    my.redirectUrl = my.redirectUrl.concat("/", categoryId);

    prepareForDragAndDrop();
    prepareSectionForTags();

    $(my.submitFormButtonSelector).click(submitFile);
});

function prepareForDragAndDrop() {
    dragAndDrop = new DragAndDrop(my.imageUploadDivId, ['jpg', 'jpeg', 'png']);
    $(my.imageUploadDivId).remove();
}

function prepareSectionForTags() {
    $(my.tagsDivSelector).empty();

    //Fill tag section
    $("<p>Tags</p>").appendTo($(my.tagsDivSelector));
    $("<div class='tag-container'></div>").appendTo($(my.tagsDivSelector));
    $("<input id='single-tag' class='form-control' placeholder='tag'/>").appendTo($(my.tagsDivSelector));
    $('<br /><input value="Add tag" type="button" id="add-tag" class="btn btn-primary" />').appendTo($(my.tagsDivSelector));

    $(my.addTagButtonSelector).click(addTag)
}

function addTag() {
    let newTag = $(my.tagInputSelector).val();
    if (!newTag) {
        console.log("Tag is empty");
        return;
    }

    $(my.tagInputSelector).val("");
    let newHtmlTagElement = "<div class='tag-item'>"
        .concat("<span class='tag-text'>").concat(newTag).concat("</span>")
        .concat("<span class='remove-tag'>X</span>")
        .concat("</div>");
    $(newHtmlTagElement).appendTo($(my.tagContainerDivSelector));

    $(".remove-tag").last().on("click", removeTag);
}

function removeTag() {
    let tagItem = $(this).parent();
    tagItem.remove();
}

function getAllTags() {
    let tagsClassArray = $(my.tagTextSelector).toArray();
    let finalTagArray = [];

    tagsClassArray.forEach(function (item) {
        finalTagArray.push($(item).text());
    });

    return finalTagArray.join(", ");
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
    let description = $(my.descriptionInputSelector).val();
    let tag = getAllTags();

    upload.addItem(my.descriptionInputId, description);
    upload.addItem(my.imageId, file);
    upload.addItem(my.tagId, tag);

    upload.uploadForm();
}

function uploadResult(result) {
    if (!result.status) {
        let form = $(my.formSelector);
        upload.removeItem(my.descriptionInputId);
        upload.removeItem(my.imageId);
        upload.removeItem(my.tagId);

        $("<p> Problem with adding image </p>").insertBefore(form);
    }
    else {
        let form = $(my.formSelector);
        $("<p> Image succesfuly added </p>").insertAfter(form);
        form.remove();

        setTimeout(function () {
            window.location.replace(my.redirectUrl);
        }, 2000);
    }
}

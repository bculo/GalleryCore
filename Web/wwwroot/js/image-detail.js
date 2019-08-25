my = {}

my.selectorId = function (objectId) {
    return "#".concat(objectId);
}

my.selectorClass = function (objectId) {
    return ".".concat(objectId);
}

my.viewname = {
    init: function (settings) {
        my.imageId = settings.imageId;

        my.commentUrl = settings.commentUrl;
        my.commentInputId = settings.commentInputId;
        my.commentButtonId = settings.commentButtonId;

        my.likeButtonId = settings.likeButtonId;
        my.likeValueId = settings.likeValueId;
        my.likeUrl = settings.likeUrl;

        my.dislikeValueId = settings.dislikeValueId;
        my.dislikeButtonId = settings.dislikeButtonId;
    }
}

$(document).ready(function () {


    $(my.selectorId(my.commentButtonId)).click(comment);
    $(my.selectorId(my.likeButtonId)).click(like);
    $(my.selectorId(my.dislikeButtonId)).click(dislike);
});

function prepareData() {
    let imageId = $(my.selectorId(my.imageId)).val();
    let data = { Id: imageId };
    return data;
}

function comment(e) {
    e.preventDefault();

    //Get and comment check value
    let commentValue = $(my.selectorId(my.commentInputId)).val();
    if (!commentValue) {
        console.log("Comment cant be empty string");
        return;
    }

    //Prepare data
    let data = prepareData();
    data.Comment = commentValue;

    //Clean comment value
    $(my.selectorId(my.commentInputId)).val("");

    //Get upload url
    let uploadUrl = my.commentUrl;

    //Submit data
    submit(uploadUrl, data);
}

function like(e) {
    e.preventDefault();

    //Prepare data
    let data = prepareData();
    data.Like = true;

    //Get upload url
    let uploadUrl = my.likeUrl;

    //Submit data
    submit(uploadUrl, data);
}

function dislike(e) {
    e.preventDefault();

    //Prepare data
    let data = prepareData();
    data.Like = false;

    //Get upload url
    let uploadUrl = my.likeUrl;

    //Submit data
    submit(uploadUrl, data);
}

function submit(submitUrl, object) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    object.__RequestVerificationToken = token;

    $.ajax({
        type: "post",
        url: submitUrl,
        data: object,
        success: function (data){
            alert(data);
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

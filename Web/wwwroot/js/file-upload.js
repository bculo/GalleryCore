function Item(key, value) {
    this.key = key;
    this.value = value;
}

function AjaxCallResult(status, message) {
    this.status = status;
    this.message = message;
}

function FileUpload(onUploadFunction, destinationUrl) {
    this.executeFunctionOnUpload = onUploadFunction;
    this.items = [];
    this.destinationUrl = destinationUrl;
}

FileUpload.prototype.addItem = function (key, value) {
    let pair = new Item(key, value);
    this.items.push(pair);
}

FileUpload.prototype.removeItem = function (keyOfItem) {
    this.items = this.items.filter(item => item.key != keyOfItem);
}

FileUpload.prototype.uploadForm = function () {
    let formData = new FormData();

    for (var item of this.items) {
        formData.append(item.key, item.value)
    }

    let callBackFunction = this.executeFunctionOnUpload;

    $.ajax({
        type: "post",
        url: this.destinationUrl,
        contentType: false,
        processData: false,
        data: formData,
        success: function (result) {
            if (Reflect.has(result, "success") && Reflect.has(result, "redirectAction")) {
                callBackFunction(new AjaxCallResult(result.success, result.redirectAction));
            }

            callBackFunction(new AjaxCallResult(true, "Index"));
        },
        error: function (result) {
            if (Reflect.has(result, "success") && Reflect.has(result, "message")) {
                callBackFunction(new AjaxCallResult(result.success, result.message));
            }

            callBackFunction(new AjaxCallResult(false, "Error"));
        }
    });
}





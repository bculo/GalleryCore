function Item(key, value) {
    this.key = key;
    this.value = value;
}

function AjaxCallResult(status, message) {
    this.status = status;
    this.message = message;
}

function FileUpload(onUploadFunction, destinationUrl) {
    var executeFunctionOnUpload = onUploadFunction;
    var items = [];
    var destinationUrl = destinationUrl;

    this.addItem = function (key, value) {
        let pair = new Item(key, value);
        items.push(pair);
    }

    this.removeItem = function (keyOfItem) {
        items = items.filter(item => item.key != keyOfItem);
    }

    this.uploadForm = function () {
        let formData = new FormData();

        for (var item of items) {
            formData.append(item.key, item.value)
        }

        $.ajax({
            type: "post",
            url: destinationUrl,
            contentType: false,
            processData: false,
            data: formData,
            success: function (result) {
                if (Reflect.has(result, "success") && Reflect.has(result, "redirectAction")) {
                    executeFunctionOnUpload(new AjaxCallResult(result.success, result.redirectAction));
                }

                executeFunctionOnUpload(new AjaxCallResult(true, "Index"));
            },
            error: function (result) {
                if (Reflect.has(result, "success") && Reflect.has(result, "message")) {
                    executeFunctionOnUpload(new AjaxCallResult(result.success, result.message));
                }

                executeFunctionOnUpload(new AjaxCallResult(false, "Error"));
            }
        });
    }
}





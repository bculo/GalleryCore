//Note: this file use jquery
function Item(key, value) {
    this.key = key;
    this.value = value;
}

function FileUpload() {
    var antiforgeryToken = null;
    var fileUpload;
    var items = []; // array of FilePair items
    var destinationUrl = ""; // upload destination

    this.setFile = function (key, file) {
        fileUpload = new Item(key, file);
    }

    this.setForgeryToken = function (key, file) {
        antiforgeryToken = new Item(key, file);
    }

    this.setUploadUrl = function (url) {
        destinationUrl = url;
    }

    this.addItem = function (key, value) {
        let pair = new Item(key, value);
        items.push(pair);
    }

    this.uploadForm = function () {
        let formData = new FormData();

        console.log(fileUpload);
        console.log(antiforgeryToken);

        for (var item of items) {
            formData.append(item.key, item.value)
        }

        if (fileUpload != null) {
            formData.append(fileUpload.key, fileUpload.value);
        }

        if (antiforgeryToken != null) {
            formData.append(antiforgeryToken.key, antiforgeryToken.value);
        }

        $.ajax({
            type: "post",
            url: destinationUrl,
            contentType: false,
            processData: false,
            data: formData,
            success: function (message) {
                alert(message);
            },
            error: function () {
                alert("ERROR!");
            }
        });
    }
}





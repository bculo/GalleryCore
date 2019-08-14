//Note: this file use jquery
function Item(key, value) {
    this.key = key;
    this.value = value;
}

function FileUpload() {
    var items = []; 
    var destinationUrl = ""; 

    this.setUploadUrl = function (url) {
        destinationUrl = url;
    }

    this.addItem = function (key, value) {
        let pair = new Item(key, value);
        items.push(pair);
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
            success: function (message) {
                //Add return
            },
            error: function () {
                //Add return
            }
        });
    }
}





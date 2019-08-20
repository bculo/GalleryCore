function FileInformation(newFile) {
    this.file = newFile;
    this.fileName = newFile.name;
    this.fileExtension = this.fileName.substr(this.fileName.lastIndexOf('.') + 1);
}

function DragAndDrop(insertAfterElementWithId, validFileExtensionsAsArray) {

    //Valid file extensions must be in array
    if (!(validFileExtensionsAsArray instanceof Array)){ 
        console.log("validFileExtensionsAsArray must be array");
        return;
    }

    //Add drag and drop area
    var dragAndDropId = "#drag-and-drop-div";
    var dragAndDropDiv = "<div id='drag-and-drop-div'><p>Drag image here!</p></div>"
    $(dragAndDropDiv).insertAfter($(insertAfterElementWithId));

    var buttonRemove = "#remove-drag-and-drop-image";

    var file; // instance of FileInformation
    var fileExtension = validFileExtensionsAsArray; //array of file extensions

    //Prevent strange behavior in the browser
    $(dragAndDropId).on("dragover", function (e) {  
        e.preventDefault();
    });

    //Execute fileDroped function on file drop action
    $(dragAndDropId).on("drop", function (e) {
        //Prevent strange behavior in the browser
        e.preventDefault();
        e.stopPropagation();

        //get all uploded files
        let uplodedFiles = e.originalEvent.dataTransfer.files;

        //check if file exists
        if (uplodedFiles.length <= 0) {
            console.log("No file");
            return;
        }

        // get first uploded file
        let firstUploadedFile = uplodedFiles[0];

        //Create temporery instance of FileInformation
        let temp = new FileInformation(firstUploadedFile);

        //set private variable
        file = temp;

        showImage();
    });

    function showImage (){
        //Create instance of FileReader
        var reader = new FileReader();

        $(reader).on("load", function (e) {
            // remove everything from drag and drop text
            $(dragAndDropId).empty(); 

            //add image to drag and drop div
            $("<img id='image-drag-and-drop' alt='image'/>").appendTo($(dragAndDropId));

            //add src attribute to image
            $("#image-drag-and-drop").attr('src', e.target.result);

            //Add remove button and add on click listener
            $("<div id='remove-drag-and-drop-image'>X</div>").appendTo($(dragAndDropId));
            $(buttonRemove).click(clearContent)
        });

        //Read file
        reader.readAsDataURL(file.file);
    }

    //Clear content from drag and drop area
    function clearContent() {
        removeFile();
        $(dragAndDropId).empty();
        $("<p>Drag image here!</p>").appendTo($(dragAndDropId));
        $(buttonRemove).remove();
    }

    function removeFile() {
        file = null;
    }

    this.getFile = function() {
        return file;
    }
}



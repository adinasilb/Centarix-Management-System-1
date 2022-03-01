import 'regenerator-runtime/runtime'


var MaxFileSizeMB = 1;
var BufferChunkSize = MaxFileSizeMB * (1024 * 1024) * 2;

async function UploadFile(TargetFile, formData) {
    // create array to store the buffer chunks
    var FileChunk = [];
    // the file object itself that we will work with
    var file = TargetFile;
    // set up other initial vars

    var FileStreamPos = 0;
    // set the initial chunk length
    var EndPos = BufferChunkSize;
    var Size = file.size;
    console.log("target file " + file)
    console.log(...formData)

    console.log(FileStreamPos + "< " + Size)
    // add to the FileChunk array until we get to the end of the file
    while (FileStreamPos < Size) {
        // "slice" the file from the starting position/offset, to  the required length
        FileChunk.push(file.slice(FileStreamPos, EndPos));
        FileStreamPos = EndPos; // jump by the amount read
        EndPos = FileStreamPos + BufferChunkSize; // set next chunk length
    }
    // get total number of "files" we will be sending
    var TotalParts = FileChunk.length;
    var PartCount = 0;
    // loop through, pulling the first item from the array each time and sending it
    var FilePartName = file.name;
    var chunk;
    while (chunk = FileChunk.shift()) {
        if (PartCount == 0) {
            formData.set("IsFirstPart", true);
        }
        else {
            formData.set("IsFirstPart", false);
        }
        PartCount++;
        FilePartName = await UploadFileChunk(chunk, FilePartName, formData)
        console.log("filepartName " + FilePartName)
    }

}

async function UploadFileChunk(Chunk, FileName, formData) {
    console.log("upload file")
    formData.set('FilesToSave', Chunk, FileName);
    var fileName = FileName;
    var section = document.querySelector("#masterSectionType").value;
    var urlbeginning = "/Requests/"
    if (section === "Protocols") {
        urlbeginning = "/Protocols/"
    }
    else if (section == "Biomarkers") {
        urlbeginning = "/Biomarkers/"
    }
    return fetch(urlbeginning + 'UploadFile/',
        {
            method: "POST",
            body: formData
        })
        .then((data) => { return data })

}
async function DirectlyUploadDocFromCard(folderName, parentFolderName, objectID, guid = "") {
    console.log("in direct upload function")


    var targetFile = new FormData(document.querySelector(".ordersItemForm")).getAll("FilesToSave").filter(f => f.size > 0)

    var uploadFileFormData = new FormData()
    uploadFileFormData.append("ObjectID", objectID)
    uploadFileFormData.append("ParentFolderName", parentFolderName)
    uploadFileFormData.append("FolderName", folderName)
    uploadFileFormData.append("Guid", guid)
    await UploadFile(targetFile[0], uploadFileFormData)
    var folderID = objectID == "0" ? guid : objectID;
    return (fetch("/Requests/_DocumentsCard?requestFolderNameEnum=" + folderName + "&id=" + folderID + "&parentFolderName=" + parentFolderName,
        {
            method: "GET"
        }
    )
        .then((response) => { return response.json() }))



}

export async function FileSelectChange(element, folderName, parentFolderName, objectID, guid = "") {

    console.log(element)

    console.log("upload file submitted");
    var dontAllowMultipleFiles = document.getElementById("DontAllowMultiple")?.value;
    if (dontAllowMultipleFiles == false) {
        console.log("disable more files")
    }

    var filePath = element.value;
    console.log(filePath)

    var fileName = filePath.split("\\")[2]
    if (document.querySelector("document-name") != null) {
        document.querySelector("document-name").innerHTML = fileName;
        document.querySelector("document-name#FileName").value = fileName
    }

    var extn = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

    //if (extn != "pdf" && extn != "png" && extn != "jpg" && extn != "jpeg" && extn != "docx" && extn != "doc" && extn != "ppt" && extn != "pptx" && extn !="") {
    //	alert("invalid file extension");
    //	return;
    //}

    console.log("in save doc files");


    if (element.classList.contains("direct-upload")) {
        console.log("direct upload")
        return await DirectlyUploadDocFromCard(folderName, parentFolderName, objectID, guid).then(response => {
            return JSON.parse(response)
        })

    }
    else {
        var CustomMainObjectID = document.getElementById(".CustomMainObjectID")?.value;

        var formData = new FormData(document.querySelector(".documentModalForm"));

        var targetFile = formData.getAll("FilesToSave")
        console.log(targetFile)
        for (var i = 0; i < targetFile.length; i++) {
            console.log(targetFile[i])
            await UploadFile(targetFile[i], formData);
        }

        return await OpenDocumentsModal(folderName, objectID, guid, parentFolderName, dontAllowMultipleFiles, CustomMainObjectID).then(response => {
            return JSON.parse(response)
        })
    }
    var folderInput = document.getElementById(foldername + "Input");
    if (folderInput != null) {
        folderInput.classList.add("contains-file");
        if (folderInput.rules()) {
            folderInput.valid();
        }
    }
    return true;
};

async function OpenDocumentsModal(enumString, requestId, guid, parentFolder, dontAllowMultipleFiles, $CustomMainObjectID) {
    //document.getElementById('loading');
    var section = document.getElementById("masterSectionType").value;
    var urlbeginning = "/Requests/"
    if (section === "Protocols") {
        urlbeginning = "/Protocols/"
    }
    else if (section == "Biomarkers") {
        urlbeginning = "/Biomarkers/"
    }

    var urltogo = urlbeginning + "DocumentsModal?id=" + requestId + "&Guid=" + guid + "&RequestFolderNameEnum=" + enumString + "&parentFolderName=" + parentFolder + "&dontAllowMultipleFiles=" + dontAllowMultipleFiles + "&CustomMainObjectID=" + $CustomMainObjectID;

    console.log(urltogo);
    return fetch(urltogo, {
        method: "GET"
    })
        .then(data => {
            //document.getElementById('loading').hide();
            
            //if ($(".addFunctionForm").length > 0) {
            //    if (numCards < 1) {
            //        $(".upload-file").attr('disabled', false)
            //        $(".upload-file").classList.remove('disabled-background-color')
            //        $(".upload-file").classList.add('section-bg-color')
            //        $(".file-select").attr('disabled', false)
            //        $(".upload-file").classList.remove("d-none");
            //        $(".saveFunction").classList.add("d-none");
            //        $(".saveFunction").attr('disabled', true)
            //    }
            //    else {
            //        $(".upload-file").attr('disabled', true)
            //        $(".upload-file").classList.add('disabled-background-color')
            //        $(".upload-file").classList.remove('section-bg-color')
            //        $(".file-select").attr('disabled', true)
            //        $(".upload-file").classList.add("d-none");
            //        $(".saveFunction").attr('disabled', false)
            //        $(".saveFunction").classList.remove("d-none");
            //    }

            //}
            return data.json();
        })
    return true;
};

export function CheckEditable(modalType) {
    var isEditable = false;
    if (modalType == "Create") {
        isEditable = true;
    }
    else if (modalType == "Edit") {
        var editSwitchName = document.querySelector(".turn-edit-on-off").getAttribute("name");
        isEditable = editSwitchName == "edit" ? true : false
    }
    return isEditable;
}
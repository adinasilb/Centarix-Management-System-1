import React, { useState, useEffect } from 'react';
import ReactDOM from 'react-dom';
import {
    useLocation
} from 'react-router-dom';
import { GetFileString } from "../Utility/document-fuctions.jsx"
import GlobalModal from '../Utility/global-modal.jsx';
import { } from '../Utility/root-function.jsx'

export default function DeleteDocumentModal(props) {
    const location = useLocation();
    const [viewModel, setViewModel] = useState({
        FileName: location.state.FileName,
        FolderName: location.state.FolderName,
        ParentFolderName: location.state.ParentFolderName,
        ObjectID: location.state.ID
    });

    function deleteFile(e) {
        e.preventDefault()
        console.log("delete file")
        location.state.deleteFunction(viewModel);
    }
    //useEffect(() => {
    //    var url = "/Requests/DeleteDocumentModal?FileString=" + location.state.FileName + "&id=" + ID + "&RequestFolderNameEnum=" + location.state.FolderName + "&parentFolderName=" + location.state.ParentFolderName;
    //    fetch(url, {
    //        method: "GET"
    //    })
    //        .then((response) => { return response.json(); })
    //        .then(result => {
    //            setViewModel(JSON.parse(result));
    //        });

    //}, [state.requestID]);


    return (

        <GlobalModal backdrop={props.backdrop} value={viewModel.ID} modalKey={props.modalKey} key={viewModel.ID} hideModalFooter={true} size="md" header={"Delete File"} >

            <form action="" method="post" encType="multipart/form-data" id="myForm" className="modal-padding DeleteDocumentModalForm">
                <div asp-validation-summary="ModelOnly" className="text-danger-centarix"></div>
                <input id="ObjectID" name="ObjectID" type="hidden" value={viewModel.ObjectID} />
                <input id="FileName" name="FileName" type="hidden" value={viewModel.FileName} />
                <input id="FolderName" name="FolderName" type="hidden" value={viewModel.FolderName} />
                <input id="ParentFolderName" name="ParentFolderName" type="hidden" value={viewModel.ParentFolderName} />
                <div className="card document-border m-0">
                    <div className="card-body responsive-iframe-container">
                        <iframe src={GetFileString(viewModel.FileName)} title="View" className="responsive-iframe delete" scrolling="no"></iframe>

                    </div>
                    <div className="card-body text-center">
                        Are you sure you want to delete?<br />
                        <button type="button" className="custom-button custom-button-font section-bg-color" onClick={deleteFile}>Delete</button>
                    </div>
                </div>
            </form>
        </GlobalModal>
    );
}

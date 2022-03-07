import React, { useState, useEffect } from 'react';
import {
    useLocation, Link
} from 'react-router-dom';
import GlobalModal from '../Utility/global-modal.jsx';
import { FileSelectChange, GetFileString } from "../Utility/document-fuctions.jsx";
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import Carousel from 'react-multi-carousel';
import 'react-multi-carousel/lib/styles.css';

import 'regenerator-runtime/runtime'
import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import { useDispatch } from 'react-redux';
import { useForm } from 'react-hook-form';


export default function DocumentsModal(props) {
    const location = useLocation();
    const dispatch = useDispatch();
    const { register, handleSubmit, formState: { errors } } = useForm();
    const [ID, setID] = useState(location.state.ID)
    const [isEditable, setisEditable] = useState(location.state.IsEditable)
    const [showSwitch, setshowSwitch] = useState(location.state.ShowSwitch)
    const [viewModel, setViewModel] = useState({ FileStrings: [] });
    const [resetParentCard] = useState(() => location.state.resetDocInfo)

    console.log("rerender doc modal")

    useEffect(() => {
        var url = "/Requests/DocumentsModal?id=" + ID + "&RequestFolderNameEnum=" + location.state.FolderName + "&parentFolderName=" + location.state.ParentFolderName;
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                setViewModel(JSON.parse(result));
            });

    }, [ID]);

    //useEffect(() => {
    //    MapDocCarouselItems(viewModel.FileStrings)
    //}, [viewModel.FileStrings])

    var id = viewModel?.ObjectID == null ? "0" : viewModel.ObjectID;

    async function uploadFile(e) {
        var response = await FileSelectChange(e.target, viewModel.FolderName, viewModel.ParentFolderName, id)
        console.log(response)
        setViewModel(response)
    }

    function deleteFile(deleteFileViewModel) {
        var url = "/Requests/DeleteDocumentModal";
        var formData = new FormData();
        for (let field in deleteFileViewModel) {
            formData.append(field.toString(), deleteFileViewModel[field])
        }
        console.log(formData)
        fetch(url, {
            method: "POST",
            body: formData
        })
            .then((response) => { return response.json(); })
            .then(result => {
                dispatch(Actions.removeModal(ModalKeys.DELETE_DOCUMENTS));
                setViewModel(JSON.parse(result));
            });

    }
    var onSubmit = (data, e) => {
        var url = "/Requests/DocumentsModal";
        var formData = new FormData(e.target);
        fetch(url, {
            method: "POST",
            body: formData
        })
            .then((response) => { return response.json(); })
            .then(result => {
                dispatch(Actions.removeModal(ModalKeys.DOCUMENTS));
                var docInfo = JSON.parse(result)
                resetParentCard(docInfo);
            });
    }


    return (

        <GlobalModal backdrop={props.backdrop} value={ID} modalKey={props.modalKey} key={ID} size="xl" header={viewModel.FolderName + " Files"}>
            <form action="" data-string="" method="post" encType="multipart/form-data" className="m-5 modal-padding documentModalForm" onSubmit={handleSubmit(onSubmit)} id={props.modalKey}>
                <input type="hidden" name="FolderName" id="FolderName" value={viewModel.FolderName || ""} />
                <input type="hidden" name="DontAllowMultiple" id="DontAllowMultiple" value={viewModel.DontAllowMultiple || ""} />
                <input type="hidden" name="ParentFolderName" id="ParentFolderName" value={viewModel.ParentFolderName || ""} />
                <input type="hidden" name="ObjectID" id="ObjectID" value={viewModel.ObjectID || ""} />
                <input type="hidden" name="Guid" id="Guid" value={viewModel.Guid || ""} />

                <div className="container">
                    <hr />
                    <div className="row">
                        <div className="col-3 offset-9">
                            {(showSwitch) ?
                                <div className=" row text-right mb-0">
                                    <div className="switch col-12 switch-margin">
                                        <label>
                                            <label className="edit-mode-switch-description "> {isEditable ? "Edit Mode On" : "Edit Mode Off"}</label>
                                            <input type="checkbox" className={"turn-edit-doc-on-off" + (isEditable ? " checked" : "")} value={ID} />
                                            <span className="lever"></span>
                                        </label>
                                    </div>
                                </div>
                                : null
                            }
                        </div>
                    </div>
                </div>
                {console.log("view model file strings: " + viewModel?.FileStrings)}
                {!isEditable && viewModel?.FileStrings?.length == 0 ?
                    <div className="text-center text-lowercase">
                        <div className="mt-6">
                            <img src="/images/document_empty_image.png" />
                        </div>
                        <div className="mt-4" style={{ fontSize: "2rem", color: "1.125rem", fontWeight: "400" }} >
                            <span className="text-capitalize">No</span> {viewModel?.FolderName} have been uploaded
                        </div>
                    </div>
                    :
                    <div>
                        {(isEditable || showSwitch) ?
                            <div className=" col-4 doc-card-outer-div m-0 text-center h-100">
                                <div className=" card document-border h-100">
                                    <div className=" d-flex align-items-center justify-content-center flex-column">
                                        <label>
                                            <i className="icon-upload_file_black_24dp-1 section-filter m-0" alt="order" style={{ fontSize: "2rem" }}></i>
                                            <input type="file" onChange={uploadFile} className="file-selects d-none mark-readonly" accept=".png, .jpg, .jpeg, .pdf, .pptx, .ppt, .docx, .doc, .xlsx, .xls" id="FilesToSave" name="FilesToSave" />


                                        </label>

                                        <span className="section-filter text">Upload File</span>
                                    </div>
                                </div>
                            </div>
                            : null}
                        <Carousel
                            infinite
                            renderButtonGroupOutside
                            responsive={{
                                desktop: {
                                    breakpoint: {
                                        max: 3000,
                                        min: 1024
                                    },
                                    items: 3
                                }
                            }}
                            deviceType="desktop">

                            {viewModel.FileStrings.map((fileString, i) => {
                                return (
                                    <div key={"FileString" + i} className=" doc-card-outer-div col-12 m-0">
                                        <div className="card iframe-container document-border m-0">
                                            <div className="card-body responsive-iframe-container">

                                                <iframe src={GetFileString(fileString)} title="View" className="responsive-iframe" scrolling="no"></iframe>
                                            </div>
                                            <div className="card-body d-flex text-center align-items-center justify-content-center">

                                                <a href={"\\" + fileString} target="_blank" className="mx-3  view-img">
                                                    {fileString.split('\\').pop()}
                                                </a>
                                                <Link className="" to={{
                                                    pathname: "/DeleteDocumentModal",
                                                    state: { ID: ID, FolderName: viewModel.FolderName, ParentFolderName: viewModel.ParentFolderName, FileName: fileString, deleteFunction: deleteFile }
                                                }}>
                                                    <i style={{ fontSize: "2rem" }} className="icon-delete-24px documents-delete-icon hover-bold"></i>
                                                </Link>
                                            </div>
                                        </div>
                                    </div>
                                )
                            })}

                        </Carousel>
                    </div>}

            </form>
        </GlobalModal>
    );
}

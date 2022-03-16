import React, { useState, useEffect, useRef } from 'react';
import {
    useLocation, Link, useHistory
} from 'react-router-dom';
import GlobalModal from '../Utility/global-modal.jsx';
import { FileSelectChange, GetFileString } from "../Utility/document-fuctions.jsx";
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import Carousel from 'react-multi-carousel';
import 'react-multi-carousel/lib/styles.css';
import * as Routes from '../Constants/Routes.jsx'

import 'regenerator-runtime/runtime'
import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import { useDispatch } from 'react-redux';
import { useForm } from 'react-hook-form';
import { MDBBtn} from 'mdb-react-ui-kit';



export default function DocumentsModal(props) {
    const location = useLocation();
    const dispatch = useDispatch();
    const history = useHistory();
    const inputFileRef = useRef();
    const { register, handleSubmit, formState: { errors } } = useForm();
    const [state, setState] = useState({
        ID: location.state.ID,
        isEditable: location.state.IsEditable,
        showSwitch: location.state.ShowSwitch,
        viewModel: {},
        resetParentCard: (location.state.resetDocInfo)
    })

    console.log("rerender doc modal")

    useEffect(() => {
        var url = "/Requests/DocumentsModal?id=" + state.ID + "&RequestFolderNameEnum=" + location.state.FolderName + "&parentFolderName=" + location.state.ParentFolderName;
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                setState({ ...state, viewModel: JSON.parse(result) });
            });

    }, [state.ID]);

    //useEffect(() => {
    //    MapDocCarouselItems(viewModel.FileStrings)
    //}, [viewModel.FileStrings])

    var id = state.viewModel?.ObjectID == null ? "0" : state.viewModel.ObjectID;

    async function uploadFile(e) {
        var response = await FileSelectChange(e.target, state.viewModel.FolderName, state.viewModel.ParentFolderName, id)
        setState({ ...state, viewModel: response })
    }

    function deleteFile(deleteFileViewModel) {
        var url = "/Requests/DeleteDocumentModal";
        var formData = new FormData();
        for (let field in deleteFileViewModel) {
            formData.append(field.toString(), deleteFileViewModel[field])
        }
        console.dir(formData)
        fetch(url, {
            method: "POST",
            body: formData
        })
            .then((response) => { return response.json(); })
            .then(result => {
                dispatch(Actions.removeModal(ModalKeys.DELETE_DOCUMENTS));
                setState({ ...state, viewModel: JSON.parse(result) })
            });

    }
    var onSubmit = (e) => {
        e.target && e.preventDefault()
        console.log(e)
        var url = "/Requests/DocumentsModal";
        var formData = new FormData(document.getElementById(props.modalKey));
        fetch(url, {
            method: "POST",
            body: formData
        })
            .then((response) => { return response.json(); })
            .then(result => {
                var docInfo = JSON.parse(result)
                state.resetParentCard(docInfo);
                dispatch(Actions.removeModal(ModalKeys.DOCUMENTS));
                
            });
    }
    function handleFileBtnClick() {
        console.log("file button click")
        inputFileRef.current.click();
    }

    var pathname = (history.entries[1] && history.entries[1].pathname != Routes.DELETE_DOCUMENTS) ? history.entries[1].pathname : ""
    console.log("pathname " + pathname)

    return (

        <GlobalModal backdrop={props.backdrop} value={state.ID} modalKey={props.modalKey} key={state.ID} size="xl" hideModalFooter={true} closeClick={onSubmit} header={state.viewModel.FolderName + " Files"}>
            <form action="" data-string="" method="post" encType="multipart/form-data" className="m-5 modal-padding" onSubmit={handleSubmit(onSubmit)} id={props.modalKey}>
                <input type="hidden" name="FolderName" id="FolderName" value={state.viewModel.FolderName || ""} />
                <input type="hidden" name="DontAllowMultiple" id="DontAllowMultiple" value={state.viewModel.DontAllowMultiple || ""} />
                <input type="hidden" name="ParentFolderName" id="ParentFolderName" value={state.viewModel.ParentFolderName || ""} />
                <input type="hidden" name="ObjectID" id="ObjectID" value={state.viewModel.ObjectID || ""} />
                <input type="hidden" name="Guid" id="Guid" value={state.viewModel.Guid || ""} />

                <div className="container">
                    <hr />
                    <div className="row">
                        <div className="col-3 offset-9">
                            {(state.showSwitch) ?
                                <div className=" row text-right mb-0">
                                    <div className="switch col-12 switch-margin">
                                        <label>
                                            <label className="edit-mode-switch-description "> {state.isEditable ? "Edit Mode On" : "Edit Mode Off"}</label>
                                            <input type="checkbox" className={"turn-edit-doc-on-off" + (state.isEditable ? " checked" : "")} value={state.ID} />
                                            <span className="lever"></span>
                                        </label>
                                    </div>
                                </div>
                                : null
                            }
                        </div>
                    </div>
                </div>
                {console.log("view model file strings: " + state.viewModel?.FileStrings)}
                {(state.viewModel.FileStrings == null || state.viewModel?.FileStrings.length == 0 )?
                    <div className="text-center text-lowercase">
                        <div className="mt-6">
                            <img src="/images/document_empty_image.png" />
                        </div>
                        <div className="mt-4" style={{ fontSize: "2rem", color: "1.125rem", fontWeight: "400" }} >
                            <span className="text-capitalize">No</span> {state.viewModel?.FolderName} have been uploaded
                        </div>
                    </div>
                    :
                    <div>
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

                            {state.viewModel.FileStrings.map((fileString, i) => {
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
                                                    pathname: pathname+ "/DeleteDocumentModal",
                                                    state: { ID: state.ID, FolderName: state.viewModel.FolderName, ParentFolderName: state.viewModel.ParentFolderName, FileName: fileString, deleteFunction: deleteFile }
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
                <div className="row">
                    {(state.isEditable || state.showSwitch) &&
                        <div>
                        <MDBBtn type="button" className="custom-button custom-button-font section-bg-color between-button-margin" onClick={handleFileBtnClick} >Upload File</MDBBtn>
                        <input type="file" ref={inputFileRef} onChange={uploadFile} className="file-selects d-none mark-readonly" accept=".png, .jpg, .jpeg, .pdf, .pptx, .ppt, .docx, .doc, .xlsx, .xls" id="FilesToSave" name="FilesToSave" />
                    </div>
                }
                    <MDBBtn type="submit" color="white" className="custom-cancel custom-button " >Cancel</MDBBtn>
            </div>
            </form>
        </GlobalModal>
    );
}

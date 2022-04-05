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

    useEffect(() => {
        var url = "/Requests/UploadDocumentInfoModal?id=" + ID;
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                setViewModel(JSON.parse(result));
            });

    }, [ID]);


    var id = viewModel?.ObjectID == null ? "0" : viewModel.ObjectID;

    async function uploadFile(e) {
        var response = await FileSelectChange(e.target, viewModel.FolderName, viewModel.ParentFolderName, id)
        console.log(response)
        setViewModel(response)
    }

    var onSubmit = (data, e) => {
        var url = "/Requests/UploadDocumentInfoModal";
        var formData = new FormData(e.target);
        fetch(url, {
            method: "POST",
            body: formData
        })
            .then((response) => { return response.json(); })
            .then(result => {
                dispatch(Actions.removeModal(ModalKeys.UPLOAD_INVOICE));
                var docInfo = JSON.parse(result)
                resetParentCard(docInfo);
            });
    }


    return (

        <GlobalModal backdrop={props.backdrop} value={ID} modalKey={props.modalKey} key={ID} size="sm" header={viewModel.FolderName + " Files"}>
            <form action="" data-string="" method="post" encType="multipart/form-data" className="m-5 modal-padding documentModalForm" onSubmit={handleSubmit(onSubmit)} id={props.modalKey}>
                <input type="hidden" name="FolderName" id="FolderName" value={viewModel.FolderName || ""} />
                <div className="container">
                    <div className="row">
                        <div className=" col-4 doc-card-outer-div m-0 text-center h-100">
                            <div className=" card document-border h-100">
                                <div className=" d-flex align-items-center justify-content-center flex-column">
                                    <label>
                                        <i className="icon-upload_file_black_24dp-1 section-filter m-0" alt="order" style={{ fontSize: "2rem" }}></i>
                                        <input type="file" onChange={uploadFile} className="file-selects d-none mark-readonly" accept=".png, .jpg, .jpeg, .pdf, .pptx, .ppt, .docx, .doc, .xlsx, .xls" id="FilesToSave" name="FilesToSave" {...register("filesToSave", { required: true })}/>

                                    </label>
                                    <span className="section-filter text">Upload File</span>
                                </div>
                            </div>
                        </div>
                        {errors.filesToSave && <span>This field is required</span>}
                        <div className="col-4 pl-5 pt-4">
                            <div className="row">

                                <div className=" col-md-6">
                                    <div className="form-group ">
                                        <label className=" control-label m-0 mt-2" style="width:100%;">{viewModel.FolderName} Number</label>
                                        <input id="Number" name="Number" className="no-arrow-input form-control-plaintext border-bottom align-with-select timeline-light-item-orange p-0 m-0" {...register("number", { required: true })} />
                                        {errors.number && <span>This field is required</span>}

                                    </div>
                                </div>
                                <div className="col-md-6">
                                    <label className=" control-label m-0 mt-2" style="width:100%;">{viewModel.FolderName} Date</label>
                                    <input id="Date" name="Date" value={viewModel.Date} type="text" className="datepicker form-control-plaintext border-bottom p-0 m-0" {...register("date", { required: true })} />
                                    {errors.date && <span>This field is required</span>}

                                </div>

                            </div>
                        </div>
                    </div>
                    </div>
            </form>
        </GlobalModal>
            );
}

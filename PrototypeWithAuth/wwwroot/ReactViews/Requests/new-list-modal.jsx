﻿import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import { ErrorMessage } from '@hookform/error-message';
import GlobalModal from '../Utility/global-modal.jsx';

import { useForm } from 'react-hook-form';
import { DevTool } from "@hookform/devtools";

export default function NewListModal(props) {
    const dispatch = useDispatch();
    const location = useLocation();
    const { register, handleSubmit, control, formState: { errors, isValid } } = useForm({ mode: 'onChange' });
    const [state, setState] = useState({ viewModel: null, requestToAddId: location.state.requestToAddId, requestPreviousListID: location.state.requestPreviousListID });
    console.log("isvalid:" + isValid)
    console.dir(errors)
    useEffect(() => {
        var url = "/Requests/NewListModalJson/?requestToAddId=" + state.requestToAddId
        if (state.requestPreviousListID != undefined) {
            url = url + "&requestPreviousListID=" + state.requestPreviousListID
        }
     
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                setState({ ...state, viewModel: JSON.parse(result) });
            });

    }, [state.requestToAddId]);

    var onSubmit = (e) => {
            e.preventDefault();
            alert("in submit")
            console.log(errors);
            var url = "/Requests/NewListModal";
            var formData = new FormData(e.target);
           // ajaxPartialIndexTable(dispatch, url, "POST", formData, [ModalKeys.NEW_LIST, ModalKeys.MOVE_TO_LIST]);
       
        };

    return (
        <GlobalModal backdrop={props.backdrop} size="lg" value={state.viewModel?.ID ?? ""} modalKey={props.modalKey} key={state.viewModel?.ID ?? ""} header="New List" >
            <form onSubmit={handleSubmit(onSubmit)} method="post" encType="multipart/form-data" style={{ height: "100%", overflow: "auto" }} className="" id={props.modalKey}>
                {process.env.NODE_ENV !== 'production' && <DevTool control={control} />}
                <input type="hidden" value={state.viewModel?.Request?.RequestID??""} name="Request.RequestID" className="request-to-move" />
                <input type="hidden" value={state.viewModel?.OwnerID ?? ""} name="OwnerID"  />
                <input type="hidden" value={state.viewModel?.RequestToAddID ?? ""} name="RequestToAddID" />
                <input type="hidden" value={state.viewModel?.RequestPreviousListID ?? ""} name="RequestPreviousListID" />
                <div className="contaner-fluid p-0">
                    <div className="error-message text-danger-centarix row">
                        {state.viewModel?.ErrorMessage ?? ""}
                    </div>
                    <div className="row ">
                        <div className="col-10">
                            <label  className="control-label">List Name (up to 20 characters)</label>
          
                            <input type="text" className="form-control-plaintext border-bottom"  {...register("ListTitle", {
                                value: state.viewModel?.ListTitle ?? "",
                                required: true,
                                minLength: 1,
                                maxLength: 20,
                                message:  "List title is a required feild"
                            })}
                   
                            />
                            <ErrorMessage
                                errors={errors}
                                name="ListTitle"
                                render={({ message }) => <span className="danger-text-centarix">{message}</span>}
                            />
                        </div>
                    </div>
                </div>
            </form>
        </GlobalModal>
    );

}

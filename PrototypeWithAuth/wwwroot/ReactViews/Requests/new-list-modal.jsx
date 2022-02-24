import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { ajaxPartialIndexTable, getRequestIndexString } from '../Utility/root-function.jsx'
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import GlobalModal from '../Utility/global-modal.jsx';

import { useForm } from 'react-hook-form';

export default function NewListModal(props) {
    const dispatch = useDispatch();
    const location = useLocation();
    const { register, formState: { errors, isValid } } = useForm({ mode: 'onChange' });
    const [state, setState] = useState({ viewModel: null, requestToAddId: location.state.requestToAddId, requestPreviousListID: location.state.requestPreviousListID });
    console.log("isvalid:" +isValid)
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
            if (isValid) {
 
                console.log(errors);
                var url = "/Requests/NewListModal";
                var formData = new FormData(e.target);
                ajaxPartialIndexTable(dispatch, url, "POST", formData, [ModalKeys.NEW_LIST, ModalKeys.MOVE_TO_LIST]);
            }
       
        };

    return (
        <GlobalModal backdrop={props.backdrop} size="lg" value={state.viewModel?.ID ?? ""} modalKey={props.modalKey} key={state.viewModel?.ID ?? ""} header="New List" >
            <form action="" method="post" className=" newListForm" encType="multipart/form-data" onSubmit={onSubmit} id="myForm">
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
                                maxLength: 20
                            })}
                   
                            />
                            {errors.ListTitle?.type === 'required' && "First name is required"}
                        </div>
                    </div>
                </div>
            </form>
        </GlobalModal>
    );

}

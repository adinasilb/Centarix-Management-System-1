import React, { useState, useEffect } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { ajaxPartialIndexTable, getRequestIndexString } from '../Utility/root-function.jsx'
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import GlobalModal from '../Utility/global-modal.jsx';

export default function DeleteModal(props) {
    const dispatch = useDispatch();
    const location = useLocation();
    const [state, setState] = useState({ viewModel: null, requestID: location.state.ID });
    console.log(location.state)
    useEffect(() => {
        var url = "/Requests/DeleteModalJson?id=" + state.requestID;
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                setState({ ...state, viewModel: JSON.parse(result) });
            });

    }, [state.requestID]);

    var onSubmit = (e) => {
        e.preventDefault();
        document.getElementById("loading").style.display = "block";
        var formdata = new FormData(e.target);
        var url = '/Requests/DeleteModal'
        ajaxPartialIndexTable(dispatch, url, "POST", formdata, [ModalKeys.DELETE_ITEM])
        return false;
    }

    return (
        <GlobalModal backdrop={props.backdrop} value={state.viewModel?.Request?.RequestID} modalKey={props.modalKey} key={state.viewModel?.Request?.RequestID} size="lg" header={"Are you sure you would like to delete " + state.viewModel?.Request?.Product?.ProductName + "?"} >
            <form onSubmit={onSubmit} method="post" encType="multipart/form-data" style={{ height: "100%", overflow: "auto" }} className="" id="myForm">
                <div className="error-message text-danger-centarix">
                    {state.viewModel?.ErrorMessage??""}
                </div>
                <input  id="Request_RequestID" name="Request.RequestID" type="hidden" value={state.viewModel?.Request?.RequestID??''}/>

                <div className="container-fluid p-0">
                    <span className="text ">Details:</span>
                    <hr />
                    <div className="row">
                        <div className="col-4">
                            <div className="form-group">
                                <label className="control-label ">Parent Category</label>
                                <input className="form-control-plaintext border-bottom" value={state.viewModel?.Request?.Product?.ProductSubcategory?.ParentCategory?.Description ?? ''} disabled />
                            </div>
                        </div>
                        <div className="col-4">
                            <div className="form-group">
                                <label className="control-label" style={{ fontWeight: "500"}}>Product Subcategory</label>
                                <input className="form-control-plaintext border-bottom" value={state.viewModel?.Request?.Product?.ProductSubcategory?.Description ?? ''} disabled />
                            </div>
                        </div>
                        <div className="col-4">
                            <div className="form-group">
                                <label className="control-label" style={{fontWeight:"500"}}>Vendor</label>
                                <input className="form-control-plaintext border-bottom" value={state.viewModel?.Request?.Product?.Vendor?.VendorEnName ?? ''} disabled />
                            </div>
                        </div>

                    </div>
                </div>
            </form>
       </GlobalModal>
    );

}

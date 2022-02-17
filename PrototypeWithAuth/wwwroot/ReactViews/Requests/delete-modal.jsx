import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import CloseButton, { CancelButton } from '../Utility/close-button.jsx'
import { openModal } from '../Utility/modal-functions.jsx'
import { ajaxPartialIndexTable, getRequestIndexString } from '../Utility/root-function.jsx'
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import * as Routes from '../Constants/Routes.jsx'
export default function DeleteModal(props) {
    const dispatch = useDispatch();
    const location = useLocation();
    const [state, setState] = useState({ view: "", requestID: location.state.requestID });
    useEffect(() => {
        var url = "/Requests/_DeleteModal?id=" + state.requestID + "&" + getRequestIndexString();
        fetch(url, {
            method: "GET"
        })
        .then((response) => { return response.text(); })
        .then(result => {
            setState({ ...state, view: result });
            openModal("modal");
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
        <div className={"modal  modal-view modal"} id="myModal" role="dialog" aria-labelledby="Request" >

            <div className="modal-dialog-centered modal-lg mx-auto " role="document" style={{ maxHeight: "100%", overflowY: "auto" }}>

                <div className="modal-content d-inline-block modal-border-radius modal-box-shadow ">
                    <div className="close-button"><CloseButton modalKey={ModalKeys.DELETE_ITEM} pathname={Routes.DELETE_ITEM}  /></div>
                    <form onSubmit={onSubmit} method="post" encType="multipart/form-data" style={{ height: "100%", overflow: "auto" }} className="modal-padding" id="myForm">
                        <div dangerouslySetInnerHTML={{ __html: state.view }} />
                        <div className="modal-footer">
                            <div className="text-center mx-auto modal-footer-mt">
                                <button type="submit" className="custom-button custom-button-font section-bg-color between-button-margin" value={state.requestID}>Confirm</button>
                                <CancelButton modalKey={ModalKeys.DELETE_ITEM}  />
                            </div>
                        </div>

                    </form>
                </div>
            </div>
        </div>
    );

}

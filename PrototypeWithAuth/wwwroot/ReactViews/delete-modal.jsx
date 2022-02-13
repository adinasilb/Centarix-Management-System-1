import React, { useState, useEffect } from 'react';
import ReactDOM from 'react-dom';
import {
    useLocation
} from 'react-router-dom';
import CloseButton, { CancelButton } from './close-button.jsx'
import { openModal } from './modal-functions.jsx'

export default function DeleteModal() {
    const location = useLocation();

    const [state, setState] = useState({ isLoaded: false, view: "" });

    useEffect(() => {
        openModal("modal");
    });

    if (state.isLoaded) {
        console.log("isloaded")
        return ReactDOM.createPortal(
            <div className="modal modal-view" id="myModal" role="dialog" aria-labelledby="Request" data-backdrop="false">

                <div className="modal-dialog-centered modal-lg mx-auto " role="document" style={{ maxHeight: "100%", overflowY: "auto" }}>

                    <div className="modal-content d-inline-block modal-border-radius modal-box-shadow ">
                        <div className="close-button"><CloseButton /></div>
                        <form action="" method="post" encType="multipart/form-data" style={{ height: "100%", overflow: "auto" }} className="modal-padding" id="myForm">
                            <div dangerouslySetInnerHTML={{ __html: state.view }} />
                            <div className="modal-footer">
                                <div className="text-center mx-auto modal-footer-mt">
                                    <button type="submit" className="custom-button custom-button-font section-bg-color between-button-margin submit-delete" value={location.state.requestID}>Confirm</button>
                                    <CancelButton />
                                </div>
                            </div>

                        </form>
                    </div>
                </div>
            </div>,
            document.getElementsByClassName("delete-item")[0]
        )
    }
    else {
        console.log("in is not loaded")
        fetch("/Requests/_DeleteModal?id=" + location.state.requestID, {
            method: "GET"
        })
            .then((response) => { return response.text(); })
            .then(result => {
                setState({ view: result, isLoaded: true });
            });
        return null;
    }
}

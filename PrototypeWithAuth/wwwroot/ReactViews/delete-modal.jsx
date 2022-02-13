import React, { useState, useEffect } from 'react';
import ReactDOM from 'react-dom';
import {
    useLocation
} from 'react-router-dom';
import CloseButton, { CancelButton } from './Utility/close-button.jsx'
import { openModal } from './Utility/modal-functions.jsx'

export default function DeleteModal() {
    const location = useLocation();

    const [state, setState] = useState({ isLoaded: false, view: "" });

    useEffect(() => {
        openModal("modal");
    });

    onSubmit = (e) => {
        e.preventDefault();
        console.log("submit delete");
        document.getElementById("loading").style.display = "block";
        //var PageType = $('#masterPageType').val();
        var sidebarType = $('#masterSidebarType').val();
        var viewClass = "._IndexTableData";
        if (sidebarType == "Quotes" || sidebarType == "Orders" || sidebarType == "Cart") {
            viewClass = "._IndexTableDataByVendor";
        }
        var formdata = new FormData($("#myForm")[0])
        $.fn.ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/DeleteModal", viewClass, "POST", formdata, "delete-item");
        return false;
    }

    if (state.isLoaded) {
        console.log("isloaded")
        return ReactDOM.createPortal(
            <div className="modal modal-view" id="myModal" role="dialog" aria-labelledby="Request" >

                <div className="modal-dialog-centered modal-lg mx-auto " role="document" style={{ maxHeight: "100%", overflowY: "auto" }}>

                    <div className="modal-content d-inline-block modal-border-radius modal-box-shadow ">
                        <div className="close-button"><CloseButton /></div>
                        <form action="" method="post" encType="multipart/form-data" style={{ height: "100%", overflow: "auto" }} className="modal-padding" id="myForm">
                            <div dangerouslySetInnerHTML={{ __html: state.view }} />
                            <div className="modal-footer">
                                <div className="text-center mx-auto modal-footer-mt">
                                    <button type="submit" onSubmit={onSubmit} className="custom-button custom-button-font section-bg-color between-button-margin" value={location.state.requestID}>Confirm</button>
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

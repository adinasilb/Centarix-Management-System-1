import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { ajaxPartialIndexTable, getRequestIndexString } from '../Utility/root-function.jsx'
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import GlobalModal from '../Utility/global-modal.jsx';
import { MDBSelect, MDBSelectInput, MDBSelectOptions, MDBSelectOption } from 'mdbreact';

export default function ShareModal(props) {
    const dispatch = useDispatch();
    const location = useLocation();
    const [state, setState] = useState({ viewModel: null, requestID: location.state.ID, modelsEnum : location.state.modelsEnum });
    console.log(location.state)
    useEffect(() => {
        var url = "/Requests/ShareModalJson?id=" + state.requestID + "& modelsEnum=" + state.modelsEnum+"&" + getRequestIndexString();
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
        e.stopPropagation();
        var url = "/" + document.getElementById('masterSectionType')?.value + "/ShareModal";
        document.getElementById("loading").style.display = "block";
        var formData = new FormData(e.target);
        fetch(url, {
            method: "POST",
            body: formData
        }).then(response => response.json())
            .then(result => {
                dispatch(Actions.removeModals(modals));
                document.getElementById("loading").style.display = "none";                

            }).catch(jqxhr => {
                document.querySelectorAll('.error-message').forEach(e => e.classList.add("d-none"));
                document.querySelector('.error-message').innerHTML = jqxhr;
                document.querySelector('.error-message').classList.remove("d-none");
            });
    }

    return (
        <GlobalModal backdrop={props.backdrop} size="" value={state.viewModel?.ID} modalKey={props.modalKey} key={state.viewModel?.ID} header={"Share " + state.viewModel?.ObjectDescription + " With"} >
            <form action="" method="post" className="sharemodal" encType="multipart/form-data" onSubmit={onSubmit} id="myForm">
                <input type="hidden" id="ID" name="ID" value={state.viewModel?.ID} />
                <div className="contaner-fluid p-0">
                    <div className="row ">
                        <div className="col-10 offset-1">
                            <MDBSelect options={state.viewModel?.ApplicationUsers.map((u) => ({ value:u.Value, text:u.Text }))} className="" name="ApplicationUserIDs" id="ApplicationUserIDs" selectAll  multiple >
                        </MDBSelect>
                        </div>
                    </div>
                </div>
            </form>
        </GlobalModal>
    );

}

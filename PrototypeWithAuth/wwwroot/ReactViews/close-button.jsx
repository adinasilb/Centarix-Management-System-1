import React from 'react';
import { useHistory } from "react-router-dom";
import { closeModal } from "./modal-functions.jsx"

export default function CloseButton() {

    const history = useHistory();
    var back = e => {
        e.stopPropagation();
        history.goBack();
        closeModal("delete-item");
    };
    return (<button
        className="close modal-close-padding modal-close-style"
        onClick={back}>
        <span aria-hidden="true">&times;</span>
    </button>
    );
}

export function CancelButton(props) {
    const history = useHistory();
    var back = e => {
        e.stopPropagation();
        history.goBack();
        closeModal("delete-item");
    };
    return (<button type="button" className="custom-cancel custom-button " onClick={back} > Cancel</button >
    );
}
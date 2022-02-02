import React from 'react';
import ReactDOM from 'react-dom';
import { useLocation, useHistory } from "react-router-dom";


export default function CloseButton() {
    const history = useHistory();
    var back = e => {
         e.stopPropagation();
         console.log("go back")
        history.goBack();
        console.log(history.location)
    };

    return (<button
        className="close modal-close-padding modal-close-style"
        onClick={back}>
        <span aria-hidden="true">&times;</span>
    </button>
    );
}
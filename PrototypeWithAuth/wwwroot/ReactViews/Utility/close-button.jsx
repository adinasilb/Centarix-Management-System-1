import React from 'react';
import {  useDispatch } from 'react-redux'
import { removeModal } from '../ReduxRelatedUtils/actions.jsx'

export default function CloseButton(props) {
    var dispatch = useDispatch();
    return (<button
        className="close modal-close-padding modal-close-style"
        onClick={(e) => {
            e.stopPropagation();
            dispatch(removeModal(props.modalKey));           
        }}>
        
        <span aria-hidden="true">&times;</span>
    </button>
    );
}


export function CancelButton(props) {
    var dispatch = useDispatch();
    return (<button type="button" className="custom-cancel custom-button " onClick={(e) => {      
        e.stopPropagation();
        dispatch(removeModal(props.modalKey));
    }} > Cancel</button >
    );
}
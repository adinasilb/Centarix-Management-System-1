﻿import { connect } from 'react-redux';
import React, { useEffect, lazy, Suspense } from 'react';
import { Route, useHistory } from 'react-router-dom';
import DeleteModal from '../Requests/delete-modal.jsx';
import ShareModal from '../Requests/share-modal.jsx';
/*import DocumentsModal from '../Shared/documents-modal.jsx';*/
import DeleteDocumentModal from '../Shared/delete-document-modal.jsx';
import * as Actions from '../ReduxRelatedUtils/actions.jsx'
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import * as Routes from '../Constants/Routes.jsx'
import MoveToListModal from '../Requests/move-to-list-modal.jsx';
import NewListModal from '../Requests/new-list-modal.jsx';
function ModalLoader(props) {
    console.log(props.modals)
    useEffect(() => {
        console.log("props modal key update");
        var keyExists = props.modals.indexOf(props.modalKey) > -1;
        if (!keyExists) {
            props.addModal(props.modalKey);
        }
    }, [props.modalKey, props.uid]);
    const DocumentsModal = lazy(() => import('../Shared/documents-modal.jsx'));
  
    var modalsComponents = [];
    for (var i = 0; i < props.modals?.length; i++) {
        var backdrop = false;
        if (i == 0) {
            backdrop = true;
        }

        switch (props.modals[i]) {
            case ModalKeys.DELETE_ITEM:
                modalsComponents.push(<DeleteModal backdrop={backdrop} key={props.modals[i]} modalKey={props.modals[i]} />)
                break;
            case ModalKeys.SHARE:
                modalsComponents.push(<ShareModal backdrop={backdrop} key={props.modals[i]} modalKey={props.modals[i]} />)
                break;
            case ModalKeys.MOVE_TO_LIST:
                modalsComponents.push(<MoveToListModal backdrop={backdrop} key={props.modals[i]} modalKey={props.modals[i]} />)
                break;
            case ModalKeys.NEW_LIST:
                modalsComponents.push(<NewListModal backdrop={backdrop} key={props.modals[i]} modalKey={props.modals[i]} />)
                break;
            case ModalKeys.DOCUMENTS:
                modalsComponents.push(<Suspense key={props.modals[i]} fallback={<div>Loading...</div>}><DocumentsModal backdrop={backdrop} key={props.modals[i]} modalKey={props.modals[i]} /></Suspense>)
                break;
            case ModalKeys.DELETE_DOCUMENTS:
                modalsComponents.push(<DeleteDocumentModal backdrop={backdrop} key={props.modals[i]} modalKey={props.modals[i]} />)
                break;

        }
    }
   
    return modalsComponents;
 

}
const mapDispatchToProps = dispatch => (
    {
        addModal: (modalKey) => dispatch(Actions.addModal(modalKey))
    }
);

const mapStateToProps = state => {
    console.log("mapstate to props")
    return {
        modals: state.modals
    };
};

export default connect(
    mapStateToProps, mapDispatchToProps
)(ModalLoader)
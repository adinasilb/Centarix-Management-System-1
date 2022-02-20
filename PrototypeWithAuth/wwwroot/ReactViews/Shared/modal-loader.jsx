import { connect } from 'react-redux';
import React, { useEffect } from 'react';
import { Route, useHistory } from 'react-router-dom';
import DeleteModal from '../Requests/delete-modal.jsx';
import * as Actions from '../ReduxRelatedUtils/actions.jsx'
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import * as Routes from '../Constants/Routes.jsx'
function ModalLoader(props) {
    console.log(props.modals)
    useEffect(() => {
        console.log("props modal key update")
            props.addModal(props.modalKey);
    }, [props.modalKey, props.uid]);

  
    var modalsComponents = [];
    for (var i = 0; i < props.modals?.length; i++) {
        switch ( props.modals[i]) {
            case ModalKeys.DELETE_ITEM:
                modalsComponents.push(<DeleteModal key={props.modals[i]} modalKey={props.modals[i]} />)
                break;
            case ModalKeys.REORDER:
                modalsComponents.push(<DeleteModal key={props.modals[i]} modalKey={props.modals[i]}  />)
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
import { connect } from 'react-redux';
import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { removeExtraModalBackDrop } from '../Utility/modal-functions.jsx';
import DeleteModal from '../Requests/delete-modal.jsx';
import * as Actions from '../ReduxRelatedUtils/actions.jsx'

function ModalLoader(props) {
    useEffect(() => {
            props.addModal(props.modalKey);
    }, [props.modalKey]);
    var history = useHistory();
    useEffect(() => {
        removeExtraModalBackDrop(history);
    },[props.modals]);

    var modalsComponents = [];
    for (var i = 0; i < props.modals?.length; i++) {
        switch ( props.modals[i]) {
            case DELETE_ITEM:
                modalsComponents.push(<DeleteModal key={props.modals[i]} />)
                break;
            case REORDER:
                modalsComponents.push(<DeleteModal key={ props.modals[i]} />)
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
    return {
        modals: state.modals
    };
};

export default connect(
    mapStateToProps, mapDispatchToProps
)(ModalLoader)
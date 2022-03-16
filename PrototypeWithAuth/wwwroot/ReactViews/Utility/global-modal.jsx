import { MDBContainer, MDBBtn, MDBModal, MDBModalBody, MDBModalHeader, MDBModalFooter, MDBModalDialog, MDBModalContent, MDBModalTitle } from 'mdb-react-ui-kit';
import { useDispatch, connect } from 'react-redux'
import * as ModalKeys from '../Constants/ModalKeys.jsx'


import { removeModal} from '../ReduxRelatedUtils/actions.jsx'
export default function GlobalModal(props) {
    var dispatch = useDispatch();
    const onClick = (e) => {
        if (props.closeClick) {
            props.closeClick(e)
        }
        else {
            e.stopPropagation();
            dispatch(removeModal(props.modalKey));
        }
    }

    return (
        <MDBContainer>
            <MDBModal staticBackdrop backdrop={props.backdrop} tabIndex="-1" size={props.size} id="myModal" show={true} >
                <MDBModalDialog size={props.size} centered>
                    <MDBModalContent>
                        <MDBModalHeader className="border-0" ><MDBModalTitle className="heading-1">{props.header}</MDBModalTitle>  <MDBBtn className=' modal-close-padding close modal-close-style btn-link no-box-shadow' color="transparent" onClick={onClick}>   <span aria-hidden="true">&times;</span></MDBBtn> </MDBModalHeader>
                <MDBModalBody>
                    {props.children}
                </MDBModalBody>
                {!props.hideModalFooter?
                <MDBModalFooter className=" border-0 justify-content-center">
                    <MDBBtn className="custom-button custom-button-font section-bg-color between-button-margin" value={props.value} type="submit" form={props.modalKey} >Save</MDBBtn>
                    <MDBBtn  color="white" className="custom-cancel custom-button " onClick={onClick}>Cancel</MDBBtn>               
                    </MDBModalFooter>
                            : ''}
                    </MDBModalContent>
                        </MDBModalDialog>
                   
            </MDBModal>
        </MDBContainer>
        )
}
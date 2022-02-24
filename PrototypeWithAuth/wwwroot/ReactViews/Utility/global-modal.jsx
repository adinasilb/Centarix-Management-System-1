import { MDBContainer, MDBBtn, MDBModal, MDBModalBody, MDBModalHeader, MDBModalFooter } from 'mdbreact';
import { useDispatch } from 'react-redux'
import { removeModal} from '../ReduxRelatedUtils/actions.jsx'
export default function GlobalModal(props) {
    var dispatch = useDispatch();
    const onClick = (e) => {
        e.stopPropagation();
        dispatch(removeModal(props.modalKey));
    }

    return (
        <MDBContainer>
            <MDBModal  backdrop={props.backdrop} tabIndex="-1" size={props.size} id="myModal" isOpen={true} centered toggle={onClick}>
                <MDBModalHeader className="border-0" toggle={onClick}> <span className="heading-1">{props.header}</span></MDBModalHeader>
                <MDBModalBody>
                    {props.children}
                </MDBModalBody>
                {!props.hideModalFooter?
                <MDBModalFooter className=" border-0 justify-content-center">
                    <MDBBtn className="custom-button custom-button-font section-bg-color between-button-margin" value={props.value} type="submit" form={props.modalKey} >Save</MDBBtn>
                    <MDBBtn  color="white" className="custom-cancel custom-button " onClick={onClick}>Cancel</MDBBtn>               
                    </MDBModalFooter>
                : ''}
            </MDBModal>
        </MDBContainer>
        )
}
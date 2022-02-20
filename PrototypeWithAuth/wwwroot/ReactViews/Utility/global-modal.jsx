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
            <MDBModal size="lg" id="myModal" isOpen={true} toggle={onClick}>
                <MDBModalHeader className="border-0" toggle={onClick}> <span className="heading-1">{props.header}</span></MDBModalHeader>
                <MDBModalBody>
                    {props.children}
                </MDBModalBody>
                <MDBModalFooter className="justify-content-center border-0">
                    <MDBBtn className="custom-button custom-button-font section-bg-color between-button-margin" value={props.value} type="submit" form="myForm">Save</MDBBtn>
                    <MDBBtn  color="white" className="custom-cancel custom-button " onClick={onClick}>Cancel</MDBBtn>                
                </MDBModalFooter>
            </MDBModal>
        </MDBContainer>
        )
}
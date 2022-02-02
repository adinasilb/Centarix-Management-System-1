import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import CloseButton from './close-button.jsx'
import { openModal } from './modal-functions.jsx'
export default class DeleteModal extends Component {
    state = { isLoaded: false, view: "", showModal:false };

   
    componentWillUnmount() {
        console.log('unmount')
    }
    componentDidUpdate() {
        openModal("modal");       
    }
    componentDidMount() {
/*        if (this.state.isLoaded == true) {*/
            fetch("/Requests/DeleteModal?id=4", {
                method: "GET"
            })
                .then((response) => { return response.text(); })
                .then(result => {
                    this.setState({
                        view: result,
                        isLoaded: true,
                        showModal: true
                    });
                });
      /*  }*/
    }

    render() {
        if (this.state.showModal == true) {
            return ReactDOM.createPortal(<div><div className="close-button"><CloseButton/><div dangerouslySetInnerHTML={{ __html: this.state.view }} /></div></div>,
                document.getElementsByClassName("delete-item")[0]
            );
        }
        else {
            return null;
        }
        
    }
}

//if (typeof window !== 'undefined') {
//    ReactDOM.render(<DeleteModal />, document.getElementById("deleteItem"));
//}

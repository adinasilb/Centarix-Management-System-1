import React, { Component } from 'react';
import ReactDOM from 'react-dom';
export default class DeleteModal extends Component {
    state = { isLoaded: false, view: "" };

    componentDidUpdate() {
        var myModal = new bootstrap.Modal(document.getElementsByClassName('modal')[0],  {
            backdrop: true,
            keyboard: false,
        });
        myModal.show()
    }
    componentDidMount() {
        fetch("/Requests/DeleteModal?id=4", {
            method: "GET"            
        })
            .then((response) => { return response.text(); })
            .then(result => {
                alert(result);
                this.setState({
                    view: result,
                    isLoaded: true
                });
            });
    }

    render() {
        const { isLoaded } = this.state;

        if (!isLoaded) {
            return <div>Loading...</div>;
        }
        return ReactDOM.createPortal(<div dangerouslySetInnerHTML={{ __html: this.state.view }} />,
        document.getElementById("deleteItem")
        );
    }
}
//if (typeof window !== 'undefined') {
//    ReactDOM.render(<DeleteModal />, document.getElementById("deleteItem"));
//}

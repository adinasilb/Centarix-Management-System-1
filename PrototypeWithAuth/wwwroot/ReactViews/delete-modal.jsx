import React, { Component } from 'react';
import createPortal  from 'react-dom';
export default class DeleteModal extends Component {
    state = { isLoaded: false, view: "" };

    componentDidMount() {
        fetch("/Requests/DeleteModal?id=4", {
            method: "GET"            
        })
           
            .then(res => {
                this.setState({
                    view: res,
                    isLoaded: true
                });
            });
    }

    render() {
        const { isLoaded } = this.state;

        if (!isLoaded) {
            return <div>Loading...</div>;
        }
        return createPortal(this.state.view,
        document.getElementById("deleteItem")
        );
    }
}
//if (typeof window !== 'undefined') {
//    ReactDOM.render(<DeleteModal />, document.getElementById("deleteItem"));
//}

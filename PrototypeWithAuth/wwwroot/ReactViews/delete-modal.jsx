import React, { Component } from 'react';
import createPortal from 'react-dom';
export default class DeleteModal extends Component {
    state = { isLoaded: false, view: "" };

    htmlDecode(input) {
        var e = document.createElement('div');
        e.innerHTML = input;
        return e.childNodes.length === 0 ? "" : e.childNodes[0].nodeValue;
    }

    componentDidMount() {
        console.log("here")
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
        return createPortal(<div dangerouslySetInnerHTML={{ __html: this.htmlDecode(this.state.view) }} />,
        document.getElementById("deleteItem")
        );
    }
}
//if (typeof window !== 'undefined') {
//    ReactDOM.render(<DeleteModal />, document.getElementById("deleteItem"));
//}

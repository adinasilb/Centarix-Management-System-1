import React from 'react'
import ReactDOM from 'react-dom'
import DeleteModal from './delete-modal.jsx';
export default class OpenModalButton extends React.Component {

    constructor(props) {
        super(props);
        this.handleClick = this.handleClick.bind(this);
    }

    handleClick(){
        console.log("handle click")
           //ReactDOM.render(<DeleteModal/>,
           // document.getElementsByClassName("delete-item")[0]
        //);
    }
    render() {
        return (
            <div>
                <button onClick={this.handleClick} type="button">hi</button>
            </div>
        );
    }
}

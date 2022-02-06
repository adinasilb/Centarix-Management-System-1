import React from 'react'
import ReactDOM from 'react-dom'
import DeleteModal from './delete-modal.jsx';
import { useHistory } from 'react-router-dom';
import { createBrowserHistory } from 'history';

export default class OpenModalButton extends React.Component {

    constructor(props) {
        super(props);
        this.handleClick = this.handleClick.bind(this);
    }
  
    handleClick() {
        const history = createBrowserHistory();
        history.push(this.props.currentUrl);
        ReactDOM.render(<RootComponent history={history} deleteModalShow={ true} />,
            document.getElementsByClassName("modals")[0]
        );
    }
    render() {

        return (
            <div>
                <button onClick={this.handleClick} type="button">hi</button>
            </div>);
    }
}
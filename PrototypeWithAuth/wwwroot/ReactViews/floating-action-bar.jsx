import React from 'react';
import { Link, Router, Route, useHistory, Switch } from 'react-router-dom';

export default class FloatingActionBar extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            value: ""
        };

    }

    render() {
       // let history = useHistory();
        let location = useLocation();
        let background = location.state && location.state.background;
        return (

            <div>
                <Router >
                    <Switch location={background || location}>
                        <Route path="/" component={React} />
                        <Route path="/view/:postId" component={React} />
                    </Switch>
                </Router>
                <ul className="list-group list-group-horizontal">
                    <li className="list-group-item">
                        <Link to="/" ><i className="icon-approve-24px"></i>Order</Link>
                    </li>
                    <li className="list-group-item">
                        <Link to="/" ><i className="icon-description-24px2"></i>Add Quote</Link>
                    </li>
                    <li className="list-group-item">
                        <Link to="/" ><i className="con-insert_drive_file-24px-1"></i>Add File</Link>
                    </li>
                    <li className="list-group-item">
                        <Link to="/" ><i className="icon-delete-24px1"></i>Delete</Link>
                    </li>
                </ul>
                </div>
        );
    }
}
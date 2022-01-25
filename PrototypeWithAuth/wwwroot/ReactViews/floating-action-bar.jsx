import React from 'react';
import {
    Link, Route, Switch, StaticRouter
} from 'react-router-dom';
import DeleteModal from './delete-modal.jsx';

export default class FloatingActionBar extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            value: ""
        };

    }



    render() {

        return (

                    <div className="floating-action-bar">
                <StaticRouter >
                    <Switch  >
                        <Route path="/DeleteModal" render={() => {
                            return (
                                <DeleteModal />)
                        }}
                 />
                    </Switch>
                    <div>
                        
                        <ul className="list-group list-group-horizontal">
                            <li className="list-group-item ">
                                <span>2 items selected</span>
                            </li>
                            <li className="list-group-item ">
                                
                            </li>
                            <li className="list-group-item ">
                                <Link to="/" ><i className="icon-approve-24px"></i>Order</Link>
                            </li>
                            <li className="list-group-item ">
                                <Link to="/" ><i className="icon-description-24px2"></i>Add Quote</Link>
                            </li>
                            <li className="list-group-item ">
                                <Link to="/" ><i className="icon-insert_drive_file-24px-1"></i>Add File</Link>
                            </li>
                            <li className="list-group-item ">
                                <Link to="/DeleteModal" ><i className="icon-delete-24px1"></i>Delete</Link>
                            </li>
                            <li className="list-group-item ">

                            </li>
                            <li className="list-group-item ">
                                <button type="button" className="close ">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </li>
                        </ul>

                      
                    </div>
                </StaticRouter >
                    </div>
        );
    }
}


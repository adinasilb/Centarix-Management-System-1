import React from 'react';
import {
    Link,
    BrowserRouter,
    Route,
    Switch,
    StaticRouter,
    Redirect,
    MemoryRouter,
} from 'react-router-dom';
import DeleteModal from './delete-modal.jsx';
import FloatingActionBar from './floating-action-bar.jsx';

export default class RootComponent extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        const app = (
            <div>

                <div className="two-factor-expired"></div>
                <div className="add-invoice"></div>
                <div className="adjust-payments"></div>
                <div className="confirm-edit"></div>
                <div className="confirm-exit"></div>
                <div className="confirm-email"></div>
                <div className="documents-delete"></div>
                <div className="documents"></div>
                <div className="edit-quote"></div>
                <div className="payments-pay"></div>
                <div className="payments-invoice"></div>
                <div className="received-item"></div>
                <div className="reorder-item"></div>
                <div className="terms"></div>
                <div className="upload-order"></div>
                <div className="upload-quote"></div>
                <div className="step-1"></div>
                <div className="step-2"></div>
                <div className="cart-total"></div>
                <div className="visual-zoom"></div>
                <div className="suspend-user"></div>
                <div className="user-picture"></div>
                <div className="deny-approval"></div>
                <div className="delete-item"></div>
                <div className="history-item"></div>
                <div className="edits"></div>
                <div className="add-location"></div>
                <div className="update-time-worked"></div>
                <div className="off-day"></div>
                <div className="hours-awaiting-approval"></div>
                <div className="share-request"></div>
                <div className="add-material"></div>
                <div className="late-order"></div>
                <div className="add-resources"></div>
                <div className="material-info"></div>
                <div className="resource-notes-modal"></div>
                <div className="share-modal"></div>
                <div className="new-report"></div>
                <div className="confirm-archive"></div>
                <div className="save-report"></div>
                <div className="protocol-details"></div>
                <div className="add-function"></div>
                <div className="add-change-div"></div>
                <div className="add-participant"></div>
                <div className="edit-participant"></div>
                <div className="new-entry"></div>
                <div className="save-bio-test-modal"></div>
                <div className="move-list"></div>
                <div className="new-list"></div>
                <div className="delete-list-request"></div>
                <div className="list-settings"></div>
                <div className="delete-list"></div>
                <div className="save-list-settings"></div>
                <div className="products-warning-modal"></div>
                <div className="vendor-float-modal"></div>
                <div className="right-invalid"></div>
                <div className="unit-warning-modal"></div>
                <FloatingActionBar/>
                <Switch>                    
                    <Route path="/DeleteModal" component={DeleteModal} />
                    <Route
                        path="*"
                        component={({ staticContext }) => {
                            if (staticContext) staticContext.status = 404;

                            return <h1>Not Found :(</h1>;
                        }}
                    />
                </Switch>
            </div>
        );

        if (typeof window === 'undefined') {
            return (
                <StaticRouter
                    context={this.props.context}
                    location={this.props.location}
                >
                    {app}
                </StaticRouter>
            );
        }
        return <MemoryRouter>{app}</MemoryRouter>;
    }
}
import React from 'react';
import {
    Link,
    BrowserRouter,
    Route,
    Switch,
    StaticRouter,
    Redirect,
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
        return <BrowserRouter>{app}</BrowserRouter>;
    }
}
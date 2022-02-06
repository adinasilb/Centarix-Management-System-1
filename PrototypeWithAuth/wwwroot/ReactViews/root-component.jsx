import React from 'react';
import {
    Link,
    BrowserRouter,
    Route,
    Switch,
    StaticRouter,
    Redirect,
    MemoryRouter,
    Router,
    useHistory
} from 'react-router-dom';
import DeleteModal from './delete-modal.jsx';
import _IndexTableData from './index-table-data.jsx';
import FloatingActionBar from './floating-action-bar.jsx';

export default class RootComponent extends React.Component {
    constructor(props) {
        super(props);
       
        this.state = { viewModel: this.props.viewModel, showView: this.props.showView };
    }
    render() {
        console.log(this.state.viewModel)
        const app = (
     
            <div>
    
                <FloatingActionBar showFloatingActionBar={false} />
                <_IndexTableData viewModel={this.state.viewModel} showView={true} />
                <Switch>                    
                    <Route path="/DeleteModal" component={DeleteModal} />
                    <Route path="/_IndexTableData" component={_IndexTableData} />
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
        return <MemoryRouter >{app}</MemoryRouter>;
    }
}

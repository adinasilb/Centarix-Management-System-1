import React, { useEffect } from 'react';
import * as Routes from '../Constants/Routes.jsx'
import PageLoader from './page-loader.jsx'
import { Route, Switch, useHistory, useLocation } from 'react-router-dom';
import FloatingActionBar from '../Requests/floating-action-bar.jsx';

export default function App(props) {
    const location = useLocation();
    const history = useHistory();

    useEffect(() => {
        history.push(props.viewEnum);
    }, [props.viewEnum]);
    var view = props.viewEnum
    return (

        <div>

            <FloatingActionBar showFloatingActionBar={false} />

            <Switch >
                <Route path={Routes.INDEX_TABLE_DATA} render={(props) => <PageLoader {...props} viewModel={props.viewModel} pageNumber={location.state?.pageNumber ?? "1"} viewEnum={view} />} />
                <Route path={Routes.INDEX_TABLE_DATA_BY_VENDOR} render={(props) => <PageLoader {...props} viewModel={props.viewModel} viewEnum={view} />} />
                <Route path={Routes.SETTINGS_INVENTORY} render={(props) => <PageLoader {...props} viewModel={props.viewModel} viewEnum={props.viewEnum} />} />
            </Switch>

        </div>

    )
}
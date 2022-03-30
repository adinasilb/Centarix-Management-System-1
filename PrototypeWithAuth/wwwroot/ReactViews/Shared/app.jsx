import React, { useEffect } from 'react';
import * as Routes from '../Constants/Routes.jsx'
import PageLoader from './page-loader.jsx'
import { Route, Switch, useHistory, useLocation } from 'react-router-dom';
import FloatingActionBar from '../Requests/floating-action-bar.jsx';

export default function App(props) {
    const location = useLocation();
    const history = useHistory();
    console.log("app")
    useEffect(() => {
        history.push(props.viewEnum);
    }, [props.viewEnum]);
    return (

        <div>           
            <FloatingActionBar showFloatingActionBar={false} />            
            <Switch >
                <Route path={Routes.INDEX_TABLE} render={(innerProps) => <PageLoader {...innerProps} viewModel={props.viewModel} viewEnum={props.viewEnum} />} />
    
                <Route path={Routes.INDEX_TABLE_BY_VENDOR} render={(innerProps) => <PageLoader {...innerProps}  viewModel={props.viewModel}  viewEnum={props.viewEnum} />} />
                <Route path={Routes.SETTINGS_INVENTORY} render={(innerProps) => <PageLoader {...innerProps} viewEnum={props.viewEnum} />} />
                <Route path={Routes.ORDER_TAB} render={(innerProps) => <PageLoader  {...innerProps} viewEnum={props.viewEnum} />} />
                <Route path={Routes.ORDER_EMAIL} render={(innerProps) => <PageLoader  {...innerProps} viewModel={props.viewModel} tempRequestList={props.tempRequestList} viewEnum={props.viewEnum} />} />
            </Switch>

        </div>

    )
}


import React, { lazy, Suspense, useEffect, useState} from 'react';
import { Provider } from 'react-redux';
import { createStore, } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import { Route, Switch, MemoryRouter, useHistory, useLocation } from 'react-router-dom';
import _IndexTableData from '../Requests/index-table-data.jsx';
import _IndexTableDataByVendor from '../Requests/index-table-data-by-vendor.jsx';
import SettingsInventory from "../Requests/settings-inventory.jsx";
import FloatingActionBar from '../Requests/floating-action-bar.jsx';
import reducer from '../ReduxRelatedUtils/reducers.jsx';
import ModalLoader from './modal-loader.jsx';
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import * as Routes from '../Constants/Routes.jsx'
import PageLoader from './page-loader.jsx'

export default function RootComponent(props) {
    const store = createStore(reducer, { viewModel: props.viewModel, modals: [] }, composeWithDevTools());
    const Scripts = lazy(() => import('../scripts.jsx')); 
    function App() {
        const location = useLocation();
        const history = useHistory();
        
        useEffect(() => {
            history.push(props.viewEnum);
        }, [props.viewEnum]);
        var view = props.viewEnum
        return (
            
            <div>
                <Suspense fallback={<div></div>}><Scripts/></Suspense>
                <FloatingActionBar showFloatingActionBar={false} />

                <Switch >
                    <Route path={Routes.INDEX_TABLE_DATA} render={(props) => <PageLoader {...props} viewModel={props.viewModel} pageNumber={location.state?.pageNumber ?? "1"} viewEnum={view} />} />
                    <Route path={Routes.INDEX_TABLE_DATA_BY_VENDOR} render={(props) => <PageLoader {...props} viewModel={props.viewModel} viewEnum={view} />} />
                    <Route path={Routes.SETTINGS_INVENTORY} render={(props) => <PageLoader {...props} viewModel={props.viewModel} viewEnum={props.viewEnum} />} />
                    <Route path={Routes.ORDER_TAB} render={(props) => <PageLoader {...props} viewModel={props.viewModel} viewEnum={props.viewEnum} />} />
                </Switch>
               
                </div>

                )
    }
   

    return (
    
        <Provider store={store}><MemoryRouter ><App/></MemoryRouter></Provider>

    );
}
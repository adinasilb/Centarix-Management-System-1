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
    useHistory,
    useLocation
} from 'react-router-dom';
import _IndexTableData from './index-table-data.jsx';
import _IndexTableDataByVendor from './index-table-data-by-vendor.jsx';
import SettingsInventory from "./Requests/settings-inventory.jsx";
import FloatingActionBar from './floating-action-bar.jsx';
import { Provider, useDispatch, useStore } from 'react-redux';
import { createStore,  } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import reducer from './ReduxRelatedUtils/reducers.jsx';
import ModalLoader from './modal-loader.jsx';
import * as Actions from './ReduxRelatedUtils/actions.jsx'
import * as ModalKeys from './modal-loader.jsx'
import { openModal } from './Utility/modal-functions.jsx';

export default function RootComponent(props) {
    const store = createStore(reducer, { viewModel: props.viewModel, modals: [] }, composeWithDevTools());

    const renderSwitch = () => {
        switch (props.viewEnum) {
            case "IndexTableDataByVendor":
                return (<_IndexTableDataByVendor viewModel={props.viewModel} showView={true} bcColor={props.bcColor} ajaxLink={props.ajaxLink} btnText={props.btnText} sectionClass={props.sectionClass} />);
                break;
            case "IndexTableData":
                return (<_IndexTableData viewModel={props.viewModel} showView={true} bcColor={props.bcColor} />);
                break;
            case "SettingsInventory":
                return (<SettingsInventory viewModel={props.viewModel} showView={true} />);
                break;
        }
    }
    function App() {
        return (
            <div>
         
                <FloatingActionBar showFloatingActionBar={false} />
                {
                    renderSwitch()
                }

                <Switch>
                    <Route path={ModalKeys.DELETE_ITEM_ROUTE} render={(props) => <ModalLoader {...props} modalKey={ModalKeys.DELETE_ITEM} />} />
                    <Route path="/fallback" render={() => { return null;}} />
                    <Route path="/_IndexTableData" component={_IndexTableData} />
                    <Route path="SettingsInventory" component={SettingsInventory} />
                </Switch>
            </div>)
    }
   

    return(
        <Provider store={store}><MemoryRouter ><App/></MemoryRouter></Provider>
    );
}
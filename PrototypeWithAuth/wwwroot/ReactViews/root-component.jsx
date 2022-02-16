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
import _IndexTableDataByVendor from './index-table-data-by-vendor.jsx';
import SettingsInventory from "./Requests/settings-inventory.jsx";
import FloatingActionBar from './floating-action-bar.jsx';
import { Provider } from 'react-redux';
import { createStore } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import reducer from './ReduxRelatedUtils/reducers.jsx'

export default function RootComponent (props) {

    const store = createStore(reducer, { viewModel: props.viewModel }, composeWithDevTools());

    const renderSwitch= () =>{
        console.log(props.viewEnum);
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
    const app = (

        <div>

            <FloatingActionBar showFloatingActionBar={false} />
            {
                renderSwitch()
            }

            <Switch>
                <Route path="/DeleteModal" component={DeleteModal} />
                <Route path="/_IndexTableData" component={_IndexTableData} />
                <Route path="SettingsInventory" component={SettingsInventory} />
            </Switch>
        </div>
    );

    return(
         <Provider store={store}><MemoryRouter >{app}</MemoryRouter></Provider>
    );
}
import React from 'react';
import { Provider } from 'react-redux';
import { createStore, } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import { Route, Switch, MemoryRouter} from 'react-router-dom';
import _IndexTableData from '../Requests/index-table-data.jsx';
import _IndexTableDataByVendor from '../Requests/index-table-data-by-vendor.jsx';
import SettingsInventory from "../Requests/settings-inventory.jsx";
import FloatingActionBar from '../Requests/floating-action-bar.jsx';
import reducer from '../ReduxRelatedUtils/reducers.jsx';
import ModalLoader from './modal-loader.jsx';
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import * as Routes from '../Constants/Routes.jsx'


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
                    <Route path={Routes.DELETE_ITEM} render={(props) => <ModalLoader {...props} modalKey={ModalKeys.DELETE_ITEM} />} />
                    <Route path={Routes.FALLBACK} render={() => { return null;}} />
                    <Route path={Routes.INDEX_TABLE_DATA} component={_IndexTableData} />
                    <Route path={Routes.SETTINGS_INVENTORY} component={SettingsInventory} />
                </Switch>
            </div>)
    }
   

    return(
        <Provider store={store}><MemoryRouter ><App/></MemoryRouter></Provider>
    );
}
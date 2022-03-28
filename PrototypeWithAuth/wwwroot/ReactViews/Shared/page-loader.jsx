import React, { lazy, Suspense } from 'react';

import { Route, Switch, MemoryRouter } from 'react-router-dom';

import _IndexTable from '../Requests/index-table.jsx';
import _IndexTableByVendor from '../Requests/index-table-by-vendor.jsx';
import SettingsInventory from "../Requests/settings-inventory.jsx";
import OrderTab from "../Requests/order-tab.jsx"

import ModalLoader from './modal-loader.jsx';
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import * as Routes from '../Constants/Routes.jsx'

export default function PageLoader(props) {
    console.log("pageloader")
    const renderSwitch = () => {
        console.log(props.viewEnum)
       
        switch (props.viewEnum) {
            case Routes.INDEX_TABLE_BY_VENDOR:
                return (<_IndexTableByVendor key={"indexTableByVendor"} viewModel={props.viewModel}  />);
                break;
            case Routes.INDEX_TABLE:
                return (<_IndexTable viewModel={props.viewModel} key={"indexTable"} />);
                break;
            case Routes.SETTINGS_INVENTORY:
                return (<SettingsInventory key={"settingsInventory"} viewModel={props.viewModel} showView={true} />);
                break;
            case Routes.ORDER_TAB:
                return (<OrderTab key={"orderTab"} viewModel={props.viewModel} />);
                break;
        }

    }

    return (
        <div>
            {renderSwitch()}
            
            <Switch>
                <Route exact path={props.match.url + Routes.DELETE_ITEM} exact render={(props) => <ModalLoader  {...props} key={"modalloader"} modalKey={ModalKeys.DELETE_ITEM} uid={props.location.key} />}  />
                <Route exact path={props.match.url + Routes.SHARE} exact render={(props) => <ModalLoader  {...props} key={"modalloader"} modalKey={ModalKeys.SHARE} uid={props.location.key} />} />
                <Route exact path={props.match.url + Routes.MOVE_TO_LIST} exact render={(props) => <ModalLoader   {...props} key={"modalloader"} modalKey={ModalKeys.MOVE_TO_LIST} uid={props.location.key}  />} />
                <Route exact path={props.match.url + Routes.NEW_LIST} exact render={(props) => <ModalLoader   {...props} key={"modalloader"} modalKey={ModalKeys.NEW_LIST} uid={props.location.key} />} />
                <Route exact path={props.match.url + Routes.ORDER_OPERATIONS_MODAL} exact render={(props) => <ModalLoader   {...props} key={"modalloader"} modalKey={ModalKeys.ORDER_OPERATIONS_MODAL} uid={props.location.key} />} />
                <Route exact path={props.match.url + Routes.UPLOAD_QUOTE} exact render={(props) => <ModalLoader   {...props} key={"modalloader"} modalKey={ModalKeys.UPLOAD_QUOTE} uid={props.location.key} />} />
                <Route exact path={props.match.url + Routes.DOCUMENTS} exact render={(props) => <ModalLoader {...props} key={"modalloader"} modalKey={ModalKeys.DOCUMENTS} uid={props.location.key} />} />
                <Route exact path={props.match.url + Routes.DELETE_DOCUMENTS} exact render={(props) => <ModalLoader {...props} key={"modalloader"} modalKey={ModalKeys.DELETE_DOCUMENTS} uid={props.location.key} />} />
                <Route exact path={props.match.url + Routes.TERMS} exact render={(props) => <ModalLoader {...props} key={"modalloader"} modalKey={ModalKeys.TERMS} uid={props.location.key} />} />
                <Route exact path={props.match.url + Routes.CONFIRM_EMAIL_MODAL} exact render={(props) => <ModalLoader {...props} key={"modalloader"} modalKey={ModalKeys.CONFIRM_EMAIL} uid={props.location.key} />} />
            </Switch>
            </div>

        )
  

}
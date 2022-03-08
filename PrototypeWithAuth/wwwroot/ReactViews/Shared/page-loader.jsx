import React, { lazy, Suspense } from 'react';

import { Route, Switch, MemoryRouter } from 'react-router-dom';
import _IndexTableData from '../Requests/index-table-data.jsx';
import _IndexTableDataByVendor from '../Requests/index-table-data-by-vendor.jsx';
import SettingsInventory from "../Requests/settings-inventory.jsx";
import OrderTab from "../Shared/order-tab.jsx"

import ModalLoader from './modal-loader.jsx';
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import * as Routes from '../Constants/Routes.jsx'

export default function PageLoader(props) {

    const renderSwitch = () => {
        console.log(props.viewEnum)
        switch (props.viewEnum) {
            case Routes.INDEX_TABLE_DATA_BY_VENDOR:
                return (<_IndexTableDataByVendor key={"indexTableDataByVendor"} viewModel={props.viewModel} showView={true} bcColor={props.bcColor} ajaxLink={props.ajaxLink} btnText={props.btnText} sectionClass={props.sectionClass} />);
                break;
            case Routes.INDEX_TABLE_DATA:
                return (<_IndexTableData key={"indexTableData"} viewModel={props.viewModel} pageNumber={props.pageNumber} />);
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
                <Route exact path={props.match.url + Routes.ORDER_OPERATIONS_MODAL} exact render={(props) => <ModalLoader   {...props} key={"modalloader"} modalKey={ModalKeys.ORDER_OPERATIONS} uid={props.location.key} />} />
            </Switch>
            </div>

        )
  

}
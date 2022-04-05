import React, { useEffect, useRef, } from 'react';
import { useDispatch, connect } from 'react-redux';
import { ajaxPartialIndexTable } from '../Utility/root-function.jsx'
import IndexFilter from './index-filter.jsx'
import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import { MenuItems } from '../Constants/AppUtility.jsx'


function _IndexTableTabs(props) {
    const dispatch = useDispatch();
    return (      
        <div>
            <div className="item-table">
                <ul className="pl-0">{
                    props.viewModel?.Tabs?.map((t, i) => (
                        <li key={t.TabValue} className={"list-inline-item m-0"}>
                            <div variant="text" className={" new-button" + (props.tabValue == t.TabValue ? " active " : " ")} onClick={() => {
                                dispatch(Actions.setTabValue(t.TabValue));
                            }} ><i className="new-icon icon-centarix-icons-04"></i>  <label className="new-button-text">{t.TabValue}</label> </div>
                        </li>
                    ))
                }
                    {props.navigationInfo.sideBarType != "Search " ?
                        <li className="list-inline-item m-0">
                            <IndexFilter key="IndexFilter" showFilter={props.navigationInfo.sectionType != MenuItems.Accounting} viewModel={props.viewModel.InventoryFilterViewModel} />
                        </li>
                        : ""}
                </ul>
            </div>
   </div>
        )
}

const mapStateToProps = state => {
    return {
        tabValue: state.tabValue,
        navigationInfo: state.navigationInfo
    };
};

export default connect(
    mapStateToProps
)(_IndexTableTabs)
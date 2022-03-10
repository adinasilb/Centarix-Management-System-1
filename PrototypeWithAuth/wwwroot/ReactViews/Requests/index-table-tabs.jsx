import React, { useEffect, useRef, useState } from 'react';
import { useLocation } from 'react-router-dom'
import _IndexTableData from './index-table-data.jsx'
import * as Routes from '../Constants/Routes.jsx'
export default function _IndexTableTabs(props) {

    return (      
        <div>
            <div className="item-table">
                        <ul className="pl-0">
                            {props.Tabs.map(t => (
                                <li className={"list-inline-item new-button m-0" + props.selectedTab == t.TabValue?"active":"" }>
                                    <Link to={{
                                        pathname: Routes.INDEX_TABLE_TABS,
                                        state: { tabValue: t.TabValue, pageNumber: ocation.state?.pageNumber ?? "1" }
                                    }} className=""  data-val="3" href="#"><i className="new-icon icon-centarix-icons-04"></i>  <label className="new-button-text">{ t.TabName}</label> </Link>
                                </li>
                            ))}
                    </ul>
            </div>

        <div>
                <_IndexTableData key={"indexTableData"} pageNumber={location.state?.pageNumber ?? "1"} />
        </div>
   </div>
        )
}

const mapStateToProps = state => {
    console.log("in map state to props")
    return {
        selectedTab: state.selectedTab
    };
};

export default connect(
    mapStateToProps
)(_IndexTableTabs)



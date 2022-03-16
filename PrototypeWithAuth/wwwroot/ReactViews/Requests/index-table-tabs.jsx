import React, { useEffect, useRef, useState } from 'react';
import { useLocation, Link } from 'react-router-dom'
import _IndexTableData from './index-table-data.jsx'
import * as Routes from '../Constants/Routes.jsx'
import { useDispatch, connect } from 'react-redux';
import { ajaxPartialIndexTable } from '../Utility/root-function.jsx'

export default function _IndexTableTabs(props) {
    console.log("indextable tabs")
    const location = useLocation();
    const dispatch = useDispatch();
    const didMount = useRef(false);

    useEffect(() => {
        if (didMount.current) {
            alert("in pagenumber use effect");
            document.getElementById("loading").style.display = "block";
            document.getElementsByClassName('page-number')[0].value = props.pageNumber
            document.getElementsByClassName('tab-name')[0].value = props.selectedTab
            ajaxPartialIndexTable(dispatch, "/Requests/GetIndexTableJson", "GET")
        } else {
            didMount.current = true;
        }

    }, [props.pageNumber, props.selectedTab]);
    return (      
        <div>
            <div className="item-table">
                        <ul className="pl-0">
                            {props.tabs.map((t,i) => (
                                <li key={i } className={"list-inline-item m-0"}>
                                    <Link to={{
                                        pathname: Routes.INDEX_TABLE_TABS,
                                        state: { tabValue: t.TabValue, pageNumber: location.state?.pageNumber ?? "1" }
                                    }} className={" new-button " + (props.selectedTab == t.TabValue ? " active " : " ")} data-val="3" href="#"><i className="new-icon icon-centarix-icons-04"></i>  <label className="new-button-text">{ t.TabName}</label> </Link>
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


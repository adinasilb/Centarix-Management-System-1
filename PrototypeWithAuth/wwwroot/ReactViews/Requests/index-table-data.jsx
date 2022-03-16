import React, { useEffect, useRef, useState} from 'react';
import IndexTableRow from './index-table-row.jsx'
import { Link, useHistory } from 'react-router-dom';
import { useDispatch, connect } from 'react-redux';
import { ajaxPartialIndexTable } from '../Utility/root-function.jsx'

import * as Routes from '../Constants/Routes.jsx'
function _IndexTableData(props) {
    console.log(props.viewModel)

    var viewModel = props.viewModel;
    const dispatch = useDispatch();
    const didMount = useRef(false);
    const history = useHistory();
    useEffect(() => {
        if (didMount.current) {
            alert("in pagenumber use effect");
            document.getElementById("loading").style.display = "block";
            document.getElementsByClassName('page-number')[0].value = props.pageNumber
            ajaxPartialIndexTable(dispatch, "/Requests/GetIndexTableJson", "GET")
        } else {
            didMount.current = true;
        }

    }, [props.pageNumber]);
        return (
            <div>               
                <div style={{ position: "absolute", left: "13rem", top: "6rem", zIndex: "1000" }}><span className="text danger-text error-msg"></span></div>
                <br />
                <div className="">
                    <input type="hidden" id="PageNumber" name="PageNumber" className="page-number" value={viewModel.PageNumber} />
                    <input type="hidden" id="TabName" name="TabName" className="tab-name" value={viewModel.TabName} />
                    <table className="table table-headerspaced table-noheaderlines table-hover ">
                        <tbody className="scroll-tbody">                          
                            {viewModel.PagedList.map((r, i) => (
                                <IndexTableRow key={r.r.RequestID} row={ r}/>

                            ))}
                        </tbody>
                    </table >
                </div >

                <div className="row">
                    <div className="col-12 justify-content-center d-flex">
                        <div className="row">
                            <div className="col-12 justify-content-center d-flex">
                                <div className="pagination-container">
                                    <ul className="pagination">
                                        {viewModel.PageNumbersToShow.map((v, i) => (
                                            <li key={v.Value} className={v.Classes}>
                                                <Link className="page-link" to={{ pathname: history.location.pathname, state: { pageNumber: v.Value } }} > {v.Value}</Link>
                                            </li>
                                                ))
                                        }

                                    </ul>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>);
        
}

const mapStateToProps = state => {
    console.log("in map state to props")
    return {
        viewModel: state.viewModel
    };
};

export default connect(
    mapStateToProps
)(_IndexTableData)
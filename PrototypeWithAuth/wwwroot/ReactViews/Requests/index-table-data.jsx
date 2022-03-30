﻿import React, { useEffect, useRef} from 'react';
import IndexTableRow from './index-table-row.jsx'
import { Link, useHistory } from 'react-router-dom';
import { useDispatch, connect } from 'react-redux';
import { ajaxPartialIndexTable } from '../Utility/root-function.jsx'
import Button from '@mui/material/Button';
import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import { batch } from 'react-redux'

function _IndexTableData(props) {
    var viewModel = props.viewModel;
    const dispatch = useDispatch();
    const didMount = useRef(false);
        return (
            <div key="indexTableDataDiv">               
                <div style={{ position: "absolute", left: "13rem", top: "6rem", zIndex: "1000" }}><span className="text danger-text error-msg"></span></div>
                <br />
                <div className="">

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
                                                <Button size="sm" className="page-link" variant="text" onClick={() => {
                                                    batch(() => {
                                                        dispatch(Actions.setPageNumber(v.Value));
                                                        dispatch(Actions.setReloadIndex(true));
                                                    })
                                            
                                                }} > {v.Value}</Button>
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
    return {
        viewModel: state.viewModel
    };
};

export default connect(
    mapStateToProps
)(_IndexTableData)
import React, { Component } from 'react';
import IndexTableColumn from './index-table-column.jsx'
import { useSelector } from 'react-redux';


export default function _IndexTableData() {
    console.log("in index table data");
    const viewModel = useSelector(state => state.viewModel);

            return (
                /*       return ReactDOM.createPortal(*/
                <div>
                    <div style={{ position: "absolute", left: "13rem", top: "6rem", zIndex: "1000" }}><span className="text danger-text error-msg"></span></div>
                    <br />
                    <div className="">
                        <input type="hidden" id="PageNumber" name="PageNumber" className="page-number" />
                        <table className="table table-headerspaced table-noheaderlines table-hover ">
                            <tbody className="scroll-tbody">
                                {viewModel.PagedList.map((r, i) => (
                                    <tr key={ "tr"+i} className="text-center one-row">
                                        {r.Columns.map((col,i) => (
                                            <IndexTableColumn key={"col"+i} columnData={col}/>
                                        ))}
                                    </tr>
                               ))}
                            </tbody>
                        </table >
                    </div >


                </div>);
        
}

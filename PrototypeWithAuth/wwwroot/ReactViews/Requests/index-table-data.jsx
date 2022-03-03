import React from 'react';
import IndexTableColumn from './index-table-column.jsx'
import { connect } from 'react-redux';
import '@fortawesome/fontawesome-free/css/all.min.css';
import 'bootstrap-css-only/css/bootstrap.min.css';
import 'mdbreact/dist/css/mdb.css';
import 'mdb-react-ui-kit/dist/css/mdb.min.css';
function _IndexTableData(props) {
    var viewModel = props.viewModel;
            return (
                /*       return ReactDOM.createPortal(*/
                <div>
                    <div style={{ position: "absolute", left: "13rem", top: "6rem", zIndex: "1000" }}><span className="text danger-text error-msg"></span></div>
                    <br />
                    <div className="">
                        <input type="hidden" id="PageNumber" name="PageNumber" className="page-number" value={viewModel.PageNumber} />
                        <input type="hidden" id="TabName" name="TabName" className="page-number" value={viewModel.TabName} />
                        <table className="table table-headerspaced table-noheaderlines table-hover ">
                            <tbody className="scroll-tbody">
                                {viewModel.PagedList.map((r, i) => (
                                    <tr key={r.r.RequestID} className="text-center one-row">
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

const mapStateToProps = state => {
    console.log("in map state to props")
    return {
        viewModel: state.viewModel
    };
};

export default connect(
    mapStateToProps
)(_IndexTableData)
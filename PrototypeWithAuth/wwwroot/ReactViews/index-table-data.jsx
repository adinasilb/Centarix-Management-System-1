import React, { Component } from 'react';
import IndexTableColumn from './index-table-column.jsx'
export default class _IndexTableData extends Component {

    constructor(props) {
        super(props);

        this.state = { isLoaded: this.props.viewModel != undefined, viewModel: this.props.viewModel, showView: this.props.showView };
    }

    componentDidMount() {
        //if (this.state.showView && this.state.viewModel == undefined) {
        //    /*        if (this.state.isLoaded == true) {*/
        //    fetch("/Requests/_IndexTableData", {
        //        method: "GET"
        //    })
        //        .then((response) => { return response.text(); })
        //        .then(result => {
        //            this.setState({
        //                viewModel: result,
        //                isLoaded: true
        //            });
        //        });
        //}
    }


    render() {
        if (this.state.isLoaded == true) {
            return (
                /*       return ReactDOM.createPortal(*/
                <div>
                    <div style={{ position: "absolute", left: "13rem", top: "6rem", zIndex: "1000" }}><span className="text danger-text error-msg"></span></div>
                    <br />
                    <div className="">
                        <input type="hidden" id="PageNumber" name="PageNumber" className="page-number" />
                        <table className="table table-headerspaced table-noheaderlines table-hover ">
                            <tbody className="scroll-tbody">
                                {this.state.viewModel.pagedList.map((r, i) => (
                                    <tr key={ "tr"+i} className="text-center one-row">
                                        {r.columns.map((col,i) => (
                                            <IndexTableColumn key={"col"+i} columnData={col}/>
                                        ))}
                                    </tr>
                               ))}
                            </tbody>
                        </table >
                    </div >


                </div>);
            //    ,
            //    document.getElementsByClassName("_IndexTab")[0]
            //);
        }
        else {
            return null;
        }

    }
}

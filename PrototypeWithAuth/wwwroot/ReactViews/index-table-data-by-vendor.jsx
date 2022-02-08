import React, { Component } from 'react';
import IndexTableColumn from './index-table-column.jsx'
export default class _IndexTableDataByVendor extends Component {

    constructor(props) {
        super(props);

        this.state = {
            isLoaded: this.props.viewModel != undefined, viewModel: this.props.viewModel, showView: this.props.showView, bcColor: this.props.bcColor, ajaxLink: this.props.ajaxLink, btnText: this.props.btnText
        };
    }

    getExtraTds = (count) => {
        var tds = [];
        var startColumnsToSkip = 0;
        for (var i = startColumnsToSkip; i < count - 7; i++) {
            tds.push(<td key={"emptyTd" + i}></td >);
        }
        return tds;
    }

    getSumSpan = (requests, currency) => {
        var currencyFormat = "he-IL";
        var sum = requests.map(r => r.totalCost).reduce(function (total, num) {
            return parseFloat(total) + parseFloat(num);
        });
        if (currency == 2) {
            currencyFormat = "en-US";
            sum = requests.map(r => r.totalCost / r.exchangeRate).reduce(function (total, num) {
                return parseFloat(total) + parseFloat(num);
            });
        }
        return (<span className="text font-weight-bold">{"Total: " + sum}</span>)
    }


    render() {
        if (this.state.isLoaded == true) {
            console.log(this.state.viewModel)

            return (
                this.state.viewModel.requestsByVendor.map((rbv, i) => (
                    <div key={"requestsByVendor" + i}>
                        <table key={"vendor" + i} className="table table-headerspaced  table-borderless table-hover mb-5 ">
                            <tbody>
                                <tr className="text-dark border-0 no-hover h-50">
                                    <td className="p-0" rowSpan="2" width="14%"><span className="heading-1 supplierName" value={rbv[0].vendor.vendorID}>{rbv[0].vendor.vendorEnName}</span></td>
                                    <td className="border-bottom"></td>
                                </tr>
                                <tr className="border-0 no-hover h-50" style={{ lineHeight: "50%" }}>
                                    <td></td>
                                </tr>
                            </tbody>
                        </table>

                        <table className="table table-headerspaced table-borderless table-hover mb-5 item-table">
                            <tbody className="@sectionClass">
                                <tr className="border-0 d-none currency-warning"><td colSpan="5"><span className="text-danger-centarix">you can only select items with the same currency</span></td></tr>
                                <tr className="border-0 d-none vendor-warning"><td colSpan="5"><span className="text-danger-centarix">you can only select items from the same vendor</span></td></tr>
                                {
                                    rbv.map((row, i) => {
                                        <tr key={"row" + i} class="text-center inv-list-item">
                                            {r.columns.map((col, i) => (
                                                <IndexTableColumn key={"col" + i} columnData={col} vendorID={rbv[0].vendor.vendorID} />
                                            ))}
                                        </tr>
                                    })
                                }
                                <tr className="border-0 mb-5 no-hover">
                                    <td colSpan="3">
                                        <button className={"float-left " + this.state.bcColor + " custom-button-font custom-button px-2rem hidden button-for-selected-items " + this.state.ajaxLink} type={this.state.viewModel.sidebarType}>{this.state.btnText}</button>
                                    </td>
                                    {this.getExtraTds(rbv[0].columns.length)}

                                    <td className="text-center">
                                        <span className="text font-weight-bold">{"Items: " + rbv.length}</span>
                                    </td>
                                    <td className="text-center">
                                        {this.getSumSpan(rbv, this.state.viewModel.pricePopoverViewModel.selectedCurrency)}
                                    </td>
                                    {rbv[0].buttonText != "" ?
                                        <td colSpan="2">
                                            <button className={"custom-button-font custom-button " + rbv[0].buttonClasses + " float-right  cart"} type="button" value={rbv[0].vendor.vendorID}>{rbv[0].buttonText}</button>
                                        </td>
                                        : null}
                                </tr>
                            </tbody>
                        </table>
                    </div>
                ))
            );

        }
        else {
            return null;
        }

    }
}

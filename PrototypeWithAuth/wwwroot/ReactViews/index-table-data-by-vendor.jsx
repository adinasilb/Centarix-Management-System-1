import React, { Component } from 'react';
import IndexTableColumn from './index-table-column.jsx'
export default class _IndexTableDataByVendor extends Component {

    constructor(props) {
        super(props);

        this.state = {
            isLoaded: this.props.viewModel != undefined, viewModel: this.props.viewModel, showView: this.props.showView, bcColor: this.props.bcColor, ajaxLink: this.props.ajaxLink, btnText: this.props.btnText, sectionClass: this.props.sectionClass
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
        var sum = requests.map(r => r.TotalCost).reduce(function (total, num) {
            return (parseFloat(total) + parseFloat(num))
        });
        var currencyString ="₪"
      
        if (currency == "USD") {
            currencyString="$"
            sum = requests.map(r => r.TotalCost / r.ExchangeRate).reduce(function (total, num) {
                return "$"+(parseFloat(total) + parseFloat(num)).toFixed(2);
            });
        }
        sum = currencyString+ sum.toFixed(2)
        return (<span className="text font-weight-bold">{"Total: " + sum}</span>)
    }


    render() {
        console.log(this.state.viewModel.RequestsByVendor)
        if (this.state.isLoaded == true) {
          

            return (
                this.state.viewModel.RequestsByVendor.map((rbv, i) => (
                    <div key={"requestsByVendor" + i}>
                        <table key={"vendor" + i} className="table table-headerspaced  table-borderless table-hover mb-5 ">
                            <tbody>
                                <tr className="text-dark border-0 no-hover h-50">
                                    <td className="p-0" rowSpan="2" width="14%"><span className="heading-1 supplierName" value={rbv[0].Vendor.VendorID}>{rbv[0].Vendor.VendorEnName}</span></td>
                                    <td className="border-bottom"></td>
                                </tr>
                                <tr className="border-0 no-hover h-50" style={{ lineHeight: "50%" }}>
                                    <td></td>
                                </tr>
                            </tbody>
                        </table>

                        <table className="table table-headerspaced table-borderless table-hover mb-5 item-table">
                            <tbody className={this.state.sectionClass}>
                                <tr className="border-0 d-none currency-warning"><td colSpan="5"><span className="text-danger-centarix">you can only select items with the same currency</span></td></tr>
                                <tr className="border-0 d-none vendor-warning"><td colSpan="5"><span className="text-danger-centarix">you can only select items from the same vendor</span></td></tr>
                                {
                                    rbv.map((row, i) => (
                                        <tr key={"row" + i} className="text-center inv-list-item">
                                            {row.Columns.map((col, i) => (
                                                <IndexTableColumn key={"col" + i} columnData={col} vendorID={rbv[0].Vendor.VendorID} sideBar={this.state.viewModel.SidebarType} />
                                            ))}
                                        </tr>
                                   ))
                               }
                                <tr className="border-0 mb-5 no-hover">
                                    <td colSpan="3">
                                        <button className={"float-left " + this.state.bcColor + " custom-button-font custom-button px-2rem hidden button-for-selected-items " + this.state.ajaxLink} type={this.state.viewModel.sidebarType}>{this.state.btnText}</button>
                                    </td>
                                    {this.getExtraTds(rbv[0].Columns.length)}

                                    <td className="text-center">
                                        <span className="text font-weight-bold">{"Items: " + rbv.length}</span>
                                    </td>
                                    <td className="text-center">
                                        {this.getSumSpan(rbv, this.state.viewModel.PricePopoverViewModel.SelectedCurrency)}
                                    </td>
                                    {rbv[0].ButtonText != "" ?
                                        <td colSpan="2">
                                            <button className={"custom-button-font custom-button " + rbv[0].ButtonClasses + "section-bg-color float-right  cart"} type="button" value={rbv[0].Vendor.VendorID}>{rbv[0].ButtonText}</button>
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

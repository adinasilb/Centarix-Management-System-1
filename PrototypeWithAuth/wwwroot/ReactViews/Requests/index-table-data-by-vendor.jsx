import React, { Component } from 'react';
import { connect } from 'react-redux';
import IndexTableColumn from './index-table-column.jsx'

function _IndexTableDataByVendor(props) {
    var viewModel = props.viewModel;

    var getExtraTds = (count) => {
        var tds = [];
        var startColumnsToSkip = 0;
        for (var i = startColumnsToSkip; i < count - 7; i++) {
            tds.push(<td key={"emptyTd" + i}></td >);
        }
        return tds;
    }

    var getSumSpan = (requests, currency) => {
        var sum = requests.map(r => r.TotalCost).reduce(function (total, num) {
            return (parseFloat(total) + parseFloat(num))?.toFixed(2)
        });
        var currencyString ="₪"
      
        if (currency == "USD") {
            currencyString="$"
            sum = requests.map(r => r.TotalCost / r.ExchangeRate).reduce(function (total, num) {
                console.log("total " + total)
                console.log("num " + num)
                return ""+(parseFloat(total) + parseFloat(num))?.toFixed(2);
            });
        }

        return (<span className="text font-weight-bold">{"Total: "+currencyString + sum}</span>)
    }


        return (
            viewModel.RequestsByVendor?.map((rbv, i) => (
                <div key={"requestsByVendor" + i}>
                    <table key={"vendor" + rbv[0].Vendor.VendorID} className="table table-headerspaced  table-borderless table-hover mb-5 ">
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
                        <tbody>
                            <tr className="border-0 d-none currency-warning"><td colSpan="5"><span className="text-danger-centarix">you can only select items with the same currency</span></td></tr>
                            <tr className="border-0 d-none vendor-warning"><td colSpan="5"><span className="text-danger-centarix">you can only select items from the same vendor</span></td></tr>
                            {
                                rbv.map((row, i) => (
                                    <tr key={row.r.RequestID+""+i} className="text-center inv-list-item">
                                        {row.Columns.map((col, i) => (
                                            <IndexTableColumn key={i + (col.ValueWithError.map(v => { return v.String }).join(""))} columnData={col} vendorID={rbv[0].Vendor.VendorID} sideBar={viewModel.SidebarType} />
                                        ))}
                                    </tr>
                                ))
                            }
                            <tr className="border-0 mb-5 no-hover">
                                <td colSpan="3">
                                    <button className="float-left section-bg-color custom-button-font custom-button px-2rem hidden button-for-selected-items " type={viewModel.sidebarType}></button>
                                </td>
                                {getExtraTds(rbv[0].Columns.length)}

                                <td className="text-center">
                                    <span className="text font-weight-bold">{"Items: " + rbv.length}</span>
                                </td>
                                <td className="text-center">
                                    {getSumSpan(rbv, viewModel.PricePopoverViewModel.SelectedCurrency)}
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
            ))??""
        );



    }

const mapStateToProps = state => {
    return {
        viewModel: state.viewModel
    };
};

export default connect(
    mapStateToProps
)(_IndexTableDataByVendor)
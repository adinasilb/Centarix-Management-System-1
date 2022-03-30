import React, { useState, useEffect } from 'react';
import moment from 'moment';
import { useDispatch, connect} from 'react-redux';
import * as AppUtility from '../Constants/AppUtility.jsx'

export default function OrderEmail(props) {
    const [state, setState] = useState({
        viewModel: props.viewModel,
        showPrice: true
        //showPrice: (!(props.navigationInfo.pageType == AppUtility.PageTypeEnum.LabManagementQuotes &&
        //    props.navigationInfo.sidebarType == AppUtility.SidebarEnum.Quotes))
    })
    console.log("order-email")
    var firstRequest = props.tempRequestList.TempRequestViewModels[0].Request ;
    firstRequest.Product = (firstRequest.SingleOrder && firstRequest.SingleOrder) || (firstRequest.RecurringOrder && firstRequest.RecurringOrder) || (firstRequest.StandingOrder && firstRequest.RecurringOrder)

    var currency = props.tempRequestList.TempRequestViewModels[0].Request.Currency == AppUtility.CurrencyEnum.NIS ? '₪' : '$';
    var colspan = 4;
    var requestText = props.navigationInfo.pageType == AppUtility.PageTypeEnum.LabManagementQuotes &&
        props.navigationInfo.sidebarType == AppUtility.SidebarEnum.Quotes ? "We would like to get a price quote for the following items:" : "";

    const priceRows = () => {
        var total = 0;
        var vat = 0;
        var requests = [];
        state.viewModel.Requests?.length > 0 ? requests = state.viewModel.Requests : props.tempRequestList.TempRequestViewModels.map(tempRequest => {
            requests.push(tempRequest.Request)
        })
        var unitPrices = requests.map(request => {
            var cost = request.Cost;
            var vat = request.VAT;
            if (request.currency == AppUtility.CurrencyEnum.NIS) {
                cost = cost / request.ExchangeRate;
                vat = vat / request.ExchangeRate
            }
            total = total + cost;
            request.Product = (request.SingleOrder && request.SingleOrder) || (request.RecurringOrder && request.RecurringOrder) || (request.StandingOrder && request.RecurringOrder)
            return (
                <tr key={request.RequestID}>
                    <td style={{ fontWeight: 500 }}>{request.Product.ProductName}</td>
                    <td>{request.Product.CatalogNumber}</td>
                    <td>{request.Unit}</td>
                    {state.showPrice && <td className="">{currency + "" + ((cost || 0) / Number(request.Unit).toFixed(2))}</td>}
                    {state.showPrice && <td className="">{(request.ParentQuote?.Discount || 0) + "%"}</td>}
                    {state.showPrice && <td className="">{currency + "" + Number(cost).toFixed(2)}</td>}
                </tr>
            )
            request.Product = null;
        })
        var shipping = firstRequest.ParentRequest?.Shipping || 0;
        var showShipping = shipping != 0;
        total = total + shipping;
        if (vat != 0) {
            vat += (shipping * .17);
        }
        var totalWithVat = total + vat;

        return (
            <tbody>
                {unitPrices}
                <tr>
                    <td colSpan={colspan} rowSpan="4" className="borderless-cell">
                        <img src={"/images/css/centarix_logo_signature.png"} alt="signature" width="250" className="mt-5" />
                    </td>
                    {showShipping && <td>Shipping</td>}
                    {showShipping && <td>{currency + "" + shipping}</td>}
                </tr>
                {state.showPrice &&
                    <tr >
                        <td>Total</td>
                    <td>{currency + "" + Number(total).toFixed(2)}</td>
                    </tr>
                }
                {state.showPrice &&
                    <tr>
                        <td>VAT [17%]</td>
                    <td>{currency + "" + Number(vat).toFixed(2)}</td>
                    </tr>
                }
                {state.showPrice &&
                    <tr>
                        <td className="font-weight-bold">Total To Pay</td>
                    <td className="font-weight-bold">{currency + "" + Number(totalWithVat).toFixed(2)}</td>
                    </tr>
                }
            </tbody>
        )
    }

   
    return (
                    <div id="orderEmail">
                        <header className="page-header">
                            <div className="row">
                                <div className="col-md-6 company-logo">
                                    <img src={"/images/centarix_logo-02.png"} width="400" alt="Centarix Logo" />
                                </div>
                                <div className="col-md-3 offset-3 pt-3 company-info">
                                    <span style={{ fontWeight: "700" ,fontSize:"1.5rem"}}>Centarix Biotech</span><br />
                                    <span>ID: 515655215</span><br />
                                    <span>Hamarpeh 3, Jerusalem</span><br />
                                    <span>Tel: 073-7896888</span>
                                </div>
                            </div>
                            <hr />
                        </header>
                        <div className="row">
                            <div className="col-6">
                                {/*{props.navigationInfo.pageType == AppUtility.PageTypeEnum.LabManagementQuotes &&*/}
                                {/*    props.navigationInfo.sidebarType == AppUtility.SidebarEnum.Quotes ?*/}
                                {/*    <span style={{ fontWeight: "500", fontSize: "1rem" }}>Price Quote Request</span>*/}
                                {/*    :*/}
                                    <span style={{ fontWeight: "500" , fontSize: "1rem" }}>{"Purchase Order: " + state.viewModel.ParentRequest?.OrderNumber}</span>
                            </div>
                            <div className="col-3 offset-3 text-center">
                                <span style={{ fontWeight: "500" }} id="date">{moment().format("DD MMM YYYY")}</span>
                            </div>
                        </div>
                       {/* <hr />*/}
                        <div className="row">
                            <div className="col-4">
                                <div className="vendor-info">
                                    <span className="heading-1">{firstRequest.Product?.Vendor.VendorEnName}</span><br />
                                    <span>{"ID: " + firstRequest.Product?.Vendor.VendorBuisnessID}</span><br />
                                    <span>{firstRequest.Product?.Vendor.VendorCity}</span><br />
                                    <span>{"Tel: " + firstRequest.Product?.Vendor.VendorTelephone}</span>
                                </div>
                            </div>
                            {firstRequest.ParentRequest?.NoteToSupplier &&
                                <div className="col-6 offset-2">
                                    <div className="note-message p-4 pl-5">

                                        <div>
                                            <span className="heading-1">Please Note:</span><br />
                                            <span className="text">{state.viewModel.ParentRequest?.NoteToSupplier}</span>
                                        </div>
                                    </div>
                                </div>
                            }
                        </div>
                        {requestText != "" &&
                            <div className="row pl-3">
                                <span className="text">{requestText}</span>
                            </div>
                        }
                        <div className="row">
                            <table className="table confirm-table">
                                <thead style={{ backgroundColor: "#D0001C" ,  color: "#ffffff" }}>
                                    <tr>
                                        <th style={{ width: "30%" }}>Items</th>
                                        <th style={{ width: "15%" }}>SKU</th>
                                        <th style={{ width: "8%" }}>Units</th>
                                        {state.showPrice && <th style={{ width: "16%" }}>Price</th>}
                                        {state.showPrice && <th style={{ width: "15%" }}>Discount</th>}
                                        {state.showPrice && <th style={{ width: "16%" }}>Total</th>}

                                    </tr>
                                </thead>
                                {priceRows()}
                            </table>
                        </div>
                        {state.viewModel.PaymentStatus &&
                            <div className="m-4" style={{ width: "25%" }}>
                                <span className="heading-1">
                                    Payment Terms:
                                </span><br />
                                <span className="text">
                                    {state.viewModel.PaymentStatusID == 5 &&
                                        (firstRequest.Installments + " ")}
                                    {state.viewModel.PaymentStatus.PaymentStatusDescription}
                                </span>{/*<br />*/}
                            </div>
                        }
                    </div>
    );
}

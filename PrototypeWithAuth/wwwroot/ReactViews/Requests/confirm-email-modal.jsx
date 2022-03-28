import React, { useState, useEffect } from 'react';
import moment from 'moment'
import {
    useLocation, Link, useHistory
} from 'react-router-dom';
import GlobalModal from '../Utility/global-modal.jsx';
import 'react-multi-carousel/lib/styles.css';
import { LocalizationProvider, MobileDatePicker } from '@mui/lab';
import DateAdapter from '@mui/lab/AdapterDateFns';
import { TextField, Select, FormControl, Chip, MenuItem, OutlinedInput, Autocomplete } from '@mui/material';
import 'regenerator-runtime/runtime'
import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import { useDispatch, connect } from 'react-redux';
import { useForm, FormProvider } from 'react-hook-form';
import { DevTool } from "@hookform/devtools";
import * as AppUtility from '../Constants/AppUtility.jsx'
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import axios from 'axios'
import { jsonToFormData, combineTwoFormDatas } from '../Utility/root-function.jsx'
import getDate from 'date-fns/esm/getDate/index';


function ConfirmEmailModal(props) {
    const location = useLocation();
    const dispatch = useDispatch();
    const methods = useForm({ mode: 'onChange' });
    const { register, handleSubmit, formState: { errors }, control } = methods;
    const [state, setState] = useState({
        ID: location.state.ID,
        viewModel: {}
    })
    const request = props.tempRequestList.TempRequestViewModels[0].Request;
    useEffect(() => {
        var guid = props.tempRequestList.Guids[props.tempRequestList.Guids.length-1];

        var url = "/Requests/ConfirmEmailModal?parentRequest=" + request.ParentRequest + "&paymentStatusID=" + request.PaymentStatusID+"&guid="+guid;
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                var updatedViewModel = JSON.parse(result);
                setState({ ...state, viewModel: updatedViewModel });
            });

    }, [state.ID]);

    var onSubmit = (data, e) => {

        var viewModelFormData = jsonToFormData(state.viewModel)
        var tempRequestFormData = jsonToFormData({ "TempRequestListViewModel": props.tempRequestList })
        var formData = combineTwoFormDatas(viewModelFormData, tempRequestFormData)

        fetch("/Requests/ConfirmEmailModal", {
            method: "POST",
            body: formData
        })
            .then((response) => {
                if (!response.ok) {
                    return response.text().then(text => {
                        console.log(text)
                        throw new Error(text)
                    })
                }
                else {
                    return response.json();
                }
            })
            .then(result => {
                props.setTempRequestList([]);
                props.removeModals(props.modals);
                props.setReloadIndex(true);

            }).catch(err => {
                console.dir(err)
                setState({
                    ...state,
                    viewModel: {
                        ...state.viewModel,
                        ErrorMessage: err
                    }
                })
            });

    }
    var priceRows = () => {
        var total = 0;
        var vat = 0;
        var showPrice = props.navigationInfo.pageType == AppUtility.PageTypeEnum.LabManagementQuotes &&
            props.navigationInfo.sidebarType == AppUtility.SidebarEnum.Quotes;
        var requests = [];
        state.viewModel.Requests.length > 0 ? requests = state.viewModel.Requests : props.tempRequestList.TempRequestViewModels.map(tempRequest => {
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
            return (
                <tr>
                    <td style="font-weight: 500;">{request.Product.ProductName}</td>
                    <td>{request.Product.CatalogNumber}</td>
                    <td>{request.Unit}</td>
                    {showPrice && <td className="">{currency + "" + ((cost || 0) / request.Unit.ToString("N2"))}</td>}
                    {showPrice && <td className="">{(request.ParentQuote?.Discount || 0) + "%"}</td>}
                    {showPrice && <td className="">{currency + "" + cost.ToString("N2")}</td>}
                </tr>
            )
        })
        var shipping = request.ParentRequest?.Shipping || 0;
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
                    <td colspan={colspan} rowspan="4" className="borderless-cell">
                        <img src="~/images/css/centarix_logo_signature.png" alt="signature" width="250" className="mt-5" />
                    </td>
                    {showShipping && <td>Shipping</td>}
                    {showShipping && <td>{currency + "" + shipping.ToString("N2")}</td>}
                </tr>
                {showPrice &&
                    <tr >
                        <td>Total</td>
                        <td>@currency @total.ToString("N2")</td>
                    </tr>
                }
                {showPrice &&
                    <tr>
                        <td>VAT [17%]</td>
                        <td>{currency + "" + vat.ToString("N2")}</td>
                    </tr>
                }
                {showPrice &&
                    <tr>
                        <td className="font-weight-bold">Total To Pay</td>
                        <td className="font-weight-bold">{currency + "" + totalWithVat.ToString("N2")}</td>
                    </tr>
                }
            </tbody>
        )
    }

    var currency = props.tempRequestList.TempRequestViewModels[0].Request.Currency == AppUtility.CurrencyEnum.NIS ? '₪' : '$';
    var showPrice = true;
    var colspan = 4;
    var requestText = props.navigationInfo.pageType == AppUtility.PageTypeEnum.LabManagementQuotes &&
        props.navigationInfo.sidebarType == AppUtility.SidebarEnum.Quotes ? "We would like to get a price quote for the following items:" : "";
    return (

        <GlobalModal backdrop={props.backdrop} value={state.ID} modalKey={props.modalKey} key={state.ID} size="lg" header="Place Order">
            {state.viewModel.ErrorMessage && <span className="danger-color">{state.viewModel.Error.String}</span>}
            <FormProvider {...methods} >
                <form action="" data-string="" method="post" encType="multipart/form-data" onSubmit={handleSubmit(onSubmit)} id={props.modalKey}>
                    <DevTool control={control} />
                    <div style="padding:1rem 2rem">
                        <header className="page-header">
                            <div className="row">
                                <div className="col-md-6 company-logo">
                                    <br />
                                    <img src="~/images/centarix_logo-02.png" width="400" alt="Centarix Logo" />
                                </div>
                                <div className="col-md-3 offset-3 pt-3 company-info">
                                    <span style="font-weight: 700; font-size:1.5rem;">Centarix Biotech</span><br />
                                    <span>ID: 515655215</span><br />
                                    <span>Hamarpeh 3, Jerusalem</span><br />
                                    <span>Tel: 073-7896888</span>
                                </div>
                            </div>
                            <hr />
                        </header>
                        <div className="row">
                            <div className="col-6">
                                {props.navigationInfo.pageType == AppUtility.PageTypeEnum.LabManagementQuotes &&
                                    props.navigationInfo.sidebarType == AppUtility.SidebarEnum.Quotes ?
                                    <span style="font-weight: 700; font-size:2rem;">Price Quote Request</span>
                                    :
                                    <span style="font-weight: 700; font-size:2rem;">{"Purchase Order: " + state.viewModel.ParentRequest?.OrderNumber}</span>
                                }
                            </div>
                            <div className="col-3 offset-3 text-center">
                                <span style="font-weight:500" id="date">{Date.now()}</span>
                            </div>
                        </div>
                        <hr />
                        <div className="row">
                            <div className="col-4">
                                <div className="vendor-info">
                                    <span className="heading-1">{request.Product?.Vendor.VendorEnName}</span><br />
                                    <span>{"ID: " + request.Product.Vendor.VendorBuisnessID}</span><br />
                                    <span>{request.Product.Vendor.VendorCity}</span><br />
                                    <span>{"Tel: " + request.Product.Vendor.VendorTelephone}</span>
                                </div>
                            </div>
                            <div className="col-6 offset-2">
                                <div className="note-message p-4 pl-5">
                                    {request.ParentRequest?.NoteToSupplier &&
                                        <div>
                                            <span className="heading-1">Please Note:</span><br />
                                            <span className="text">{request.ParentRequest.NoteToSupplier}</span>
                                        </div>

                                    }
                                </div>
                            </div>
                        </div>
                        <br />
                        <div className="row pl-3">
                            <span className="text">{requestText}</span>
                        </div><br />
                        <div className="row">
                            <table className="table confirm-table">
                                <thead style="background-color: #D0001C; color: #ffffff;">
                                    <tr>
                                        <th style="width:30%;">Items</th>
                                        <th style="width:15%;">SKU</th>
                                        <th style="width:8%;">Units</th>
                                        {showPrice && <th style="width:16%;">Price</th>}
                                        {showPrice && <th style="width:15%;">Discount</th>}
                                        {showPrice && <th style="width:16%;">Total</th>}

                                    </tr>
                                </thead>
                                {priceRows}
                            </table>
                        </div>
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        {state.viewModel.Requests.FirstOrDefault().PaymentStatus &&
                            <div className="m-4" style="width:25%;">
                                <span className="heading-1">
                                    Payment Terms:
                                </span><br />
                                <span className="text">
                                    {state.viewModel.PaymentStatusID == 5 &&
                                        (request.Installments + " ")}
                                    {state.viewModel.PaymentStatus.PaymentStatusDescription}
                                </span><br />
                            </div>
                        }
                    </div>
                </form>
            </FormProvider >
        </GlobalModal >
    );
}
const mapDispatchToProps = dispatch => (
    {
        setTempRequestList: (tempRequest) => dispatch(Actions.setTempRequestList(tempRequest)),
        addModal: (modalKey) => dispatch(Actions.addModal(modalKey)),
        removeModals: (modals) => dispatch(Actions.removeModals(modals)),
        setReloadIndex: (reload) => dispatch(Actions.setReloadIndex(reload))
    }
);

const mapStateToProps = state => {
    return {
        tempRequestList: state.tempRequestList.present,
        modals: state.modals,
        navigationInfo: state.navigationInfo
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(ConfirmEmailModal);

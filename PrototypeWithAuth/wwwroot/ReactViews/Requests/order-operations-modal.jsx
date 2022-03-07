
import React, { useState, useEffect } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { ajaxPartialIndexTable, getRequestIndexString } from '../Utility/root-function.jsx'
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import GlobalModal from '../Utility/global-modal.jsx';
import { MDBSelect } from 'mdbreact';

export default function OrderOperationsModal(props) {

    const location = useLocation();
    const [viewModel, setViewModel] = useState({});
    const [ID, setID] = useState(location.state.ID)

    useEffect(() => {
        var url = "/Requests/OrderOperationsModalJson?id=" + getRequestIndexString();
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                setViewModel(JSON.parse(result));
            });

    }, [ID]); 

    var exchangeRate = viewModel?.TempRequestListViewModel?.TempRequestViewModel?.Request?.ExchangeRate;
    var costShekel = viewModel?.TempRequestListViewModel?.TempRequestViewModel?.Request?.Cost;
    var vatShekel = viewModel?.TempRequestListViewModel?.TempRequestViewModel?.Request?.VAT
    var totalShekel = costShekel + vatShekel;
    var costDollar = costShekel / exchangeRate;
    var vatDollar = vatShekel / exchangeRate;
    var totalDollar = costDollar + vatDollar;

    var onSubmit = (e) => {
        e.preventDefault();
        document.getElementById("loading").style.display = "block";
        var formdata = new FormData(e.target);
        var url = '/Requests/OrderOperations'
        return false;
    }

    return (
        <GlobalModal backdrop={true} value={viewModel?.Request?.RequestID} modalKey={props.modalKey} key={viewModel?.Request?.RequestID} size="lg" header={"Place Order"}>
            <form onSubmit={onSubmit} method="post" encType="multipart/form-data" style={{ height: "100%", overflow: "auto" }} className="" id="myForm">
                <div className="error-message text-danger-centarix">
                    {viewModel?.ErrorMessage ?? ""}
                </div>
                <hr />
                <input id="Request_Cost" name="Request.Cost" type="hidden" value={costShekel} />
                <div className="container-fluid p-0">
                    <div className="row">
                        <div className="col-3">
                            <div className="form-group">
                                <label className="control-label">Terms</label>
                                <MDBSelect className="" options={viewModel?.Terms} name="Terms" />
                            </div>
                        </div>
                        <div className="col-3">
                            <div className="form-group">
                                <label className="control-label" style={{ fontWeight: "500" }}>Shipping Cost</label>
                                <input className="form-control-plaintext border-bottom" name="ParentRequest.Shipping" id="ParentRequest_Shipping" />
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-3 input-group">
                            <label className="control-label" style={{ fontWeight: "500" }}>Sum</label>
                            <span class="input-group-text disabled-text">&#x20aa;</span>
                            <input className="form-control-plaintext border-bottom" value={costShekel} disabled />
                        </div>
                        <div className="col-3 input-group">
                            <span class="input-group-text disabled-text">&#36;</span>
                            <input className="form-control-plaintext border-bottom" value={costDollar} disabled />
                        </div>
                        <div className="col-3 input-group">
                            <label className="control-label" style={{ fontWeight: "500" }}>VAT</label>
                            <span class="input-group-text disabled-text">&#x20aa;</span>
                            <input className="form-control-plaintext border-bottom" value={vatShekel} disabled />
                        </div>
                        <div className="col-3 input-group">
                            <span class="input-group-text disabled-text">&#36;</span>
                            <input className="form-control-plaintext border-bottom" value={vatDollar} disabled />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-3 input-group">
                            <label className="control-label" style={{ fontWeight: "500" }}>Total+VAT</label>
                            <span class="input-group-text disabled-text">&#x20aa;</span>
                            <input className="form-control-plaintext border-bottom" value={totalShekel} disabled />
                        </div>
                        <div className="col-3 input-group">
                            <span class="input-group-text disabled">&#36;</span>
                            <input className="form-control-plaintext border-bottom" value={totalDollar} disabled />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-3 input-group">
                            <button className="custom-button custom-button-font section-bg-color">Select Order</button>
                        </div>
                        <div className="col-3 order-file section-filter">
                        </div>
                        <div className="col-3 input-group">
                            <label className="control-label" style={{ fontWeight: "500" }}>Supplier Order Number</label>
                            <input className="form-control-plaintext border-bottom" name="ParentRequest.SupplierOrderNumber" id="ParentRequest_SupplierOrderNumber"/>
                        </div>
                        <div className="col-3 input-group">
                            <label className="control-label" style={{ fontWeight: "500" }}>Order Date</label>
                            <input className="form-control-plaintext border-bottom" name="ParentRequest.OrderDate" id="ParentReqeust_OrderDate"/>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-3 input-group">
                            <button className="custom-button custom-button-font section-bg-color">Select Order</button>
                        </div>
                        <div className="col-3 order-file section-filter">
                            <a href="\@Model.FileStrings[i]" target="_blank" class="mx-3  view-img d-none" disabled>
                                <i class="icon-centarix-icons-09 section-filter " style="font-size:2rem"></i>
                            </a>
                            <a href="" class="d-none delete-document delete-file-perm @deleteDocumentClass @Model.SectionType.ToString() mx-3" disabled>
                                <i style="font-size:2rem;" class="icon-delete-24px documents-delete-icon hover-bold @colorToUse"></i>
                            </a>
                        </div>
                        <div className="col-3 input-group">
                            <label className="control-label" style={{ fontWeight: "500" }}>Supplier Order Number</label>
                            <input className="form-control-plaintext border-bottom" name="ParentRequest.SupplierOrderNumber" id="ParentRequest_SupplierOrderNumber"/>
                        </div>
                        <div className="col-3 input-group">
                            <label className="control-label" style={{ fontWeight: "500" }}>Order Date</label>
                            <input className="form-control-plaintext border-bottom" name="ParentRequest.OrderDate" id="ParentReqeust_OrderDate"/>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-3 input-group">
                            <button className="custom-button custom-button-font section-bg-color">Select Invoice</button>
                        </div>
                        <div className="col-3 invoice-file">
                        </div>
                        <div className="col-3 input-group">
                            <label className="control-label" style={{ fontWeight: "500" }}>Invoice Number</label>
                            <input className="form-control-plaintext border-bottom" name="Invoice.InvoiceNumber" id="Invoice_InvoiceNumber"/>
                        </div>
                        <div className="col-3 input-group">
                            <label className="control-label" style={{ fontWeight: "500" }}>Invoice Date</label>
                            <input className="form-control-plaintext border-bottom" name="Invoice.InvoiceDate" id="Invoice_InvoiceDate"/>
                        </div>
                    </div>
                </div>
            </form>
        </GlobalModal>
    );

}

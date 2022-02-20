import React, { useState } from 'react';
import * as Constant from '../Shared/constants.js'
import CustomFieldButton from './custom-field-button.jsx';
import CustomField from './custom-field.jsx';
import ReactDOM from 'react-dom';
import { MDBBtn } from 'mdbreact';
import { useForm } from "react-hook-form";


export const SettingsForm = (props) => {
    //state = {
    //    customFieldsCount: 0
    //}

    //constructor(props) {
    //    super(props);
    //    this.state = { SettingsForm: this.props.SettingsForm, customFields: [] , validationErrors: []};
    //}

/*    this.setState = ({ SettingsForm: SettingsForm });*/
    //const [customFields, setCustomFields] = useState(1);

    React.useEffect(() => {

    });

    const { register, handleSubmit } = useForm();
    const onSubmit = data => console.log(data);



    var submit = (e) => {
        e.preventDefault();
    }

    var OpenNewCustomField = () => {
        console.log("open new custom field");
        var array = this.customFields;
        //var lastIndex = array.length - 1;
        var Num = array.length;
        array.push(Num);
        var valErrors = this.validationErrors;
        let values = { dict: { 'fieldname': [false, ''], 'datatype': [false, ''] } };
        let Error = [Num, values];
        valErrors.push(Error);
        //this.setState({ ...this.state, customFields: array });
    }

    //var RemoveCustomField = (e) => {
    //    e.preventDefault();
    //    var index = e.target.parentElement.attributes.number.value;
    //    console.log("number: " + index);
    //    var array = this.customFields;
    //    var errors = this.validationErrors;
    //    console.dir(array);
    //    if (index - 1 !== 1) {
    //        array.splice(index, 1);
    //        errors.splice(index, 1);
    //        console.log("spliced");
    //        console.dir(array);
    //        //this.setState({ ...this.state, customfields: array, validationErrors: errors });
    //    }
    //}


    //const customfieldsView = [];

    //for (var i = 0; i < this.customFields.length; i++) {
    //    console.log("in for loop");
    //    customfieldsView.push(<CustomField key={this.customFields[i]} number={i} text={this.customFields[i]} CustomFieldData={this.SettingsForm.CustomFieldData} RemoveCustomField={RemoveCustomField} ValidationErrors={this.validationErrors[i]} />);
    //};
    //console.dir(customfieldsView);

    

    var autoHeight = {
        height: 'auto'
    }
    var fontsize = {
        fontSize: 2 + 'rem'
    }
    var showItem = " active show ";
    var categoryNameRows = props.SettingsForm.Category.Description != null ?
        Math.ceil(parseFloat(props.SettingsForm.Category.Description.length) / 30) : 1;
    var catText = props.SettingsForm.CategoryDescription ?? "Select A Category";
    var subCatText = props.SettingsForm.SubcategoryDescription ?? "Select A Subcategory";
    console.log("categoryNameRows: " + categoryNameRows);
    return (
        <form onSubmit={handleSubmit(onSubmit)} action="" method="get" className="lab-man-form" id="myForm">
            <div className="row top-bar border-bottom text-center text-justify under-row-margin d-flex" >
                <div className="col-12 align-items-center justify-content-center mt-3 mb-3">
                    <span className="align-items-center">
                        <span className="lab-man-color">{props.SettingsForm.ItemCount}</span> Items &nbsp;&nbsp;&nbsp;&nbsp;
                              <span className="lab-man-color">{props.SettingsForm.RequestCount}</span> Requests
                          </span>
                </div>
            </div>
            <div className="new-modal-header modal-line-1-header-with-back modal-sides ch-scrollable pt-0">
                <div className="row modal-title-line justify-content-between under-row-margin container">
                    <div className="col-2">
                        <img src={props.SettingsForm.Category.ImageURL} className="sub-category-image top-modal-image" alt="Alternate Text" width="75" />
                    </div>
                    <div className="col-8">
                        <div className="modal-product-title ml-2" >
                            <textarea asp-for="Category.Description" className="form-control-plaintext border-bottom heading-1" placeholder="(category name)" rows={categoryNameRows} cols="50" maxLength="150"></textarea>
                        </div>
                        <span asp-validation-for="Category.Description" className="text-danger-centarix">
                            { }
                        </span>
                    </div>
                    <div className="col-2">
                        <MDBBtn onClick={submit} type="submit" className=" custom-button custom-button-font lab-man-background-color" value="Save" />
                    </div>

                </div>
                <div className="row">
                    <div className="col-12 px-0">
                        <div className="container-fluid div-tabs pt-0 text-center pl-0">
                            <ul className="nav nav-tabs container-fluid pr-0 border-bottom-0 pl-0 nav-tabs-icons">
                                <li className="nav-item icon"><a data-toggle="tab" href="#details" className="nav-link current-tab next-tab active"><i className="icon-centarix-icons-05 h2"></i><br />Details</a></li>
                                <li className="nav-item icon"><a data-toggle="tab" href="#price" className="nav-link next-tab"><i className="icon-centarix-icons-05 h2"></i><br />Price</a></li>
                                <li className="nav-item icon"><a data-toggle="tab" href="#documents" className="nav-link next-tab"><i className="icon-centarix-icons-05 h2"></i><br />Documents</a></li>
                                <li className="nav-item icon"><a data-toggle="tab" href="#received" className="nav-link next-tab"><i className="icon-centarix-icons-05 h2"></i><br />Received</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div className="settings new-modal-body edit-modal-outer-body "> {/*style="height:33.5rem;"*/}
                <div className="container-fluid edit-modal-body box-shadow orders partial-div"> {/*style="background: #FFF; border: 2px solid #EAEAEB;">*/}
                    <div className="container-fluid div-tabs no-box-shadow p-0">
                        <div className="tab-content">
                            <div id="details" className={"tab-pane fade in" + showItem} value="1">
                                <div className="row">
                                    <div className="col-md-6">
                                        <label className="control-label">Category</label>
                                        <select className="form-control-plaintext border-bottom mdb-select" disabled>
                                            <option value="0">{catText}</option>
                                        </select>
                                    </div>
                                    <div className="col-md-6">
                                        <label className="control-label">Subcategory</label>
                                        <select className="form-control-plaintext border-bottom mdb-select" disabled>
                                            <option value="0">{subCatText}</option>
                                        </select>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <label className="control-label">Vendor</label>
                                        <select className="form-control-plaintext border-bottom mdb-select" disabled>
                                            <option value="0">Select Vendor</option>
                                        </select>
                                    </div>
                                    <div className="col-md-6">
                                        <label className="control-label">Company ID (automatic)</label>
                                        <input className="form-control-plaintext border-bottom" value={Constant.IntPlaceholder} disabled />
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <label className="control-label">Catalogue No.</label>
                                        <input className="form-control-plaintext border-bottom" value={Constant.IntPlaceholder} disabled />
                                    </div>
                                    <div className="col-md-6">
                                        <label className="control-label">URL</label>
                                        <input className="form-control-plaintext border-bottom" value={Constant.UrlPlaceholder} disabled />
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <label className="control-label">Expected Supply Days</label>
                                        <input className="form-control-plaintext border-bottom" value={Constant.IntPlaceholder} disabled />
                                    </div>
                                    <div className="col-md-6">
                                        <label className="control-label">Expected Supply Date</label>
                                        <input className="form-control-plaintext border-bottom" value={Constant.DatePlaceholder} disabled />
                                    </div>
                                </div>
                                <div className="custom-fields-details lab-man-form">
                                    <div className="cf_header hidden">
                                        {/*@{await Html.RenderPartialAsync("_CFHeader.cshtml"); }*/}
                                    </div>
                                </div>
                                <div className="cf_header hidden">
                                    {/*@{await Html.RenderPartialAsync("_BorderCF.cshtml");}*/}
                                </div>
                                <div className="add-custom-fields row">
                                    {/*<input type="button" onClick={this.AddCustomField} className={"custom-button custom-cancel text border-dark " + " details"}*/}
                                    {/*    value="+ Add Custom Field" />*/}
                                    {/*{customfieldsView.map(cf => cf)}*/}
                                    <CustomFieldButton tabName={"details"} /> {/*clickhandler={OpenNewCustomField} />*/}
                                    {/*@{await Html.RenderPartialAsync("_AddCustomFields.cshtml", "details-form");}*/}
                                </div>
                            </div>
                            <div id="price" className="tab-pane fade in" value="1">

                                <div className="row no-mb">
                                    <div className="col-md-8">
                                        <span className="heading-1 modal-tab-name">Price</span><br />
                                        <span className=" large-text text-danger-centarix font-weight-bold" id="price-warning"></span>
                                    </div>
                                    <div className="col-md-4">
                                        <div className="row">
                                            <div className="col-6">
                                                <div className="form-group">
                                                    <label className="control-label">Currency</label>
                                                    <select id="currency" className="mdb-select custom select-dropdown form-control-plaintext disabled">
                                                        <option value="@AppUtility.CurrencyEnum.NIS.ToString()">&#8362; NIS</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div className="col-6">
                                                <div className="form-group">
                                                    <label className="control-label">Exchange Rate</label>
                                                    <input name="Requests[0].ExchangeRate" className="form-control-plaintext border-bottom " id="exchangeRate" />
                                                    <span className="text-danger-centarix"></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div className="custom-fields-price lab-man-form">
                                    <div className="cf_header hidden">
                                        {/*@{await Html.RenderPartialAsync("_CFHeader.cshtml"); }*/}
                                    </div>
                                </div>
                                <div className="cf_header hidden">
                                    {/*@{await Html.RenderPartialAsync("_BorderCF.cshtml");}*/}
                                </div>
                                <div className="add-custom-fields row">
                                    {/*@{await Html.RenderPartialAsync("_AddCustomFields.cshtml", "price-form");}*/}
                                </div>
                            </div>
                            <div id="documents" className="tab-pane fade in" value="1">
                                "Documents"
                                </div>
                            <div id="received" className="tab-pane fade in" value="1">
                                "Received"
                                </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    )
}

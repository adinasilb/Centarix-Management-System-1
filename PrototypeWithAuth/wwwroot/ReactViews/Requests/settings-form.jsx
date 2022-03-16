import React, { useState, setState, useEffect } from 'react';
import * as Constant from '../Shared/constants.js'
import CustomFieldButton from './custom-field-button.jsx';
import { MDBSelect, MDBSelectOptions, MDBSelectOption, MDBSelectInput, MDBInput, MDBContainer } from 'mdbreact';
import ReactDOM from 'react-dom';
import { MDBBtn } from 'mdbreact';
import { useForm, FormProvider, useFormContext, Controller } from "react-hook-form";
import cloneDeep from 'lodash/cloneDeep';
import { DevTool } from "@hookform/devtools";
import { ErrorMessage } from '@hookform/error-message';
import axios from "axios";
//import { useTheme } from '@mui/material/styles';
//import OutlinedInput from '@mui/material/OutlinedInput';
//import InputLabel from '@mui/material/InputLabel';
//import MenuItem from '@mui/material/MenuItem';
//import FormControl from '@mui/material/FormControl';
//import Select from '@mui/material/Select';
//import store from './store'
//import { useSelector } from 'react-redux'




const SettingsForm = (props) => {


    const [remove, setRemove] = useState({ key: false });
    //const [index, setIndex] = useState();
    const [countCF, setCountCF] = useState([]);
    const [customFields, setCustomFields] = useState([]);

    const [fieldValues, setFieldValues] = useState({});

    var detailsCFStyle = {};
    var priceCFStyle = {};
    var documentsCFStyle = {};
    var receivedCFStyle = {};
    if (!(customFields.some(item => item.dict.tabName == "details"))) {
        detailsCFStyle.display = 'none';
    }
    if (!(customFields.some(item => item.dict.tabName == "price"))) {
        priceCFStyle.display = 'none';
    }
    if (!(customFields.some(item => item.dict.tabName == "documents"))) {
        documentsCFStyle.display = 'none';
    }
    if (!(customFields.some(item => item.dict.tabName == "received"))) {
        receivedCFStyle.display = 'none';
    }


    //var customFields = [];

    console.log("RERENDERING");

    const onSubmit = (data) => {
        //console.log(errors);
        alert(JSON.stringify(data));
        var formData = new FormData();
        //alert(newFormData);
        //alert(JSON.stringify(props.SettingsForm));
        alert(JSON.stringify(props.SettingsForm.Category));
        formData.set("test", true);
        formData.append("Category", props.SettingsForm.Category);
        //formData.append("FieldName", refs.fieldName0)
        for (var value of formData.values()) {
            console.log(value);
        }
        const axios = require('axios');
        axios.post('/Requests/SettingsInventory', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
            .then(res => console.log("hello"))
            .catch((error) => { console.error(error) });

        //var xhr = new XMLHttpRequest();
        //xhr.open('post', '/Requests/SettingsInventory', true);
        //xhr.setRequestHeader("Content-type", "multipart/form-data");
        ////xhr.onreadystatechange = function () {
        ////    if (xhr.readyState == XMLHttpRequest.DONE && xhr.status == 200) {
        ////        // Do something on success
        ////    }
        ////}
        //console.dir(formData);
        //alert(JSON.stringify(formData));
        //xhr.send(formData);
    };


    React.useEffect(() => {
        return () => { console.log("in use effect on every render") }
    });

    //React.useEffect(() => {
    //    return () => { console.log("in use effect only once") }
    //}, []);

    //React.useEffect(() => {
    //    console.log("in cf use effect")
    //}, [customFields]);


    //React.useEffect((e) => {
    //    console.log("in outer effect");

    //}, [remove, customFields, index]);

    const methods = useForm({ mode: "onChange" });


    var OpenNewCustomField = (e) => {
        var tabName = e.target.getAttribute('tabName');
        console.log("tabName: " + tabName);
        console.log("open new custom field");
        var newCount = countCF;
        var i = newCount.length > 0 ? newCount[newCount.length - 1] + 1 : 0;
        newCount.push(i);
        setCountCF(newCount);
        var index = newCount.indexOf(parseFloat(i));
        var customField = { dict: { key: newCount[index], number: i, tabName: tabName } };

        var arr = [...customFields];
        arr.push(customField);
        setCustomFields(arr);
    }


    var RemoveCustomField = (e) => {
        e.preventDefault();
        var place = e.target.parentElement.attributes.number.value;
        var index = countCF.indexOf(parseFloat(place));
        //console.log(customFields);

        var array = cloneDeep(customFields);
        array.splice(index, 1);
        var count = countCF;
        count.splice(index, 1);
        setCustomFields(array);
        //customFields = array;
        setCountCF(count);
    }

    var UpdateFieldValue = (e) => {
        e.preventDefault();
        console.log("in update field value");
    }

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
        <>
            <FormProvider {...methods}>
                <form onSubmit={methods.handleSubmit(onSubmit)} action="" method="get" className="lab-man-form" id="myForm">
                    <DevTool control={methods.control} />
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
                                    <textarea asp-for="Category.Description" {...methods.register("categoryName", { required: true, maxLength: 10 })} className="form-control-plaintext border-bottom heading-1" placeholder="(category name)" rows={categoryNameRows} cols="50" maxLength="150"></textarea>
                                </div>
                                <span className="text-danger-centarix">
                                    {methods.formState.errors.categoryName && methods.formState.errors.categoryName.type === "required" && <span>{Constant.Required}</span>}
                                    {methods.formState.errors.categoryName && methods.formState.errors.categoryName.type === "maxLength" && <span>{Constant.MaxLength + "10"}</span>}
                                </span>
                            </div>
                            <div className="col-2">
                                <input type="submit" className="section-bg-color custom-button custom-button-font" value="Save" />
                            </div>

                        </div>
                        <div className="row">
                            <div className="col-12 px-0">
                                <div className="container-fluid div-tabs pt-0 text-center pl-0">
                                    <ul className="nav nav-tabs container-fluid pr-0 border-bottom-0 pl-0 nav-tabs-icons">
                                        <li className="nav-item icon"><a data-toggle="tab" href="#details" className="nav-link current-tab active"><i className="icon-centarix-icons-05 h2"></i><br />Details</a></li>
                                        <li className="nav-item icon"><a data-toggle="tab" href="#price" className="nav-link next-tab" ><i className="icon-centarix-icons-05 h2"></i><br />Price</a></li>
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
                                        <div className="mb-0" style={detailsCFStyle}>
                                            <BorderCF />
                                            <CustomFieldsHeader />
                                        </div>
                                        <div className="">
                                            {customFields.map(cf =>
                                                cf.dict.tabName == "details" &&
                                                <>< CustomField key={cf.dict.key} number={cf.dict.number} CustomFieldData={props.SettingsForm.CustomFieldData} RemoveCustomField={RemoveCustomField} />
                                                </>
                                            )}
                                            <CustomFieldButton tabName={"details"} clickhandler={OpenNewCustomField} />
                                        </div>
                                        <div className="" style={detailsCFStyle}>
                                            <BorderCF />
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
                                                                <option value="">&#8362; NIS</option>
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

                                        <div className="row" id="edit">
                                            <div className="col-md-4  RequestUnitCard ">
                                                <div className="border h-100 pt-4">
                                                    <div className="mx-2rem">
                                                        <div className="row">
                                                            <div className="col-md-4 form-group">
                                                                <label asp-for="Requests[0].Unit" className="control-label"></label>
                                                                <MDBInput type="number" value={Constant.AmountPlaceholder} className="disabled"
                                                                    id="unit" />
                                                                <span asp-validation-for="Requests[0].Unit" className="text-danger-centarix"></span>
                                                            </div>
                                                            <div className="col-md-8 form-group dropdown-select-div">
                                                                <label asp-for="Requests[0].Product.UnitType" className="control-label"></label>
                                                                <MDBSelect asp-for="Request.UnitTypeID" className="form-control-plaintext disabled">
                                                                    <MDBSelectInput selected={Constant.SelectPlaceholder} />
                                                                </MDBSelect>
                                                                <span className="text-danger-centarix"></span>
                                                            </div>
                                                        </div>
                                                        <div className="form-group">
                                                            <label className="control-label price-per-unit-label">Price Per Unit:</label>
                                                            <div className="row">
                                                                <div className="col-md-6">
                                                                    <div className="input-group">
                                                                        <span className="input-group-text border-bottom request-cost-dollar-icon">&#36;</span>
                                                                        <input type="text" className="form-control-plaintext border-bottom disabled" name="unit-price-dollars"
                                                                            value={Constant.SmallIntPlaceholder} readOnly={true}
                                                                            id="unit-price-dollars" />
                                                                    </div>
                                                                </div>
                                                                <div className="col-md-6">
                                                                    <div className="input-group">
                                                                        <span className="input-group-text request-cost-shekel-icon">&#x20aa;</span>
                                                                        <input type="text" className="form-control-plaintext border-bottom  disabled" name="unit-price-shekel"
                                                                            id="unit-price-shekel" value={Constant.SmallIntPlaceholder} readOnly={true} />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-4 pl-0 RequestSubunitCard">
                                                <div className="border h-100 ">
                                                    <button type="button" className="close sub-close  pr-2 pt-2  disabled " aria-label="Close" >
                                                        <span aria-hidden="true">×</span>
                                                    </button>
                                                    <div className="mx-2rem addSubUnitCard mt-4">
                                                        <div className="row " >
                                                            <div className="col-8 offset-2 text-center font-weight-light">
                                                                <input type="button" value="+" className=" addSubUnit btn m-0 p-0 no-box-shadow disabled " />
                                                                <br /><span className="text-capitalize text">add sub unit</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>


                                        <div className="row">
                                            <div className="col-md-2">
                                                <div className="form-group">
                                                    <label className="control-label">Total</label>
                                                    <div className="input-group">
                                                        <span className="input-group-text disabled-text">&#36;</span>
                                                        <input type="text" className="form-control-plaintext border-bottom disabled-text" id="vatInDollars" disabled
                                                            value={Constant.SmallIntPlaceholder} />
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-md-2 vat-info">
                                                <div className="form-group">
                                                    <label className="control-label"></label>
                                                    <div className="input-group">
                                                        <span className="input-group-text disabled-text">&#x20aa;</span>
                                                        <input type="text" value={Constant.SmallIntPlaceholder} className="form-control-plaintext border-bottom disabled-text" id="vat" disabled />
                                                        <span className="text-danger-centarix"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <MDBContainer className="mr-0">
                                                <div className="col-md-2">
                                                    <div className="form-group">
                                                        <label className="control-label"></label>
                                                        <MDBInput checked={true} label={Constant.VAT} type="radio"
                                                            id="radio1" />
                                                    </div>
                                                </div>
                                                <div className="col-md-2">
                                                    <div className="form-group">
                                                        <label className="control-label"></label>
                                                        <MDBInput checked={false} label={Constant.NoVAT} type="radio"
                                                            id="radio1" />
                                                    </div>
                                                </div>
                                            </MDBContainer>
                                        </div>
                                        <div className="row requestPriceQuote ">

                                            <div className="col-md-2 ">
                                                <div className="form-group">
                                                    <label className="control-label">VAT</label>
                                                    <div className="input-group">
                                                        <span className="input-group-text disabled-text">&#36;</span>
                                                        <input type="text" className="form-control-plaintext border-bottom disabled-text " id="sumPlusVat-Dollar" name="sumPlusVat-Dollar" disabled
                                                            value={Constant.SmallIntPlaceholder} />
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-md-2 vat-info @hideVat">
                                                <div className="form-group">
                                                    <label className="control-label"></label>
                                                    <div className="input-group">
                                                        <span className="input-group-text disabled-text">&#x20aa;</span>
                                                        <input type="text" value={Constant.SmallIntPlaceholder} className="form-control-plaintext border-bottom disabled-text" id="sumPlusVat-Shekel" name="sumPlusVat-Shekel" disabled />
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-md-2 ">
                                                <div className="form-group">
                                                    <label className="control-label">Total + VAT</label>
                                                    <div className="input-group">
                                                        <span className="input-group-text disabled-text">&#36;</span>
                                                        <input type="text" className="form-control-plaintext border-bottom disabled-text " id="sumPlusVat-Dollar" name="sumPlusVat-Dollar" disabled
                                                            value={Constant.SmallIntPlaceholder} />
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-md-2 vat-info @hideVat">
                                                <div className="form-group">
                                                    <label className="control-label"></label>
                                                    <div className="input-group">
                                                        <span className="input-group-text disabled-text">&#x20aa;</span>
                                                        <input type="text" value={Constant.SmallIntPlaceholder} className="form-control-plaintext border-bottom disabled-text" id="sumPlusVat-Shekel" name="sumPlusVat-Shekel" disabled />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div className="mb-0" style={priceCFStyle}>
                                            <BorderCF />
                                            <CustomFieldsHeader />
                                        </div>
                                        <div className="">
                                            {customFields.map(cf =>
                                                cf.dict.tabName == "price" && <CustomField key={cf.dict.key} number={cf.dict.number} CustomFieldData={props.SettingsForm.CustomFieldData} RemoveCustomField={RemoveCustomField} UpdateFieldValue={UpdateFieldValue} />
                                            )}
                                            <CustomFieldButton tabName={"price"} clickhandler={OpenNewCustomField} />
                                        </div>
                                        <div className="" style={priceCFStyle}>
                                            <BorderCF />
                                        </div>
                                    </div>
                                    <div id="documents" className="tab-pane fade in" value="1">
                                        <div className="mb-0" style={documentsCFStyle}>
                                            <BorderCF />
                                            <CustomFieldsHeader />
                                        </div>
                                        <div className="">
                                            {customFields.map(cf =>
                                                cf.dict.tabName == "documents" && < CustomField key={cf.dict.key} number={cf.dict.number} CustomFieldData={props.SettingsForm.CustomFieldData} RemoveCustomField={RemoveCustomField} />
                                            )}
                                            <CustomFieldButton tabName={"documents"} clickhandler={OpenNewCustomField} />
                                        </div>
                                        <div className="" style={documentsCFStyle}>
                                            <BorderCF />
                                        </div>
                                    </div>
                                    <div id="received" className="tab-pane fade in" value="1">
                                        <div className="row">
                                            <div className="col-6">
                                                <label className="control-label">Arrival Date</label>
                                                <input className="form-control-plaintext border-bottom" value={Constant.DatePlaceholder2} disabled />
                                            </div>
                                            <div className="col-6">
                                                <label className="control-label">Received By</label>
                                                <input className="form-control-plaintext border-bottom" value={Constant.EmailPlaceholder} disabled />
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-2 d-flex h-100">
                                                <div className="form-check d-flex align-items-end pb-2 mt-3">
                                                    <MDBInput label="Clarify" type="checkbox" className="" readOnly={true} />
                                                </div>
                                            </div>
                                            <div className="col-2 d-flex h-100">
                                                <div className="form-check d-flex align-items-end pb-2 mt-3">
                                                    <MDBInput label="Partial" type="checkbox" className="" readOnly={true} />
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-6">
                                                <label className="control-label">Batch/Lot</label>
                                                <input className="form-control-plaintext border-bottom" value={Constant.IntPlaceholder} disabled />
                                            </div>
                                            <div className="col-6">
                                                <label className="control-label">Received By</label>
                                                <input className="form-control-plaintext border-bottom" value={Constant.DatePlaceholder2} disabled />
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-3 d-flex h-100">
                                                <div className="form-check d-flex align-items-end pb-2 mt-3">
                                                    <MDBInput label="Temporary Location" type="checkbox" className="" readOnly={true} />
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-3">
                                                <label className="control-label">Location Type</label>
                                                <select className="form-control-plaintext border-bottom mdb-select" disabled>
                                                    <option value="0">{Constant.SelectPlaceholder}</option>
                                                </select>
                                            </div>
                                            <div className="col-3">
                                                <label className="control-label">Sub Location</label>
                                                <select className="form-control-plaintext border-bottom mdb-select" disabled>
                                                    <option value="0">{Constant.SelectPlaceholder}</option>
                                                </select>
                                            </div>
                                            <div className="col-3">
                                                <label className="control-label">Sub Location</label>
                                                <select className="form-control-plaintext border-bottom mdb-select" disabled>
                                                    <option value="0">{Constant.SelectPlaceholder}</option>
                                                </select>
                                            </div>
                                            <div className="col-3">
                                                <label className="control-label">Sub Location</label>
                                                <select className="form-control-plaintext border-bottom mdb-select" disabled>
                                                    <option value="0">{Constant.SelectPlaceholder}</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div className="mb-0" style={receivedCFStyle}>
                                            <BorderCF />
                                            <CustomFieldsHeader />
                                        </div>
                                        <div className="">
                                            {customFields.map(cf =>
                                                cf.dict.tabName == "received" && < CustomField key={cf.dict.key} number={cf.dict.number} CustomFieldData={props.SettingsForm.CustomFieldData} RemoveCustomField={RemoveCustomField} />
                                            )}
                                            <CustomFieldButton tabName={"received"} clickhandler={OpenNewCustomField} />
                                        </div>
                                        <div className="" style={receivedCFStyle}>
                                            <BorderCF />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    {/*{*/}
                    {/*    console.log("OUTER FORM LOOP:")*/}
                    {/*    {register.map(x => console.log(x))}*/}
                    {/*}*/}
                </form>
            </FormProvider>
        </>
    )
}

export default SettingsForm;




function CustomField(props) {

    console.log("RENDERING CUSTOM FIELD");
    const { register, formState: { errors }, control } = useFormContext({

    });
    //const Controller = useController({ control });

    const onChangeFieldName = (e) => {
        //setInput(e.currentTarget.value);
    };

    var num = props.number;

    return (
        <>
            <div className="row">
                {console.log("in custom field")}
                {/*<div className="custom-fields row">*/}
                <div className="col-4">
                    <div className="form-group">
                        <label className="control-label" >{"Field Name"}</label>
                        {/*<MDBInput*/}
                        {/*    ref={register("fieldName." + props.number, { required: Constant.Required })}*/}
                        {/*    name="multipleErrorInput"*/}

                        {/*/>*/}
                        <Controller
                            control={control}
                            name={"fieldName" + props.number}
                            render={
                                ({ field }) => <MDBInput {...field}
                                    onChange={props.UpdateFieldValue}
                                    className="form-control-plaintext border-bottom" />
                            }
                            rules={{
                                required: Constant.Required,
                                /*maxLength: { value: 10, message: Constant.MaxLength + "10" }*/
                            }}
                        />
                        <span className="text-danger-centarix"><ErrorMessage
                            errors={errors}
                            name={"fieldName" + props.number}
                        /></span>
                    </div>
                </div>
                <div className="col-4">
                    <div className="form-group">
                        <label className="control-label">Data Type</label>
                        {/*<Controller*/}
                        {/*    control={control}*/}
                        {/*    name={"dataType." + props.number}*/}
                        {/*    render={*/}
                        {/*        ({ field }) =>*/}
                        {/*            //<FormControl>*/}
                        {/*            //    <InputLabel id="demo-multiple-name-label">Name</InputLabel>*/}
                        {/*            //    <Select {...field}*/}
                        {/*            //        labelId="demo-multiple-name-label"*/}
                        {/*            //        id="demo-multiple-name"*/}
                        {/*            //        multiple*/}
                        {/*            //        value={personName}*/}
                        {/*            //        onChange={handleChange}*/}
                        {/*            //        input={<OutlinedInput label="Name" />}*/}
                        {/*            //        MenuProps={MenuProps}*/}
                        {/*            //    >*/}
                        {/*            //    </Select>*/}
                        {/*            //</FormControl>*/}
                        {/*    }*/}
                        {/*    rules={{*/}
                        {/*        required: Constant.Required*/}
                        {/*    }}*/}
                        {/*/>*/}
                        {/*<span className="text-danger-centarix"><ErrorMessage*/}
                        {/*    errors={errors}*/}
                        {/*    name={"dataType." + props.number}*/}
                        {/*/></span>*/}
                    </div>
                </div>
                <div className="col-2 d-flex ml-5 h-100 mt-4"> {/*style="height:auto;">*/}
                    <div className="form-check d-flex align-items-end pb-2 mt-3">{/* style="height:auto;">*/}
                        <MDBInput label="Required" type="checkbox" id={"checkbox" + props.number} className="" />
                        {/*<label className="form-check-label align-items-end"  >Required</label>*/}
                    </div>
                </div>
                <div className="col-1 mt-4 h-auto"> {/*style="height:auto;">*/}
                    <a href="" onClick={props.RemoveCustomField} className="remove-custom-field danger-filter align-items-end pb-2 d-flex mt-3 h-auto icon-font-size" number={props.number} > {/*style="height:auto; font-size:1.75rem;">*/}
                        <i className="icon-delete-24px align-items-end"></i>
                    </a>
                </div>
                {/*</div>*/}
            </div>
        </>
    )
}

function BorderCF() {
    return (
        <div className="col-12 thick-border-top pb-5">
        </div>
    )
}

function CustomFieldsHeader() {
    return (
        <div className="row under-row-margin">
            <div className=" col-12 top-menu lab-man-color mb-2">
                <span>Custom Fields</span>
            </div>
        </div>
    )
}
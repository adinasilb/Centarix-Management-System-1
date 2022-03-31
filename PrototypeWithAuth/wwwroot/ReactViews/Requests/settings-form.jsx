import React, { useState, setState, useEffect } from 'react';
import * as Constant from '../Shared/constants.js'
import CustomFieldButton from './custom-field-button.jsx';
import ReactDOM from 'react-dom';
import { MDBBtn } from 'mdbreact';
import { useForm, FormProvider, useFormContext, Controller } from "react-hook-form";
import cloneDeep from 'lodash/cloneDeep';
import { DevTool } from "@hookform/devtools";
import { ErrorMessage } from '@hookform/error-message';
import axios from "axios";
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import FormLabel from '@mui/material/FormLabel';
import Radio from '@mui/material/Radio';
import RadioGroup from '@mui/material/RadioGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
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
    const [countCF, setCountCF] = useState([]);
    const [customFields, setCustomFields] = useState([]);

    //const [fieldValues, setFieldValues] = useState({});

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


    useEffect(() => {
        return () => {
            console.log("unmounted inner");
        };
    });
    //var customFields = [];

    console.log("RERENDERING");

    const onSubmit = (data) => {
        var formData = new FormData();
        formData.set("DetailsCustomFields", JSON.stringify(data.DetailsCustomFields));
        formData.set("PriceCustomFields", JSON.stringify(data.PriceCustomFields));
        formData.set("DocumentsCustomFields", JSON.stringify(data.DocumentsCustomFields));
        formData.set("ReceivedCustomFields", JSON.stringify(data.ReceivedCustomFields));
        formData.set("Category", JSON.stringify(props.SettingsForm.Category));
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

    };


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

    var showItem = " active show ";
    var category = props?.SettingsForm.Category.Description;
    var categoryNameRows = category != null ?
        Math.ceil(parseFloat(category.length) / 30) : 1;
    var catText = "Select A Category";
    var subCatText = "Select A Subcategory";
    var catName = "";
    if (props?.SettingsForm?.Category?.ParentCategoryEnum == null) {
        catText = props?.SettingsForm?.Category?.ParentCategory?.Description ?? catText;
        subCatText = category ?? subCatText;
        catName = subCatText;
    }
    else {
        catText = category ?? catText;
        catName = catText;
    }
    const [categoryDescription, setCategoryDescription] = useState(catText);
    var handleCategoryDescription = (e) => {
        setCategoryDescription(e.target.value);
    }

    console.log("categoryNameRows: " );
    return (
        <>
            <FormProvider {...methods}>
                <form onSubmit={methods.handleSubmit(onSubmit)} action="" method="get" className="lab-man-form" id="myForm">
                    <DevTool control={methods.control} />
                    <div className="row top-bar border-bottom text-center text-justify under-row-margin d-flex" >
                        <div className="col-12 align-items-center justify-content-center mt-3 mb-3">
                            <span className="align-items-center">
                                <span className="lab-man-color">{props?.SettingsForm?.ItemCount}</span> Items &nbsp;&nbsp;&nbsp;&nbsp;
                              <span className="lab-man-color">{props?.SettingsForm?.RequestCount}</span> Requests
                          </span>
                        </div>
                    </div>
                    <div className="new-modal-header modal-line-1-header-with-back modal-sides ch-scrollable pt-0">
                        <div className="row modal-title-line justify-content-between under-row-margin container">
                            <div className="col-2">
                                <img src={props?.SettingsForm?.Category?.ImageURL??""} className="sub-category-image top-modal-image" alt="Alternate Text" width="75" />
                            </div>
                            <div className="col-8">
                                <div className="modal-product-title ml-2" >
                                    <textarea value={catName}
                                        {...methods.register("categoryName", { required: true })}
                                        className="form-control-plaintext border-bottom heading-1" placeholder="(category name)"
                                        onChange={handleCategoryDescription}
                                        rows={categoryNameRows} cols="50" maxLength="150"></textarea>
                                </div>
                                <span className="text-danger-centarix">
                                    {methods.formState.errors.categoryName && methods.formState.errors.categoryName.type === "required" && <span>{Constant.Required}</span>}
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
                    <div className="settings new-modal-body edit-modal-outer-body "> 
                        <div className="container-fluid edit-modal-body box-shadow orders partial-div"> 
                            <div className="container-fluid div-tabs no-box-shadow p-0">
                                <div className="tab-content">
                                    <div id="details" className={"tab-pane fade in" + showItem} value="1">
                                        <div className="row">
                                            <div className="col-md-6">
                                                <label className="control-label">Category</label>
                                                <Select className="form-control-plaintext border-bottom pt-1"
                                                    readOnly={true}
                                                    disabled={true}
                                                    variant="standard"
                                                    defaultValue="0">
                                                    <MenuItem value="0">{catText}</MenuItem>
                                                </Select>
                                            </div>
                                            <div className="col-md-6">
                                                <label className="control-label">Subcategory</label>
                                                <Select className="form-control-plaintext border-bottom pt-1"
                                                    readOnly={true}
                                                    disabled={true}
                                                    variant="standard"
                                                    defaultValue="0">
                                                    <MenuItem value="0">{subCatText}</MenuItem>
                                                </Select>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <label className="control-label">Vendor</label>
                                                <Select
                                                    readOnly={true}
                                                    disabled={true}
                                                    variant="standard"
                                                    defaultValue="0"
                                                    className="form-control-plaintext border-bottom pt-1"
                                                >
                                                    <MenuItem value="0">
                                                        Select Vendor
                                                                </MenuItem>
                                                </Select>
                                            </div>
                                            <div className="col-md-6">
                                                <label className="control-label">Company ID (automatic)</label>
                                                <input className="form-control-plaintext border-bottom " value={Constant.IntPlaceholder} disabled />
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <label className="control-label">Catalogue No.</label>
                                                <input className="form-control-plaintext border-bottom " value={Constant.IntPlaceholder} disabled />
                                            </div>
                                            <div className="col-md-6">
                                                <label className="control-label">URL</label>
                                                <input className="form-control-plaintext border-bottom " value={Constant.UrlPlaceholder} disabled />
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <label className="control-label">Expected Supply Days</label>
                                                <input className="form-control-plaintext border-bottom " value={Constant.IntPlaceholder} disabled />
                                            </div>
                                            <div className="col-md-6">
                                                <label className="control-label">Expected Supply Date</label>
                                                <input className="form-control-plaintext border-bottom " value={Constant.DatePlaceholder} disabled />
                                            </div>
                                        </div>
                                        <div className="mb-0" style={detailsCFStyle}>
                                            <BorderCF />
                                            <CustomFieldsHeader />
                                        </div>
                                        <div className="">
                                            {customFields.filter(cField => cField.dict.tabName == "details")
                                                .map((cf, i) =>
                                                    <CustomField key={cf.dict.key} number={cf.dict.number} CustomFieldData={props.SettingsForm.CustomFieldData}
                                                        RemoveCustomField={RemoveCustomField} tabName="DetailsCustomFields" tabCount={i} />
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
                                                            <Select
                                                                readOnly={true}
                                                                disabled={true}
                                                                variant="standard"
                                                                defaultValue="NIS"
                                                                className="form-control-plaintext border-bottom pt-1"
                                                            >
                                                                <MenuItem value="NIS">
                                                                    &#8362; NIS
                                                                </MenuItem>
                                                            </Select>
                                                        </div>
                                                    </div>
                                                    <div className="col-6">
                                                        <div className="form-group">
                                                            <label className="control-label">Exchange Rate</label>
                                                            <input name="Requests[0].ExchangeRate" className="form-control-plaintext border-bottom " id="exchangeRate" disabled value={props?.SettingsForm?.ExchangeRate??""} />
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
                                                                <input type="number" value={Constant.AmountPlaceholder} className="form-control-plaintext border-bottom disabled"
                                                                    id="unit" readOnly={true} />
                                                                <span asp-validation-for="Requests[0].Unit" className="text-danger-centarix"></span>
                                                            </div>
                                                            <div className="col-md-8 form-group dropdown-select-div">
                                                                <label asp-for="Requests[0].Product.UnitType" className="control-label"></label>
                                                                <Select
                                                                    readOnly={true}
                                                                    disabled={true}
                                                                    variant="standard"
                                                                    defaultValue={Constant.SelectPlaceholder}
                                                                    className="form-control-plaintext border-bottom"
                                                                >
                                                                    <MenuItem value={Constant.SelectPlaceholder}>
                                                                        {Constant.SelectPlaceholder}
                                                                    </MenuItem>
                                                                </Select>
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
                                                    <button type="button" className="close sub-close pr-4 pt-2  disabled " aria-label="Close" >
                                                        <span aria-hidden="true">×</span>
                                                    </button>
                                                    <div className="mx-2rem addSubUnitCard mt-4">
                                                        <div className="row " >
                                                            <div className="col-8 offset-2 text-center font-weight-light mt-4">
                                                                <input type="button" value="+" className=" addSubUnit btn m-0 p-0 no-box-shadow " disabled />
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
                                                        <span className="input-group-text">&#36;</span>
                                                        <input type="text" className="form-control-plaintext border-bottom" id="vatInDollars" disabled
                                                            value={Constant.SmallIntPlaceholder} />
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-md-2 vat-info">
                                                <div className="form-group">
                                                    <label className="control-label"></label>
                                                    <div className="input-group">
                                                        <span className="input-group-text">&#x20aa;</span>
                                                        <input type="text" value={Constant.SmallIntPlaceholder} className="form-control-plaintext border-bottom disabled-text" id="vat" disabled />
                                                        <span className="text-danger-centarix"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-md-4 mt-3">
                                                <FormControl>
                                                    <FormLabel id="demo-row-radio-buttons-group-label"></FormLabel>
                                                    <RadioGroup
                                                        row
                                                        defaultValue={Constant.VAT}
                                                    >
                                                        <div className="col-md-6">
                                                            <FormControlLabel value={Constant.VAT} control={<Radio />} label={Constant.VAT} disabled />
                                                        </div>
                                                        <div className="col-md-6">
                                                            <FormControlLabel value={Constant.NoVAT} control={<Radio />} label={Constant.NoVAT} disabled />
                                                        </div>
                                                    </RadioGroup>
                                                </FormControl>
                                            </div>
                                        </div>
                                        <div className="row requestPriceQuote ">

                                            <div className="col-md-2 ">
                                                <div className="form-group">
                                                    <label className="control-label">VAT</label>
                                                    <div className="input-group">
                                                        <span className="input-group-text">&#36;</span>
                                                        <input type="text" className="form-control-plaintext border-bottom " id="sumPlusVat-Dollar" name="sumPlusVat-Dollar" disabled
                                                            value={Constant.SmallIntPlaceholder} />
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-md-2 vat-info @hideVat">
                                                <div className="form-group">
                                                    <label className="control-label"></label>
                                                    <div className="input-group">
                                                        <span className="input-group-text">&#x20aa;</span>
                                                        <input type="text" value={Constant.SmallIntPlaceholder} className="form-control-plaintext border-bottom" id="sumPlusVat-Shekel" name="sumPlusVat-Shekel" disabled />
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-md-2 ">
                                                <div className="form-group">
                                                    <label className="control-label">Total + VAT</label>
                                                    <div className="input-group">
                                                        <span className="input-group-text">&#36;</span>
                                                        <input type="text" className="form-control-plaintext border-bottom" id="sumPlusVat-Dollar" name="sumPlusVat-Dollar" disabled
                                                            value={Constant.SmallIntPlaceholder} />
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-md-2 vat-info @hideVat">
                                                <div className="form-group">
                                                    <label className="control-label"></label>
                                                    <div className="input-group">
                                                        <span className="input-group-text">&#x20aa;</span>
                                                        <input type="text" value={Constant.SmallIntPlaceholder} className="form-control-plaintext border-bottom" id="sumPlusVat-Shekel" name="sumPlusVat-Shekel" disabled />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div className="mb-0" style={priceCFStyle}>
                                            <BorderCF />
                                            <CustomFieldsHeader />
                                        </div>
                                        <div className="">
                                            {customFields.filter(cField => cField.dict.tabName == "price")
                                                .map((cf, i) =>
                                                    <CustomField key={cf.dict.key} number={cf.dict.number} CustomFieldData={props.SettingsForm.CustomFieldData}
                                                        RemoveCustomField={RemoveCustomField} tabName="PriceCustomFields" tabCount={i} />
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
                                            {customFields.filter(cField => cField.dict.tabName == "documents")
                                                .map((cf, i) =>
                                                    <CustomField key={cf.dict.key} number={cf.dict.number} CustomFieldData={props.SettingsForm.CustomFieldData}
                                                        RemoveCustomField={RemoveCustomField} tabName="DocumentsCustomFields" tabCount={i} />
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
                                                    <input label="Clarify" type="checkbox" className="" readOnly={true} />
                                                </div>
                                            </div>
                                            <div className="col-2 d-flex h-100">
                                                <div className="form-check d-flex align-items-end pb-2 mt-3">
                                                    <input label="Partial" type="checkbox" className="" readOnly={true} />
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
                                                    <input label="Temporary Location" type="checkbox" className="" readOnly={true} />
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
                                            {customFields.filter(cField => cField.dict.tabName == "received")
                                                .map((cf, i) =>
                                                    <CustomField key={cf.dict.key} number={cf.dict.number} CustomFieldData={props.SettingsForm.CustomFieldData}
                                                        RemoveCustomField={RemoveCustomField} tabName="ReceivedCustomFields" tabCount={i} />
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
                    
                </form>
            </FormProvider>
        </>
    )
}

export default SettingsForm;




function CustomField(props) {
    const { register, formState: { errors }, control } = useFormContext({

    });

    //const Controller = useController({ control });

    const onChangeFieldName = (e) => {
        //setInput(e.currentTarget.value);
    };

    switch (props.tabName) {
        case "Details":
            break;
    }

    return (
        <>
            <div className="row">
                {console.log("in custom field")}
                {/*<div className="custom-fields row">*/}
                <div className="col-4">
                    <div className="form-group">
                        <label className="control-label" >{"Field Name"}</label>
                        {/*<input*/}
                        {/*    ref={register("fieldName." + props.number, { required: Constant.Required })}*/}
                        {/*    name="multipleErrorInput"*/}

                        {/*/>*/}
                        <Controller
                            control={control}
                            name={props.tabName + "[" + props.tabCount + "].FieldName"}
                            render={
                                ({ field }) => <input {...field}
                                    id={props.tabName + "[" + props.tabCount + "]__FieldName"}
                                    className="form-control-plaintext border-bottom" />
                            }
                            rules={{
                                required: Constant.Required
                            }}
                        />
                        <span className="text-danger-centarix"><ErrorMessage
                            errors={errors}
                            name={props.tabName + "[" + props.tabCount + "].FieldName"}
                        /></span>
                    </div>
                </div>
                <div className="col-4">
                    <div className="form-group">
                        <label className="control-label">Data Type</label>

                        <Controller
                            control={control}
                            name={props.tabName + "[" + props.tabCount + "].CustomDataTypeID"}
                            render={
                                ({ field }) =>
                                    <Select {...field}
                                        variant="standard"
                                        defaultValue=""
                                        className="form-control-plaintext border-bottom pt-1"
                                        id={props.tabName + "[" + props.tabCount + "]__CustomDataTypeID"}
                                    >
                                        <MenuItem key="0" value="">
                                            {Constant.SelectPlaceholder}
                                        </MenuItem>
                                        {props.CustomFieldData.map((dataType, i) =>
                                            <MenuItem key={i + 1} value={dataType.CustomDataTypeID}>
                                                {dataType.Name}
                                            </MenuItem>
                                        )}
                                    </Select>
                            }
                            rules={{
                                required: Constant.Required
                            }}
                        />
                        <span className="text-danger-centarix"><ErrorMessage
                            errors={errors}
                            name={props.tabName + "[" + props.tabCount + "].CustomDataTypeID"}
                        /></span>
                    </div>
                </div>
                <div className="col-2 d-flex ml-5 h-100 mt-4"> {/*style="height:auto;">*/}
                    <div className="form-check d-flex align-items-end pb-2">{/* style="height:auto;">*/}
                        <FormControlLabel
                            control={
                                <Controller
                                    name={props.tabName + "[" + props.tabCount + "].Required"}
                                    control={control}
                                    defaultValue={false}
                                    render={({ field: { value, ref, ...field } }) => (
                                        <Checkbox
                                            {...field}
                                            inputRef={ref}
                                            checked={!!value}
                                            disableRipple
                                            id={props.tabName + "[" + props.tabCount + "]_Required"}
                                            className="section-filter"
                                        />
                                    )}
                                />
                            }
                            label="Checkbox"
                            labelPlacement="end"
                        />
                        {/*<Controller*/}
                        {/*    control={control}*/}
                        {/*    name={props.tabName + "[" + props.number + "].Required"}*/}
                        {/*    render={*/}
                        {/*        ({ field }) => <FormControlLabel {...field} control={<Checkbox />} label="Required" value={Required} onChange={SetRequiredID}*/}
                        {/*            id={props.tabName + "[" + props.number + "]__Required"} className="section-filter" />*/}
                        {/*    }*/}
                        {/*    rules={{*/}
                        {/*        required: Constant.Required*/}
                        {/*    }}*/}
                        {/*/>*/}
                        <span className="text-danger-centarix"><ErrorMessage
                            errors={errors}
                            name={props.tabName + "[" + props.tabCount + "].Required"}
                        /></span>

                        {/*<input label="Required" type="checkbox" className="" />*/}
                        {/*<label className="form-check-label align-items-end"  >Required</label>*/}
                    </div>
                </div>
                <div className="col-1 mt-3 h-auto"> {/*style="height:auto;">*/}
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
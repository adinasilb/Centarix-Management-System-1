import React, { useState, setState } from 'react';
import * as Constant from '../Shared/constants.js'
import CustomFieldButton from './custom-field-button.jsx';
import { MDBSelect, MDBSelectOptions, MDBSelectOption, MDBSelectInput, MDBInput } from 'mdbreact';
import ReactDOM from 'react-dom';
import { MDBBtn } from 'mdbreact';
import { useForm, FormProvider, useFormContext } from "react-hook-form";
import reactDebugHooks from 'react-debug-hooks'
import cloneDeep from 'lodash/cloneDeep';
import { DevTool } from "@hookform/devtools";
import axios from "axios";
//import store from './store'
//import { useSelector } from 'react-redux'




function SettingsForm(props) {


    const [remove, setRemove] = useState({ key: false });
    //const [index, setIndex] = useState();
    const [countCF, setCountCF] = useState([]);
    const [customFields, setCustomFields] = useState([]);


    //var customFields = [];

    console.log("RERENDERING");

    const onSubmit = (data) => {
        console.log(errors);
        alert(JSON.stringify(data));
        var formData = new FormData();
        //alert(JSON.stringify(props.SettingsForm));
        alert(JSON.stringify(props.SettingsForm.Category));
        formData.set("test", true);
        formData.append("Category", JSON.stringify(props.SettingsForm.Category));
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

    const { ...methods } = useForm({ mode: "onChange" });
    const { register, control, handleSubmit, watch, formState: { errors } } = useForm({ mode: "onChange" });


    var OpenNewCustomField = () => {
        console.log("open new custom field");
        var newCount = countCF;
        var i = newCount.length > 0 ? newCount[newCount.length - 1] + 1 : 0;
        console.log("i key: " + i);
        newCount.push(i);
        setCountCF(newCount);
        var customField = { dict: { key: newCount[i], number: i } };

        /*array.push(customField);*/
        var arr = [...customFields];
        arr.push(customField);
        console.log("ARRAY:")
        console.log(arr)
        //customFields = arr;
        setCustomFields(arr);
        console.log("CUSTOM FIELD VIEW:")
    }


    var RemoveCustomField = (e) => {
        e.preventDefault();
        //console.log(e);
        var index = e.target.parentElement.attributes.number.value;
        console.log("custom fields in remove:");
        //console.log(customFields);

        var array = cloneDeep(customFields);
        array.splice(index, 1);
        var count = countCF;
        count.splice(index, 1);
        setCustomFields(array);
        //customFields = array;
        setCountCF(count);
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
                                    <textarea asp-for="Category.Description" {...register("categoryName", { required: true, maxLength: 10 })} className="form-control-plaintext border-bottom heading-1" placeholder="(category name)" rows={categoryNameRows} cols="50" maxLength="150"></textarea>
                                </div>
                                <span className="text-danger-centarix">
                                    {errors.categoryName && errors.categoryName.type === "required" && <span>{Constant.Required}</span>}
                                    {errors.categoryName && errors.categoryName.type === "maxLength" && <span>{Constant.MaxLength + "10"}</span>}
                                </span>
                            </div>
                            <div className="col-2">
                                <input type="submit" className=" custom-button custom-button-font lab-man-background-color" value="Save" />
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
                                            {customFields.map(cf => <CustomField key={cf.dict.key} number={cf.dict.number} CustomFieldData={props.SettingsForm.CustomFieldData} RemoveCustomField={RemoveCustomField} />)}

                                            <CustomFieldButton tabName={"details"} clickhandler={OpenNewCustomField} />
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
    const { register, errors } = useFormContext();

    console.log(register);

    return (
        <div className="w-100">
            { console.log("in custom field")}
            <div className="custom-fields row">
                <div className="col-4">
                    <div className="form-group">
                        <label className="control-label" >{"Field Name"}</label>
                        <input className="form-control-plaintext border-bottom" {...register("random", { required: true, maxLength: 10 })} />
                        <span className="text-danger-centarix">
                            {errors.random && <span>{Constant.MaxLength + "10"}</span>}
                        </span>
                    </div>
                </div>
                <div className="col-4">
                    <div className="form-group">
                        <label className="control-label">Data Type</label>
                        <MDBSelect className="form-control-plaintext border-bottom mdb-select" >
                            <MDBSelectInput selected="Choose your option" />
                            <MDBSelectOptions search>
                                {props.CustomFieldData.CustomDataTypes.map((customField, i) => (
                                    <MDBSelectOption value={customField.CustomDataTypeID} key={i}>{customField.Name}</MDBSelectOption>
                                ))}
                            </MDBSelectOptions>
                        </MDBSelect>
                        <span className="centarix-error-style" >{ }</span>
                    </div>
                </div>
                <div className="col-2 d-flex ml-5"> {/*style="height:auto;">*/}
                    <div className="form-check d-flex align-items-end pb-2">{/* style="height:auto;">*/}
                        <MDBInput label="Required" type="checkbox" id={"checkbox" + props.number} />
                        {/*<label className="form-check-label align-items-end"  >Required</label>*/}
                    </div>
                </div>
                <div className="col-1 d-flex"> {/*style="height:auto;">*/}
                    <a href="" onClick={props.RemoveCustomField} className="remove-custom-field danger-filter align-items-end pb-2 d-flex" number={props.number} > {/*style="height:auto; font-size:1.75rem;">*/}
                        <i className="icon-delete-24px align-items-end"></i>
                    </a>
                </div>
            </div>
        </div>
    )
}
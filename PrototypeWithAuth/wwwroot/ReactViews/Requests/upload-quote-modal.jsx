import React, { useState, useEffect } from 'react';
import moment from 'moment'
import {
    useLocation, Link, useHistory
} from 'react-router-dom';
import GlobalModal from '../Utility/global-modal.jsx';
import { FileSelectChange, GetFileString } from "../Utility/document-fuctions.jsx";
import DocumentsCard from '../Shared/documents-card.jsx'
import 'react-multi-carousel/lib/styles.css';
import { LocalizationProvider, MobileDatePicker } from '@mui/lab';
import DateAdapter from '@mui/lab/AdapterDateFns';
import TextField from '@mui/material/TextField';

import 'regenerator-runtime/runtime'
import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import * as ModalKeys from '../Constants/ModalKeys.jsx'

import { useDispatch, connect } from 'react-redux';
import { useForm, FormProvider } from 'react-hook-form';
import { DevTool } from "@hookform/devtools";



function UploadQuoteModal(props) {
    const location = useLocation();
    const dispatch = useDispatch();
    const methods = useForm({ mode: 'onChange' });
    const { register, handleSubmit, formState: { errors }, control } = methods;
    const [state, setState] = useState({
        ID: location.state.ID,
        moveToTerms: location.state.moveToTerms,
        viewModel: {},
        expectedSupplyDate: (moment(new Date()).format('DD MMM YYYY')),
        isReorder: props.isReorder
    })
    console.log("in upload quote")

    useEffect(() => {
        console.log("ID " + state.ID)
        var url = "/Requests/UploadQuoteModal?id=" + state.ID;
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
        console.log("upload form submit")

        if (state.moveToTerms) {
            var newTempRequestList = props.tempRequestList;
            console.log(" update parent quote")
            newTempRequestList.TempRequestViewModels[0].Request.ParentQuote = data.ParentQuote
            newTempRequestList.TempRequestViewModels[0].Request.ParentQuoteID = data.ParentQuote.ParentQuoteID
            newTempRequestList.TempRequestViewModels[0].Request.QuoteStatusID = '4'
            props.setTempRequestList(newTempRequestList)
            props.addModal(ModalKeys.TERMS)
        }
        else {
            var url = "/Requests/UploadQuoteModal";
            var formData = new FormData()
            formData.append(data.ParentQuote)
            console.log(formData)
            fetch(url, {
                method: "POST",
                body: formData
            })
                .then((response) => { return response.json(); })
                .then(result => {

                    //dispatch(Actions.removeModal(ModalKeys.terms modal));
                });
        }


    }

    const handleDateChange = (newValue) => {
        //var newValue = Date.parse(newValue)
        setState(prevstate => ({
            ...prevstate,
            viewModel: {
                ...prevstate.viewModel,
                ParentQuote: { ...prevstate.viewModel.ParentQuote, ExpirationDate: newValue }
            }
        }));
    };

    function handleExpectedSupplyDateChange(newValue) {
        var today = getDate();
        var newDate = Date.parse(newValue);
        var difference = newDate.getTime() - today.getTime()
        var days = Math.ceil(difference / (1000 * 3600 * 24));
        console.log("days "+ days)
        setState(prevstate => ({
            ...prevstate,
            viewModel: {
                ...prevstate.viewModel,
                ExpectedSupplyDays: { ...prevstate.viewModel.ParentQuote, ExpirationDate: days }
            }
        }));
    }

    function changeQuote() {
        var url = "/Requests/ChangeQuote?guid=" + state.ID;
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                var updatedViewModel = JSON.parse(result);
                setState({ ...state, viewModel: updatedViewModel });
            });
    }

    function updateExpectedSupplyDate(e) {
        var today = new Date();
        var updatedDate = moment(today).add(e.target.value, 'days').format('DD MMM YYYY')
        setState({ ...state, expectedSupplyDate: updatedDate })
    }

    return (

        <GlobalModal backdrop={props.backdrop} value={state.ID} modalKey={props.modalKey} key={state.ID} size="lg" header="Upload Quote">
            <FormProvider {...methods} >
            <form action="" data-string="" method="post" encType="multipart/form-data" onSubmit={handleSubmit(onSubmit)} id={props.modalKey}>
                <DevTool control={control} />
                <input type="hidden" id="ParentQuote_ParentQuoteID" defaultValue={state.viewModel.ParentQuote?.ParentQuoteID ?? 0} {...register("ParentQuote.ParentQuoteID")} />
                <div className="container">
                    <div className="row">
                        <div className=" col-4 doc-card-outer-div m-0 text-center h-100">
                            {state.viewModel.DocumentsInfo &&
                                        <DocumentsCard documentsInfo={state.viewModel.DocumentsInfo} fileRequired={true} isEditable={true} modalType={'Summary'} />

                            }
                        </div>
                        <div className="col-8 pl-5 pt-4">
                            <div className="row">

                                <div className=" col-md-6">
                                    <div className="form-group ">
                                        <label className=" control-label m-0 mt-2" style={{ width: "100%" }}>Quote Number</label>
                                            <input id="ParentQuote_QuoteNumber" name="ParentQuote.QuoteNumber" defaultValue={state.viewModel.ParentQuote?.QuoteNumber} className="no-arrow-input form-control-plaintext border-bottom align-with-select timeline-light-item-orange p-0 m-0" {...register("ParentQuote.QuoteNumber", { required: true })} />
                                            {errors["ParentQuote"] && <span>This field is required</span>}
                                            {console.log(errors)}

                                    </div>
                                </div>
                                    <div className="col-md-6">
                                        <div className="form-group ">
                                            <label className=" control-label m-0 mt-2" style={{ width: "100%" }}>Quote Date</label>
                                    <LocalizationProvider dateAdapter={DateAdapter}>
                                        <MobileDatePicker                                            
                                            inputFormat="dd MMM yyyy"
                                            value={state.viewModel.ParentQuote?.ExpirationDate}
                                            onChange={handleDateChange}
                                                renderInput={(params) =>
                                                    <TextField variant="standard" className="form-control-plaintext border-bottom align-with-select" {...params} {...register("ParentQuote.ExpirationDate")} />
                                            }
                                            minDate={new Date()}

                                        />
                                    </LocalizationProvider>

                                            </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    {state.isReorder &&
                        <div className="row ">
                            <div className=" col-4 text-center">

                                <input type="button" className="btn  btn-rounded border no-box-shadow pt-1 pb-1 pr-4 pl-4 text-capitalize change-quote" onClick={changeQuote} defaultValue="Change Quote" />
                            </div>
                            <div className=" col-8 ">
                                <div className="row">
                                    <div className=" col-4 ">
                                        <div className="form-group ">
                                            <label className=" control-label m-0 mt-2"></label>
                                            <input id="ExpectedSupplyDays" name="ExpectedSupplyDays" defaultValue={state.viewModel.ExpectedSupplyDays} onChange={updateExpectedSupplyDate} className="no-arrow-input form-control-plaintext border-bottom align-with-select p-0 m-0" {...register("ExpectedSupplyDays", { required: true })} />
                                        </div>
                                    </div>

                                    <div className=" col-6">
                                        <LocalizationProvider dateAdapter={DateAdapter}>
                                            <MobileDatePicker
                                                label="Expected Supply Date"

                                                inputFormat="dd MMM yyyy"
                                                value={state.expectedSupplyDate}
                                                onChange={handleExpectedSupplyDateChange}
                                                renderInput={(params) =>
                                                    <TextField variant="standard" {...params} {...register("ParentQuote.ExpirationDate")} />
                                                }
                                                minDate={new Date()}

                                            />
                                        </LocalizationProvider>
                                    </div>
                                </div>
                            </div>
                        </div>
                    }
                </div>
                </form>
            </FormProvider>
        </GlobalModal>
    );
}
const mapDispatchToProps = dispatch => (
    {
        setTempRequestList: (tempRequest) => dispatch(Actions.setTempRequestList(tempRequest)),
        addModal: (modalKey) => dispatch(Actions.addModal(modalKey))
    }
);

const mapStateToProps = state => {
    return {
        tempRequestList: state.tempRequestList.present
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(UploadQuoteModal);

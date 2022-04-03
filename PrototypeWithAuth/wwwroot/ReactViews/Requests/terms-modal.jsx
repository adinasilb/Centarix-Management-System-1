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


function TermsModal(props) {
    const location = useLocation();
    const dispatch = useDispatch();
    const methods = useForm({ mode: 'onChange' });
    const { register, handleSubmit, formState: { errors }, control } = methods;
    const [state, setState] = useState({
        ID: location.state.ID,
        viewModel: {}
    })
    const request = props.tempRequestList.TempRequestViewModels[0].Request;
    var vendorID = (request.SingleOrder && request.SingleOrder.VendorID) || (request.RecurringOrder && request.RecurringOrder.VendorID) || (request.StandingOrder && request.RecurringOrder.VendorID)
    console.log(vendorID)
    useEffect(() => {

        var url = "/Requests/TermsModal?vendorID=" + vendorID;
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                var updatedViewModel = JSON.parse(result);
                setState({ ...state, viewModel: updatedViewModel });
            });

    }, [state.ID]);

    function handleTermsChange(e) {
        setState({
            ...state,
            viewModel: {
                ...state.viewModel,
                SelectedTerm: e.target.value
            }
        })
    }
    function closeClick(e) {
        e.preventDefault()
        props.tempRequestList.undo();
        dispatch(removeModal(props.modalKey));
    }

    const handleDateChange = (newValue) => {
        //var newValue = Date.parse(newValue)
        setState(prevstate => ({
            ...prevstate,
            viewModel: {
                ...prevstate.viewModel,
                InstallmentDate: newValue
            }
        }));
    };

    const emailsKeyPress = (e, emails) => {
        if (state.viewModel.EmailAddresses.length < 5) {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(e.target.value)) {
                setState(prevstate => ({
                    ...prevstate,
                    viewModel: {
                        ...prevstate.viewModel,
                        EmailAddresses: emails
                    }
                }));
            }
        }
    };

    const handleDeleteChip = (email) => {
        if (state.viewModel.EmailAddresses.length > 1) {
            setState(prevstate => ({
                ...prevstate,
                viewModel: {
                    ...prevstate.viewModel,
                    EmailAddresses: prevstate.viewModel.EmailAddresses.filter(e => e != email)
                }
            }));
        }
    };

    var onSubmit = (data, e) => {

        var viewModelFormData = jsonToFormData(state.viewModel)
        viewModelFormData.set("ParentRequest.Shipping", data.shipping)
        viewModelFormData.set("Installments", data.Installments)
        viewModelFormData.set("InstallmentDate", data.InstallmentDate)
        var tempRequestFormData = jsonToFormData({ "TempRequestListViewModel": props.tempRequestList })
        var formData = combineTwoFormDatas(viewModelFormData, tempRequestFormData)

        fetch("/Requests/TermsModal", {
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
                if (result == null) {
                    console.log("redirect to index")
                    props.setTempRequestList([]);
                    props.removeModals(props.modals);
                    props.setReloadIndex(true);
                }
                else {
                    props.setTempRequestList(JSON.parse(result));
                    props.addModal(ModalKeys.CONFIRM_EMAIL)
                }

            }).catch(err => {
                console.dir(err)
                setState({
                    ...state,
                    viewModel: {
                        ...state.viewModel,
                        ErrorMessage : err
                    }
                })
            });

    }

    const currency = request.Currency == AppUtility.CurrencyEnum.NIS ? "₪" : "$";
    var sum = request.Currency == AppUtility.CurrencyEnum.NIS ? request.Cost : (request.Cost / request.ExchangeRate);
    var vat = sum * 0.17;

    return (

        <GlobalModal backdrop={props.backdrop} value={state.ID} closeClick={ closeClick} modalKey={props.modalKey} key={state.ID} size="lg" header="Place Order">
            {state.viewModel.ErrorMessage && <span className="danger-color">{state.viewModel.Error.String}</span>}
            <FormProvider {...methods} >
                <form action="" data-string="" method="post" encType="multipart/form-data" onSubmit={handleSubmit(onSubmit)} id={props.modalKey}>
                    <DevTool control={control} />
                    <div className="container-fluid">
                        <div className="row">
                            <div className="col-4">
                                <label className="control-label">Terms</label>
                                <FormControl fullWidth variant="standard">
                                    <Select
                                        id="Terms"

                                        className="form-control-plaintext border-bottom"
                                        value={state.viewModel.SelectedTerm ?? ""}
                                        onChange={handleTermsChange}
                                        input={<OutlinedInput label="Tag" />}
                                    >
                                        {state.viewModel.TermsList?.map(terms =>
                                            <MenuItem key={terms.PaymentStatusID} value={terms.PaymentStatusID} >
                                                {terms.PaymentStatusDescription}
                                            </MenuItem>
                                        )}
                                    </Select>
                                </FormControl>
                            </div>

                            <div className="col-3">
                                <label className="control-label">Shipping</label>
                                <div className="input-group">
                                    <span className="input-group-text pr-2">{currency}</span>
                                    <input name="ParentRequest.Shipping" className="form-control-plaintext border-bottom"
                                        {...register("shipping", { min: 0 })} />
                                </div>
                            </div>
                            {state.viewModel.SelectedTerm == 5 &&
                                <div className="col-5">
                                    <div className="row">
                                        <div className="col-4 installments-amount-block">
                                            <label className="control-label">Installments</label>
                                            <input defaultValue={state.viewModel.Installments} name="Installments" className="form-control-plaintext border-bottom" {...register("Installments", { min: 1 })} />
                                            {errors["Installments"] && <span>This field is required</span>}
                                        </div>
                                        <div className="col-6 installments-amount-block">
                                            <LocalizationProvider dateAdapter={DateAdapter}>
                                                <MobileDatePicker
                                                    label="Installment Date"
                                                    inputFormat="dd MMM yyyy"
                                                    value={state.viewModel.InstallmentDate}
                                                    onChange={handleDateChange}
                                                    renderInput={(params) =>
                                                        <TextField variant="standard" className="form-control-plaintext border-bottom align-with-select" {...params} {...register("InstallmentDate")} />
                                                    }
                                                />
                                            </LocalizationProvider>
                                        </div>
                                    </div>
                                </div>
                            }

                        </div>
                        <div className="row">
                            <div className="col-12">
                                <label className="control-label">Send to: (Add up to 5 email addresses- including the suppliers email address)</label>

                                <Autocomplete
                                    multiple
                                    options={[]}
                                    value={state.viewModel.EmailAddresses || []}
                                    freeSolo
                                    onChange={(e, value) => emailsKeyPress(e, value)}
                                    renderTags={(value, getTagProps) => (
                                        value.map((email, index) => {
                                            return (
                                                <Chip
                                                    key={index}
                                                    label={email}
                                                    {...getTagProps({ index })}
                                                    onDelete={() => handleDeleteChip(email)}
                                                />
                                            );
                                        }))
                                    }
                                    renderInput={(params) => (
                                        <TextField
                                            {...params}
                                            variant="standard"
                                        />
                                    )}
                                />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-12">
                                <label className="control-label">Notes to the supplier</label>
                                <input defaultValue={state.viewModel.ParentRequest?.NoteToSupplier} name="ParentRequest.NoteToSupplier" className="form-control-plaintext border-bottom"
                                    onChange={(e) => setState({ ...state, viewModel: { ...state.viewModel, ParentRequest: { ...state.viewModel.ParentRequest, NoteToSupplier: e.target.value } } })} />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-4">
                                <label className="control-label">Sum</label>
                                <div className="input-group">
                                    <span className="input-group-text pr-2">{currency}</span>
                                    <input defaultValue={sum.toFixed(2)} disabled={true} className="form-control-plaintext border-bottom" />
                                </div>
                            </div>
                            <div className="col-4">
                                <label className="control-label">VAT</label>
                                <div className="input-group">
                                    <span className="input-group-text pr-2">{currency}</span>
                                    <input defaultValue={vat.toFixed(2)} disabled={true} className="form-control-plaintext border-bottom" />
                                </div>
                            </div>
                            <div className="col-4">
                                <label className="control-label">Total+VAT</label>
                                <div className="input-group">
                                    <span className="input-group-text pr-2">{currency}</span>
                                    <input defaultValue={(sum + vat).toFixed(2)} disabled={true} className="form-control-plaintext border-bottom" />
                                </div>
                            </div>
                        </div>

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
        modals: state.modals
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(TermsModal);

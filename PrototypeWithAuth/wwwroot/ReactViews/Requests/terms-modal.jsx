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

    useEffect(() => {

        var url = "/Requests/TermsModal?vendorID=" + request.Product.VendorID;
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
        console.log(emails)
        console.log(e.target.value)
        if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(e.target.value)) {
            setState(prevstate => ({
                ...prevstate,
                viewModel: {
                    ...prevstate.viewModel,
                    EmailAddresses: emails
                }
            }));
        }
    };

    const handleDeleteChip = (email) => {
        console.log(email)
        setState(prevstate => ({
            ...prevstate,
            viewModel: {
                ...prevstate.viewModel,
                EmailAddresses: prevstate.viewModel.EmailAddresses.filter(e => e != email)
            }
        }));
    };

    var onSubmit = (data, e) => {
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

    const currency = request.Currency == AppUtility.CurrencyEnum.NIS ? "₪" : "$";
    var sum = request.Currency == AppUtility.CurrencyEnum.NIS ? request.Cost : (request.Cost / request.ExchangeRate);
    var vat = sum * 0.17;

    return (

        <GlobalModal backdrop={props.backdrop} value={state.ID} modalKey={props.modalKey} key={state.ID} size="lg" header="Place Order">
            {state.viewModel.Error && state.viewModel.Error.Bool == false}
            <FormProvider {...methods} >
                <form action="" data-string="" method="post" encType="multipart/form-data" className="m-5 modal-padding" onSubmit={handleSubmit(onSubmit)} id={props.modalKey}>
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
                                <div className="input-group">
                                    <label className="control-label">Shipping</label>
                                    <span className="input-group-text pr-2">{currency}</span>
                                    <input defaultValue={state.viewModel.ParentRequest?.Shipping} name="ParentRequest.Shipping" className="form-control-plaintext border-bottom" {...register("ParentRequest.Shipping")} />
                                </div>
                            </div>
                            {state.viewModel.SelectedTerm == 5 &&
                                <div className="col-5">
                                <div className = "row">
                                    <div className="col-4 installments-amount-block">
                                        <label className="control-label">Installments</label>
                                        <input defaultValue={state.viewModel.Installments} name="Installments" className="form-control-plaintext border-bottom" {...register("Installments", { min:1 })} />
                                        {errors["Installments"] && <span>This field is required</span>}
                                    </div>
                                    <div className="col-6 installments-amount-block">
                                        <LocalizationProvider dateAdapter={DateAdapter}>
                                            <MobileDatePicker
                                                label="Installment Date"
                                                variant="standard"
                                                inputFormat="dd MMM yyyy"
                                                value={state.viewModel.InstallmentDate}
                                                onChange={handleDateChange}
                                                renderInput={(params) =>
                                                    <TextField  {...params} {...register("InstallmentDate")} />
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
                                <input defaultValue={state.viewModel.ParentRequest?.NoteToSupplier} name="ParentRequest.NoteToSupplier" className="form-control-plaintext border-bottom" {...register("ParentRequest.NoteToSupplier")} />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-4">
                                <label className="control-label">Sum</label>
                                <span className="input-group-text pr-2">{currency}</span>
                                <input defaultValue={sum.toFixed(2)} className="form-control-plaintext border-bottom" />
                            </div>
                            <div className="col-4">
                                <label className="control-label">VAT</label>
                                <span className="input-group-text pr-2">{currency}</span>
                                <input defaultValue={vat.toFixed(2)} className="form-control-plaintext border-bottom" />
                            </div>
                            <div className="col-4">
                                <label className="control-label">Total+VAT</label>
                                <span className="input-group-text pr-2">{currency}</span>
                                <input defaultValue={(sum + vat).toFixed(2)} className="form-control-plaintext border-bottom" />
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
        setTempRequestList: (tempRequest) => dispatch(Actions.setTempRequestList(tempRequest))
    }
);

const mapStateToProps = state => {
    return {
        tempRequestList: state.tempRequestList.present
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(TermsModal);

import React, { useState, useEffect } from 'react';
import moment from 'moment'
import {
    useLocation, Link, useHistory
} from 'react-router-dom';
import GlobalModal from '../Utility/global-modal.jsx';
import { FileSelectChange, GetFileString } from "../Utility/document-fuctions.jsx";
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import DocumentsCard from './documents-card.jsx'
import 'react-multi-carousel/lib/styles.css';

import 'regenerator-runtime/runtime'
import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import { useDispatch, connect } from 'react-redux';
import { useForm } from 'react-hook-form';


function UploadQuoteModal(props) {
    const location = useLocation();
    const dispatch = useDispatch();
    const { register, handleSubmit, formState: { errors } } = useForm();
    const [state, setState] = useState({
        ID: location.state.ID,
        moveToTerms: location.state.moveToTerms,
        viewModel: {},
        expectedSupplyDate: (moment(new Date()).format('DD MMM YYYY')),
        isReorder: props.isReorder
    })

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

        var formData = new FormData(e.target);
        if (state.moveToTerms) {
            var newTempRequestJson = props.tempRequestJson;
            newTempRequestJson.Request.ParentQuote = formData.get("ParentQuote")
            newTempRequestJson.Request.ParentQuoteID = formData.get("ParentQuoteID")
            newTempRequestJson.Request.QuoteStatusID = '4'
            props.setTempRequestJson(newTempRequestJson)
        }
        else {
            var url = "/Requests/UploadQuoteModal";
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

    var formattedExpirationDate = state.viewModel.ParentQuote?.ExpirationDate && moment(state.viewModel.ParentQuote?.ExpirationDate).format('DD MMM YYYY')
    return (

        <GlobalModal backdrop={props.backdrop} value={state.ID} modalKey={props.modalKey} key={state.ID} size="lg" header="Upload Quote">
            <form action="" data-string="" method="post" encType="multipart/form-data" className="m-5 modal-padding" onSubmit={handleSubmit(onSubmit)} id={props.modalKey}>
                <input type="hidden" name="ParentQuote.ParentQuoteID" id="ParentQuote_ParentQuoteID" defaultValue={state.viewModel.ParentQuote?.ParentQuoteID} />
                <div className="container">
                    <div className="row">
                        <div className=" col-4 doc-card-outer-div m-0 text-center h-100">
                            {state.viewModel.DocumentsInfo &&
                                <DocumentsCard documentsInfo={state.viewModel.DocumentsInfo} isEditable={true} modalType={'Summary'} />
                            }
                        </div>
                        <div className="col-8 pl-5 pt-4">
                            <div className="row">

                                <div className=" col-md-6">
                                    <div className="form-group ">
                                        <label className=" control-label m-0 mt-2" style={{ width: "100%" }}>Quote Number</label>
                                        <input id="ParentQuote.QuoteNumber" name="ParentQuote_QuoteNumber" defaultValue={state.viewModel.ParentQuote?.QuoteNumber} className="no-arrow-input form-control-plaintext border-bottom align-with-select timeline-light-item-orange p-0 m-0" {...register("quoteNumber", { required: true })} />
                                        {errors.quoteNumber && <span>This field is required</span>}

                                    </div>
                                </div>
                                <div className="col-md-6">
                                    <label className=" control-label m-0 mt-2" style={{ width: "100%" }}>Expiration Date</label>
                                    <input id="ParentQuote.ExpirationDate" name="ParentQuote_ExpirationDate" defaultValue={formattedExpirationDate} type="text" className="datepicker form-control-plaintext border-bottom p-0 m-0" {...register("expirationDate", { required: true })} />
                                    {errors.expirationDate && <span>This field is required</span>}

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
                                            <input id="ExpectedSupplyDays" name="ExpectedSupplyDays" defaultValue={state.viewModel.ExpectedSupplyDays} onChange={updateExpectedSupplyDate} className="no-arrow-input form-control-plaintext border-bottom align-with-select p-0 m-0" />
                                        </div>
                                    </div>

                                    <div className=" col-6">
                                        <div className="form-group">
                                            <label className="control-label">Expected supply date</label>
                                            <label className="form-control-plaintext border-bottom datepicker" type="text"> {state.expectedSupplyDate}</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    }
                </div>
            </form>
        </GlobalModal>
    );
}
const mapDispatchToProps = dispatch => (
    {
        setTempRequestJson: (tempRequestJson) => dispatch(Actions.setTempRequestJson(tempRequestJson))
    }
);

const mapStateToProps = state => {
    return {
        tempRequestJson: state.tempRequestJson
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(UploadQuoteModal);

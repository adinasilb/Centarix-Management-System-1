import React, { useState, useEffect } from 'react';
import {
    useLocation, Link
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
    const [ID, setID] = useState(location.state.ID)
    const [moveToTerms, setMoveToTerms] = useState(location.state.moveToTerms)
    const [viewModel, setViewModel] = useState({ FileStrings: [] });
    const [expectedSupplyDate, setExpectedSupplyDate] = useState(new Date())

    useEffect(() => {
        console.log("ID " + ID)
        var url = "/Requests/UploadQuoteModal?id=" + ID;
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                var updatedViewModel = JSON.parse(result);
                setViewModel(updatedViewModel);
                //setTempRequestJson(updatedViewModel.TempRequestListViewModel)
            });

    }, [ID]);


    var onSubmit = (data, e) => {
        
        var formData = new FormData(e.target);
        if (moveToTerms) {
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

    function updateExpectedSupplyDate() {
        var today = new Date();
        setExpectedSupplyDate(today.setDate(today.getDate() + parseInt(response.ExpectedSupplyDays)))
    }

    return (

        <GlobalModal backdrop={props.backdrop} value={ID} modalKey={props.modalKey} key={ID} size="md" header="Upload Quote">
            <form action="" data-string="" method="post" encType="multipart/form-data" className="m-5 modal-padding" onSubmit={handleSubmit(onSubmit)} id={props.modalKey}>
                <input type="hidden" name="ParentQuote.ParentQuoteID" id="ParentQuote_ParentQuoteID" defaultValue={viewModel.ParentQuote?.ParentQuoteID} />
                <div className="container">
                    <div className="row">
                        <div className=" col-4 doc-card-outer-div m-0 text-center h-100">
                            {viewModel.DocumentsCardViewModel &&
                                <DocumentsCard documentsInfo={viewModel.DocumentsCardViewModel} modalType={'Summary'} />
                            }
                        </div>
                        <div className="col-4 pl-5 pt-4">
                            <div className="row">

                                <div className=" col-md-6">
                                    <div className="form-group ">
                                        <label className=" control-label m-0 mt-2" style={{ width: "100%" }}>Quote Number</label>
                                        <input id="ParentQuote.QuoteNumber" name="ParentQuote_QuoteNumber" defaultValue={viewModel.ParentQuote?.QuoteNumber} className="no-arrow-input form-control-plaintext border-bottom align-with-select timeline-light-item-orange p-0 m-0" {...register("quoteNumber", { required: true })} />
                                        {errors.quoteNumber && <span>This field is required</span>}

                                    </div>
                                </div>
                                <div className="col-md-6">
                                    <label className=" control-label m-0 mt-2" style={{ width: "100%" }}>Quote Date</label>
                                    <input id="ParentQuote.ExpirationDate" name="ParentQuote_ExpirationDate" defaultValue={viewModel.ParentQuote?.QuoteDate} type="text" className="datepicker form-control-plaintext border-bottom p-0 m-0" {...register("quoteDate", { required: true })} />
                                    {errors.quoteDate && <span>This field is required</span>}

                                </div>

                            </div>
                        </div>
                    </div>
                    <div className="row ">
                        <div className=" offset-4 text-center">
                            <input type="button" className="btn  btn-rounded border no-box-shadow pt-1 pb-1 pr-4 pl-4 text-capitalize change-quote" defaultValue="Change Quote" />
                        </div>
                        <div className=" col-6 pl-5 ">
                            <div className="row">
                                <div className=" col-4 ">
                                    <div className="form-group ">
                                        <label className=" control-label m-0 mt-2"></label>
                                        <input id="ExpectedSupplyDays" name="ExpectedSupplyDays" defaultValue={viewModel.ExpectedSupplyDays} onChange={ updateExpectedSupplyDate } className="no-arrow-input form-control-plaintext border-bottom align-with-select timeline-light-item-orange p-0 m-0 expected-supply-days" min="1" />
                                    </div>
                                </div>

                                <div className=" col-6">
                                    <div className="form-group">
                                        <label className="control-label">Expected supply date</label>
                                        <input defaultValue={expectedSupplyDate} className="form-control-plaintext border-bottom datepicker expected-supply-date" type="text"  />
                                    </div>
                                </div>
                            </div>
                        </div>
                        </div>
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

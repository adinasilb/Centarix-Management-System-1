import React, { useState, useEffect } from 'react';
import {
    useLocation, Link, useHistory
} from 'react-router-dom';
import GlobalModal from '../Utility/global-modal.jsx';
import 'regenerator-runtime/runtime'
import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import { useDispatch, connect, batch } from 'react-redux';
import { useForm, FormProvider } from 'react-hook-form';
import { DevTool } from "@hookform/devtools";
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import { jsonToFormData, combineTwoFormDatas } from '../Utility/root-function.jsx'
import OrderEmail from './order-email.jsx'
import OrderPDF from './order-pdf.jsx'
import { jsPDF } from "jspdf";
import html2canvas from "html2canvas";
import ReactPDF from '@react-pdf/renderer'
import ReactDOMServer from 'react-dom/server';
import { SidebarEnum } from '../Constants/AppUtility.jsx'


function ConfirmEmailModal(props) {
    const location = useLocation();
    const dispatch = useDispatch();
    const methods = useForm({ mode: 'onChange' });
    const { register, handleSubmit, formState: { errors }, control } = methods;
    const [state, setState] = useState({
        ID: location.state.ID,
        viewModel: {}
    })
    const guid = props.tempRequestList.Guids[props.tempRequestList.Guids.length - 1];
    var firstRequest = props.tempRequestList.TempRequestViewModels[0].Request;

    useEffect(() => {

        let parentRequestQuery = Object.keys(firstRequest.ParentRequest)
            .map(k => k + '=' + firstRequest.ParentRequest[k] ?? null)
            .join('&');
        var url = "/Requests/ConfirmEmailModal?" + parentRequestQuery + "&paymentStatusID=" + firstRequest.PaymentStatusID + "&guid=" + guid;
        console.log(url)
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                var updatedViewModel = JSON.parse(result);
                setState({ ...state, viewModel: updatedViewModel });
            });

    }, [state.ID]);

    //const createPDF = async () => {
    //    const pdf = new jsPDF("portrait", "pt", "a4");
    //    const data = await html2canvas(document.querySelector("#orderEmail"));
    //    const img = data.toDataURL("image/png");
    //    const imgProperties = pdf.getImageProperties(img);
    //    const pdfWidth = pdf.internal.pageSize.getWidth();
    //    const pdfHeight = (imgProperties.height * pdfWidth) / imgProperties.width;
    //    pdf.addImage(img, "PNG", 0, 0, pdfWidth, pdfHeight);
    //    return pdf.output();
    //};

    async function onSubmit(data, e) {
        //var orderFileBlob = await createPDF()
        //var orderPdf = new File([orderFileBlob], "OrderPdf.pdf")
        //console.log(orderPdf)

        var orderPdf = ReactDOMServer.renderToString(<OrderEmail viewModel={state.viewModel} tempRequestList={props.tempRequestList} navigationInfo={props.navigationInfo} />)
        firstRequest.Product = null;
        var viewModelFormData = jsonToFormData(state.viewModel)
        var tempRequestFormData = jsonToFormData({ "TempRequestListViewModel": props.tempRequestList })
        var formData = combineTwoFormDatas(viewModelFormData, tempRequestFormData)
        formData.set("OrderPDF", orderPdf)
        document.getElementById("loading").style.display = "block";

        fetch("/Requests/ConfirmEmailModal", {
            method: "POST",
            body: formData
        })
            .then((response) => {
                if (props.navigationInfo.sideBarType = SidebarEnum.Add) {

                    window.location.href = "/Requests"
                }
                else {
                    props.removeAllModals();
                    props.setTempRequestList([]);
                    batch(() => {
                        props.setReloadIndex(true);
                        props.setPageNumber(1)
                    }
                    )
                    document.getElementById("loading").style.display = "none";

                }
            })
            .catch(err => {
                console.dir(err)
                setState({
                    ...state,
                    viewModel: {
                        ...state.viewModel,
                        ErrorMessage: err
                    }
                })
            });

    }

    return (

        <GlobalModal backdrop={props.backdrop} value={state.ID} modalKey={props.modalKey} key={state.ID} size="lg" header="Place Order">
            {state.viewModel.ErrorMessage && <span className="danger-color">{state.viewModel.Error.String}</span>}
            <FormProvider {...methods} >
                <form action="" data-string="" method="post" encType="multipart/form-data" onSubmit={handleSubmit(onSubmit)} id={props.modalKey}>
                    <DevTool control={control} />
                    <OrderEmail viewModel={state.viewModel} tempRequestList={props.tempRequestList} navigationInfo={props.navigationInfo} />
                </form>
            </FormProvider >
        </GlobalModal >
    );
}
const mapDispatchToProps = dispatch => (
    {
        setTempRequestList: (tempRequest) => dispatch(Actions.setTempRequestList(tempRequest)),
        addModal: (modalKey) => dispatch(Actions.addModal(modalKey)),
        removeAllModals: () => dispatch(Actions.removeAllModals()),
        setReloadIndex: (reload) => dispatch(Actions.setReloadIndex(reload)),
        setPageNumer: (number) => dispatch(Actions.setPageNumber(number))
    }
);

const mapStateToProps = state => {
    return {
        tempRequestList: state.tempRequestList.present,
        navigationInfo: state.navigationInfo
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(ConfirmEmailModal);

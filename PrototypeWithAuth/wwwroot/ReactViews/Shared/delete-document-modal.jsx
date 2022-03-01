import React, { useState, useEffect } from 'react';
import ReactDOM from 'react-dom';
import {
    useLocation
} from 'react-router-dom';
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import GlobalModal from '../Utility/global-modal.jsx';

export default function DeleteDocumentModal(props) {
    const [ID, setID] = useState({ requestID: location.state.ID })
    const [folderName, setFolderName] = useState({ requestID: location.state.ID })
    const [viewModel, setViewModel] = useState();
    useEffect(() => {
        var url = "/Requests/DeleteDocumentModal?id=" + ID;
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                setViewModel(JSON.parse(result));
            });

    }, [state.requestID]);

    function getFileString(fileString) {
        var newFileString = "";
        var ext = fileString.Split(".").Last();
        switch (ext) {
            case "pptx":
            case "ppt":
                newFileString = "images/powerpoint.png";
                break;
            case "doc":
            case "docx":
                newFileString = "images/wordicon.png";
                break;
            case "xlsx":
                newFileString = "images/excel.png";
                break;
            case "doc":
            case "docx":
                newFileString = "images/wordicon.png";
            default:
                newFileString = fileString
        }
        return newFileString;
    }
    var carouselCards = [];
    function MapDocCarouselItems(fileStrings) {
        var ModelCount = fileStrings.Count;
        var EndCount = ModelCount;
        switch (ModelCount % 3) {
            case 1:
                EndCount += 2;
                break;
            case 2:
                EndCount += 1;
                break;
        }
        for (var i = 0; i < EndCount; i++)
        {
            carouselCards.push(<div class={"carousel-item "(i == 0) ? "active" : ""}>
                <div class=" doc-card-outer-div col-md-4 m-0">
                        <div class="card iframe-container document-border m-0">
                            <div class="card-body responsive-iframe-container">
                                <img class="card-img-top" src="@Model.FileStrings[i]" alt="Card image cap" />

                                <a href="@Model.FileStrings[i]" class="linkwrap" target="_blank">
                                    <div class="blocker"></div>
                                    <iframe src={getFileString(viewModel.FileStrings[i])} title="View" class="responsive-iframe" scrolling="no"></iframe>

                                </a>
                            </div>
                            <div class="card-body d-flex text-center align-items-center justify-content-center">

                                <a href={viewModel.FileStrings[i]} target="_blank" class="mx-3  view-img">
                                    <i class="icon-centarix-icons-09 section-filter " style="font-size:2rem"></i>
                                </a>
                                <a href="" class="delete-document mx-3">
                                    <i style="font-size:2rem;" class="icon-delete-24px documents-delete-icon hover-bold"></i>
                                </a>
                                <p class="align-to-bottom w-100">{viewModel.FileStrings[i]}</p>
                            </div>
                        </div>
                </div>
            </div>)
        }
    }

    return (
        
        <GlobalModal backdrop={props.backdrop} value={state.viewModel?.Request?.RequestID} modalKey={props.modalKey} key={state.viewModel?.Request?.RequestID} size="lg" header={"Are you sure you would like to delete " + state.viewModel?.Request?.Product?.ProductName + "?"} >
            
            {(Model.FileStrings != null && Model.FileStrings?.Count > 0) ?
                <div id="carousel-example-multi" class="carousel slide carousel-multi-item v-2" data-ride="carousel">


                    <div class="row">
                        <div class="col-1">
                            {(viewModel.FileStrings.Count > 3) ?
                                <a class="heading-1" href="#carousel-example-multi" data-slide="prev">
                                    <i class="fas fa-chevron-left section-filter"></i>
                                </a>
                                :
                                null
                            }
                        </div>
                        <div class="col-10">
                            <div class="carousel-inner v-2" role="listbox">
                                {carouselCards}
                            </div>
                        </div>

                        <div class="col-1 ">
                            {(viewModel.FileStrings.Count > 3) ?
                                <a class="heading-1" href="#carousel-example-multi" data-slide="next">
                                    <i class="fas fa-chevron-right section-filter"></i>
                                </a>
                                :
                                null
                            }
                        </div>
                    </div>


                </div>
                :
                <div class="text-center text-lowercase">
                    <div class="mt-6">
                        <img src="~/images/document_empty_image.png" />
                    </div>
                    <div class="mt-4" style="font-size: 1.125rem; color: #646464; font-weight:400;">
                        <span class="text-capitalize">No</span> {viewModel.FolderName.ToString().ToLower()} have been uploaded
                    </div>
                    <div class='mt-2 pb-5'>
                        <div class='@((!Model.ShowSwitch && !Model.IsEdittable) ? "d-none" : "")' style="font-size: 1rem; color: #646464; font-weight:400;">
                            <span class="text-capitalize">Press</span> upload to add @Model.FolderName.ToString().TrimEnd('s').ToLower()
                        </div>
                    </div>
                </div>
            }
        </GlobalModal>
    );
}

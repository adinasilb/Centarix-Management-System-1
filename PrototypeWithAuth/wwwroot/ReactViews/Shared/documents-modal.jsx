import React, { useState, useEffect } from 'react';
import {
    useLocation
} from 'react-router-dom';
import GlobalModal from '../Utility/global-modal.jsx';
import { FileSelectChange } from "../Utility/document-fuctions.jsx"
import Carousel from "react-multi-carousel";
import "react-multi-carousel/lib/styles.css";

import 'regenerator-runtime/runtime'

export default function DocumentsModal(props) {
    const location = useLocation();
    const [ID, setID] = useState(location.state.ID)
    const [isEditable, setisEditable] = useState(location.state.IsEditable)
    const [showSwitch, setshowSwitch] = useState(location.state.ShowSwitch)
    const [viewModel, setViewModel] = useState({ FileStrings: [] });
    const [carouselCards, setCarouselCards] = useState([])

    useEffect(() => {
        var url = "/Requests/DocumentsModal?id=" + ID + "&RequestFolderNameEnum=" + location.state.FolderName + "&parentFolderName=" + location.state.ParentFolderName;
        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                setViewModel(JSON.parse(result));
            });

    }, [ID]);

    //useEffect(() => {
    //    MapDocCarouselItems(viewModel.FileStrings)
    //}, [viewModel.FileStrings])


    var id = viewModel?.ObjectID == null ? "0" : viewModel.ObjectID;

    async function uploadFile(e) {
        var response = await FileSelectChange(e.target, viewModel.FolderName, viewModel.ParentFolderName, id)
        console.log(response)
        setViewModel(response)
    }

    function getFileString(fileString) {
        var newFileString = "";
        console.log("filestring: " + fileString)
        var ext = fileString?.split(".").pop();
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

    function MapDocCarouselItems(fileStrings) {
        console.log("file strings " + fileStrings)
        var ModelCount = fileStrings?.length;
        var carouselCardsLocal = [];
        var EndCount = ModelCount;
        //switch (ModelCount % 3) {
        //    case 1:
        //        EndCount += 2;
        //        break;
        //    case 2:
        //        EndCount += 1;
        //        break;
        //}
        if (isEditable || showSwitch) {
            carouselCardsLocal.push(
                <div key="upload" >
                    <div className=" card document-border col-md-4 m-0">
                        <label>
                            <i className="icon-upload_file_black_24dp-1 opac54 m-0" alt="order" style={{ fontSize: "1rem" }}></i>
                            <input type="file" onChange={uploadFile} className="file-selects d-none mark-readonly" accept=".png, .jpg, .jpeg, .pdf, .pptx, .ppt, .docx, .doc, .xlsx, .xls" id="FilesToSave" name="FilesToSave" />
                            <span className="section-filter text">Upload File</span>

                        </label>

                    </div>
                </div>
            )
        }
        for (var i = 0; i < EndCount; i++) {
            console.log("i: " + i)

            if (ModelCount > i) {
                carouselCardsLocal.push(
                    <div key={"File" + i} >
                        <div className="card iframe-container document-border m-0">
                            <div className="card-body responsive-iframe-container">

                                <iframe src={getFileString(viewModel.FileStrings[i])} title="View" className="responsive-iframe" scrolling="no"></iframe>
                            </div>
                            <div className="card-body d-flex text-center align-items-center justify-content-center">

                                <a href={"\\" + viewModel.FileStrings[i]} target="_blank" className="mx-3  view-img">
                                    {viewModel.FileStrings[i].split('\\').pop()}
                                </a>
                                <a href="" className="delete-document mx-3">
                                    <i style={{ fontSize: "2rem" }} className="icon-delete-24px documents-delete-icon hover-bold"></i>
                                </a>
                            </div>
                        </div>
                    </div>)
            }
        }
        console.log()
        setCarouselCards(carouselCardsLocal)
    }

    return (

        <GlobalModal backdrop={props.backdrop} value={ID} modalKey={props.modalKey} key={ID} size="xl" header={location.state.FolderName + " Files"} >
            <form action="" data-string="" method="post" encType="multipart/form-data" className="m-5 modal-padding documentModalForm" id="myForm">
                <input type="hidden" name="FolderName" id="FolderName" value={viewModel.FolderName || ""} />
                <input type="hidden" name="DontAllowMultiple" id="DontAllowMultiple" value={viewModel.DontAllowMultiple || ""} />
                <input type="hidden" name="ParentFolderName" id="ParentFolderName" value={viewModel.ParentFolderName || ""} />
                <input type="hidden" name="ObjectID" id="ObjectID" value={viewModel.ObjectID || ""} />
                <input type="hidden" name="Guid" id="Guid" value={viewModel.Guid || ""} />

                <div className="container">
                    <div className="row">
                        <div className="col-3 offset-9">
                            {(location.state.ShowSwitch) ?
                                <div className=" row text-right mb-0">
                                    <div className="switch col-12 switch-margin">
                                        <label>
                                            <label className="edit-mode-switch-description "> {isEditable ? "Edit Mode On" : "Edit Mode Off"}</label>
                                            <input type="checkbox" className={"turn-edit-doc-on-off" + (isEditable ? " checked" : "")} value={ID} />
                                            <span className="lever"></span>
                                        </label>
                                    </div>
                                </div>
                                : null
                            }
                        </div>
                    </div>
                </div>
                {console.log("view model file strings: " + viewModel?.FileStrings)}
                {!isEditable && viewModel?.FileStrings?.length == 0 ?
                    <div className="text-center text-lowercase">
                        <div className="mt-6">
                            <img src="/images/document_empty_image.png" />
                        </div>
                        <div className="mt-4" style={{ fontSize: "2rem", color: "1.125rem", fontWeight: "400" }} >
                            <span className="text-capitalize">No</span> {viewModel?.FolderName} have been uploaded
                        </div>
                    </div>
                    :
                    <Carousel
                        swipeable={false}
                        draggable={false}
                        showDots={true}
                        responsive={responsive}
                        ssr={true} // means to render carousel on server-side.
                        infinite={true}
                        autoPlay={true}
                        autoPlaySpeed={5000}
                        keyBoardControl={true}
                        sliderClass="carousel-slider"
                        containerClass="carousel-container"
                        dotListClass="none"
                        itemClass="carousel-item-padding-40-px"
                        responsive={{
                        desktop: {
                            breakpoint: {
                                max: 3000,
                                min: 1024
                            },
                            items: 3,
                            partialVisibilityGutter: 40
                        }
                    }}>
                        <div>Item 1</div>
                        <div>Item 2</div>
                        <div>Item 3</div>
                        <div>Item 4</div>
                        {/*<div className="card iframe-container document-border m-0">*/}
                        {/*    <div className="card-body responsive-iframe-container">*/}

                        {/*    */}{/*    <iframe src={getFileString(viewModel.FileStrings[0])} title="View" className="responsive-iframe" scrolling="no"></iframe>*/}
                        {/*    </div>*/}
                        {/*    <div className="card-body d-flex text-center align-items-center justify-content-center">*/}

                        {/*        <a href={"\\" + viewModel.FileStrings[0]} target="_blank" className="mx-3  view-img">*/}
                        {/*            {viewModel.FileStrings[0].split('\\').pop()}*/}
                        {/*        </a>*/}
                        {/*        <a href="" className="delete-document mx-3">*/}
                        {/*            <i style={{ fontSize: "2rem" }} className="icon-delete-24px documents-delete-icon hover-bold"></i>*/}
                        {/*        </a>*/}
                        {/*    </div>*/}
                        {/*</div>*/}
                        {/*<div className="card iframe-container document-border m-0">*/}
                        {/*    <div className="card-body responsive-iframe-container">*/}

                        {/*    */}{/*    <iframe src={getFileString(viewModel.FileStrings[1])} title="View" className="responsive-iframe" scrolling="no"></iframe>*/}
                        {/*    </div>*/}
                        {/*    <div className="card-body d-flex text-center align-items-center justify-content-center">*/}

                        {/*        <a href={"\\" + viewModel.FileStrings[1]} target="_blank" className="mx-3  view-img">*/}
                        {/*            {viewModel.FileStrings[1].split('\\').pop()}*/}
                        {/*        </a>*/}
                        {/*        <a href="" className="delete-document mx-3">*/}
                        {/*            <i style={{ fontSize: "2rem" }} className="icon-delete-24px documents-delete-icon hover-bold"></i>*/}
                        {/*        </a>*/}
                        {/*    </div>*/}
                        {/*</div>*/}
                        {/*<div className="card iframe-container document-border m-0">*/}
                        {/*    <div className="card-body responsive-iframe-container">*/}

                        {/*    */}{/*    <iframe src={getFileString(viewModel.FileStrings[2])} title="View" className="responsive-iframe" scrolling="no"></iframe>*/}
                        {/*    </div>*/}
                        {/*    <div className="card-body d-flex text-center align-items-center justify-content-center">*/}

                        {/*        <a href={"\\" + viewModel.FileStrings[2]} target="_blank" className="mx-3  view-img">*/}
                        {/*            {viewModel.FileStrings[2].split('\\').pop()}*/}
                        {/*        </a>*/}
                        {/*        <a href="" className="delete-document mx-3">*/}
                        {/*            <i style={{ fontSize: "2rem" }} className="icon-delete-24px documents-delete-icon hover-bold"></i>*/}
                        {/*        </a>*/}
                        {/*    </div>*/}
                        {/*</div>*/}
                        {/*<div className="card iframe-container document-border m-0">*/}
                        {/*    <div className="card-body responsive-iframe-container">*/}

                        {/*    */}{/*    <iframe src={getFileString(viewModel.FileStrings[3])} title="View" className="responsive-iframe" scrolling="no"></iframe>*/}
                        {/*    </div>*/}
                        {/*    <div className="card-body d-flex text-center align-items-center justify-content-center">*/}

                        {/*        <a href={"\\" + viewModel.FileStrings[3]} target="_blank" className="mx-3  view-img">*/}
                        {/*            {viewModel.FileStrings[3].split('\\').pop()}*/}
                        {/*        </a>*/}
                        {/*        <a href="" className="delete-document mx-3">*/}
                        {/*            <i style={{ fontSize: "2rem" }} className="icon-delete-24px documents-delete-icon hover-bold"></i>*/}
                        {/*        </a>*/}
                        {/*    </div>*/}
                        {/*</div>*/}
                        {/*{carouselCards}*/}
                        {/*{(viewModel?.FileStrings?.length > 0) ?*/}
                        {/*    viewModel.FileStrings.map((fileString, i) => {*/}
                        {/*        return (*/}
                        {/*            <div key={"FileString" + i} className=" doc-card-outer-div col-12 m-0">*/}
                        {/*                <div className="card iframe-container document-border m-0">*/}
                        {/*                    <div className="card-body responsive-iframe-container">*/}

                        {/*                        <iframe src={getFileString(fileString)} title="View" className="responsive-iframe" scrolling="no"></iframe>*/}
                        {/*                    </div>*/}
                        {/*                    <div className="card-body d-flex text-center align-items-center justify-content-center">*/}

                        {/*                        <a href={"\\" + fileString} target="_blank" className="mx-3  view-img">*/}
                        {/*                            {fileString.split('\\').pop()}*/}
                        {/*                        </a>*/}
                        {/*                        <a href="" className="delete-document mx-3">*/}
                        {/*                            <i style={{ fontSize: "2rem" }} className="icon-delete-24px documents-delete-icon hover-bold"></i>*/}
                        {/*                        </a>*/}
                        {/*                    </div>*/}
                        {/*                </div>*/}
                        {/*            </div>*/}
                        {/*        )*/}
                        {/*    })*/}
                        {/*    : <div/>*/}


                    </Carousel>

                }




                {/*<div id="carousel-example-multi" className="carousel slide carousel-multi-item" data-ride="carousel">
                        <div className="row">
                            <div className="col-1">
                                {(viewModel?.FileStrings.length > 3) ?
                                    <a className="heading-1" href="#carousel-example-multi" data-slide="prev">
                                        <i className="fas fa-chevron-left section-filter"></i>
                                    </a>
                                    :
                                    null
                                }
                            </div>
                            <div className="col-10">
                                <div className="carousel-inner v-2" role="listbox">
                                    {/*{(isEditable || showSwitch) ?*/}
                {/*    <div className={"carousel-item col-4 active"}>*/}
                {/*        <div className=" doc-card-outer-div">*/}
                {/*            <div className=" card document-border col-md-4 m-0">*/}
                {/*                <label>*/}
                {/*                    <i className="icon-upload_file_black_24dp-1 opac54 m-0" alt="order" style={{ fontSize: "1rem" }}></i>*/}
                {/*                    <input type="file" onChange={uploadFile} className="file-selects d-none mark-readonly" accept=".png, .jpg, .jpeg, .pdf, .pptx, .ppt, .docx, .doc, .xlsx, .xls" id="FilesToSave" name="FilesToSave" />*/}
                {/*                    <span className="section-filter text">Upload File</span>*/}

                {/*                </label>*/}

                {/*            </div>*/}
                {/*        </div>*/}
                {/*    </div> : null}*/}
                {/*{(viewModel?.FileStrings?.length > 0) ?*/}
                {/*    MapDocCarouselItems(viewModel?.FileStrings).map(carouselItem => carouselItem) : null}*/}
                {/*                </div>*/}
                {/*</div> */}
                {/*<div className="col-1">*/}
                {/*    <a className="heading-1" href="#carousel-example-multi" data-slide="next">*/}
                {/*        <i className="fas fa-chevron-right section-filter"></i>*/}
                {/*    </a>*/}
                {/*</div>*/}
                {/*</div>*/}
                {/*</div>}*/}
            </form>
        </GlobalModal>
    );
}

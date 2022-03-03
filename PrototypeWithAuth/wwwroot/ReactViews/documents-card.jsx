import React, { useState, useEffect } from 'react';
import ReactDOM from 'react-dom';
import { FileSelectChange } from "./document-fuctions.jsx"
import 'regenerator-runtime/runtime'

export default function DocumentsCard(props) {

    const [documentsInfo, setDocumentsInfo] = useState(props.documentsInfo);
    const [modalType, setModalType] = useState(props.modalType);

    async function uploadFile(e) {
        var response = await FileSelectChange(e.target)
        setDocumentsInfo(response)
    }


    var id = documentsInfo.ObjectID == null ? "0" : documentsInfo.ObjectID;
    return (
        <div>
            <div className={"card document-border " + (documentsInfo.FileStrings?.length > 0 ? "hasFile" :"")}>
            <div className="document-card text-center pb-2" >
                <div className="col-4 pr-0 d-flex align-items-end mb-1">
                    <label className="control-label">{documentsInfo.FileStrings?.length} Files</label>
                </div>
                    <div className="col-4 d-flex align-items-center">
                        <a href="" className=" open-document-modal mark-edditable" data-string={documentsInfo.FolderName} data-id={id} id={documentsInfo.FolderName} data-val="true" parent-folder={documentsInfo.ParentFolderName} show-switch="false" >
                            <i className={documentsInfo.Icon + " document-icon m-0"} alt="order" style={{ fontSize: "1.5rem" }}></i>
                    </a>
                </div>
                <div className="col-4 d-flex align-items-end mb-1 ">
                        <label>
                            <i className="icon-upload_file_black_24dp-1 opac54 m-0" alt="order" style={{ fontSize: "1rem" }}></i>
                            <input type="file" onChange={uploadFile} className="file-selects direct-upload d-none mark-readonly" accept=".png, .jpg, .jpeg, .pdf, .pptx, .ppt, .docx, .doc, .xlsx, .xls" id="FilesToSave" name="FilesToSave" />
                    </label>
                </div>
            </div>

        </div>
            <label className="control-label text-center text document-text-margin" style={{ width: "100%" }}>{documentsInfo.FolderName}</label>
    </div>
        )
}
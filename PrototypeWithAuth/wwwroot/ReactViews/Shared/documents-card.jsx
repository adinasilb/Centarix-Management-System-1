import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { FileSelectChange } from "../Utility/document-fuctions.jsx"
import 'regenerator-runtime/runtime'

export default function DocumentsCard(props) {

    const [documentsInfo, setDocumentsInfo] = useState(props.documentsInfo);
    const [showSwitch, setShowSwitch] = useState(props.showSwitch);
    const [isEditable, setIsEditable] = useState(props.isEditable);
    var id = documentsInfo.ObjectID == null ? "0" : documentsInfo.ObjectID;
    async function uploadFile(e) {
        var response = await FileSelectChange(e.target, documentsInfo.FolderName, documentsInfo.ParentFolderName, id)
        setDocumentsInfo(response)
    }


    
    return (
        <div>
            <div className={"card document-border " + (documentsInfo.FileStrings?.length > 0 ? "hasFile" :"")}>
            <div className="document-card text-center pb-2" >
                <div className="col-4 pr-0 d-flex align-items-end mb-1">
                        <label className="control-label">{documentsInfo.FileStrings?.length > 0 ? documentsInfo.FileStrings.length : 0} Files</label>
                </div>
                    <div className="col-4 d-flex align-items-center">
                        <Link className="mark-edditable open-document-modal" to={{
                            pathname: "/DocumentsModal",
                            state: { ID: id, IsEditable: isEditable, ShowSwitch: showSwitch, FolderName: documentsInfo.FolderName, ParentFolderName: documentsInfo.ParentFolderName}
                        }} >
                            <i className={documentsInfo.Icon + " document-icon m-0"} alt="order" style={{ fontSize: "1.5rem" }}></i>
                        </Link>
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
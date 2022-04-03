import React, { useState, useEffect } from 'react';
import { Link, useHistory } from 'react-router-dom';
import { FileSelectChange } from "../Utility/document-fuctions.jsx"
import * as Routes from '../Constants/Routes.jsx'
import 'regenerator-runtime/runtime'
import { useForm, useFormContext } from 'react-hook-form';


export default function DocumentsCard(props) {
    const history = useHistory();

    const formMethods = useFormContext();

    const [state, setState] = useState({
        documentsInfo: props.documentsInfo,
        fileRequired: props.fileRequired
    });
    var id = state.documentsInfo?.ObjectID == null ? document.querySelector(".hidden-guid").getAttribute("value") : state.documentsInfo.ObjectID;

    async function uploadFile(e) {
        var formID = e.target.closest("form").getAttribute("id");
        console.log(formID)
        var response = await FileSelectChange(e.target, state.documentsInfo.FolderName, state.documentsInfo.ParentFolderName, id, formID)
        setState({ ...state, documentsInfo:response })
    }

    useEffect(() => {
        formMethods.setValue("HasFiles", state.documentsInfo.FileStrings?.length > 0)
    }, [state.documentsInfo])

    function ResetDocInfo(newdocumentInfo) {
        console.log("reset doc info")
        console.log(newdocumentInfo)
        setState({ ...state, documentsInfo: newdocumentInfo })
        
    }

    var pathname = (history.entries[1] && history.entries[1].pathname != Routes.DOCUMENTS) ? history.entries[1].pathname : ""
    var hasFile = state.documentsInfo.FileStrings?.length > 0 ? true : false;
    var iconFilter = hasFile ? "section-filter" : "disabled-filter"
        
    return (
        <div>
            <input type="hidden" name="HasFiles" defaultValue={hasFile} {...formMethods.register("HasFiles", { required: value => value === true })} />
            {console.log("in doc card return:" + state.documentsInfo.FolderName)}
            <div className={"card document-border " + (hasFile ? "hasFile" :"")}>
            <div className="document-card text-center pb-2" >
                <div className="col-4 pr-0 d-flex align-items-end mb-1">
                        <label className="control-label">{hasFile ? state.documentsInfo.FileStrings.length : 0} Files</label>
                </div>
                    <div className="col-4 d-flex align-items-center">
                        <Link className="mark-edditable" to={{
                            pathname: pathname + "/DocumentsModal",
                            state: { ID: id, IsEditable: props.isEditable, ShowSwitch: props.showSwitch, FolderName: state.documentsInfo.FolderName, ParentFolderName: state.documentsInfo.ParentFolderName, resetDocInfo: ResetDocInfo }
                        }} >
                            <i className={state.documentsInfo.Icon + " document-icon m-0 mark-readonly " + iconFilter} alt="order" style={{ fontSize: "1.5rem" }}></i>
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
            <label className="control-label text-center text document-text-margin" style={{ width: "100%" }}>{state.documentsInfo.FolderName}</label>
    </div>
        )
}
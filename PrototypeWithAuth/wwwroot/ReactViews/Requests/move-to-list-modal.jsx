import { useState, useEffect } from 'react';
import { useLocation, Link, useHistory } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { ajaxPartialIndexTable, getRequestIndexString } from '../Utility/root-function.jsx'
import * as ModalKeys from '../Constants/ModalKeys.jsx'
import { NEW_LIST } from '../Constants/Routes.jsx'
import GlobalModal from '../Utility/global-modal.jsx';


export default function MoveToListModal(props) {
    const dispatch = useDispatch();
    const location = useLocation();
    const history = useHistory();
    const [state, setState] = useState({ viewModel: null, requestID: location.state.ID, modelsEnum: location.state.modelsEnum });

    var origClasses = "list-name  list-group-item mb-2";
    var classes = origClasses + " selected";
    var newListFilter = "section-filter";

    console.log(location.state)
    useEffect(() => {
        var prevListID = document.getElementById("ListID")?.value;
        var url = "/Requests/MoveToListModalJson/?requestID=" + state.requestID
        if (prevListID != null) {
            url = url + "&prevListID=" + prevListID
        }
        url = url + "&" + getRequestIndexString();

        fetch(url, {
            method: "GET"
        })
            .then((response) => { return response.json(); })
            .then(result => {
                setState({ ...state, viewModel: JSON.parse(result) });
            });

    }, [state.requestID]);

    useEffect(() => {
        console.log(state.viewModel)
        if (state.viewModel?.RequestLists?.length == 5 || (state.viewModel?.PreviousListID != 0 && state.viewModel?.RequestLists?.length == 4)) {
            newListFilter = "disabled disabled-filter";
        }
    }, [state.viewModel]);

    var onClick = (e) => {
        e.preventDefault();
        e.stopPropagation();
        console.log(e.target.getAttribute("listid"))
        console.log(e.target.attributes)
        var url = "/Requests/MoveToListModal";
        document.getElementById("NewListID").value = e.target.getAttribute("listid");
        var formData = new FormData(document.getElementsByClassName("moveListItemForm")[0]);
        document.getElementById("loading").style.display = "block";
        ajaxPartialIndexTable(dispatch, url, "POST", formData, [ModalKeys.MOVE_TO_LIST])
    }


        return (
            <GlobalModal backdrop={props.backdrop} hideModalFooter={true} size="" value={state.viewModel?.ID} modalKey={props.modalKey} key={state.viewModel?.ID} header={"Move " + state.viewModel?.Request?.Product?.ProductName + " To List"} >
                <form action="" method="post" className=" moveListItemForm" encType="multipart/form-data" id={props.modalKey}>
                    <input type="hidden" value={state.viewModel?.Request?.RequestID??""} name="Request.RequestID" className="request-to-move" />
                    <input type="hidden" value={state.viewModel?.PreviousListID??""} name="PreviousListID" />
                    <input type="hidden" value={state.viewModel?.PageType??""} name="PageType" />
                    <input type="hidden" value={state.viewModel?.NewListID??""} name="NewListID" id="NewListID"  />
                    <div className="contaner-fluid p-0">
                        <div className="error-message text-danger-centarix row">
                            {state.viewModel?.ErrorMessage ?? ""}
                        </div>
                        <div className="row">
                            <div className="col " style={{ overflow: "scroll", maxHeight: "15rem" }}>
                                <ul className="list-group">
                                    {
                                        state.viewModel?.RequestLists?.map((v, i) => {
                                        
                                            return (
                                                <li key={i} className={classes} onClick={onClick} listid={state.viewModel?.RequestLists[i]?.ListID} id={"List" + state.viewModel?.RequestLists[i]?.ListID}>
                                                    <input type="hidden" value={state.viewModel?.RequestLists[i]?.ListID??""} name={"RequestLists[" + i + "].ListID"} />   <input type="hidden" value={state.viewModel?.RequestLists[i]?.ListID??""} name={"RequestLists[" + i + "].ListID"} />
                                                    <a className="d-block bold-hover-effects">
                                                        {state.viewModel?.RequestLists[i]?.Title}
                                                    </a>
                                                </li>)
                                            classes = origClasses;
                                        })
                                    }
                                </ul>

                            </div>
                        </div>
                        <div className="row">
                            <div className="col text-center">     
                                <Link className={"btn-link text no-hover requests add-new-list fill-list text-center " + newListFilter} to={{
                                    pathname: history.entries[1].pathname + NEW_LIST, state: { requestToAddId: state.viewModel?.Request.RequestID, requestPreviousListID: state.viewModel?.PreviousListID }
                                }}>
                                    <i className="icon-add_circle_outline-24px1 " style={{fontSize: "24px"}}></i>
                                    <label className="new-button-text">New List</label>
                                </Link>
                            </div>
                        </div>
                    </div>
                </form>
            </GlobalModal>
        );

}

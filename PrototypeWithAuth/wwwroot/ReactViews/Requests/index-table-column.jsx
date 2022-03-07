import React, { useState } from 'react';
import { Link, useHistory, useLocation } from 'react-router-dom';
import { MDBBtn, MDBPopover, MDBPopoverBody } from 'mdbreact';
import * as Routes from '../Constants/Routes.jsx'
export default function IndexTableColumn(props) {
    const history = useHistory();
    const [state, setState] = useState({ col: props.columnData, vendorID: props.vendorID, sideBar: props.sideBar });

    const getInsideView=(col, sideBar)=> {
        var imgDangerColor = col.Image.includes("error") ? " text-danger-centarix " : "";
        if (col.Image != "") {

            return (<img src={col.Image} alt="Image" width="75" className={"category-image " + imgDangerColor} />)
        }
        else if (col.Icons != null) {
            var popoverID = col.AjaxID + "accNotification";

            return (
                <div className="d-inline-flex">
                    {col.Icons.map((icon, i) => {
                        if (icon.IconPopovers?.length >= 1) {
                          
                            return (
                                <div key={"icon" + i} className="table-icon-div">
                                    <MDBPopover
                                        placement="bottom"
                                        popover
                                        clickable
                                    >
                                        <MDBBtn flat className={"btn p-0 m-0 no-box-shadow "} value={col.AjaxID + "more"}> <i className={icon.IconClass + " hover-bold px-1"} style={{ fontSize: "2rem" }}></i>
                                        </MDBBtn>
                                        <div>
                                            <MDBPopoverBody>
                                                {icon.IconPopovers.map((iconpopover, i) => (
                                                    <div key={"iconPopover" + i} className="row px-3 icon-more-popover accounting-popover">
                                                        <Link className={"btn-link popover-text no-hover requests " + iconpopover.AjaxCall} to={{ pathname: history.location.pathname + iconpopover.AjaxCall, state: { ID: col.AjaxID, newStatus: iconpopover.Description, modelsEnum: "Request" } }} >
                                                            <i className={iconpopover.Icon} style={{ color : ""+iconpopover.Color }}></i>
                                                            <label className="m-2 ">{iconpopover.DescriptionDisplayName}</label>
                                                        </Link>
                                                    </div>
                                                )
                                                )}
                                            </MDBPopoverBody>
                                        </div>
                                    </MDBPopover>
                                </div>
                            );
                        }
                        else if (icon.IconClass == "Clarify") {
                            return (<div key={"icon" + i} className="table-icon-div">
                                <ul className="list-unstyled p-0 m-0">
                                    <li>
                                       
                                        <button type="button" data-toggle="popover" data-placement="bottom" data-container="body" data-trigger="focus" className="btn p-0 m-0 no-box-shadow accNotification" value={popoverID}>
                                            <i className="icon-notification_didnt_arrive-24px green-overlay px-1" style={{ fontSize: "1.6rem" }}></i>
                                        </button>

                                    </li>
                                    <div style={{ display: "none" }} id={popoverID}>
                                        <div className="container">
                                            <div className="row border py-3 px-4 mb-3">{col.Note}</div>
                                            <div class="row small-text ">
                                                Has the order been clarified?
                                            </div>
                                            <div className="row mt-2 text-center">

                                                <div className="col-6 p-1">
                                                    <a href={"/Requests/HandleNotifications?requestID=" + col.AjaxID + "&type=" + sideBar}
                                                        className="rounded-pill small-text p-0 m-0 btn text-capitalize  handleNotification "
                                                        style={{ width: " 100%", border: " 1px solid var(--acc-color)", backgroundColor: "transparent", color: " var(--acc-color)" }}>Save</a>
                                                </div>
                                                <div className="col-6 p-1 ">
                                                    <button className="rounded-pill p-0 m-0 small-text" style={{ backgroundColor: "transparent", borderWidth: "1px !important", width: "100%" }}>
                                                        Cancel
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ul>
                            </div>)
                        }
                        else if (icon.IconClass == "Resend") {
                            return (<div key={"icon" + i} className="pr-2">
                                <button className="confirm-quote-resend btn-icon section-bg-color" data-title="Resend" value={col.AjaxID}>Resend</button>
                            </div>);
                        }
                        else if (icon.IconClass == "Placeholder" && sideBar == "Quotes") {
                            return (<div key={"icon" + i} className="px-5" style={{ minWidth: "125px" }}>&nbsp;</div>);
                        }
                        else {
                            return (<div key={"icon" + i} className="table-icon-div">
                                <Link className={"btn p-0 m-0 no-box-shadow requests " + icon.IconAjaxLink} to={{ pathname: icon.IconAjaxLink, state: { ID: col.AjaxID} }} data-toggle="tooltip" data-placement="top"
                                    title={icon.TooltipTitle} value={col.AjaxID}>
                                    <i style={{ fontSize: "2rem", color: icon.Color }} className={icon.IconClass + " hover-bold"}></i>
                                </Link>
                            </div>
                            )
                        }

                    })}
                </div>
            );
        }

        else {
            return (
                col.ValueWithError.map((ve, i) => {

                    var dangerColor = ve.Bool ? " text-danger-centarix " : "";

                    if (ve.String == "Checkbox") {
                        return (<div key={"value" + i}><div className={"form-check accounting-select " + dangerColor}>
                            <input type="checkbox" className={"form-check-input fci-acc filled-in " + col.AjaxLink} id={col.AjaxID} vendorid={state.vendorID} />
                            <label className="form-check-label" htmlFor={col.AjaxID}></label>
                        </div>{i != 0 ? <br /> : null}</div>
                        )
                    }
                    else if ((col.AjaxLink != null && col.AjaxLink != "") || col.ShowTooltip == true) {
                        var title = col.ShowTooltip ? ve.String : "";

                        return (<div key={"value" + i}><a className={"btn p-0 m-0 inv-link-clr " + col.AjaxLink + " no-box-shadow"} data-toggle="tooltip" data-placement="top" title={title} value={col.AjaxID} data-target="item" href="#/">
                            <div className="d-block">
                                <p className={"m-0 text-center " + dangerColor} style={{ overflow: "hidden", textOverflow: "ellipsis", WebkitLineClamp: "3", WebkitBoxDirection: "vertical", maxHeight: "5rem", display: " -webkit-box" }}>{ve.String}</p>
                            </div>
                        </a>{i != 0 ? <br /> : null}</div>)

                    }
                    else {
                        var textCase = {};
                        if (col.Title == "Amount") {
                            textCase = { textTransform: "none" };
                        }
                        return (<div key={"value" + i}><label className={"m-0 p-0 " + dangerColor} style={textCase}>{ve.String}</label>{i != 0 ? <br /> : null}</div>)
                    }
                })
            )
        }
    }


        return (
            <td width={state.col.Width + "%"} className={state.col.Width == 0 ? "p-0" : ""} >
                {console.log("in render of index table data")}
                {getInsideView(state.col, state.sideBar)}
            </td>
        )

}

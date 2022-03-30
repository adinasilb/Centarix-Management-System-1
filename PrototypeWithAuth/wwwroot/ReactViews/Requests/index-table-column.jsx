import React, { useState } from 'react';
import { Link, useHistory } from 'react-router-dom';
import { Popover, Typography, Tooltip } from '@mui/material';
import PopupState, { bindTrigger, bindPopover } from 'material-ui-popup-state';
import * as Routes from '../Constants/Routes.jsx'

export default function IndexTableColumn(props) {
    const history = useHistory();
    const [state, setState] = useState({ col: props.columnData, vendorID: props.vendorID, sideBar: props.sideBar });

    const getInsideView=(col, sideBar)=> {
        var imgDangerColor = col.Image.includes("error") ? " text-danger-centarix " : "";
        if (col.Image != "") {
            console.log(col.Image)
            return (<img src={col.Image} alt="Image" width="75" className={"category-image " + imgDangerColor} />)
        }
        else if (col.Icons != null) {
            var popoverID = col.AjaxID + "accNotification";

            return (
                <div className="d-inline-flex">
                    {col.Icons.map((icon, i) => {
                        if (icon.IconPopovers?.length >= 1) {
                          
                            return (
                                <PopupState key={"icon" + i} variant="popover" popupId={col.AjaxID + "more"}>
                                    {(popupState) => (
                                <div className="table-icon-div">                             
                                            <i className={icon.IconClass + " hover-bold px-1"} style={{ fontSize: "2rem" }} aria-describedby={col.AjaxID + "more"}  {...bindTrigger(popupState)}></i>
                       
                                            <Popover
                                                
                                        id={col.AjaxID + "more"}
                                        {...bindPopover(popupState)}
                                        anchorOrigin={{
                                            vertical: 'bottom',
                                            horizontal: 'center',
                                        }}
                                            >
                                               
                                        <Typography sx={{ p: 2 }}> {icon.IconPopovers.map((iconpopover, i) => (
                                            <span key={"iconPopover" + i} className="row px-3 icon-more-popover accounting-popover">
                                                <Link className={" popover-text requests black-87 " + iconpopover.AjaxCall} to={{ pathname: history.location.pathname + iconpopover.AjaxCall, state: { ID: col.AjaxID, newStatus: iconpopover.Description, modelsEnum: "Request" } }} >
                                                    <i className={iconpopover.Icon} style={{ color: "" + iconpopover.Color }}></i>
                                                    <label className="m-2 ">{iconpopover.DescriptionDisplayName}</label>
                                                </Link>
                                            </span>
                                        )
                                        )}</Typography>
                                    </Popover>

                                        </div>
                                    )}
                                </PopupState>
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
                                <Tooltip title={icon.TooltipTitle??""} arrow>
                                 <Link className={"d-flex requests " + icon.IconAjaxLink} to={{ pathname: icon.IconAjaxLink, state: { ID: col.AjaxID } }}  value={col.AjaxID}>
                                        <i style={{ fontSize: "2rem", color: icon.Color }} className={icon.IconClass + " hover-bold"}></i>
                                    </Link>
                                    </Tooltip>
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
                    var key = "value" + i + ve;
                    var dangerColor = ve.Bool ? " text-danger-centarix " : "";

                    if (ve.String == "Checkbox") {
                        return (<div key={key}><div className={"form-check accounting-select " + dangerColor}>
                            <input type="checkbox" className={"form-check-input fci-acc filled-in " + col.AjaxLink} id={col.AjaxID} vendorid={state.vendorID} />
                            <label className="form-check-label" htmlFor={col.AjaxID}></label>
                        </div>{i != 0 ? <br /> : null}</div>
                        )
                    }
                    else if ((col.AjaxLink != null && col.AjaxLink != "") || col.ShowTooltip == true) {
                        var title = col.ShowTooltip ? ve.String : "";

                        return (<div key={key}><Tooltip title={ title??""} arrow>
                            <a className={"btn p-0 m-0 inv-link-clr " + col.AjaxLink + " no-box-shadow"}    value={col.AjaxID}  href="#/">
                                <div className="d-block">
                                    <p className={"m-0 text-center " + dangerColor} style={{ overflow: "hidden", textOverflow: "ellipsis", WebkitLineClamp: "3", WebkitBoxDirection: "vertical", maxHeight: "5rem", display: " -webkit-box" }}>{ve.String}</p>
                                </div>
                            </a>
                        </Tooltip>{i != 0 ? <br /> : null}</div>)

                    }
                    else {
                        var textCase = {};
                        if (col.Title == "Amount") {
                            textCase = { textTransform: "none" };
                        }
                        return (<div key={key}><label className={"m-0 p-0 " + dangerColor} style={textCase}>{ve.String}</label>{i != 0 ? <br /> : null}</div>)
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

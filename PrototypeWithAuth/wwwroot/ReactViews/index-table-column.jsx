import React, { Component } from 'react';

export default class IndexTableColumn extends Component {

    constructor(props) {
        super(props);
        this.state = { col: this.props.columnData, vendorID: this.props.vendorID, sideBar: this.props.sideBar };
    }


    getInsideView = (col, sideBar) => {
        var imgDangerColor = col.Image.includes("error") ? " text-danger-                          centarix " : "";
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
                                    <ul className="list-unstyled p-0 m-0">
                                        <li>
                                            <button href="#" type="button" data-toggle="popover" data-placement="bottom" data-container="body" data-trigger="focus" className={"btn p-0 m-0 no-box-shadow " + icon.IconAjaxLink} value={col.AjaxID + "more"}>
                                                <i className={icon.IconClass + " hover-bold px-1"} style={{ fontSize: "2rem" }}>
                                                </i>
                                            </button>
                                        </li>
                                        <li>
                                            <div style={{ display: "none" }} id={col.AjaxID + "more"}>
                                                {icon.IconPopovers.map((iconpopover, i) => (
                                                    <div key={"iconPopover" + i} className="row px-3 icon-more-popover accounting-popover">
                                                        <a className={"btn-link popover-text no-hover requests " + iconpopover.AjaxCall} value={col.AjaxID} data-action={iconpopover.Action} data-controller={iconpopover.Controller} data-share-resource-id={col.AjaxID}
                                                            data-route-request={col.AjaxID} data-route-new-status={iconpopover.Description}>
                                                            <i className={iconpopover.Icon} style={{ color: iconpopover.Color + "" }}></i>
                                                            <label className="m-2 ">{iconpopover.DescriptionDisplayName}</label>
                                                        </a>

                                                    </div>
                                                )
                                                )}
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            );
                        }
                        else if (icon.IconClass =="Clarify") {
                            return (<div key= { "icon" + i } className="table-icon-div">
                                <ul className="list-unstyled p-0 m-0">
                                    <li>                                      
                                        <button type="button" data-toggle="popover" data-placement="bottom" data-container="body" data-trigger="focus" className="btn p-0 m-0 no-box-shadow accNotification" value={popoverID}>
                                            <i className="icon-notification_didnt_arrive-24px green-overlay px-1" style={{fontSize:"1.6rem"}}></i>
                                        </button>                                                        

                                    </li>
                                    <div style={{display:"none"}} id={popoverID}>
                                        <div className="container">
                                            <div className="row border py-3 px-4 mb-3">{col.Note}</div>
                                            <div class="row small-text ">
                                                Has the order been clarified?
                                            </div>
                                            <div className="row mt-2 text-center">

                                                <div className="col-6 p-1">
                                                    <a href={"/Requests/HandleNotifications?requestID=" + col.AjaxID + "&type=" + sideBar}
                                                        className="rounded-pill small-text p-0 m-0 btn text-capitalize  handleNotification "
                                                    style={{ width:" 100%", border:" 1px solid var(--acc-color)", backgroundColor: "transparent", color:" var(--acc-color)"}}>Save</a>
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
                        else if (icon.IconClass=="Resend") {
                            return (<div key={"icon" + i} className="pr-2">
                                <button className="confirm-quote-resend btn-icon lab-man-background-color" data-title="Resend" value={col.AjaxID}>Resend</button>
                            </div>);
                        }
                        else if (icon.IconClass == "Placeholder" && sideBar=="Quotes") {
                            return (<div key={"icon" + i} className="px-5" style={{ minWidth: "125px" }}>&nbsp;</div>);
                        }
                        else {
                            return (<div key={"icon" + i} className="table-icon-div">
                                <a className={"btn p-0 m-0 no-box-shadow requests " + icon.IconAjaxLink} data-toggle="tooltip" data-placement="top"
                                    title={icon.TooltipTitle} value={col.AjaxID}>
                                    <i style={{ fontSize: "2rem", color: icon.Color }} className={icon.IconClass + " hover-bold"}></i>
                                </a>
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
    
                    if (ve.String=="Checkbox") {
                        return (<div key={"value" + i}><div className={"form-check accounting-select pl-4" + dangerColor}>
                            <input type="checkbox" className={"form-check-input fci-acc filled-in " + col.AjaxLink} id={col.AjaxID} vendorid={this.state.vendorID} />
                            <label className="form-check-label" htmlFor={col.AjaxID}></label>
                        </div>{ i != 0 ? <br /> : null }</div>
                        )
                    }
                    else if ((col.AjaxLink != null && col.AjaxLink != "") || col.ShowTooltip == true) {
                        var title = col.ShowTooltip ? ve.String : "";

                        return (<div key={"value" + i}><a  className={"btn p-0 m-0 inv-link-clr " + col.AjaxLink + " no-box-shadow"} data-toggle="tooltip" data-placement="top" title={title} value={col.AjaxID} data-target="item" href="#/">
                            <div className="d-block">
                                <p className={"m-0 text-center " + dangerColor} style={{ overflow: "hidden", textOverflow: "ellipsis", WebkitLineClamp: "3", WebkitBoxDirection: "vertical", maxHeight: "5rem", display: " -webkit-box" }}>{ve.String}</p>
                            </div>
                        </a>{ i != 0 ? <br /> : null }</div>)

                    }
                    else {
                        var textCase = {};
                        if (col.Title == "Amount") {
                            textCase = { textTransform: "none" };
                        }
                        return (<div key={"value" + i}><label className={"m-0 p-0 " + dangerColor} style={textCase}>{ve.String}</label>{ i != 0 ? <br /> : null }</div>)
                    }
                })
            )
        }
    }

    render() {
        return (
            <td width={this.state.col.Width + "%"} className={this.state.col.Width == 0 ? "p-0" : ""} >
                {this.getInsideView(this.state.col, this.state.sideBar)}
            </td>
        )

    }
}

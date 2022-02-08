import React, { Component } from 'react';

export default class IndexTableColumn extends Component {

    constructor(props) {
        super(props);
        this.state = { col: this.props.columnData, vendorID: this.props.vendorID };
    }


    getInsideView = (col) => {
        var imgDangerColor = col.image.includes("error") ? " text-danger-                          centarix " : "";
        if (col.image != "") {

            return (<img src={col.image} alt="Image" width="75" className={"category-image " + imgDangerColor} />)
        }
        else if (col.icons != null) {
            var popoverID = col.AjaxID + "accNotification";
 
            return (
                <div className="d-inline-flex">
                    {col.icons.map((icon, i) => {
                        if (icon.iconPopovers?.length >= 1) {
                            return (
                                <div key={"icon" + i} className="table-icon-div">
                                    <ul className="list-unstyled p-0 m-0">
                                        <li>
                                            <button href="#" type="button" data-toggle="popover" data-placement="bottom" data-container="body" data-trigger="focus" className={"btn p-0 m-0 no-box-shadow " + icon.iconAjaxLink} value={col.ajaxID + "more"}>
                                                <i className={icon.iconClass + " hover-bold px-1"} style={{ fontSize: "2rem" }}>
                                                </i>
                                            </button>
                                        </li>
                                        <li>
                                            <div style={{ display: "none" }} id={col.ajaxID + "more"}>
                                                {icon.iconPopovers.map((iconpopover, i) => (
                                                    <div key={"iconPopover" + i} className="row px-3 icon-more-popover accounting-popover">
                                                        <a className={"btn-link popover-text no-hover requests " + iconpopover.ajaxCall} value={col.ajaxID} data-action={iconpopover.action} data-controller={iconpopover.controller} data-route-current-status={iconpopover.currentLocation} data-share-resource-id={col.ajaxID}
                                                            data-route-request={col.ajaxID} data-route-new-status={iconpopover.description}>
                                                            <i className={iconpopover.icon} style={{ color: iconpopover.color + "" }}></i>
                                                            <label className="m-2 ">{iconpopover.descriptionDisplayName}</label>
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
                        else if (icon.IconClass?.Equals("Partial") ?? false) {
                            <div className="table-icon-div">
                                <ul className="list-unstyled p-0 m-0">
                                    <li>                                      
                                        <button type="button" data-toggle="popover" data-placement="bottom" data-container="body" data-trigger="focus" className="btn p-0 m-0 no-box-shadow accNotification" value={popoverID}>
                                            <i className="icon-remove_shopping_cart-24px green-overlay px-1" style={{fontSize:"1.6rem"}}></i>
                                        </button>                                                        

                                    </li>
                                    <div style="display:none;" id={popoverID}>
                                        <div className="container">
                                            <div className="row border py-3 px-4 mb-3">{col.note}</div>     
                                        </div>
                                    </div>
                                </ul>
                            </div>
                        }
                        else if (icon.IconClass?.Equals("Resend") ?? false) {
                            <div class="pr-2">
                                <button class="confirm-quote-resend btn-icon lab-man-background-color" data-title="Resend" value={col.AjaxID}>Resend</button>
                            </div>
                        }
                        else if (icon.IconClass?.Equals("Placeholder") ?? false) {
                            <div class="px-5" style="min-width: 125px">&nbsp;</div>
                        }
                        else {
                            return (<div key={"icon" + i} className="table-icon-div">
                                <a className={"btn p-0 m-0 no-box-shadow requests " + icon.iconAjaxLink} data-toggle="tooltip" data-placement="top"
                                    title={icon.tooltipTitle} value={col.ajaxID}>
                                    <i style={{ fontSize: "2rem", color: icon.color }} className={icon.iconClass + " hover-bold"}></i>
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
                col.valueWithError.map((ve, i) => {

                    var dangerColor = ve.bool ? " text-danger-centarix " : "";
                    if (i != 0) {
                        <br />
                    }
                    if (col.ValueWithError[val].String.Equals("Checkbox")) {
                        <div className="form-check accounting-select @dangerColor ">
                            <input type="checkbox" className={"form-check-input fci-acc filled-in " + col.ajaxLink} id={col.AjaxID} vendorid={this.state.vendorID} />
                            <label className="form-check-label" for={col.ajaxID}></label>
                       </div>
  
                    }
                    else if ((col.ajaxLink != null && col.ajaxLink != "") || col.showTooltip == true) {
                        var title = col.showTooltip ? ve.string : "";

                        return (<a key={"value" + i} className={"btn p-0 m-0 inv-link-clr " + col.ajaxLink + " no-box-shadow"} data-toggle="tooltip" data-placement="top" title={title} value={col.ajaxID} data-target="item" href="#/">
                            <div className="d-block">
                                <p className={"m-0 text-center " + dangerColor} style={{ overflow: "hidden", textOverflow: "ellipsis", webkitLineClamp: "3", webkitBoxOrient: "vertical", maxHeight: "5rem", display: " -webkit-box" }}>{ve.string}</p>
                            </div>
                        </a>)

                    }
                    else {
                        var textCase = {};
                        if (col.title == "Amount") {
                            textCase = { textTransform: "none" };
                        }
                        return (<label key={"value" + i} className={"m-0 p-0 " + dangerColor} style={textCase}>{ve.string}</label>)
                    }
                })
            )
        }
    }

    render() {
        return (
            <td width={this.state.col.width + "%"} className={this.state.col.width == 0 ? "p-0" : ""} >
                {this.getInsideView(this.state.col)}
            </td>
        )

    }
}

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import CloseButton, { CancelButton } from './close-button.jsx'
import { openModal } from './modal-functions.jsx'
export default class _IndexTableData extends Component {

    constructor(props) {
        super(props);

        this.state = { isLoaded: this.props.viewModel != undefined, viewModel: this.props.viewModel, showView: this.props.showView };
    }

    componentDidMount() {
        //if (this.state.showView && this.state.viewModel == undefined) {
        //    /*        if (this.state.isLoaded == true) {*/
        //    fetch("/Requests/_IndexTableData", {
        //        method: "GET"
        //    })
        //        .then((response) => { return response.text(); })
        //        .then(result => {
        //            this.setState({
        //                viewModel: result,
        //                isLoaded: true
        //            });
        //        });
        //}
    }

    getInsideView = (col) => {
        var imgDangerColor = col.image.includes("error") ? " text-danger-                          centarix " : "";
            if (col.image != "") {

             return(   <img src={col.image} alt="Image" width="75" className={"category-image " + imgDangerColor} />)
            }
            else if (col.icons != null) {
                return (
                    <div className="d-inline-flex">
                        {col.icons.map((icon,i )=> {
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
                                                        <div key={ "iconPopover"+i} className="row px-3 icon-more-popover accounting-popover">
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
                            else {
                                return (<div key= { "icon"+i } className="table-icon-div">
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
                    if ((col.ajaxLink != null && col.ajaxLink != "") || col.showTooltip == true) {
                        var title = col.showTooltip ? ve.string : "";

                        return (<a key={"value"+i} className={"btn p-0 m-0 inv-link-clr " + col.ajaxLink + " no-box-shadow"} data-toggle="tooltip" data-placement="top" title={title} value={col.ajaxID} data-target="item" href="#/">
                            <div className="d-block">
                                <p className={"m-0 text-center " + dangerColor} style={{ overflow: "hidden", textOverflow: "ellipsis", webkitLineClamp: "3", webkitBoxOrient: "vertical", maxHeight: "5rem", display: " -webkit-box" }}>{ve.string}</p>
                            </div>
                        </a>)

                    }
                    else {
                        var textCase = {};
                        if (col.title=="Amount") {
                            textCase = { textTransform: "none" };
                        }
                        return (<label key={"value" + i} className={"m-0 p-0 " + dangerColor} style={textCase}>{ve.string}</label>)
                    }
                })
)
            }
        }

    render() {
        if (this.state.isLoaded == true) {
            return (
                /*       return ReactDOM.createPortal(*/
                <div>
                    <div style={{ position: "absolute", left: "13rem", top: "6rem", zIndex: "1000" }}><span className="text danger-text error-msg"></span></div>
                    <br />
                    <div className="">
                        <input type="hidden" id="PageNumber" name="PageNumber" className="page-number" />
                        <table className="table table-headerspaced table-noheaderlines table-hover ">
                            <tbody className="scroll-tbody">
                                {this.state.viewModel.pagedList.map((r, i) => (
                                    <tr key={ "tr"+i} className="text-center one-row">
                                        {r.columns.map((col,i) => {
                           
                                            return(
                                                <td key={ "td"+i} width={col.width + "%"} className={col.width == 0 ? "p-0" : ""} >
                                                    {this.getInsideView(col)}
                                                </td>
                                                )
                                        })}
                                    </tr>
                               ))}
                            </tbody>
                        </table >
                    </div >


                </div>);
            //    ,
            //    document.getElementsByClassName("_IndexTab")[0]
            //);
        }
        else {
            return null;
        }

    }
}

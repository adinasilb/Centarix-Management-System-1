import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import CloseButton, { CancelButton } from './close-button.jsx'
import { openModal } from './modal-functions.jsx'
export default class DeleteModal extends Component {
    state = { isLoaded: false, view: "", showModal: false };


    componentWillUnmount() {
        console.log('unmount')
    }
    componentDidUpdate() {
        openModal("modal");
    }
    componentDidMount() {
        /*        if (this.state.isLoaded == true) {*/
        fetch("/Requests/_DeleteModal?id=4", {
            method: "GET"
        })
            .then((response) => { return response.text(); })
            .then(result => {
                this.setState({
                    view: result,
                    isLoaded: true,
                    showModal: true
                });
            });
        /*  }*/
    }


    //submit(e) {
    //        e.preventDefault();
    //        $("#loading").show();
    //        //var PageType = $('#masterPageType').val();
    //        var sidebarType = $('#masterSidebarType').val();
    //        var viewClass = "._IndexTableData";
    //        if (sidebarType == "Quotes" || sidebarType == "Orders" || sidebarType == "Cart") {
    //            viewClass = "._IndexTableDataByVendor";
    //        }
    //        var formdata = new FormData($("#myForm")[0])
    //        $.fn.ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/DeleteModal", viewClass, "POST", formdata, "delete-item");
    //        return false;
    //}

    render() {
        if (this.state.showModal == true) {
            return ReactDOM.createPortal(
                <div className="modal modal-view" id="myModal" role="dialog" aria-labelledby="Request" data-backdrop="false">

                    <div className="modal-dialog-centered modal-lg mx-auto " role="document" style={{maxHeight:"100%",  overflowY:"auto"}}>

                        <div className="modal-content d-inline-block modal-border-radius modal-box-shadow ">
                            <div className="close-button"><CloseButton /></div>
                            <form action="" method="post" encType="multipart/form-data" style={{ height: "100%", overflow: "auto" }} className="modal-padding" id="myForm">
                                <div dangerouslySetInnerHTML={{ __html: this.state.view }} />
                                <div className="modal-footer">
                                    <div className="text-center mx-auto modal-footer-mt">
                                        <button type="submit" className="custom-button custom-button-font @bcColor between-button-margin submit-delete" value="@Model.Request.RequestID">Confirm</button>
                                        <CancelButton />
                                    </div>
                                </div>

                            </form>
                        </div>
                    </div>
                </div>,
                document.getElementsByClassName("delete-item")[0]
            );
        }
        else {
            return null;
        }

    }
}

//if (typeof window !== 'undefined') {
//    ReactDOM.render(<DeleteModal />, document.getElementById("deleteItem"));
//}

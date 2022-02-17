

const openModal = function (modalClass) {
    var modalElements = document.getElementsByClassName(modalClass);
    var showBackDrop = "static";
    if (document.querySelector(".modal-backdrop")!=null) {
        showBackDrop = false;
    }
    var modalElement = modalElements[0]
    if (modalElement != undefined) {
        var myModal = new bootstrap.Modal(modalElement, {
            backdrop: showBackDrop,
            keyboard: false,
        });
        myModal.show()
    }
    //ReactDOM.render(<CloseButton/>,
    //    document.getElementsByClassName("close-button")[0]
    //);
}


const removeExtraModalBackDrop = function (history) {
    var modalElements = document.getElementsByClassName("modal");
    if (modalElements.length == 0) {
        if (document.getElementsByClassName("modal-backdrop")[0] != null) {
            history.push("/fallback");
        }
        document.getElementsByClassName("modal-backdrop")[0]?.remove();
        document.getElementsByClassName('body')[0].classList.remove('modal-open');


    }
    //we will deal with this later
    //$("body, .modal").bind("click");
}

export { openModal, removeExtraModalBackDrop};
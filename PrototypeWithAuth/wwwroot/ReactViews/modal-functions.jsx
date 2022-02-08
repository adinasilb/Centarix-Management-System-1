﻿

var openModal = function (modalClass) {
    var modalElement = document.getElementsByClassName(modalClass)[0];
    if (modalElement != undefined) {
        var myModal = new bootstrap.Modal(modalElement, {
            backdrop: true,
            keyboard: false,
        });
        myModal.show()
    }
    //ReactDOM.render(<CloseButton/>,
    //    document.getElementsByClassName("close-button")[0]
    //);
}


var closeModal = function (modalClass) {
    var modalElements = document.getElementsByClassName("modal");
    console.log(modalElements)
    if (modalElements.length == 1) {
        alert("here")
        document.getElementsByClassName("modal-backdrop")[0].remove();
        document.getElementsByClassName('body')[0].classList.remove('modal-open');
    }
    //we will deal with this later
    //$("body, .modal").bind("click");
}

export { openModal, closeModal};
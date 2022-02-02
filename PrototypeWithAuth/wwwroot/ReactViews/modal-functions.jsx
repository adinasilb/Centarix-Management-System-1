import ReactDOM, {  unmountComponentAtNode, findDOMNode } from "react-dom";

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
export { openModal };
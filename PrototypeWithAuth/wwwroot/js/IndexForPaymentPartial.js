
$(".load-vendor-edit").on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
    var $itemurl = "";

    console.log("Vendors/Edit/?id")
    //takes the item value and calls the Products controller with the ModalView view to render the modal inside
    $itemurl = "/Vendors/Edit/?id=" + $(this).attr('val') + '&SectionType=' + $(this).attr('sectionType');

    $.fn.CallPageRequest($itemurl, "details");
    return false;
});
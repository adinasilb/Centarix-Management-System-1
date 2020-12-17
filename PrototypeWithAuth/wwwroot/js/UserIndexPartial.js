
$(".open-user-modal").on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
    var $itemurl = "/Admin/EditUser/?id=" + $(this).attr("value");
    console.log("itemurl: " + $itemurl);
    $.fn.CallPageUser($itemurl);
    return false;
});
$.fn.SuspendUserModal = function (userid, suspend) {
    console.log("in delete user modal function");
    $itemurl = '/Admin/SuspendUserModal?id=' + userid;
    $.fn.CallPageUser($itemurl, '');
    return false;
};
$(".prevent-js-reload").on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    return false;
});

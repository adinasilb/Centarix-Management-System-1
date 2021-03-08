$(function () {

    $(".open-user-modal").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        $("#loading").show();
        var $itemurl = "/Admin/EditUser/?id=" + $(this).attr("value");
        console.log("itemurl: " + $itemurl);
        $.fn.CallPageUser($itemurl);
        return false;
    });
    $(".suspend-user-modal-icon").click(function (e) {
        e.preventDefault();
        $.fn.SuspendUserModal($(this).attr('data-userid'), null);
    });
    $.fn.SuspendUserModal = function (userid, suspend) {
        $itemurl = '/Admin/SuspendUserModal?id=' + userid;
        $.ajax({
            async: true,
            url: $itemurl,
            type: 'GET',
            cache: true,
            success: function (data) {
                //console.log("success!");
                $.fn.OpenModal('suspend-user-modal', 'suspend-user', data)
            }
        });
        return false;
    };
    $(".prevent-js-reload").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    });

});
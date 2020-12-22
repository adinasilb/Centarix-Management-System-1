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
    $("body").on("click", ".suspend-user-modal-icon", function (e) {
        e.preventDefault();
        alert("suspend user modal click");
        $.fn.SuspendUserModal($(this).attr('data-userid'), null);
    });
    $.fn.SuspendUserModal = function (userid, suspend) {
        console.log("in delete user modal function");
        $itemurl = '/Admin/SuspendUserModal?id=' + userid;
        $('.suspend-user-modal').replaceWith('');
        $(".modal-backdrop").remove();
        $.ajax({
            async: true,
            url: $itemurl,
            type: 'GET',
            cache: true,
            success: function (data) {
                //$('.suspend-user-modal').replaceWith('');
                //$(".modal-backdrop").remove();
                //console.log("success!");
                var modal = $(data);
                $('body').append(modal);
                $(".modal-view").modal({
                    backdrop: true,
                    keyboard: true,
                });
                $(".suspend-user-modal").modal('show');
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
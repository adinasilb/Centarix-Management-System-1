$(".approve-hours").off('click').click(function (e) {
	$.ajax({
		async: true,
		url: "ApproveHours" + '?id=' + $(this).val(),
		type: 'GET',
		cache: false,
		success: function (data) {
			$(".render-partial").html(data);
		}
	});
});
$('body').on('click', '.deny-approval-modal-icon', function (e) { e.preventDefault(); }).click();

function OpenDenyApprovalModal (id) {
    //e.preventDefault();
    $itemurl = '/ApplicationUsers/DenyApprovalRequestModal?ehaaId=' + id;
    console.log($itemurl);
    $.ajax({
        async: true,
        url: $itemurl,
        type: 'GET',
        cache: true,
        success: function (data) {
            $.fn.OpenModal('deny-approval-modal', 'deny-approval', data)
        }
    });
    return false;
};

$("#submit-deny-approval-request").click(function (e) {
    e.preventDefault();
    var formData = new FormData($("#denyForm")[0]);
    $itemurl = '/ApplicationUsers/DenyApprovalRequestModal';
    $.ajax({
        processData: false,
        contentType: false,
        data: formData,
        async: true,
        url: $itemurl,
        type: 'POST',
        cache: true,
        success: function (data) {
            $.fn.CloseModal('deny-approval');
            $(".render-partial").html(data);
        }
    });
    return false;
});


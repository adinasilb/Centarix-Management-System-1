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
    $('.modal').replaceWith('');
    $(".modal-backdrop").remove();
    console.log($itemurl);
    $.ajax({
        async: true,
        url: $itemurl,
        type: 'GET',
        cache: true,
        success: function (data) {
            $('body').append(data);
            $(".modal").modal({
                backdrop: true,
                keyboard: true,
            });
            $(".modal").modal('show');
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
            $(".deny-approval-modal").modal('hide');
            $(".render-partial").html(data);
        }
    });
    return false;
});


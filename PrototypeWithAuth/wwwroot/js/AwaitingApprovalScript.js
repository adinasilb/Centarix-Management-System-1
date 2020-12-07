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


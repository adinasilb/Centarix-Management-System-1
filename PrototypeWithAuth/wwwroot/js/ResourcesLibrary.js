$(function () {
	$(".add-resource").on("click", function (e) {
		var url = "/Protocols/AddResource?ResourceType=";
		if ($(".articles").hasClass("active")) {
			url += "1";
		}
		else if ($(".resources").hasClass("active")) {
			url += "2";
		}
		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			cache: true,
			success: function (data) {
				$.fn.OpenModal('add-resource-modal', 'add-resources', data)
			}
		});
	});
});
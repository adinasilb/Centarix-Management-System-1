$(function () {

	$("#createPieChart").click(function (e) {
		e.preventDefault();
		$.fn.getChart("/Expenses/_PieChart");
		return false;
	});
	$(".chart-dropdownlists").on("click", "#createGraphChart", function (e) {
		e.preventDefault();
		$.fn.getChart("/Expenses/_GraphChart");
		return false;
	});
	$.fn.getChart = function(url)
	{
		var formData = new FormData($(".chartForm")[0]);
		$.ajax({
			url: url,
			method: 'POST',
			data: formData,

			success: function (data) {
				$('.chartDiv').html(data);
			},
			processData: false,
			contentType: false
		});
	}
})
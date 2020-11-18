$(function () {

	$("#createPieChart").click(function (e) {
		e.preventDefault();
		var formData = new FormData($(".chartForm")[0]);
		$.ajax({
			url: "/Expenses/_PieChart",
			method: 'POST',
			data: formData,		
			
			success: function (data) {
				$('.chartDiv').html(data);
			},
			processData: false,
			contentType: false
		});
		return false;
	});

})
$(function () {
	$.validator.setDefaults({
		ignore: ':not(select:hidden, input:visible, textarea:visible), [disabled]',
		errorPlacement: function (error, element) {
			if (element.hasClass('select-dropdown')) {
				error.insertAfter(element);
			} else {
				error.insertAfter(element);
			}
		}
	});

	$('.chartForm').validate({
		rules: {
			"SelectedYears": { selectRequired: true },
			"SelectedMonths": {
				selectRequired: true,
			},
			"Currency": {
				required: true,
			}
		}
	});

	$("#createPieChart").click(function (e) {
		//alert("validate form");
		e.preventDefault();
		$(".chartForm").data("validator").settings.ignore = "";
		var valid = $(".chartForm").valid();
		console.log("valid form: " + valid)
		if (!valid) {
			$(".chartForm").data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
			if (!$('input[type="submit"], button[type="submit"] ').hasClass('disabled-submit')) {
				$('input[type="submit"], button[type="submit"] ').addClass('disabled-submit')
			}
			return false;
		}
		else {
			$('input[type="submit"], button[type="submit"] ').removeClass('disabled-submit')
			$.fn.getChart("/Expenses/_PieChart");
			return true;
		}
		
	});
	$(".chart-dropdownlists").on("click", "#createGraphChart", function (e) {
		e.preventDefault();
		//alert("validate form");
		$(this).data("validator").settings.ignore = "";
		var valid = $(this).valid();
		console.log("valid form: " + valid)
		if (!valid) {
			$(this).data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
			if (!$('input[type="submit"], button[type="submit"] ').hasClass('disabled-submit')) {
				$('input[type="submit"], button[type="submit"] ').addClass('disabled-submit')
			}
			return false;
		}
		else {
			$('input[type="submit"], button[type="submit"] ').removeClass('disabled-submit')
			$.fn.getChart("/Expenses/_GraphChart");
			return true;
		}
	
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
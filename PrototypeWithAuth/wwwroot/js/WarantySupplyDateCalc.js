
$(".expected-supply-days").change(function () {
	var selector = "input[name='expected-supply-days']";
	if ($(this).attr("index") != null) {
		selector += "." + $(this).attr("index");
	}
	//console.log(this)
	$.fn.SetExpectedSupplyDateFromDays(selector, $(this));

});
$.fn.SetExpectedSupplyDateFromDays = function (selector, thisElement) {
	var OrderDate;
	if ($('.for-supply-date-calc').length > 0) {
		OrderDate = moment($('.for-supply-date-calc').val(), 'D MMM YYYY');

	} else {
		OrderDate = moment();
	}
	var supplyDate = OrderDate.add(thisElement.val(), "d").format("D MMM YYYY");
	console.log(supplyDate)
	$(selector).val(supplyDate);
	$(selector).attr("data-val", supplyDate/*.split("/").reverse().join("-")*/);
}

$(".expected-supply-date").change(function () {
	var selector = ".expected-supply-days";
	if ($(this).attr("index") != null) {
		selector += "." + $(this).attr("index");
	}
	console.log(selector)
	$.fn.SetExpectedSupplyDaysFromDate(selector, $(this));
});
$.fn.SetExpectedSupplyDaysFromDate = function(selector, thisElement) {
	var val = thisElement.val();
	console.log(val);
	//val = val.split("/").reverse().join("-");
	var date = moment(val, 'D MMM YYYY');
	date.set("time", 0)
	var OrderDate;
	if ($('.for-supply-date-calc').length > 0) {
		console.log("in first of of check roder date")
		OrderDate = moment($('.for-supply-date-calc').val(), 'D MMM YYYY');
	} else {
		OrderDate = moment();
	}
	if (date < OrderDate && !OrderDate.isSame(date, 'day')) {
		return;
	}
	var amountOfDays = Math.ceil(date.diff(OrderDate, 'days', true))
	$(selector).val(amountOfDays);
}
$(".warranty-months").change(function () {
			var date; 
		if ($("#Request_ParentRequest_OrderDate").length > 0) {
			console.log("in first of of check roder date")
			date =moment($("#Request_ParentRequest_OrderDate").val().split("/").reverse().join("-"));
			
		} else {
			date = moment();
			
		}
		var warrantyDate = date.add($(this).val(), "M").format('DD/MM/yyyy');
		$("input[name='WarrantyDate']").val(warrantyDate);
		$("input[name='WarrantyDate']").attr("data-val",warrantyDate.split("/").reverse().join("-"));
	});
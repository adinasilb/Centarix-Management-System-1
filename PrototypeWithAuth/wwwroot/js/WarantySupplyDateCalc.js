
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
/*	if ($('.for-supply-date-calc').length > 0) {
		OrderDate = moment($('.for-supply-date-calc').val()*//*.split("/").reverse().join("-")*//*);

	} else {*/
		OrderDate = moment();
//	}
	var supplyDate = OrderDate.add(thisElement.val(), "d").format("D MMM YYYY");
	console.log(supplyDate)
	$(selector).val(supplyDate);
	$(selector).attr("data-val", supplyDate/*.split("/").reverse().join("-")*/);
}

$("#expected-supply-date").change(function () {
	var selector = ".expected-supply-days";
	if ($(this).attr("index") != null) {
		selector += "." + $(this).attr("index");
	}
	$.fn.SetExpectedSupplyDaysFromDate(selector, $(this));
});
$.fn.SetExpectedSupplyDaysFromDate = function(selector, thisElement) {
	var val = thisElement.val();
	val = val.split("/").reverse().join("-");
	var date = moment(val);
	date.set("time", 0)
	if (date < moment() && !moment().isSame(date, 'day')) {
		return;
	}
	var OrderDate;
	//if ($('.for-supply-date-calc').length > 0) {
	//	console.log("in first of of check roder date")
	//	OrderDate = moment($('.for-supply-date-calc').val().split("/").reverse().join("-"));

	//} else {
		OrderDate = moment();

	//}
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
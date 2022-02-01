$(function () {
    $(".order-type").off("click").on("click", function (e) {
        e.preventDefault()
        var selectedStyles = "custom-button-font oper-background-color"
        var unselectedStyles = "custom-oper"
        if ($(this).attr("id") == "single") {
            $(".recurring-info").addClass("d-none")
            $("#single").addClass(selectedStyles).removeClass(unselectedStyles)
            $("#recurring").addClass(unselectedStyles).removeClass(selectedStyles)
            $("#OrderType").val("SingleOrder")
            ReplaceProductTypeNames("RecurringOrder", "SingleOrder")

        }
        else {
            $(".recurring-info").removeClass("d-none")
            //if standing order is checked - ordertype = 3
            $("#OrderType").val("RecurringOrder")
            $("#single").addClass(unselectedStyles).removeClass(selectedStyles)
            $("#recurring").addClass(selectedStyles).removeClass(unselectedStyles)
            ReplaceProductTypeNames("SingleOrder", "RecurringOrder")
        }
    })
    $('.ordersItemForm').on('click', "#alreadyPaid", function (e) {
        console.log("pay")
        if ($("#alreadyPaid:checkbox").is(":checked")) {
            $(".paid-info").addClass("d-none")
            $("#alreadyPaid:checkbox").prop("checked", false)
            $("#Paid").val(false)
        }
        else {
            $(".paid-info").removeClass("d-none")
            $("#alreadyPaid:checkbox").prop("checked", true)
            $("#standingOrder:checkbox").prop("checked", false)
            $("#Paid").val(true)
        }

    });
    $('.ordersItemForm').on('click', "#standingOrder", function (e) {
        console.log("pay")
        if ($("#standingOrder:checkbox").is(":checked")) {
            $(".paid-info").addClass("d-none")
            $("#standingOrder:checkbox").prop("checked", false)
            $("#OrderType").val("StandingOrder")
            ReplaceProductTypeNames("StandingOrder", "RecurringOrder")
        }
        else {
            $(".paid-info").removeClass("d-none")
            $("#standingOrder:checkbox").prop("checked", true)
            $("#alreadyPaid:checkbox").prop("checked", false)
            $("#Paid").val(false)
            $("#OrderType").val("StandingOrder")
            ReplaceProductTypeNames("RecurringOrder", "StandingOrder")
        }

    });
    function ReplaceProductTypeNames(prevProductType, newProductType) {
        $("input, select").each(function () {
            var name = $(this).attr("name")
            if (name != null) {
                name = name.replace(prevProductType, newProductType)
                $(this).attr("name", name)
            }
        })
    }
})

$.fn.ajaxPartialIndexTable = function (status, url, viewClass, type, formdata, modalClass = "", /*months, years, */) {
    console.log("in ajax partial index call " + url);
    //alert('before bind filter')
    if ($('#searchHiddenForsForm').length) {
        var moreFormData = new FormData($('#searchHiddenForsForm')[0])
        console.log(...moreFormData);
        formdata = $.fn.CombineTwoFormDatas(moreFormData, formdata);
        console.log(...formdata);

    }
    if ($("#inventoryFilterContent").length) {
        var selectedFilters = $.fn.BindSelectedFilters("");
        console.log('in if')
    }
    var monthsString = "";
    var yearsString = "";
    var listString = "";
    var months = $("#Months").val();
    var years = $("#Years").val();
    var listID = $("#ListID").val();
    if (months != undefined) {
        months.forEach(month => monthsString += "&months=" + month)
    }
    if (years != undefined) {
        years.forEach(year => yearsString += "&years=" + year)
    }
    if (listID != undefined) {
        listString += "&listID=" + listID
    }
    /*        var selectedPriceSort = [];
            $("#priceSortContent1 .priceSort:checked").each(function (e) {
                selectedPriceSort.push($(this).attr("enum"));
            })
            var selectedPriceSortObj = { "SelectedPriceSort": selectedPriceSort }
            console.log(selectedPriceSortObj)*/
    if (modalClass != "") {

        var classes = modalClass.split(",")
        console.log(modalClass)
        $.each(classes, function (index) {
            console.log(classes[index])
            $.fn.CloseModal($.trim(classes[index]));
        });

    } else {
        console.log("in else");
        /*formdata = {
            PageNumber: $('.page-number').val(),
            RequestStatusID: status,
            PageType: $('#masterPageType').val(),
            SectionType: $('#masterSectionType').val(),
            SidebarType: $('#masterSidebarType').val(),
            //SelectedPriceSort: selectedPriceSort,
            SelectedCurrency: $('#tempCurrency').val(),
            SidebarFilterID: $('.sideBarFilterID').val(),
            CategorySelected: $('#categorySortContent .select-category').is(":checked"),
            SubCategorySelected: $('#categorySortContent .select-subcategory').is(":checked"),
            months: months,
            years: years,
            IsArchive: isArchive
        };
        console.log(formdata);*/
        if (!url.includes("?")) {
            url += "?"
        } else {
            url += "&";
        }
        url += $.fn.getRequestIndexString(status);
        url += monthsString;
        url += yearsString;
        url += listString;
        //formdata = {}; //so won't crash when do object.assign()
        //console.log(formdata)
    }
    //var objectsToAdd = [];
    //if (selectedFilters != undefined/*should also somehow check if anything is chosen...*/) { objectsToAdd.push(selectedFilters) }
    //if (selectedPriceSortObj != undefined) { objectsToAdd.push(selectedPriceSortObj) }
    if (selectedFilters != undefined/*should also somehow check if anything is chosen...*/) {
        formdata = $.fn.AddObjectToFormdata(formdata, selectedFilters);
        console.log(...formdata);
    }

    var contentType = true;
    var processType = true;
    if (formdata != undefined) {
        type = "POST"
        processType = false;
        contentType = false;
    }

    $.ajax({
        contentType: contentType,
        processData: processType,
        async: true,
        url: url,
        data: formdata,
        traditional: true,
        type: type,
        cache: true,
        success: function (data) {
            $(viewClass).html(data);
            $(".tooltip").remove();
            $("#loading").hide();
            //workaround for price radio button not coming in when switching from nothing is here tab
            var id = "#nis";
            //alert($(id).prop('checked'))
            if ($('#tempCurrency').val() === "USD") {
                id = "#usd";
            }
            if ($(id).attr("checked") !== "checked") {
                console.log('checking button')
                $(id).attr("checked", "checked");
            }
            return true;
        },
        error: function (jqxhr) {
            $('.error-message').addClass("d-none");
            $('.error-message').html(jqxhr.responseText);
            $('.error-message:first').removeClass("d-none");
        }
    });

    return false;
}
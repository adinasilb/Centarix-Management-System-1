$(function () {
//FROM THE RECEIVED MODAL
    $("input:checkbox.p-c").on('click', function () {
        // in the handler, 'this' refers to the box clicked on
        var $box = $(this);
        if (!$box.is(":checked")) {
            console.log('in box checked')
            $box.prop("checked", false);
        } else {
            switch ($box.attr('name')) {
                case "PartialDelivery":
                    console.log('in partial')
                    $("#PartialDelivery").prop("checked", true);
                    $("#Clarify").prop("checked", false);
                    break;
                case "Clarify":
                    console.log('in clarify')
                    $("#PartialDelivery").prop("checked", false);
                    $("#Clarify").prop("checked", true);
                    break;
            }
        }
    });

    $(".open-sublocations-types").on("click", function () {
        var id = $(this).attr("id");
        loadReceivedModalSubLocations(id);
    });

    //AJAX load full partial view for modalview manage locations
    function loadReceivedModalSubLocations(val) {
        var myDiv = $(".divSublocations");
        $.ajax({
            //IMPORTANT: ADD IN THE ID
            url: "/Requests/ReceivedModalSublocations/?LocationTypeID=" + val,
            type: 'GET',
            cache: false,
            context: myDiv,
            success: function (result) {
                $(this).html(result);
                $('.visualView').html('');
            }
        });
    };





//FROM THE RECEIVED MODAL SUBLOCATIONS
    //$(".SLI-click").on("click", function () {
    //    alert("sli clicked!");
    //    SLI($(this));
    //});
    $(".modal").off("click", ".SLI-click").on("click", ".SLI-click", function () {
        SLI($(this));
    });

    function SLI(el) {
        //ONE ---> GET THE NEXT DROPDOWNLIST
        var nextSelect = $(el).parents('.form-group').nextAll().first().find('.dropdown-menu')
        $(nextSelect).html('');
        console.log(nextSelect)
        var locationInstanceParentId = $(el).val();;
        var url = "/Requests/GetSublocationInstancesList";/*/?LocationInstanceParentID=" + locationInstanceParentId;*/

        if (nextSelect != undefined) { //if there is another one
            $(nextSelect).html('');
            $(nextSelect).parents('.dropdown-main').find('span:not(.caret)').text('select');
            $.getJSON(url, { locationInstanceParentId, locationInstanceParentId }, function (result) {
                var item = "<li>Select Location Instance</li>";
                $.each(result, function (i, field) {
                    item += '<li value="' + field.locationInstanceID + '" id="' + field.locationInstanceID + ' "  class="SLI-click" >' + field.locationInstanceName + '</li>'
                });
                $(nextSelect).append(item);
            });

        }
        //TWO ---> FILL VISUAL VIEW
        var myDiv = $(".visualView");
        if (locationInstanceParentId == 0) { //if zero was selected
            console.log("selected was 0");
            //check if there is a previous select box
            var oldSelectClass = name.replace(place.toString(), (place - 1).toString());
            var oldSelect = $("select[name='" + oldSelectClass + "']");
            if (oldSelect.length) {
                console.log("oldSelectClass " + oldSelectClass + " exists and refilling with that");
                var oldSelected = $("." + oldSelect).children("option:selected").val();
                console.log("oldSelected: " + oldSelected);
                $.ajax({
                    url: "/Requests/ReceivedModalVisual/?LocationInstanceID=" + oldSelected,
                    type: 'GET',
                    cache: false,
                    context: myDiv,
                    success: function (result) {
                        $(this).html(result);
                    }
                });
            }
            else {
                console.log("oldSelectClass " + oldSelectClass + " does not exist and clearing");
                myDiv.html("");
            }
        }
        else {
            console.log("regular visual");
            $.ajax({
                url: "/Requests/ReceivedModalVisual/?LocationInstanceID=" + locationInstanceParentId,
                type: 'GET',
                cache: false,
                context: myDiv,
                success: function (result) {
                    $(this).html(result);
                }
            });
        }

        $(el).parents('.dropdown-main').find('span:not(.caret)').text($(el).text());
    };

});
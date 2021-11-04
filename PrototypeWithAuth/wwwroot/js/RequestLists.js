$(function () {

    $("#listSettings").click(function (e) {
        $.ajax({
            async: true,
            url: "/Requests/ListSettingsModal",
            traditional: true,
            type: "GET",
            cache: false,
            success: function (data) {
                $.fn.OpenModal("listSettingsModal", "list-settings", data)
                return true;
            },
            error: function (jqxhr) {
                $('.error-message').html(jqxhr.responseText);
            }
        });
    })

    $(".add-new-list").click(function (e) {
        var url = "/Requests/NewListModal";

        if ($(this).hasClass("fill-list")) {
            url = url + "/?requestToAddId=" + $(".request-to-move").val() +"&requestPreviousListID="+$("#ListID").val()
        }
        $.ajax({
            async: true,
            url: url,
            traditional: true,
            type: "GET",
            cache: false,
            success: function (data) {
                $.fn.OpenModal("newListModal", "new-list", data)
                return true;
            },
            error: function (jqxhr) {
                $('.error-message').html(jqxhr.responseText);
            }
        });
    })

    $(".change-list").off("click").on("click",function (e) {
        $("#ListID").val($(this).attr("value"))
        var url = "/Requests/_IndexTableWithListTabs/?" + $.fn.getRequestIndexString()
        $.fn.ajaxPartialIndexTable(-1,url, "._IndexTableListTabs", "GET")
    })

    $(".save-list").off("click").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        var valid = $('.listSettingsForm').valid();
        if (valid) {
            var formData = new FormData($(".listSettingsForm")[0]);
            var viewClass = "._IndexTableListTabs"
            $.fn.ajaxPartialIndexTable(-1, "/Requests/ListSettingsModal", viewClass, "POST", formData, "list-settings");
        }
    });

    $(".request-list-name").off("click").on("click", function (e) {
        var valid = $('.listSettingsForm').valid();
        if (valid) {
            var url = "/Requests/_ListSettings"
            var saveList = $(this).hasClass("changed")
            var newList = $(this);
            if (saveList) {
                url = "/Requests/SaveListModal/?listID=" + $(".request-list-name.selected").attr("listId");
                newList.addClass("next-list")
            }
            else {
                url = url + "/?listID=" + $(this).attr("listId");
                
            }
            $.ajax({
                url: url,
                method: 'GET',
                success: function (data) {
                    if (saveList) {
                        $.fn.OpenModal("save-list-modal", "save-list-settings", data);
                    }
                    else {
                        $(".request-list-name.selected").removeClass("selected");
                        newList.addClass("selected")
                        $("#SelectedList_ListID").val(newList.attr("listId"))
                        $(".listInfo").html(data)
                    }
                },
                error: function (jqxhr) {
                    $('.moveListItemForm .error-message').html(jqxhr.responseText);
                },
                processData: false,
                contentType: false
            })
        }
    })

    $(".delete-my-list").on("click", function (e) {
        e.preventDefault();
        console.log("delete list")
        $.ajax({
            url: "/Requests/DeleteListModal/?listID=" + $(this).attr("value"),
            method: 'GET',
            success: function (data) {
                $.fn.OpenModal("deleteListModal", "delete-list", data)
            },
            error: function (jqxhr) {
                $('.listSettingsForms .error-message').html(jqxhr.responseText);
            },
            processData: false,
            contentType: false
        })
    })

    $("#Title").on('input', function (e) {
        if (!$(".request-list-name").hasClass("changed")) {
            $(".request-list-name").addClass("changed")
        }
        $('.listSettingsForm').valid();
    })

    $(".createList").off("click").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        var valid = $('.newListForm').valid();
        console.log(valid)
        if (valid) {
            var fromMoveModal = $("#RequestToAddID").val() != 0
            console.log("from move "+ fromMoveModal)
            var url = "/Requests/NewListModal";
            var formData = new FormData($(".newListForm")[0]);
            var viewClass = "._IndexTableListTabs"
            $.fn.ajaxPartialIndexTable(-1, "/Requests/NewListModal", viewClass, "POST", formData, "new-list, move-list");
        }
    });
    $("#ListTitle").on('input', function (e) {
        var valid = $('.newListForm').valid();
    })

    $(".close-settings").off("click").on("click", function (e) {
        var viewClass = "._IndexTableListTabs"
        var url = "/Requests/_IndexTableWithListTabs/?"+ $.fn.getRequestIndexString()
        $.fn.ajaxPartialIndexTable(-1, url, viewClass, "GET", null, "list-settings");
    })

    $(".cancel-save").on("click", function (e) {
        e.preventDefault();
        $(".new-list").removeClass("new-list")
    });

    $(".submit-save").on("click", function (e) {
        e.preventDefault();
        $(".request-list-name").removeClass("changed")
        var formData = new FormData($(".listSettingsForm")[0]);
        var listFormData = new FormData();
        for (var pair of formData.entries()) {
            if (pair[0].startsWith("SelectedList")) {
                listFormData.append(pair[0], pair[1])
            }
        }
        console.log(...listFormData)
        $.ajax({
            contentType: false,
            processData: false,
            async: true,
            url: "/Requests/SaveListModal",
            data: listFormData,
            traditional: true,
            type: "POST",
            cache: false,
            success: function (data) {
                $.fn.CloseModal('save-list-settings');
                var listID = $(".next-list").attr("listId")
                $.ajax({
                    url: "/Requests/ListSettingsModal/?selectedListID=" + listID,
                    method: 'GET',
                    success: function (data) {
                        $.fn.OpenModal("listSettingsModal", "list-settings", data)
                        $(".request-list-name.selected").removeClass("selected");
                        $("#List" + $("#SelectedList_ListID").val()).addClass("selected")
                    },
                    error: function (jqxhr) {
                        $('.listSettingsForms .error-message').html(jqxhr.responseText);
                    },
                    processData: false,
                    contentType: false
                })
                
                
            },
            error: function (jqxhr) {
                $('.listSettingsForms .error-message').html(jqxhr.responseText);
            }
        });
    });
    $(".delete-requestlist").off('click').on("click", function (e) {
        e.preventDefault();
        console.log("delete list");
        var formdata = new FormData($(".deleteListForm")[0])
        $.ajax({
            url: "/Requests/DeleteListModal",
            method: 'POST',
            data: formdata,
            success: function (data) {
                $.fn.CloseModal("delete-list");
                $.fn.OpenModal("listSettingsModal","list-settings", data)
                return true;
            },
            error: function (jqxhr) {
                $('.deleteListForm .error-message').html(jqxhr.responseText);
            },
            processData: false,
            contentType: false
        })
        return false;
    });

    $(".moveListItem").off("click").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        var url = "/Requests/MoveToListModal";
        $("#NewListID").val($(this).attr("listId"));
        var formData = new FormData($(".moveListItemForm")[0]);
        var viewClass = "._IndexTableListTabs"
        $.fn.ajaxPartialIndexTable(-1, url, viewClass, "POST", formData, "move-list");
    });
})
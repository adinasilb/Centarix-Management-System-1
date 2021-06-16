  $("#protocolParentList").change(function () {
            var parentCategoryId = $("#protocolParentList").val();
            var url = "/Protocols/GetSubCategoryList";

      	$.getJSON(url, { ParentCategoryId: parentCategoryId }, function (data) {
			  $("#protocolSubList").children("option").each(function (i, option) {
				option.remove();
			});
			var firstitem1 = '<option value=""> Select Subcategory</option>';
			
			$("#protocolSubList").append(firstitem1);

			$.each(data, function (i, subCategory) {
                var newitem1 = '<option value="' + subCategory.protocolSubCategoryTypeID + '">' + subCategory.protocolSubCategoryTypeDescription + '</option>'
				$("#protocolSubList").append(newitem1);
            });
			$("#protocolSubList").materialSelect();
			return false;
		});
  });


  $(".saveProtocol").on("click", function (e) {
        e.preventDefault();
        $('.createProtocolForm').data("validator").settings.ignore = "";
        var valid = $('.createProtocolForm').valid();
        console.log("valid form: " + valid)
        if (!valid) {
            e.preventDefault();
            if (!$('.activeSubmit').hasClass('disabled-submit')) {
                $('.activeSubmit').addClass('disabled-submit')
            }

        }
        else {
            $('.activeSubmit').removeClass('disabled-submit')
            var selectedTab = $('.tab-pane .active').parent().index() + 1;
            console.log(selectedTab);
            $(".selectedTab").val(selectedTab);
            var formData = new FormData($(".createProtocolForm")[0]);
            $.ajax({
                url: "/Protocols/CreateProtocol",
                traditional: true,
                data: formData,
                contentType: false,
                processData: false,
                type: "POST",
                success: function (data) {
                    $("._CreateProtocolTabs").html(data)
                    $(".mdb-select").materialSelect();
                    if ($(this).hasClass("lines-tab")/* && $(".createProtocolMasterProtocolID").val()=="0"*/) {
                        $(".only-protocol-tab.li-function-bar").removeClass("d-none");
                    }
                    else {
                        $(".only-protocol-tab").addClass("d-none");
                    }
                },
                error: function (jqxhr) {
                    if (jqxhr.status == 500) {
                        $("._CreateProtocol").html(jqxhr.responseText)
                    }
                    $(".mdb-select").materialSelect();
                    return true;
                }
            });
        }
        $('.createProtocolForm').data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';


    });

    $(".saveLines").off("click").click(function (e) {
        e.preventDefault();
        $(".saving-spinner").removeClass("d-none");
        $.ajax({
            processData: false,
            contentType: false,
            data: new FormData($("#myForm")[0]),
            url: "/Protocols/SaveTempLines?ProtocolID=" + $(".createProtocolMasterProtocolID").val(),
            type: 'POST',
            success: function (data) {
                //$("._Lines").html(data);
                $(".saving-spinner").addClass("d-none");
                $(".saving-done").removeClass("d-none");
                setTimeout(function () {
                    $(".saving-done").addClass("d-none");
                }, 1000 * 30);
            },
            error: function (jqxhr) {
                $(".error-message").html(jqxhr.responseText);
                $(".error-message").removeClass("d-none");
                $(".saving-spinner").addClass("d-none");
            }
        });
    });



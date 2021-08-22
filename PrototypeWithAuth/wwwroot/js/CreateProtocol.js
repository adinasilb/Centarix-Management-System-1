$(function () {
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

    $("form").off("click", ".saveProtocol").on("click", ".saveProtocol", function (e) {        
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
            var tab= $(this);
            var selectedTab = tab.parent(".nav-item").index() +1;
          
            console.log(selectedTab);
            $(".selectedTab").val(selectedTab);
            var formData = new FormData($(".createProtocolForm")[0]);
            var modalType = $(".modalType").val();
            var url= "/Protocols/CreateProtocol";
            if(selectedTab ==1 || selectedTab==2)
            {
                url+="?IncludeSaveLines=true";
            }
            alert(modalType)
            $.ajax({
                url: url,
                traditional: true,
                data: formData,
                contentType: false,
                processData: false,
                type: "POST",
                success: function (data) {
                    if(modalType == "Create")
                    {
                        $("._CreateProtocolTabs").html(data)
                    }
                    else{
                        $("._IndexTable").html(data)
                    }

                    $(".mdb-select").materialSelect();
          
      
                    if (tab.hasClass("lines-tab")/* && $(".createProtocolMasterProtocolID").val()=="0"*/) {
                        $("."+modalType+".only-protocol-tab.li-function-bar").removeClass("d-none");
                    }
                    else {
                        $("."+modalType+".only-protocol-tab").addClass("d-none");
                    }
                     $("."+modalType+":not(.only-protocol-tab):not(.only-results-tab)").removeClass("d-none");

                },
                error: function (jqxhr) {
                     if(modalType == "Create")
                    {
                        $("._CreateProtocolTabs").html(jqxhr.responseText)
                    }
                    else{
                        $("._IndexTable").html(jqxhr.responseText)
                    }
                    $(".mdb-select").materialSelect();
                    return true;
                }
            });
        }
        $('.createProtocolForm').data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';


    });


    $(".results-tab").on("click", function (e) {
       $(".only-results-tab").removeClass("d-none");      
    }); 

    $("form").off("click", ".next-tab:not(.results-tab)").on("click", ".next-tab:not(.results-tab)", function (e) {     
      if($(".modalType").val() =="CheckListMode")
      {
        $(".only-results-tab").addClass("d-none");  
      }
    }); 

    $(".saveLines").off("click").click(function (e) {
        e.preventDefault();
        $(".saving-spinner").removeClass("d-none");
        $.ajax({
            processData: false,
            contentType: false,
            data: new FormData($("#myForm")[0]),
            url: "/Protocols/SaveTempLines?ProtocolVersionID=" + $(".createProtocolMasterProtocolVersionID").val()+"&guid=" + $(".createProtocolMasterGuid").val(),
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
    $(".saveResults").off("click").click(function (e) {
        e.preventDefault();
        $(".saving-spinner").removeClass("d-none");
        $.ajax({
            processData: false,
            contentType: false,
            data: new FormData($("#myForm")[0]),
            url: "/Protocols/SaveResults",
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
    $(".start-protocol-fx").off("click").click(function (e) {
        e.preventDefault();
        //switch this to universal share request and the modelsenum send in
        $.fn.StartProtocol($(this).attr("value"), false, 3);
    });
});

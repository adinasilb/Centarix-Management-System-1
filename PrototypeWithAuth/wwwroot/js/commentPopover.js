$(".addComment").off("click").click(function () {
    $('[data-toggle="popover"]').popover('dispose');
    $('.addComment').popover({
        sanitize: false,
        placement: 'bottom',
        html: true,
        content: function () {
            return $('#popover-content').html();
        }
    });
    $('.addComment').popover('toggle');
    $(".popover .add-comment").off("click").on("click", function (e) {
        e.preventDefault()
        var type = $(this).attr("data-val");
        var index = $('#comment-index').val();
        var sectionType = $("#masterSectionType").val();
        var controller = "Requests";
        if (sectionType == "LabManagement") {
            controller = "Vendors"
        }
        $.ajax({
            async: false,
            url: '/' + controller + '/_CommentInfoPartialView?typeID=' + type + '&index=' + index + "&modeltype=" + $(this).attr("model-type"),
            type: 'GET',
            cache: false,
            success: function (data) {
                $("#comment-info").append(data);
                $('#comment-index').val(++index);
            }
        });
    });


});



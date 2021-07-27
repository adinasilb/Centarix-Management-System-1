$(function(){ 
    $(".text-editor").on('change click', function (e) {
        console.log("click")
        var node = $.fn.GetCurrentPositionParents();
        $('#closingTags').val(node ?? " ")
        $(".focusedText").blur()
        $(".focusedText").remove()
        pasteHtmlAtCaret("<span class='focusedText'></span>")
        $("#TemporaryReportText").val($(this).html())

    })
    function pasteHtmlAtCaret(html) {
        var sel, range;
        if (window.getSelection) {
            // IE9 and non-IE
            sel = window.getSelection();
            if (sel.getRangeAt && sel.rangeCount) {
                range = sel.getRangeAt(0);
                //range.deleteContents();

                // Range.createContextualFragment() would be useful here but is
                // non-standard and not supported in all browsers (IE9, for one)
                var el = document.createElement("div");
                el.innerHTML = html;
                var frag = document.createDocumentFragment(), node, lastNode;
                while ((node = el.firstChild)) {
                    lastNode = frag.appendChild(node);
                }
                range.insertNode(frag);

                 //Preserve the selection
                //if (lastNode) {
                //    range = range.cloneRange();
                //    range.setStartAfter(lastNode);
                //    //range.collapse(false);
                //    sel.removeAllRanges();
                //    sel.addRange(range);
                //}
            }
        } 
    }
    $.fn.GetCurrentPositionParents = function (e) {
        var nodeString = "";
        var target = null;
        if ($(window.getSelection().baseNode).closest("#freeText").attr("id") == "freeText"){
            console.log("inside-div")
            if (window.getSelection().rangeCount > 0) {
                target = window.getSelection().getRangeAt(0).commonAncestorContainer;
                if (target.nodeType != 1) {
                    target = target.parentNode;
                }
                else if (document.selection) {
                    var target = document.selection.createRange().parentElement();
                }
                while (!$(target).hasClass('start-div') && target != null) {
                    nodeString = nodeString + (target?.tagName?? "") + ",";
                    target = target.parentNode
                }
            }
            return nodeString + "div";
        }
      
    }

    $('.reports-iframe, .results-iframe').on("load", function () {
        console.log("iframe")
        $(this).contents().find('img').css({ maxHeight: '100%' });
    });
});
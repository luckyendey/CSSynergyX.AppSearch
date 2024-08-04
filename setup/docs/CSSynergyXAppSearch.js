function initAppSearch() {
    var iframe = document.getElementById('Products');
    var iframeDocument = iframe.contentDocument || iframe.contentWindow.document;
    var iframeElement = $(iframeDocument).find('#ShowNotificationArrow');

    $.ajax({
        url: 'CSSynergyXAppSearch.html',
        dataType: 'html',
        success: function (data) {

            // Append the HTML content
            iframeElement.before(data);

            $(iframeDocument).find('#searchInput').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "CSSynergyXAppSearchCallback.aspx?Action=1&InputSearch=" + request.term,
                        dataType: "json",
                        data: {
                            term: request.term
                        },
                        success: function( data ) {
                            response( $.map(data, function( item ) {
                                return {
                                    label: item.title, 
                                    details: [item.moduleCaption, item.categoryLevel1Caption, item.categoryLevel2Caption].filter(s => s !== undefined && s !== null).join(' - '), 
                                    link: item.link }; 
                                })
                            );
                        }
                    });
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(iframeDocument).find("#MainWindow")[0].src = ui.item.link;
                    $(this).val('');
                    $(this).blur();
                    var waitMessage = $('#Products').contents().find("#MainWindow").contents().find("#WaitMessage")
                    if (waitMessage !== undefined) {
                        waitMessage.show();
                    }
                },
                open: function () {
                    $(this).data('ui-autocomplete').menu.element.scrollTop(0);
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                    .append("<div class='autocomplete-item-title'>" + item.label + "</div><div class='autocomplete-item-details'>" + item.details + "</div>")
                    .appendTo(ul);
            };
        },
        error: function () {
            console.error('Error loading CSSynergyXAppSearch.html');
        }
    });
}

$(document).ready(function () {
    $('#Products').on('load', function () {
        initAppSearch();
    });
});

$(function () {

    function scrollFunction() {
        $("#searchResults").hide();
        $("#searchInput").val("");
    }
    window.onscroll = scrollFunction;
});

function getNames(u) {
    var rx = new RegExp;
    //rx = /[^a-z]/gi;
    rx = /[^АаБбВвГгДдЕеЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъьЮюЯяa-zA-Z]/gi;
    var replaced = u.search(rx) >= 0;
    if (replaced) {
        u = u.replace(rx, "");
        document.getElementById("searchInput").value = u;
    }
    if (u == "") {
        document.getElementById("searchResults").style.display = "none";
        return false;
    }
    
    var actionUrl = "/Products/SearchWithAjax";
    $.ajax({
        type: "POST",
        url: actionUrl,
        data: { searchTerm: u },
        dataType: "text",
        success: function (msg) {
            var data = msg;
            var searchResults = $("#searchResults");

            searchResults.html(data);
            searchResults.show();
        },
        error: function (req, status, error) {
            alert(error);
        }
    });
}
var childContainer;
var htmlChild = '';

$(document).ready(function() {
    childContainer = $(".menu-lv2 ul.left");

    $(".menu-lv1 li a").mouseover(function() {
        $(".menu-lv1 li a").removeClass("actived");
        $(this).addClass("actived");
        htmlChild = $(this).parent().find("ul").html();
        childContainer.html(htmlChild);
    });
    selectedMenu();

});

function selectedMenu() {
    //select PageOnload
    var currentUrl = location.pathname;
    var strSelectorSideNav;
    strSelectorSideNav = "a.child[href='" + currentUrl + "']:first";
    $(strSelectorSideNav).addClass("actived");

    var urlSideNav = currentUrl.replace(/\/ct\/cms\/\w+\/(.*)/g, "$1");
    urlSideNav = location.pathname.replace(urlSideNav, "");
    if (urlSideNav.lastIndexOf('/') > 0)
        urlSideNav = urlSideNav.substring(0, urlSideNav.lastIndexOf('/'));
    strSelectorSideNav = ".menu-lv1 li a.parent[href^='" + urlSideNav + "']:first";
    if ($(strSelectorSideNav).length == 0) {
        strSelectorSideNav = ".menu-lv1 li a.parent:first";
    }

    $(strSelectorSideNav).addClass("actived");
    htmlChild = $(strSelectorSideNav).parent().find("ul").html();
    childContainer.html(htmlChild);
}
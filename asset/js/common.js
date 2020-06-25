
//open the corresponding menu
$(function () {
    var url = document.location.href.toString();
  
    var arrUrl = url.split("/");
 
    var $navItem = $(".nav-sidebar").find(".nav-item")
    $navItem.each(function (index, ele) {
        var navUrl = $(ele).find("a").prop("href");
        var arrNavUrl = navUrl.split("/");
        if (arrUrl[arrUrl.length - 1] === arrNavUrl[arrNavUrl.length - 1]) {
          
            $(ele).find(".nav-link").addClass("active");
            $(ele).parent().prev().addClass("active");
            $(ele).parent().parent().addClass("menu-open");
        }


    })
})
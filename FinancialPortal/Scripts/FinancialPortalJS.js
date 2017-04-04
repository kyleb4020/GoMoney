function getUserTime(time) {
    var userTime = new Date(Date.parse(time))
    var hours = userTime.getHours();
    var ampm = "AM";
    if (hours > 12) {
        hours = hours - 12;
        ampm = "PM";
    }
    if (hours === 12) {
        ampm = "PM"
    }
    if (hours === 0) {
        hours = 12;
    }
    var minutes = userTime.getMinutes();
    if (minutes < 10) {
        minutes = "0" + minutes;
    }
    var seconds = userTime.getSeconds();
    if (seconds < 10) {
        seconds = "0" + seconds;
    }
    var correctTime = (userTime.getMonth() + 1) + "/" + (userTime.getDate()) + "/" + userTime.getFullYear() + " " + hours + ":" + minutes + ":" + seconds + " " + ampm
    return (correctTime)
}

$(document).ready(function () {

    if (top.location.pathname === '/Home/Index') {
        $("#side-menu li").removeClass("selected");
        $("#home-nav").addClass("selected");
    }
    if (top.location.pathname === '/Home') {
        $("#side-menu li").removeClass("selected");
        $("#home-nav").addClass("selected");
    }
    if (top.location.pathname === '/Home/') {
        $("#side-menu li").removeClass("selected");
        $("#home-nav").addClass("selected");
    }
    if (top.location.pathname === '/') {
        $("#side-menu li").removeClass("selected");
        $("#home-nav").addClass("selected");
    }
    if (top.location.pathname === '/Households/') {
        $("#side-menu li").removeClass("selected");
        $("#house-nav").addClass("selected");
    }
    if (top.location.pathname === '/Households') {
        $("#side-menu li").removeClass("selected");
        $("#house-nav").addClass("selected");
    }
    if (top.location.pathname === '/Households/Index') {
        $("#side-menu li").removeClass("selected");
        $("#house-nav").addClass("selected");
    }
});

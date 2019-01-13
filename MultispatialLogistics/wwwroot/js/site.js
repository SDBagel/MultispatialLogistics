// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function checkEmpty() {
    var origin = document.getElementById("origin").nodeValue;
    var destination = document.getElementById("destination").nodeValue;
    if (origin === "" || destination === "") {
        return false;
    }
}
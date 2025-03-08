// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function previewImage(inputElement) {
    var reader = new FileReader();

    reader.onload = function (e) {
        var img = new Image();
        img.src = e.target.result;

        img.onload = function () {
            var resizedDataUrl = resizeImage(img);
            var output = document.getElementById('imagePreview');
            output.src = resizedDataUrl;
        };
    };

    reader.readAsDataURL(inputElement.files[0]);
}

function resizeImage(img) {
    var canvas = document.createElement('canvas');
    var ctx = canvas.getContext('2d');

    var width = 500
    var height = 700


    canvas.width = width;
    canvas.height = height;
    ctx.drawImage(img, 0, 0, width, height);

    return canvas.toDataURL('image/jpeg');
}
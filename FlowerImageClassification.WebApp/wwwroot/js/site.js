// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// The following Javascript codes are used for initializing webpage events

console.log('%cWARNING! You are in development mode', 'color: red; font-size: 28px; font-weight: bold;');

/////////////// Constants ///////////////
const webcam = document.getElementById("my_camera");
const form = document.querySelector('form');
const inputFile = document.getElementById("inputFile");
const inputWebcam = document.getElementById("inputWebcam");
const background = document.querySelector("body > ul");

/////////////// Form submit ///////////////
if (inputFile != null || inputWebcam != null)
    form.addEventListener('submit', e => uploadAndClassify(e));

/////////////// File upload form ///////////////
$(document).ready(customFileInput);

/////////////// Webcam settings ///////////////
if (webcam != null) {
    var width = document.getElementById("formButton").offsetWidth;
    var height = width * 3 / 4;
    Webcam.set({ width: width, height: height, image_format: "jpeg", jpeg_quality: 90 });
    Webcam.attach('#my_camera');
}

/////////////// Random background images ///////////////
if (background != null)
    for (let index = 1; index <= 5; index++) {
        var element = document.querySelector("body > ul > li:nth-child(" + index + ") > span ");
        var random = getRandomArbitraryNumber(1, 15);
        element.style.backgroundImage = 'url(../img/' + random + '.jpg)';
        element.style.objectFit = 'cover';
        element.style.animationDelay = (index - 1) * 6 + "s";
    }

/////////////// Initialize dictionary about flower categories ///////////////
var dict = {
    "Carnation": "Hoa cẩm chướng",
    "Hydrangea": "Hoa cẩm tú cầu",
    "Jacaranda": "Hoa phượng tím",
    "Lavender": "Hoa oải hương",
    "Gladiolus": "Hoa lay ơn",
    "Lily flower": "Hoa loa kèn",
    "Lisianthus": "Hoa cát tường",
    "Rose": "Hoa hồng",
    "Sunflower": "Hoa hướng dương",
    "Tulip": "Hoa uất kim hương"
}
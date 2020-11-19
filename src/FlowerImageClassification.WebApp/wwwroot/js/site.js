﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// The following Javascript codes are used for initializing webpage events

console.log('%cWARNING! You are in development mode', 'color: red; font-size: 28px; font-weight: bold;');

/////////////// Constants ///////////////
const webcam = document.getElementById("my_camera");
const mainForm = document.querySelector('#mainForm');
const sentimentForm = document.querySelector('#sentimentForm');
const inputFile = document.getElementById("inputFile");
const inputWebcam = document.getElementById("inputWebcam");
const background = document.querySelector("body > ul");

/////////////// Form submit ///////////////
if (inputFile != null || inputWebcam != null)
	mainForm.addEventListener('submit', e => uploadAndClassify(e));
if (sentimentForm != null)
	sentimentForm.addEventListener('submit', e => SubmitUserSentiment(e));

/////////////// File upload form ///////////////
$(document).ready(customFileInput());

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
		var random = getRandomArbitraryNumber(1, 14);
		element.style.backgroundImage = 'url(../img/' + random + '.jpg)';
		element.style.objectFit = 'cover';
		element.style.animationDelay = (index - 1) * 10 + "s";
	}

/////////////// Dictionary about flower categories ///////////////
var dict = {
	"daisy": "Hoa cúc",
	"dandelion": "Hoa bồ công anh",
	"rose": "Hoa hồng",
	"sunflower": "Hoa hướng dương",
	"tulip": "Hoa uất kim hương"
}
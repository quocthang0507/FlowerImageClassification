// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const webcam = document.getElementById("my_camera");
const form = document.querySelector('form');
const inputFile = document.getElementById("inputFile");
const inputWebcam = document.getElementById("inputWebcam");

/////////////// Form submit ///////////////
if (inputFile != null || inputWebcam != null)
	form.addEventListener('submit', e => {
		e.preventDefault();

		var url = '';
		var formData = new FormData();
		var checked = $('input[type=checkbox]').prop('checked');

		//////////// User input file ///////////
		if (inputFile != null) {
			const files = inputFile.files;
			if (!fileValidation())
				return;
			formData.append('imageFile', files[0]);
			if (checked)
				url = 'api/CollectAndClassifyImage';
			else
				url = 'api/ClassifyImage';
		}
		//////////// User webcam image ////////////
		else {
			var file = document.getElementById("inputWebcam").src;
			formData.append("base64image", file);
			if (checked)
				url = 'api/CollectAndClassifyImageBase64';
			else
				url = 'api/ClassifyImageBase64'
		}

		fetch(url, {
			method: 'POST',
			body: formData
		}).then(response => response.json()
		).then(response2 => {
			console.log('Received response from server: ', response2);
			document.getElementById("divResult_right").style.visibility = "visible";
			document.getElementById('divImageId').innerHTML = "Tên tập tin vừa tải lên: " + response2.imageID;
			document.getElementById('divPrediction').innerHTML = "Kết quả dự đoán là: " + response2.predictedLabel;
			document.getElementById('divProbability').innerHTML = "Xác suất: " + (response2.probability * 100).toFixed(3) + "%";
			document.getElementById('divExecutionTime').innerHTML = "Thời gian dự đoán: " + response2.predictionExecutionTime + " mili giây";
			return response2;
		}).catch(error => {
			console.log('Đã có lỗi xảy ra: ', error);
			alert('Đã có lỗi xảy ra, vui lòng thử lại');
		});
	});

/////////////// File upload form ///////////////
$(document).ready(function () {
	// Add the following code if you want the name of the file appear on select
	$(".custom-file-input").on("change", function () {
		var fileName = $(this).val().split("\\").pop();
		$(this).siblings(".custom-file-label").addClass("selected").html(fileName);
		fileValidation();
	});
});

/////////////// Validate file extension ///////////////
function fileValidation() {
	var fileInput = document.querySelector('[type=file]');
	var filePath = fileInput.value;
	// Allowing 2 extensions
	var allowedExtensions = /(\.jpg|\.png)$/i;
	if (fileInput.files.length == 0)
		alert("Hình ảnh không được để trống!");
	else if (!allowedExtensions.exec(filePath)) {
		alert('Chỉ chấp nhận hình JPG và PNG!');
		fileInput.value = '';
	}
	else {
		// Image preview 
		if (fileInput.files && fileInput.files[0]) {
			var reader = new FileReader();
			reader.onload = function (e) {
				document.getElementById("divResult_left").style.visibility = "visible";
				document.getElementById('divImagePreview').innerHTML = '<img class="imgPreview" src="' + e.target.result + '"/>';
			};
			reader.readAsDataURL(fileInput.files[0]);
			return true;
		}
	}
	return false;
}

/////////////// Webcam settings ///////////////
if (webcam != null) {
	var width = document.getElementById("formButton").offsetWidth;
	var height = width * 3 / 4;
	Webcam.set({ width: width, height: height, image_format: "jpeg", jpeg_quality: 90 });
	Webcam.attach('#my_camera');
	function takeSnapshot() {
		Webcam.snap(function (data_uri) {
			document.getElementById("inputWebcam").src = data_uri;
		});
	}
}
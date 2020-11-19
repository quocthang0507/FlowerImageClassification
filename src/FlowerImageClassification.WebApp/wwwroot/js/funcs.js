// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// The following Javascript functions are used for supporting site.js

/**
 * Returns a random number between min (inclusive) and max (exclusive)
 * @param {number} min 
 * @param {number} max 
 */
function getRandomArbitraryNumber(min, max) {
	return Math.round(Math.random() * (max - min) + min);
}

/**
 * Take a webcam snapshot
 */
function takeSnapshot() {
	Webcam.snap(function (data_uri) {
		document.getElementById("inputWebcam").src = data_uri;
	});
}

/**
 * Add 'loading' class
 */
function addLoadingClass() {
	$("body").addClass("loading");
}

/**
 * Remove 'loading' class
 */
function removeLoadingClass() {
	$("body").removeClass("loading");
}

/**
 * Validate file extension in file input element
 */
function validateFileExtension() {
	var fileInput = document.querySelector('[type=file]');
	var filePath = fileInput.value;
	// Allowing 2 extensions
	var allowedExtensions = /(\.jpg|\.png)$/i;
	if (fileInput.files.length == 0)
		alert("Hình ảnh không được để trống!");
	else if (!allowedExtensions.exec(filePath)) {
		alert('Chỉ chấp nhận hình JPG và PNG!');
		fileInput.value = '';
	} else {
		// Image preview 
		if (fileInput.files && fileInput.files[0]) {
			const Bytes = fileInput.files[0].size;
			const MB = Math.round((Bytes / 1024));
			// The size of the file. 
			if (MB > 2048) {
				alert('Kích thước tập tin quá lớn, vui lòng gửi tập tin nhỏ hơn 2MB');
				return false;
			} else {
				var reader = new FileReader();
				reader.onload = function (e) {
					document.getElementById("divResult_left").style.visibility = "visible";
					document.getElementById('divImagePreview').innerHTML = '<img class="imgPreview" src="' + e.target.result + '"/>';
				};
				reader.readAsDataURL(fileInput.files[0]);
				return true;
			}
		}
	}
	return false;
}

/**
 * This function run when webpage is ready
 */
function customFileInput() {
	// Add the following code if you want the name of the file appear on select
	$(".custom-file-input").on("change", function () {
		var fileName = $(this).val().split("\\").pop();
		$(this).siblings(".custom-file-label").addClass("selected").html(fileName);
	});
}

/**
 * Upload an image (file or webcam snapshot) to server and classify them
 * @param {Event} e 
 */
function uploadAndClassify(e) {
	e.preventDefault();

	var url = '';
	var formData = new FormData();

	//////////// User input file ///////////
	if (inputFile != null) {
		const files = inputFile.files;
		if (!validateFileExtension())
			return;
		formData.append('imageFile', files[0]);
		url = 'api/ClassifyImage';
	}
	//////////// User webcam image ////////////
	else {
		var file = document.getElementById("inputWebcam").src;
		formData.append("base64image", file);
		url = 'api/ClassifyBase64'
	}

	addLoadingClass();

	fetch(url, {
		method: 'POST',
		body: formData
	}).then(response => response.json()).then(response2 => {
		console.log('Đã nhận được phản hồi: ', response2);
		document.getElementById("divResult_right").style.visibility = "visible";
		document.getElementById('divImageId').innerHTML = "Tên tập tin vừa tải lên: " + response2.imageID;
		document.getElementById('divPrediction').innerHTML = `Kết quả dự đoán là: ${dict[response2.predictedLabel]}`;
		document.getElementById('divEnPrediction').innerHTML = response2.predictedLabel;
		document.getElementById('divProbability').innerHTML = "Xác suất: " + (response2.probability * 100).toFixed(3) + "%";
		document.getElementById('divExecutionTime').innerHTML = "Thời gian dự đoán: " + response2.predictionExecutionTime + " mili giây";
		getInfo(response2.predictedLabel);

		removeLoadingClass();
	}).catch(error => {
		console.error('Đã có lỗi xảy ra: ', error);
		alert('Đã có lỗi xảy ra, vui lòng thử lại.');

		removeLoadingClass();
	});
}

/**
 * Send request to get info by flower name
 * @param {Text} name 
 */
function getInfo(name) {
	var url = 'api/GetInfo/' + name;
	$.get(url, function (data) {
		document.querySelector('#modal-info > div > div > div.modal-body').innerHTML = data;
	});
}

/**
 * Submit user sentiment to server
 * @param {any} e
 */
function SubmitUserSentiment(e) {
	e.preventDefault();

	var url = 'api/Contribution';
	var formData = new FormData();

	const prediction = document.getElementById('divEnPrediction').innerHTML;

	//////////// User input file ///////////
	if (inputFile != null) {
		const files = inputFile.files;
		if (!validateFileExtension())
			return;
		formData.append('imageFile', files[0]);
		formData.append('predictedLabel', prediction);
	}
	//////////// User webcam image ////////////
	else {
		var file = document.getElementById("inputWebcam").src;
		formData.append("base64image", file);
		formData.append('predictedLabel', prediction);
		url = 'api/Base64Contribution';
	}

	fetch(url, {
		method: 'POST',
		body: formData
	}).then(response => response.json()).then(response2 => {
		
		console.log('Đã nhận được phản hồi: ', response2);
	}).catch(error => {
		console.error('Lỗi gửi phản hồi:', error);
		alert('Lỗi gửi phản hồi lên server.');
	});
}

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const url = 'api/ClassifyImage';
const form = document.querySelector('form');

/////////////// Form submit ///////////////
if (form != null)
	form.addEventListener('submit', e => {
		e.preventDefault();

		const files = document.querySelector('[type=file]').files;
		if (!fileValidation()) {
			alert("Tập tin không hợp lệ!");
			return;
		}
		const formData = new FormData();

		formData.append('imageFile', files[0]);

		fetch(url, {
			method: 'POST',
			body: formData
		}).then(response => response.json()
		).then(response2 => {
			console.log('Received response from server: ', response2);
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

/////////////// Add 'active' class to current element ///////////////
$(document).ready(function () {
	var url = window.location;
	$('.collapse .navbar-nav').find('.active').removeClass('active');
	$('.collapse .navbar-nav li a').each(function () {
		if (this.href == url) {
			$(this).parent().addClass('active');
		}
	});
});

/////////////// File upload form ///////////////
$(document).ready(function () {
	// Add the following code if you want the name of the file appear on select
	$(".custom-file-input").on("change", function () {
		var fileName = $(this).val().split("\\").pop();
		$(this).siblings(".custom-file-label").addClass("selected").html(fileName);
	});
});

/////////////// Validate file extension ///////////////
function fileValidation() {
	var fileInput = document.querySelector('[type=file]');
	var filePath = fileInput.value;
	// Allowing 2 extensions
	var allowedExtensions = /(\.jpg|\.png)$/i;
	if (!allowedExtensions.exec(filePath)) {
		alert('Chỉ chấp nhận hình JPG và PNG!');
		fileInput.value = '';
	}
	else {
		// Image preview 
		if (fileInput.files && fileInput.files[0]) {
			var reader = new FileReader();
			reader.onload = function (e) {
				document.getElementById('divImagePreview').innerHTML = '<img src="' + e.target.result + '"/>';
			};
			reader.readAsDataURL(fileInput.files[0]);
			return true;
		}
	}
	return false;
}
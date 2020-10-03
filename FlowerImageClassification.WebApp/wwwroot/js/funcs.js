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
    Webcam.snap(function(data_uri) {
        document.getElementById("inputWebcam").src = data_uri;
    });
}

/**
 * Validate file extension in file input element
 */
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
    } else {
        // Image preview 
        if (fileInput.files && fileInput.files[0]) {
            var reader = new FileReader();
            reader.onload = function(e) {
                document.getElementById("divResult_left").style.visibility = "visible";
                document.getElementById('divImagePreview').innerHTML = '<img class="imgPreview" src="' + e.target.result + '"/>';
            };
            reader.readAsDataURL(fileInput.files[0]);
            return true;
        }
    }
    return false;
}

/**
 * This function run when webpage is ready
 */
function runWhenReady() {
    // Add the following code if you want the name of the file appear on select
    $(".custom-file-input").on("change", function() {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
        fileValidation();
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
    }).then(response => response.json()).then(response2 => {
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
}
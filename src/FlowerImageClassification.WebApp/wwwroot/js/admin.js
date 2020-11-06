// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// The following Javascript codes are used for supporting admin page

let _id = 0;

/**
 * Load content to the form by flower's id
 * @param {any} id
 */
function getItemById(id) {
	_id = id;
	var length = $('#listFlowers button').length;
	for (var i = 1; i <= length; i++) {
		var btn = document.getElementById(i);
		if (i !== id)
			btn.className = btn.className.replace(" active", "");
		else
			btn.className += " active";
	}
	var url = 'api/GetById/' + id;
	fetch(url)
		.then(response => response.json())
		.then(data => displayItem(data))
		.catch(error => {
			console.error('Lỗi khi truy xuất thông tin từ server:', error);
			alert('Lỗi khi truy xuất thông tin từ server.')
		});
}

$(document).ready(function () {
	getItemById(1);
});

/**
 * Display item in form
 * @param {any} item
 */
function displayItem(item) {
	document.getElementById('tbxEnglishName').value = item.englishName;
	document.getElementById('tbxVietnameseName').value = item.vietnameseName;
	document.getElementById('imgThumbnail').src = item.thumbnail;
	document.getElementById('imgThumbnail').alt = item.vietnameseName;
	_thumbnail = item.thumbnail;
	if (item.richTextInfo !== null)
		_editor.setData(item.richTextInfo);
	else
		_editor.setData('');
}

$('form').on('submit', submit);

/**
 * Update flower info
 */
function submit() {
	const url = 'api/update';

	const engName = document.getElementById('tbxEnglishName');
	const viName = document.getElementById('tbxVietnameseName');

	const flower = {
		ID: _id,
		EnglishName: engName.value,
		VietnameseName: viName.value,
		RichTextInfo: _editor.getData(),
		Thumbnail: _thumbnail
	};
	fetch(url, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json'
		},
		body: JSON.stringify(flower)
	}).then().then(() => {
		alert('Lưu thành công!');
	}).catch(error => {
		console.error('Lỗi cập nhật:', error);
		alert('Lỗi cập nhật thông tin lên server.');
	});

}
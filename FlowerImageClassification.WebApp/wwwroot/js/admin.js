// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// The following Javascript codes are used for supporting admin page

/**
 * Load content to the form by flower's id
 * @param {any} id
 */
function getItemById(id) {
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
		.catch(error => console.error('Unable to get item:', error));
}

/**
 * Display item in form
 * @param {any} item
 */
function displayItem(item) {
	document.getElementById('tbxEnglishName').value = item.englishName;
	document.getElementById('tbxVietnameseName').value = item.vietnameseName;
	document.getElementById('rtbInfo').innerHTML = item.richTextInfo;
	document.getElementById('imgThumbnail').src = item.thumbnail;
	document.getElementById('imgThumbnail').alt = item.vietnameseName;
}

$(document).ready(function () {
	getItemById(1);
});
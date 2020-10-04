// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// The following Javascript codes are used for supporting admin page

/**
 * Load content to the form by flower's id
 * @param {any} id
 */
function getItemById(id) {
	var url = 'api/GetById/' + id;
	fetch(url)
		.then(response => response.json())
		.then(data => displayItem(data))
		.catch(error => console.error('Unable to get item:', error));
}

function displayItem(item) {
	document.getElementById('tbxEnglishName').value = item.englishName;
	document.getElementById('tbxVietnameseName').value = item.vietnameseName;
	document.getElementById('rtbInfo').innerHTML = item.richTextInfo;
}
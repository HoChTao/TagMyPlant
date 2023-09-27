const imageUpload = document.getElementById('imageUpload');
const imagePreview = document.getElementById('imagePreview');
const deleteImage = document.getElementById('deleteImage');

// File upload field change eventFile upload field change event
imageUpload.addEventListener('change', function () {
    const file = this.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            imagePreview.src = e.target.result;
            // Show delete button
            deleteImage.style.display = 'inline-block';
        };
        reader.readAsDataURL(file);
    } else {
        // User deselects file or does not select file
        imagePreview.src = 'placeholder.jpg'; // Clear image
        // Hide delete button
        deleteImage.style.display = 'none';
    }
});

// Delete button click event
deleteImage.addEventListener('click', function () {
    // Clear image preview
    imagePreview.src = 'placeholder.jpg'; // We can set a placeholder image or default image
    // Reset file upload fields
    imageUpload.value = ''; // Clear file selection
    // Hide delete button
    deleteImage.style.display = 'none';
});

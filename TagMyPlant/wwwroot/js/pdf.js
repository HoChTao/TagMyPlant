const pdfUploadDE = document.getElementById('pdfUploadDE');
const pdfPreviewLinkDE = document.getElementById('pdfPreviewLinkDE');
const deletePdfDE = document.getElementById('deletePdfDE');

const pdfUploadGB = document.getElementById('pdfUploadGB');
const pdfPreviewLinkGB = document.getElementById('pdfPreviewLinkGB');
const deletePdfGB = document.getElementById('deletePdfGB');

// PDF file upload field change event - PDF(DE)
pdfUploadDE.addEventListener('change', function () {
    const file = this.files[0];
    if (file) {
        // Show preview link - PDF(DE)
        pdfPreviewLinkDE.href = URL.createObjectURL(file);
        pdfPreviewLinkDE.style.display = 'inline-block';
        // Show delete button - PDF(DE)
        deletePdfDE.style.display = 'inline-block';
    } else {
        // User deselects file or does not select file
        // Hide preview link - PDF(DE)
        pdfPreviewLinkDE.style.display = 'none';
        // Hide delete button - PDF(DE)
        deletePdfDE.style.display = 'none';
    }
});

// Hide PDF button click event - PDF(DE)
deletePdfDE.addEventListener('click', function (e) {
    e.preventDefault();
    // Reset PDF file upload fields - PDF(DE)
    pdfUploadDE.value = '';
    // Hide preview link - PDF(DE)
    pdfPreviewLinkDE.style.display = 'none';
    // Hide delete button - PDF(DE)
    deletePdfDE.style.display = 'none';
});

// PDF file upload field change event - PDF(GB)
pdfUploadGB.addEventListener('change', function () {
    const file = this.files[0];
    if (file) {
        // Show preview link - PDF(GB)
        pdfPreviewLinkGB.href = URL.createObjectURL(file);
        pdfPreviewLinkGB.style.display = 'inline-block';
        // Show delete button - PDF(GB)
        deletePdfGB.style.display = 'inline-block';
    } else {
        // User deselects file or does not select file
        // Hide preview link - PDF(GB)
        pdfPreviewLinkGB.style.display = 'none';
        // Hide delete button - PDF(GB)
        deletePdfGB.style.display = 'none';
    }
});

// Hide PDF button click event - PDF(GB)
deletePdfGB.addEventListener('click', function (e) {
    e.preventDefault();
    // Reset PDF file upload fields - PDF(GB)
    pdfUploadGB.value = '';
    // Hide preview link - PDF(GB)
    pdfPreviewLinkGB.style.display = 'none';
    // Hide delete button - PDF(GB)
    deletePdfGB.style.display = 'none';
});
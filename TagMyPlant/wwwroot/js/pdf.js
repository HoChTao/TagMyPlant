const pdfUploadDE = document.getElementById('pdfUploadDE');
const pdfPreviewLinkDE = document.getElementById('pdfPreviewLinkDE');
const deletePdfDE = document.getElementById('deletePdfDE');

const pdfUploadGB = document.getElementById('pdfUploadGB');
const pdfPreviewLinkGB = document.getElementById('pdfPreviewLinkGB');
const deletePdfGB = document.getElementById('deletePdfGB');

// PDF文件上传字段变化事件 - PDF(DE)
pdfUploadDE.addEventListener('change', function () {
    const file = this.files[0];
    if (file) {
        // 显示预览链接 - PDF(DE)
        pdfPreviewLinkDE.href = URL.createObjectURL(file);
        pdfPreviewLinkDE.style.display = 'inline-block';
        // 显示删除按钮 - PDF(DE)
        deletePdfDE.style.display = 'inline-block';
    } else {
        // 用户取消选择文件或未选择文件
        // 隐藏预览链接 - PDF(DE)
        pdfPreviewLinkDE.style.display = 'none';
        // 隐藏删除按钮 - PDF(DE)
        deletePdfDE.style.display = 'none';
    }
});

// 删除PDF按钮点击事件 - PDF(DE)
deletePdfDE.addEventListener('click', function (e) {
    e.preventDefault();
    // 重置PDF文件上传字段 - PDF(DE)
    pdfUploadDE.value = '';
    // 隐藏预览链接 - PDF(DE)
    pdfPreviewLinkDE.style.display = 'none';
    // 隐藏删除按钮 - PDF(DE)
    deletePdfDE.style.display = 'none';
});

// PDF文件上传字段变化事件 - PDF(GB)
pdfUploadGB.addEventListener('change', function () {
    const file = this.files[0];
    if (file) {
        // 显示预览链接 - PDF(GB)
        pdfPreviewLinkGB.href = URL.createObjectURL(file);
        pdfPreviewLinkGB.style.display = 'inline-block';
        // 显示删除按钮 - PDF(GB)
        deletePdfGB.style.display = 'inline-block';
    } else {
        // 用户取消选择文件或未选择文件
        // 隐藏预览链接 - PDF(GB)
        pdfPreviewLinkGB.style.display = 'none';
        // 隐藏删除按钮 - PDF(GB)
        deletePdfGB.style.display = 'none';
    }
});

// 删除PDF按钮点击事件 - PDF(GB)
deletePdfGB.addEventListener('click', function (e) {
    e.preventDefault();
    // 重置PDF文件上传字段 - PDF(GB)
    pdfUploadGB.value = '';
    // 隐藏预览链接 - PDF(GB)
    pdfPreviewLinkGB.style.display = 'none';
    // 隐藏删除按钮 - PDF(GB)
    deletePdfGB.style.display = 'none';
});
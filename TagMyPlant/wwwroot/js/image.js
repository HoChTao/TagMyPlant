const imageUpload = document.getElementById('imageUpload');
const imagePreview = document.getElementById('imagePreview');
const deleteImage = document.getElementById('deleteImage');

// 文件上传字段变化事件
imageUpload.addEventListener('change', function () {
    const file = this.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            imagePreview.src = e.target.result;
            // 显示删除按钮
            deleteImage.style.display = 'inline-block';
        };
        reader.readAsDataURL(file);
    } else {
        // 用户取消选择文件或未选择文件
        imagePreview.src = 'placeholder.jpg'; // 清空图片
        // 隐藏删除按钮
        deleteImage.style.display = 'none';
    }
});

// 删除按钮点击事件
deleteImage.addEventListener('click', function () {
    // 清空图片预览
    imagePreview.src = 'placeholder.jpg'; // 可以设置一个占位图或默认图片
    // 重置文件上传字段
    imageUpload.value = ''; // 清空文件选择
    // 隐藏删除按钮
    deleteImage.style.display = 'none';
});

function enableEditing() {
    document.querySelectorAll('.profile-info input').forEach(input => {
        input.removeAttribute('readonly');
    });
    document.getElementById('editButton').style.display = 'none';
    document.getElementById('actionButtons').style.display = 'block';
}

function saveChanges() {
    // Kaydetme işlemi burada yapılır, backend'e gerekli veriler gönderilir
    alert("Değişiklikler kaydedildi!");
    cancelChanges();
}

function cancelChanges() {
    document.querySelectorAll('.profile-info input').forEach(input => {
        input.setAttribute('readonly', 'true');
    });
    document.getElementById('editButton').style.display = 'block';
    document.getElementById('actionButtons').style.display = 'none';
}

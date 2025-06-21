function openCreateModal() {
    $.get('/NewsArticle/Create', function (html) {
        $('#modalContainer').html(html);
        $('#createModal').modal('show');
    });
}

function openEditModal(id) {
    $.get(`/NewsArticle/Edit?id=${id}`, function (html) {
        $('#modalContainer').html(html);
        $('#editModal').modal('show');
    });
}

function confirmDelete(id) {
    if (confirm("Are you sure you want to delete this article?")) {
        $.post(`/NewsArticle/Delete`, { id: id }, function () {
            location.reload();
        });
    }
}

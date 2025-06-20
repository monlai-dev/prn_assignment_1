const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .build();

// Listener function for real-time updates
connection.on("ReceiveTagUpdate", function (action, tag) {
    if (action === "create") {
        // Add new tag to the list
        var row = `<tr>
                <td>${tag.tagName}</td>
                <td>${tag.note}</td>
                <td>
                    <button class="btn btn-sm btn-success me-1" onclick="openEditModal(${tag.tagId})">Edit</button>
                    <button class="btn btn-sm btn-danger" onclick="openDeleteModal(${tag.tagId}, '${tag.tagName}')">Delete</button>
                </td>
            </tr>`;
        $("table tbody").append(row);
    } else if (action === "edit") {
        // Update tag in the list
        var $rows = $("table tbody tr");
        $rows.each(function () {
            var $editBtn = $(this).find("button.btn-success");
            if ($editBtn.length && $editBtn.attr("onclick") && $editBtn.attr("onclick").includes(`openEditModal(${tag.tagId})`)) {
                $(this).find("td").eq(0).text(tag.tagName);
                $(this).find("td").eq(1).text(tag.note);
            }
        });
    } else if (action === "delete") {
        // Remove tag from the list
        var $rows = $("table tbody tr");
        $rows.each(function () {
            var $deleteBtn = $(this).find("button.btn-danger");
            if ($deleteBtn.length && $deleteBtn.attr("onclick") && $deleteBtn.attr("onclick").includes(`openDeleteModal(${tag.tagId},`)) {
                $(this).remove();
            }
        });
    }
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

function openEditModal(tagId) {
    let data = tags.find(tag => tag.TagId == tagId)

    $("#editTagId").val(data.TagId);
    $("#editTagName").val(data.TagName);
    $("#editTagNote").val(data.Note);
    var editModal = new bootstrap.Modal(document.getElementById('editModal'));
    editModal.show();
}
function openDeleteModal(tagId, tagName) {
    $("#deleteTagId").val(tagId);
    $("#deleteTagName").text(tagName);
    var deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
    deleteModal.show();
}
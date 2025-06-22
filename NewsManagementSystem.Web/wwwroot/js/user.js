//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/hub")
//    .build();

//connection.on("ReceiveUserUpdate", function () {
//    location.reload(); // Reload to get the latest user list
//});

//connection.start().catch(function (err) {
//    return console.error(err.toString());
//});

// Modal helpers
window.openEditModal = function (id) {
    const row = document.querySelector(`tr[data-id='${id}']`);
    document.getElementById('editUserId').value = id;
    document.getElementById('editUserName').value = row.children[0].innerText;
    document.getElementById('editUserEmail').value = row.children[1].innerText;
    document.getElementById('editUserRole').value = row.children[2].innerText;
    document.getElementById('editUserPassword').value = '';
    new bootstrap.Modal(document.getElementById('editModal')).show();
};

window.openDeleteModal = function (id, name) {
    document.getElementById('deleteUserId').value = id;
    document.getElementById('deleteUserName').innerText = name;
    new bootstrap.Modal(document.getElementById('deleteModal')).show();
};
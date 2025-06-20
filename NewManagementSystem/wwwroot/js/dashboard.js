function displayArticles(page, size) {
    const startIndex = (page - 1) * size;
    const endIndex = startIndex + size;
    const pageArticles = articlesData.slice(startIndex, endIndex);

    const tbody = document.getElementById('articlesTableBody');
    tbody.innerHTML = '';

    pageArticles.forEach(article => {
        const row = document.createElement('tr');
        const statusBadge = article.IsActive
            ? '<span class="badge bg-success">Active</span>'
            : '<span class="badge bg-secondary">Inactive</span>';

        const createdDate = article.CreatedDate
            ? new Date(article.CreatedDate).toISOString().split('T')[0]
            : '';

        row.innerHTML = `
                <td>${article.Title}</td>
                <td>${article.Category}</td>
                <td>${article.Author}</td>
                <td>${createdDate}</td>
                <td>${statusBadge}</td>
            `;
        tbody.appendChild(row);
    });

    updatePageInfo(page, size);
    generatePagination(page, size);
}

function updatePageInfo(page, size) {
    const startIndex = (page - 1) * size + 1;
    const endIndex = Math.min(page * size, articlesData.length);
    const pageInfo = document.getElementById('pageInfo');
    pageInfo.textContent = `Showing ${startIndex} to ${endIndex} of ${articlesData.length} entries`;
}

function generatePagination(page, size) {
    const totalPages = Math.ceil(articlesData.length / size);
    const pagination = document.getElementById('pagination');
    pagination.innerHTML = '';

    // Previous button
    const prevLi = document.createElement('li');
    prevLi.className = `page-item ${page === 1 ? 'disabled' : ''}`;
    const prevLink = document.createElement('a');
    prevLink.className = 'page-link';
    prevLink.href = '#';
    prevLink.textContent = 'Previous';
    prevLink.addEventListener('click', function (e) {
        e.preventDefault();
        if (page > 1) changePage(page - 1);
    });
    prevLi.appendChild(prevLink);
    pagination.appendChild(prevLi);

    // Page numbers
    const startPage = Math.max(1, page - 2);
    const endPage = Math.min(totalPages, page + 2);

    if (startPage > 1) {
        const firstLi = document.createElement('li');
        firstLi.className = 'page-item';
        const firstLink = document.createElement('a');
        firstLink.className = 'page-link';
        firstLink.href = '#';
        firstLink.textContent = '1';
        firstLink.addEventListener('click', function (e) {
            e.preventDefault();
            changePage(1);
        });
        firstLi.appendChild(firstLink);
        pagination.appendChild(firstLi);

        if (startPage > 2) {
            const ellipsisLi = document.createElement('li');
            ellipsisLi.className = 'page-item disabled';
            ellipsisLi.innerHTML = '<span class="page-link">...</span>';
            pagination.appendChild(ellipsisLi);
        }
    }

    for (let i = startPage; i <= endPage; i++) {
        const li = document.createElement('li');
        li.className = `page-item ${i === page ? 'active' : ''}`;
        const link = document.createElement('a');
        link.className = 'page-link';
        link.href = '#';
        link.textContent = i;
        link.addEventListener('click', function (e) {
            e.preventDefault();
            changePage(i);
        });
        li.appendChild(link);
        pagination.appendChild(li);
    }

    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            const ellipsisLi = document.createElement('li');
            ellipsisLi.className = 'page-item disabled';
            ellipsisLi.innerHTML = '<span class="page-link">...</span>';
            pagination.appendChild(ellipsisLi);
        }

        const lastLi = document.createElement('li');
        lastLi.className = 'page-item';
        const lastLink = document.createElement('a');
        lastLink.className = 'page-link';
        lastLink.href = '#';
        lastLink.textContent = totalPages;
        lastLink.addEventListener('click', function (e) {
            e.preventDefault();
            changePage(totalPages);
        });
        lastLi.appendChild(lastLink);
        pagination.appendChild(lastLi);
    }

    // Next button
    const nextLi = document.createElement('li');
    nextLi.className = `page-item ${page === totalPages ? 'disabled' : ''}`;
    const nextLink = document.createElement('a');
    nextLink.className = 'page-link';
    nextLink.href = '#';
    nextLink.textContent = 'Next';
    nextLink.addEventListener('click', function (e) {
        e.preventDefault();
        if (page < totalPages) changePage(page + 1);
    });
    nextLi.appendChild(nextLink);
    pagination.appendChild(nextLi);
}

function changePage(page) {
    const totalPages = Math.ceil(articlesData.length / pageSize);
    if (page >= 1 && page <= totalPages) {
        currentPage = page;
        displayArticles(currentPage, pageSize);
    }
}

function changePageSize() {
    const select = document.getElementById('pageSize');
    pageSize = parseInt(select.value);
    currentPage = 1;
    displayArticles(currentPage, pageSize);
}

document.addEventListener('DOMContentLoaded', function () {
    // Status Chart
    new Chart(document.getElementById('statusChart').getContext('2d'), {
        type: 'doughnut',
        data: {
            labels: ['Active', 'Inactive'],
            datasets: [{
                data: statusData,
                backgroundColor: ['#27ae60', '#95a5a6'],
                borderWidth: 2,
                borderColor: '#fff'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { position: 'bottom' },
                title: { display: true, text: 'Article Status Distribution' }
            }
        }
    });

    // Category Chart
    new Chart(document.getElementById('categoryChart').getContext('2d'), {
        type: 'bar',
        data: {
            labels: categoryLabels,
            datasets: [{
                label: 'Articles Count',
                data: categoryData,
                backgroundColor: '#3498db',
                borderColor: '#2980b9',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: {
                y: { beginAtZero: true, ticks: { stepSize: 1 } }
            }
        }
    });

    // Timeline Chart
    new Chart(document.getElementById('timelineChart').getContext('2d'), {
        type: 'line',
        data: {
            labels: timelineLabels,
            datasets: [{
                label: 'Articles Published',
                data: timelineData,
                borderColor: '#e74c3c',
                backgroundColor: 'rgba(231, 76, 60, 0.1)',
                borderWidth: 2,
                fill: true,
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { position: 'top' } },
            scales: {
                y: { beginAtZero: true, ticks: { stepSize: 1 } },
                x: { ticks: { maxRotation: 45 } }
            }
        }
    });

    // Initialize pagination
    displayArticles(currentPage, pageSize);

    // Add event listener for page size selector
    document.getElementById('pageSize').addEventListener('change', changePageSize);
});
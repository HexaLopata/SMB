function confirmDelete(argumentName, id, path) {
    let modalWindow = document.querySelector(".modalWindow")
    modalWindow.style.display = "block"
    modalWindow.innerHTML = `
        <b class="modalWindowText">Вы уверены, что хотите удалить это?</b>
        <form class="modalWindowButtons" method="post" action="${path}">
            <input type="hidden" name="${argumentName}" value="${id}" />
            <input id="deleteButton" onclick="closeModalWindow()" type="submit" value="Удалить" />
            <button type="button" id="backButton" onclick="closeModalWindow()">Назад</button>
        </form >`
}

function closeModalWindow() {
    let modalWindow = document.querySelector(".modalWindow")
    modalWindow.style.display = "none"
}
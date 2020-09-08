function confirmDelete(argumentName, id, path) {
    let modalWindow = document.querySelector(".modalWindow")
    modalWindow.style.display = "block"
    modalWindow.innerHTML = `<form method="post" action="${path}">
                                <input type="hidden" name="${argumentName}" value="${id}" />
                                <input onclick="closeModalWindow()" type="submit" value="Удалить" />
                            </form >
                            <button onclick="closeModalWindow()">Назад</button>`
}

function closeModalWindow() {
    let modalWindow = document.querySelector(".modalWindow")
    modalWindow.style.display = "none"
}